# L4 软件缓存器设计方案

> 项目：StgSharp  
> 作者：Nitload  
> 版本：草案 v0.3  
> 日期：2026-03-10

---

## 1. 目的

### 1.1 问题背景

StgSharp 的矩阵计算系统正从"全局单一上下文表"迁移为"逐线程上下文"架构，
以适配混合架构 CPU（如 Intel P-Core / E-Core）的调度特性。
迁移后每个线程独立持有计算上下文，但矩阵数据仍可能由 HLSF 分配器
或全局 `malloc` 分配在远端 NUMA 节点上，导致跨节点访问延迟显著增大。

同时，矩阵乘法（FMA）中 A 矩阵的核行数据需要从棋盘形 kernel grid 布局
重排（Pack）为微内核友好的连续排列。此 Pack 操作在 OpenBLAS 等库中
每次 tile 切换都重新执行，而 L4 可缓存 Pack 结果避免重复重排。

此外，矩阵乘法的分段累加（segment）需要将中间 partial sum 暂存在本地，
避免每个 segment 都跨 NUMA 写回目标矩阵。L4 可作为 partial sum 的
本地累加缓冲区，所有 segment 在本地完成后一次性写回。

### 1.2 解决方案

L4 缓存器是一个纯软件实现的**数据缓存层**，
位于 CPU 硬件缓存（L1/L2/L3）与主存之间的逻辑位置。
其核心职责：

1. **Pack 结果缓存**：缓存 FMA 中 A 核行的重排结果，避免重复 Pack
2. **NUMA 搬运缓存**：将远端 NUMA 数据搬运到本地并缓存，消除跨节点延迟
3. **Segment 累加缓冲**：暂存 FMA 分段计算的 partial sum，减少远端写操作
4. **PLU 中间数据缓冲**：为 PLU 分解提供本地连续工作区

### 1.3 设计目标

- 为逐线程矩阵计算上下文提供 NUMA 友好的本地数据访问
- 固定容量（64 MB），不做运行时扩容，确定性延迟
- 透明的指针重定向：上层代码通过原始指针查找，L4 返回本地缓存指针
- 基于 **RRPV 掩码 + 窗口 OPT 预测** 的缓存逐出策略
- 支持 **StreamTransparent（直通）** 和 **CacheBuffer（缓冲区）** 两种工作模式
- 支持**可写缓存行**，逐出或显式刷新时通过回调函数预处理并写回

---

## 2. 整体架构

### 2.1 三组件模型

```
┌���─────────────────────────────────────────────────────────────────┐
│                              L4                                  │
│                                                                  │
│  ┌──────────────┐  ┌────────────────────┐  ┌──────────────────┐  │
│  │  CacheLine    │  │     L4 本体        │  │    Predictor     │  │
│  │  缓存行句柄   │  │     缓存器         │  │    预取预测器     │  │
│  │              │  │                    │  │                  │  │
│  │ • 本地地址    │  │ • Swiss Table      │  │ • 2048 条缓冲    │  │
│  │ • 引用计数    │  │ • Slab 分配器      │  │ • 1024 阈值      │  │
│  │ • Dispose    │  │ • RRPV 掩码        │  │ • 窗口 OPT       │  │
│  │   释放引用    │  │ • SIMD 逐出        │  │ • 活跃 key 集    │  │
│  │              │  │ • 写回回调管理      │  │                  │  │
│  └──────────────┘  └────────────────────┘  └──────────────────┘  │
│                                                                  │
└──────────────────────────────────────────────────────────────────┘
```

### 2.2 数据访问路径

```
计算线程访问数据：

  ┌─────────────────────────────────────────────────────────┐
  │  只读路径：L4.Map(originKey)                             │
  │     ├─ StreamTransparent + 无需重排？                    │
  │     │  └─ 是 → 直接返回原始指针（零开销）                │
  │     │                                                   │
  │     └─ CacheBuffer 模式                                 │
  │        ├─ Swiss Table 查找（2.6ns，无锁）                │
  │        ├─ 命中 → 更新 RRPV=0，返回 CacheLine 句柄       │
  │        └─ 未命中 → SpinWait 等待 Predictor 预取完成      │
  ├─────────────────────────────────────────────────────────┤
  │  可写路径：L4.MapWritable(originKey, preprocess, writeback) │
  │     ├─ 命中 → 返回已有的 partial sum 缓存行             │
  │     └─ 未命中 → 分配零初始化缓存行，注册写回回调         │
  │        缓存行标记为脏（dirty=1）                         │
  │        逐出时自动调用 preprocess + writeback             │
  ├─────────────────────────────────────────────────────────┤
  │  计算完成后：                                            │
  │  3. CacheLine.Dispose()（refCount--）                    │
  │  4. L4.StepComplete()                                   │
  │     ├─ Predictor.AdvanceExecutionPoint()                │
  │     └─ Predictor.NeedsPrefetch?                         │
  │        └─ 是 → CAS 抢锁，执行预取刷新                   │
  └─────────────────────────────────────────────────────────┘
```

### 2.3 关键参数

| 参数                 | 值                     | 说明                              |
|---------------------|------------------------|-----------------------------------|
| 缓存区总容量         | 64 MB                  | per-NUMA-node 连续内存             |
| 缓存行大小           | 8192 Bytes             | = 64 个 `double MAT_KERNEL` 的大小 |
| 缓存行总数           | 8192                   | 64MB / 8192B                      |
| 映射表类型           | Swiss Table            | 1024 桶 × 16 槽，SIMD 加速         |
| 缓存行存储池         | Slab Allocator         | 非并发、固定容量、不自动扩容        |
| 逐出策略             | RRPV 掩码 + 窗口 OPT   | SoA 布局 + SSE2 批量扫描           |
| 预取策略             | Predictor 窗口 OPT     | 2048 条缓冲，超前 1024 步          |
| 写回策略             | 回调驱动               | 脏行逐出/显式刷新时调用注册的回调   |

---

## 3. 重要组件

### 3.1 Swiss Table（指针映射表）

**职责**：维护 `原始指针 → 缓存指针` 的映射关系。

- 键（Key）：原始指针 `nint`，指向 HLSF/malloc 分配的远端数据
- 值（Value）：缓存指针 `nint`，指向本地 Slab 分配的缓存行
- 哈希：使用硬件 `CRC32C` 指令对 64 位指针计算哈希
  - 低 10 位 → 桶索引（1024 个桶）
  - 第 10-16 位 → H2 控制字节（用于 SIMD 预筛选）
- 扫描：SSE2 `pmovmskb` 指令并行比较 16 个控制字节
- 墓碑机制：删除时 ctrl 设为 `0xFE`，保护探测链完整性
- 容量：1024 × 16 = 16,384 条映射，远大于缓存行总数（8192 行），
  保证极低的哈希碰撞率

**性能实测**（Ryzen 7 6800H, .NET 8, BenchmarkDotNet）：

| 操作                    | Swiss Table  | Dictionary  | 倍速    |
|------------------------|-------------|-------------|--------|
| Zipfian Lookup (8192次) | 20.0 μs     | 76.1 μs     | 3.8x   |
| Uniform Lookup (8192次) | 18.6 μs     | 50.6 μs     | 2.7x   |
| Insert (4096 条)        | 18.5 μs     | 46.5 μs     | 2.5x   |
| SteadyState (4096 ops)  | 25.3 μs     | 68.3 μs     | 2.7x   |

单次查找延迟 ≈ 2.6 ns，GC 分配 = 0。

### 3.2 Slab Allocator（缓存行存储池）

**职责**：管理固定大小（8192B）缓存行的分配与回收。

- 模式：**非并发、固定容量**（`FixedSlabAllocator<T>` 模式），
  复用现有 `SlabAllocator.FromBuffer<T>()` 接口
- 不自动扩容：容量在初始化时确定（64MB / 8192B = 8192 行），
  分配失败时触发逐出流程而非扩容
- 分配复杂度：O(1)，从空闲栈弹出
- 回收复杂度：O(1)，压回空闲栈
- 对齐：所有缓存行 64B 对齐，确保不跨硬件缓存行边界

### 3.3 逐出元数据（SoA 布局 + RRPV 掩码 + 写回回调）

**职责**：追踪缓存行的引用状态、热度和写回信息，决定逐出优先级。

取代 v0.1 中的 4 级环形链表，采用 Structure-of-Arrays 布局的平行数组：

```
SoA 布局 — 平行数组

  _refFlags[i]       : byte      引用计数快照（0=空闲, 非0=被持有）
  _evictionFlags[i]  : byte      dirty(1bit) + RRPV(2bit) + costClass(1bit)
  _originKeys[i]     : nuint     原始指针（逐出时反向注销 Swiss Table）
  _preprocess[i]     : nint      写回预处理函数指针（脏行逐出前调用）
  _writeback[i]      : nint      写回函数指针（脏行逐出时调用）
  _callbackCtx[i]    : nuint     回调上下文（传递给 preprocess/writeback）
```

**逐出标志位布局**（`_evictionFlags[i]`）：

RRPV数组使用byte存储每一个cacheline的使用情况，其低二位表示如下逻辑

**RRPV 语义**：

| RRPV | 含义                     | 逐出优先级 |
|------|--------------------------|-----------|
| 0    | 马上会被再次访问          | 最低（不逐出）|
| 1    | 一会儿会被访问            | 低         |
| 2    | 比较久以后才会被访问       | 中         |
| 3    | 可能再也不会被访问了       | 最高（优先逐出）|

**RRPV 操作时机**：

| 时机           | 操作                              | 说明                        |
|---------------|-----------------------------------|-----------------------------|
| 加载时（插入） | RRPV = 0                         | 刚加载的数据马上要用          |
| 命中时         | `_evictionFlags[i] &= 0xF9`      | bit 1-2 清零，RRPV 归零      |
| 逐出扫描时     | 找 RRPV=3 的行逐出                | 找不到则全体 RRPV+1（老化）   |

**相比 v0.1 的 4 级环形链表的优势**：

| 对比项         | v0.1 环形链表           | v0.3 RRPV 掩码 + 回调       |
|---------------|------------------------|------------------------------|
| 数据结构       | 4 条双向链表 + next 指针 | byte 数组 + 函数指针数组      |
| 升温操作       | 摘节点 + 插节点（4 指针）| `+= 1`（1 字节写入）      |
| 老化操作       | 逐节点移链表             | SIMD 批量 +1（16 个/周期）   |
| 逐出扫描       | 遍历链表头（指针追逐）    | SIMD 模式匹配（16 个/周期）  |
| 缓存友好性     | 差（指针跳跃）           | 好（连续内存扫描）            |
| 引用保护       | 无                      | refCount > 0 时不可逐出      |
| 写回支持       | 无                      | 回调函数驱动                  |

### 3.4 写回回调机制

**职责**：为可写缓存行提供灵活的写回策略，通过本机函数指针避免托管开销。

**设计原则**：所有可写缓存行默认视为脏数据。逐出或显式刷新时，
L4 通过注册的函数指针调用写回逻辑，调用者决定如何预处理和写回。

```
写回流程：

  逐出脏缓存行 slot[i]:
    1. 检查 dirty 位：(_evictionFlags[i] & 0x01) != 0
    2. 调用预处理：_preprocess[i](cacheline_addr, _callbackCtx[i])
       → 例如：将 Pack 格式反向重排为原始矩阵布局
    3. 调用写回：  _writeback[i](origin_addr, cacheline_addr, _callbackCtx[i])
       → 例如：累加 partial sum 到目标矩阵
    4. 标准逐出：注销映射，归还 Slab
```

**回调签名**（本机函数指针）：

```csharp
/// <summary>
/// 写回预处理回调。在写回前对缓存行数据进行变换。
/// 例如：将 Pack 重排的数据恢复为原始布局。
/// </summary>
/// <param name="cacheLineAddr">缓存行本地地址</param>
/// <param name="context">调用者提供的上下文</param>
delegate* unmanaged<nuint, nuint, void> Preprocess;

/// <summary>
/// 写回回调。将缓存行数据写回到目标地址。
/// 例如：将 partial sum 累加到目标矩阵。
/// </summary>
/// <param name="originAddr">原始目标地址</param>
/// <param name="cacheLineAddr">缓存行本地地址</param>
/// <param name="context">调用者提供的上下文</param>
delegate* unmanaged<nuint, nuint, nuint, void> Writeback;
```

**典型回调实现**：

```
场景 1：Segment 累加（FMA partial sum）
  preprocess = null（无需预处理，数据已经是 double 数组）
  writeback  = AccumulateAdd（逐元素 dst[i] += src[i]，AVX2 向量化）

场景 2：Pack 重排的中间结果写回
  preprocess = UnpackKernelRow（将 Pack 格式反向重排为行主序）
  writeback  = AccumulateAdd

场景 3：PLU 分解中间数据
  preprocess = null
  writeback  = DirectCopy（直接覆写 memcpy）
```

### 3.5 CacheLine（缓存行句柄）

**职责**：计算线程持有的缓存行访问句柄，通过引用计数防止正在使用的行被逐出。

```csharp
public ref struct CacheLine : IDisposable
{
    private ref int _activeCount;    // L4 全局活跃计数
    private ref int _refCount;       // 该缓存行的引用计数
    private nuint   _handle;         // 本地缓存数据地址

    // 构造时：Interlocked.Increment(ref _activeCount)
    //         Interlocked.Increment(ref _refCount)
    // Dispose：Interlocked.Decrement 两个计数器
}
```

- `ref struct` 确保不会逃逸到堆上
- 引用计数通过 `Interlocked` 原子操作保证线程安全
- `_refCount > 0` 的缓存行在逐出扫描中被跳过
- StreamTransparent 模式下返回的 CacheLine 不占用缓存槽位
- 只读路径和可写路径返回相同的 CacheLine 类型，
  调用者通过 `.Address` 决定读写行为

### 3.6 Predictor（预取预测器）

**职责**：基于窗口 OPT 策略，为 L4 提供确定性的预取预测。

**核心原理**：矩阵计算的任务序列是确定性的——调度器预先分配了每个线程的
任务块 `(kr, kc, zz)`，因此可以精确计算未来每步任务需要的缓存行 key。
这使得 L4 拥有硬件缓存不可能具备的预测能力：**直接看到未来**，
而非像 Hawkeye 等策略那样从历史推断未来。

```
Predictor 滑动窗口：

  任务序列：... [T_now] [T+1] [T+2] ... [T+1023] ... [T+2047]
                 ↑ 执行位点                          ↑ 缓冲区尾部

  窗口容量 = 2048 条预测条目
  预取触发阈值 = 预取前沿领先执行位点不足 1024 步时触发刷新
  每次刷新批量 = 64 条（避免单次刷新耗时过长）
```

**数据结构**：

```csharp
internal class Predictor
{
    private const int WindowCapacity = 2048;
    private const int RefillThreshold = 1024;

    private PrefetchEntry[] _buffer;      // 环形预测缓冲区
    private int _head;                     // 预取前沿（已预取到此）
    private int _tail;                     // 生产位置（调度器填充到此）
    private int _executed;                 // 执行位点（计算线程已完成到此）

    private HashSet<nuint> _activeKeys;    // 窗口内所有活跃 key（去重）
}

struct PrefetchEntry
{
    public nuint Key;              // 缓存行标识
    public nuint SourceAddress;    // 源数据地址
    public bool  NeedsRepack;      // 是否需要非连续重排
}
```

**与逐出策略的协作**：

```
逐出优先级（从先逐出到后逐出）：

  1. refCount==0, 不在窗口中, RRPV==3, dirty==0  ← 最先（零成本逐出）
  2. refCount==0, 不在窗口中, RRPV==3, dirty==1  ← 其次（需写回但已冷）
  3. refCount==0, 不在窗口中, RRPV<3,  dirty==0  ← 按 RRPV 排序
  4. refCount==0, 不在窗口中, RRPV<3,  dirty==1  ← 按 RRPV 排序
  5. refCount==0, 在窗口中                         ← 尽量不逐出
  6. refCount>0                                    ← 绝不逐出
```

窗口 OPT 使得大多数逐出决策无需依赖 RRPV——"不在窗口中"已足以判定可逐出。
RRPV 在窗口外的行中做细粒度区分；dirty 位在同等 RRPV 下优先逐出
干净行以避免写回开销。

### 3.7 并发模型

L4 采用**无额外线程**设计，所有操作由计算线程自身完成：

| 操作                    | 执行者            | 并发性                        |
|------------------------|-------------------|-------------------------------|
| Lookup（Swiss Table 读）| 各计算线程         | ✅ 完全并行，无锁              |
| HIT 更新 RRPV          | 各计算线程         | ✅ 单字节写，无需同步          |
| refCount 增减           | 各计算线程         | ✅ Interlocked 原子操作        |
| 预取刷新（逐出+加载）    | CAS 抢锁的单个线程 | 串行（仅一个线程进入）          |
| 脏行写回                | CAS 抢锁的线程     | 串行（在逐出流程内执行）        |
| FlushAllDirty          | 计算结束后单线程    | 串行（无并发）                  |

**预取刷新 CAS 锁**：

```csharp
private int _refreshLock;  // 0=free, 1=locked

private void TryRefreshPrefetch()
{
    if (Interlocked.CompareExchange(ref _refreshLock, 1, 0) != 0)
        return;  // 另一个线程在做了，当前线程继续计算
    try { /* 逐出 + 预取 */ }
    finally { Volatile.Write(ref _refreshLock, 0); }
}
```

**Swiss Table 并发读安全保证**：预取刷新线程插入时遵守严格写顺序：
先写 Slab 数据，再写逐出元数据和回调指针，`sfence`，最后写 Swiss Table 的
控制字节和 key/value，确保计算线程读到控制字节时数据已完整可见。

---

## 4. 操作流程

### 4.1 缓存查找（只读命中）

1. 计算原始指针的 CRC32C 哈希
2. Swiss Table 查找：SIMD 扫描 ctrl → 比较 key → 返回缓存指针
3. 更新 RRPV：`_evictionFlags[slot] &= 0xF9`（RRPV 归零，dirty 位不变）
4. 构造 CacheLine 句柄（refCount++）
5. 返回 CacheLine，上层代码通过 `.Address` 直接访问本��数据

### 4.2 缓存查找（可写命中）

1. Swiss Table 查找命中
2. 更新 RRPV：`_evictionFlags[slot] &= 0xF9`
3. 构造 CacheLine 句柄（refCount++）
4. 返回 CacheLine，上层代码通过 `.Address` 读写缓存行（如 partial sum +=）

### 4.3 缓存未命中（只读）

1. Swiss Table 查找未命中
2. 计算线程进入 SpinWait 等待循环
3. 等待 Predictor 驱动的预取刷新将该行加载进 L4
4. Swiss Table 出现命中后退出等待
5. 构造 CacheLine 句柄并返回

正常运行时，Predictor 始终超前执行位点 1024 步以上，
miss 仅在冷启动或极端突发时发生。

### 4.4 缓存未命中（可写）

1. Swiss Table 查找未命中
2. 从 Slab 分配缓存行
3. 若 Slab 无空闲 → 触发逐出（见 4.6）
4. 缓存行零初始化（`Unsafe.InitBlock(dest, 0, 8192)`）
5. 注册写回回调：`_preprocess[slot]`, `_writeback[slot]`, `_callbackCtx[slot]`
6. 设置逐出标志：`dirty=1, RRPV=0`
7. 写入 `_originKeys[slot]`
8. `sfence`
9. 写入 Swiss Table 映射
10. 构造 CacheLine 句柄并返回

### 4.5 预取刷新

由计算线程在 `StepComplete()` 中触发，CAS 抢锁确保单线程执行：

1. 同步 refFlags 快照（`_refCounts[i] > 0 → _refFlags[i] = 1`）
2. 从 Predictor 取出待预取条目（每次最多 64 条）
3. 对每个条目：
   - Swiss Table 命中 → 标记 RRPV=0（热行保护）
   - Swiss Table 未命中 → 执行加载：
     a. Slab 分配缓存行
     b. 若 Slab 无空闲 → 触发逐出（见 4.6）
     c. 根据 `NeedsRepack` 标志执行 Pack 重排或 memcpy 搬运
     d. 写入逐出元数据（originKey, RRPV=0, dirty=0, 回调=null）
     e. `sfence`
     f. 写入 Swiss Table 映射

### 4.6 缓存逐出（SIMD 扫描 + 写回）

```
逐出扫描伪代码：

  Pass 1: 找 refCount==0, RRPV==3, dirty==0（零成本逐出）
    for i in range(scanCursor, CacheLineCount), step=16:
      refChunk = SSE2.Load(refFlags + i)
      refFreeMask = SSE2.MoveMask(SSE2.CompareEqual(refChunk, zero))

      evChunk = SSE2.Load(evictionFlags + i)
      // RRPV==3 且 dirty==0 → byte & 0x07 == 0x06
      masked = SSE2.And(evChunk, 0x07)
      coldCleanMask = SSE2.MoveMask(SSE2.CompareEqual(masked, 0x06))

      candidates = refFreeMask & coldCleanMask
      while candidates != 0:
        bit = TrailingZeroCount(candidates)
        idx = i + bit
        if originKeys[idx] not in Predictor.ActiveKeys:
          evict(idx)    // 无写回，直接丢弃
          return
        candidates &= candidates - 1

  Pass 2: 找 refCount==0, RRPV==3, dirty==1（需写回）
    同上，但匹配 byte & 0x07 == 0x07
    evict 时先执行写回回调

  Pass 3: 全体 RRPV+1（老化），重试
    最多 3 轮
```

**逐出执行**：

```csharp
private void Evict(int slot)
{
    byte flags = _evictionFlags[slot];

    // 脏行写回
    if ((flags & 0x01) != 0)
    {
        nuint origin = _originKeys[slot];
        nuint cached = SlotAddressFromIndex(slot);
        nuint ctx    = _callbackCtx[slot];

        // 1. 预处理（可选）
        var preprocess = (delegate* unmanaged<nuint, nuint, void>)_preprocess[slot];
        if (preprocess != null)
            preprocess(cached, ctx);

        // 2. 写回
        var writeback = (delegate* unmanaged<nuint, nuint, nuint, void>)_writeback[slot];
        if (writeback != null)
            writeback(origin, cached, ctx);
    }

    // 标准逐出
    _map.TryRemove(_originKeys[slot], out _);
    _slab.Free(SlotAddressFromIndex(slot));
    _evictionFlags[slot] = 0;
    _originKeys[slot] = 0;
    _preprocess[slot] = 0;
    _writeback[slot] = 0;
    _callbackCtx[slot] = 0;
}
```

### 4.7 显式刷新

```csharp
/// <summary>
/// 显式将指定脏缓存行写回。在所有 segment 完成后调用。
/// </summary>
public void FlushDirty(nuint originKey)
{
    if (_map.TryGet(originKey, out nuint addr))
    {
        int slot = SlotIndex(addr);
        if ((_evictionFlags[slot] & 0x01) != 0)
        {
            // 执行预处理 + 写回
            InvokeWriteBack(slot);
            _evictionFlags[slot] &= 0xFE;  // 清除 dirty 位
        }
    }
}

/// <summary>
/// 批量写回所有脏缓存行。在整个矩阵计算结束后调用。
/// </summary>
public void FlushAllDirty()
{
    for (int i = 0; i < CacheLineCount; i++)
    {
        if ((_evictionFlags[i] & 0x01) != 0 && _originKeys[i] != 0)
        {
            InvokeWriteBack(i);
            _evictionFlags[i] &= 0xFE;
        }
    }
}
```

### 4.8 工作模式切换

```csharp
public enum L4Mode
{
    /// <summary>
    /// 直通模式：Map() 直接返回原始指针，L4 不介入。
    /// 适用于单 NUMA + 无需重排的场景（如向量加法、标量乘法）。
    /// </summary>
    StreamTransparent,

    /// <summary>
    /// 缓冲区模式：查找/Pack/缓存/逐出全流程。
    /// 适用于需要重排的 FMA 场景 或 多 NUMA 场景。
    /// </summary>
    CacheBuffer
}
```

模式切换在构造时确定，运行时不变。判断逻辑：

- 需要 Pack 重排（FMA 的 A 核行）→ CacheBuffer
- 多 NUMA 节点 → CacheBuffer
- 需要 segment 累加 → CacheBuffer
- 单 NUMA + 无需重排 + 无 segment → StreamTransparent

---

## 5. Segment 累加场景

### 5.1 问题

矩阵乘法 `C = A × B` 当 K 维度很大时，需要分段累加：

```
segment 0:  C_partial  = A[kr, 0..KC]    × B[0..KC, kc]
segment 1:  C_partial += A[kr, KC..2KC]  × B[KC..2KC, kc]
...
segment N:  C_partial += A[kr, N*KC..K]  × B[N*KC..K, kc]

最终 C[kr, kc] = Σ 所有 segment 的 partial sum
```

如果直接将每个 segment 的结果 `+=` 到 C 矩阵，
且 C 在远端 NUMA 节点上，则每个 segment 都有跨节点写延迟。

### 5.2 L4 解决方案

将 C 的 partial sum 暂存在 L4 的可写缓存行中：

```
segment 0: c = l4.MapWritable(C_key, null, &AccumulateAdd)
           // 首次未命中 → 分配零初始化缓存行
           FMA(c.Address, a.Address, b.Address)  // c += a*b
           c.Dispose()

segment 1: c = l4.MapWritable(C_key, null, &AccumulateAdd)
           // 命中！返回已有的 partial sum 缓存行
           FMA(c.Address, a.Address, b.Address)  // c += a*b
           c.Dispose()

... 所有 segment 完成后 ...

l4.FlushDirty(C_key)
// 调用 AccumulateAdd(C_origin, cacheline, ctx)
// → 将本地 partial sum 一次性加回远端 C 矩阵
```

### 5.3 收益

```
不用 segment 缓存（OpenBLAS 做法）：
  每个 segment 都跨 NUMA 写 C 矩阵
  远端写次数 = Zk 次
  每次写延迟 = ~200-300ns（跨节点）

用 L4 segment 缓存：
  所有 segment 的 += 在本地 L4 缓存行完成
  partial sum 大概率留在 L1/L2（8KB 远小于 L1 容量）
  远端写次数 = 1 次（最终 FlushDirty）
  写回用 AVX2 向量化：8192B = 32 次 256-bit store

  远端写减少比 = Zk : 1
```

---

## 6. 独特性与优势

### 6.1 确定性预取（窗口 OPT）

与硬件缓存替换策略（LRU, RRIP, Hawkeye）的本质区别：

| 策略              | 信息来源     | 预测方式           | 精度       |
|------------------|-------------|-------------------|-----------|
| LRU              | 过去的访问顺序| 最近最少使用       | 中         |
| RRIP             | 插入/命中标记| 预测重用距离       | 中高       |
| Hawkeye          | OPTgen 回溯 | 学习过去推断未来    | 高         |
| **L4 Predictor** | **未来任务队列** | **直接看到未来** | **完美**（窗口内）|

L4 的 Predictor 是**确定性的 Oracle**——它不预测，它知道。
因为矩阵计算的任务序列在调度器分配时就已完全确定。

### 6.2 NUMA 自适应

同一套代码在不同硬件配置下自动获得 NUMA 收益：

| 硬件配置         | L4 行为                                    |
|-----------------|-------------------------------------------|
| 单路 CPU        | Pack 缓存 + segment 累加缓冲               |
| 双路 CPU        | Pack 缓存 + NUMA 搬运缓存 + segment 累加    |
| 混合架构 CPU    | Pack 缓存 + 跨域搬运缓存 + segment 累加     |

### 6.3 回调驱动的写回策略

通过本机函数指针实现写回，避免了：
- 虚方法调用开销
- 接口分发开销
- 委托的 GC 压力
- 硬编码特定写回逻辑

调用者在 `MapWritable` 时注册 preprocess 和 writeback 回调，
L4 在逐出或 Flush 时直接通过函数指针调用，
**零托管开销，零虚分发，与 C 函数调用等价的性能**。

### 6.4 引用计数保护

通过 `CacheLine` 句柄的引用计数，正在被计算线程使用的缓存行
绝对不会被逐出（包括不会触发写回）。SIMD 逐出扫描通过 `_refFlags`
数组在第一时间跳过 `refCount > 0` 的行，无需检查链表或加锁。

### 6.5 缓存行大小与矩阵 Kernel 对齐

8192B 的缓存行大小不是任意选取的：
- 1 个 `double MAT_KERNEL`（4×4 double 矩阵）= 128 Bytes
- 1 个缓存行 = 64 个 `MAT_KERNEL`
- 矩阵乘法的微内核每次处理的数据量与缓存行完全对齐
- 计算核函数以缓存行为单位操作，不存在跨缓存行的情况

### 6.6 固定容量的确定性

- Slab 分配器不扩容 → 内存占用上界已知（64 MB）
- SIMD 逐出扫描 → 延迟上界已知（最多 3 轮老化 × 512 次 SIMD 比较）
- Swiss Table 固定 16,384 槽位 → 查找延迟上界已知
- 写回回调延迟可由调用者控制 → 可预测
- 整个 L4 的最坏延迟 = 查找 + 逐出(含写回) + 分配 + memcpy，全部可静态计算

### 6.7 与 StgSharp 现有基础设施的复用

| L4 组件         | 复用的 StgSharp 基础设施                 |
|----------------|----------------------------------------|
| 映射表          | `SwissTable`（已实现，经过 benchmark 验证）|
| 映射表 SIMD     | `IBucketScanner` + `SIMDID` 架构检测体系 |
| 缓存行分配       | `SlabAllocator.FromBuffer<T>()`        |
| 哈希计算         | 硬件 `CRC32C` 指令                     |
| 内存分配         | `NativeMemory.AlignedAlloc`            |
| 未来多平台支持    | `IBucketScanner` + NRGA 编译管道       |

### 6.8 开创性分析

经过对现有主流数值计算库的调研，L4 缓存器的设计在已知公开实现中
**没有直接的先例或等价物**。

#### 现有库的 NUMA 策略对比

| 库 / 框架         | NUMA 策略                       | 软件缓存层 | 映射表     | 逐出策略          | 脏行写回     |
|-------------------|-------------------------------|-----------|-----------|-------------------|------------|
| Intel MKL         | 线程绑核 + `numactl`            | 无        | 无        | 无                | 无         |
| OpenBLAS          | 线程亲和性 + 分块对齐             | 无        | 无        | 无                | 无         |
| BLIS              | 多级分块对齐 L1/L2/L3            | 无        | 无        | 无                | 无         |
| Eigen             | 无显式 NUMA 处理                 | 无        | 无        | 无                | 无         |
| cuBLAS / rocBLAS  | GPU 显存（硬件解决）              | 无        | 无        | 无                | 无         |
| **StgSharp L4**   | **软件管理的线程本地缓存**        | **有**    | **Swiss Table** | **RRPV+窗口OPT** | **回调驱动** |

---

## 7. 从 v0.1 到 v0.3 的变更摘要

| 项目             | v0.1                          | v0.3                                 |
|-----------------|-------------------------------|---------------------------------------|
| 逐出数据结构     | 4 级环形双向链表               | RRPV 2-bit 掩码 + 8KB SoA 数组        |
| 逐出扫描方式     | 链表头指针遍历                 | SSE2 批量模式匹配（16 行/周期）         |
| 热���更新         | 链表节点摘插（4 指针写）        | 单字节位运算 `&= 0xF9`                |
| 老化操作         | 逐节点移链表                   | SIMD 批量 +1                          |
| 引用保护         | 无                            | refCount > 0 时 SIMD 扫描跳过          |
| 预取策略         | 无                            | Predictor 窗口 OPT（2048 条缓冲）      |
| 线程模型         | L4 服务线程 + Command Queue    | 无额外线程，CAS 抢锁                   |
| 命令队列         | 链式 CommandCache 分段          | 不需要（预取刷新替代命令驱动）           |
| 缓存行元数据     | Slab 分配的 CacheLineHead 结构 | SoA 平行数组 + 函数指针数组             |
| 工作模式         | 单一模式                       | StreamTransparent / CacheBuffer 切换   |
| 写回策略         | 无（只读缓存）                  | dirty 位 + preprocess/writeback 回调   |
| 写回机制         | 无                            | 本机函数指针，零托管开销                |

---

## 8. 未来工作

- [ ] L4 根类型重构（组合 Swiss Table、Slab、SoA 元数据、Predictor）
- [ ] Predictor 与矩阵乘法调度器的集成
- [ ] Segment 累加写回的 AVX2 向量化实现
- [ ] SIMD 逐出扫描的 AVX2 升级（32 行/周期）
- [ ] Swiss Table ARM NEON 实现
- [ ] Swiss Table AVX-512 升级（64 槽扫描从 16 次降至 4 次）
- [ ] 墓碑累积监控与自动 rehash 策略
- [ ] 性能基准测试（NUMA 本地 vs 远端访问延迟对比）
- [ ] Segment 累加 vs 直接写回的 benchmark 对比
- [ ] 与逐线程矩阵上下文的集成验证
- [ ] 双路 6242R 环境下的 NUMA 搬运性能验证
- [ ] 远期：PPC64LE 平台的 VSX 原生 Swiss Table 扫描支持

---

## 9. 参考

- Google Abseil Swiss Tables Design Notes
- HP Labs RRIP (Re-Reference Interval Prediction) 论文
- Hawkeye: Efficient Cache Replacement via OPTgen (ISCA 2017)
- StgSharp HLSF Allocator 设计文档
- StgSharp SIMD Architecture (`SIMDID` 体系)
- StgSharp NRGA Roadmap
- StgSharp `SlabAllocator<T>` 实现
- StgSharp `SwissTable` 实现及 Benchmark