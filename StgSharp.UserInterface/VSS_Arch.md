# VSS 架构设计文档

> **作者**：Nitload-NSI  
> **创建日期**：2026-03-17  
> **所属项目**：StgSharp 框架 / 工业上位机架构预演  
> **文档状态**：初稿

---

## 目录

1. [背景与动机](#1-背景与动机)
2. [核心思想](#2-核心思想)
3. [架构总览](#3-架构总览)
4. [V 层：I/O 适配层](#4-v-层io-适配层)
5. [S 层：状态机](#5-s-层状态机)
6. [S 层：服务层](#6-s-层服务层)
7. [层间通信协议](#7-层间通信协议)
8. [与 MVVM 的对比](#8-与-mvvm-的对比)
9. [适用场景](#9-适用场景)
10. [参考实现索引](#10-参考实现索引)

---

## 1. 背景与动机

### 1.1 MVVM 的局限

MVVM（Model-View-ViewModel）是一种面向"轻型商用软件"设计的 UI 架构模式。其隐含前提为：

- **Model 是被动的数据容器**，不具备独立生命周期
- **ViewModel 是核心调度者**，负责连接 View 与 Model
- **View 由数据变化驱动**（双向绑定）

这一前提在以下场景中失效：

| 场景 | 失效原因 |
|---|---|
| 工业上位机 | 设备/硬件具有独立生命周期和状态机，不是"被动数据" |
| 仿真集群软件 | 计算节点有自己的异步事件流，不受 ViewModel 调度 |
| 多输入源 HMI | 鼠标、专用手柄、实体按钮等多源输入在 View 层打架 |
| 重型可视化软件 | ViewModel 膨胀为"上帝类"，承担了所有业务逻辑 |

### 1.2 直接动机

本架构的直接动机来源于维护一套 WinForms + MVP 上位机项目时，**鼠标与专用手柄两种输入模式相互干扰**的问题。

解决思路的核心转变：

> **外设事件不应提交给 View，而应提交给 State。**  
> View 只负责将当前 State 状态呈现出来。

这一转变最终演化为完整的 VSS 架构。

### 1.3 设计血统

VSS 的部分设计思路借鉴自**游戏引擎架构**，核心哲学为：

> **以输入为核心，以逻辑为驱动，界面显示是被动的副产品。**

这与 Unity New Input System、虚幻引擎 Enhanced Input 等现代游戏输入系统的设计理念一脉相承。

---

## 2. 核心思想

### 2.1 两个根本原则

**原则一：所有"意图"必须经过 State 归一化**

```
鼠标点击   ──┐
手柄按键   ──┼──→  State.Dispatch(Intent)  ──→  统一的状态转移逻辑
实体按钮   ──┤
网络指令   ──┘
```

无论信号来源如何，进入 State 之前都转化为统一的 `Intent`。State 是**唯一的语义解释者**。

**原则二：View 层可以完全换掉或关闭**

```
有头模式：   V(WPF)    + State + Service   （操作员在场）
无头模式：             State + Service   （自动化运行/集群节点）
远程模式：   V(薄客户端) + State + Service   （远程接管显示）
HMI 模式：   V(实体面板) + State + Service   （无屏幕操作台）
```

State 和 Service 在任何模式下**零改动**。

### 2.2 职责边界的时间维度

不同层次的操作具有根本不同的时间特征，这是分层的真正依据：

| 层 | 时间特征 | 驱动方式 |
|---|---|---|
| V（渲染输出） | 帧级，毫秒级刷新 | 时钟主动驱动 |
| V（输入采集） | 事件级，异步到达 | 中断/回调驱动 |
| State | 事件队列消费，微秒~毫秒 | 串行队列驱动 |
| Service | 业务流程，毫秒~分钟 | Command 触发 |
| 设备/集群节点 | 独立生命周期，跨越整个运行期 | 自驱动 |

让 ViewModel 同时承担所有时间维度的调度，是 MVVM 在重型软件中失败的根本原因。

---

## 3. 架构总览

```
┌────────────────────────────────────────────────────────────┐
│  V  —  View Layer（I/O 适配层集合）                         │
│                                                            │
│  ┌─────────────────────────┐  ┌────────────────────────┐   │
│  │  输入适配器              │  │  输出适配器              │   │
│  │  IInputAdapter          │  │  IOutputAdapter         │   │
│  │                         │  │                         │   │
│  │  · WpfInputAdapter      │  │  · WpfPresenter         │   │
│  │    鼠标/键盘/触摸         │  │    帧时钟驱动 XAML 刷新  │   │
│  │                         │  │                         │   │
│  │  · HmiInputAdapter      │  │  · HmiOutputAdapter     │   │
│  │    实体按钮/旋钮/急停     │  │    LED/炫彩/面板小屏    │   │
│  │                         │  │                         │   │
│  │  · DeviceInputAdapter   │  │  · NullOutputAdapter    │   │
│  │    专用手柄/外接设备      │  │    无头模式，丢弃输出    │   │
│  └──────────┬──────────────┘  └───────────┬────────────┘   │
└─────────────┼──────────────────────────────┼───────────────┘
              │ Intent（原始输入，无语义）      │ ← Snapshot（只读快照）
              ↓                               ↑
┌─────────────────────────────────────────────────────────────┐
│  S  —  State（抽象状态机）                                   │
│                                                             │
│  · 串行 Intent 队列，保证状态转移原子性                       │
│  · 持有所有 UI 可见状态，生成 Snapshot 通知 V 层             │
│  · 接收所有输入源：V 层 Intent + Service Event              │
│  · 向 Service 发送 Command（携带完整上下文）                 │
│  · 不持有 Service 具体实现引用（依赖接口）                    │
│  · 不知道 V 层的存在                                        │
└───────────────────────────┬─────────────────────────────────┘
                            │ Command（有语义指令，携带完整上下文）
                            ↕ Event（异步完成通知）
┌───────────────────────────────────────────────────────────────┐
│  S  —  Service Layer（业务服务层）                             │
│                                                               │
│  · 每个 Service 拥有独立生命周期和内部状态机                   │
│  · 执行具体业务：设备控制、集群调度、数据持久化、通信协议等     │
│  · 通过 Event 将异步结果回传给 State                          │
│  · 完全不知道 V 层的存在                                      │
│  · Command 应自描述，Service 执行时无需回调 State             │
└───────────────────────────────────────────────────────────────┘
```

---

## 4. V 层：I/O 适配层

### 4.1 重新定义"View"

VSS 中的 V **不是"界面"**，而是**"所有与外部世界的 I/O 适配层的集合"**。

```
V 层 = 输入适配器集合 ∪ 输出适配器集合
```

这一定义来源于对"实体 HMI 也是一种 View"的认识：

- 实体按钮面板：有输入（按键），有输出（指示灯、小屏幕、炫彩灯带）
- WPF 窗口：有输入（鼠标/键盘/触摸），有输出（屏幕渲染）
- 无头节点：无 V 层，State + Service 直接运行

### 4.2 输入适配器（IInputAdapter）

```csharp
public interface IInputAdapter
{
    void Start(Action<Intent> dispatch);
    void Stop();
}
```

职责：
- 监听来自特定输入源的原始信号
- 将原始信号转化为 `Intent`（**不做语义判断**）
- 通过 `dispatch` 委托投递到 State 的事件队列

**关键约束**：输入适配器只描述"发生了什么"，不判断"应该做什么"。

### 4.3 输出适配器（IOutputAdapter）与 Presenter

```csharp
public interface IOutputAdapter
{
    void Start();
    void Stop();
    void AcceptSnapshot(StateSnapshot snapshot);  // 可能在任意线程调用
}
```

**Presenter 是输出适配器的一种**，负责屏幕渲染输出。

WPF 环境下，Presenter 挂载于 `CompositionTarget.Rendering`——这是 WPF 暴露的帧循环钩子，与显示器刷新率同步（通常 60fps）。

```
State.Commit()
    ↓ AcceptSnapshot()（任意线程，原子写 _pending）
CompositionTarget.Rendering（WPF 帧时钟，UI线程）
    ↓ 检测 _pending 变化
    ↓ PushToView()
View.DataContext = snapshot（触发 XAML 绑定刷新）
```

**帧合并机制**：State 在两帧之间可能产生多个 Snapshot（如高频设备数据更新），Presenter 的帧循环只取最新的 `_pending`，自动合并中间状态，保持渲染频率稳定。

非屏幕输出适配器（如 HMI 灯带）使用独立低频定时器，无需挂载帧时钟。

### 4.4 ViewLayer 容器

```csharp
public class ViewLayer
{
    public ViewLayer Add(IInputAdapter  adapter);
    public ViewLayer Add(IOutputAdapter adapter);
    public void Start(Action<Intent> dispatch);
    public void Stop();
    public void AcceptSnapshot(StateSnapshot snapshot);  // 广播给所有输出适配器
}
```

V 层对外只暴露两个接口：
- `Start(dispatch)`：启动所有适配器，传入 State 的 Dispatch 入口
- `AcceptSnapshot(snapshot)`：接收新状态，广播给所有输出适配器

### 4.5 WPF 中的 XAML 规范

在 VSS 架构下，WPF 的 XAML 层应遵循以下规范：

```
✅ 允许：
  · 纯布局声明（Grid、StackPanel、Border 等）
  · DataContext 绑定（{Binding PropertyName}，单向或双向均可）
  · Code-behind 中的事件转发（OnXxxClicked → Dispatch(Intent)）

❌ 禁止：
  · XAML 中的 Command 绑定（ICommand、RelayCommand 等）
  · Code-behind 中的业务逻辑判断
  · ViewModel 类（无 ViewModel 层）
  · Code-behind 直接调用 Service
```

Code-behind 的唯一职责：

```csharp
// 用户操作 → 转发为 Intent，不做任何判断
private void OnPrintClicked(object s, RoutedEventArgs e)
    => _dispatch?.Invoke(new PrintRequestedIntent());
```

---

## 5. S 层：状态机

### 5.1 职责定义

State 是 VSS 的核心，也是**唯一持有应用状态的地方**。

```
State 做且只做：
  1. 接收 Intent
  2. 决策状态如何转移
  3. 向 Service 发 Command
  4. 生成新 Snapshot 通知 V 层
```

State **不做**：
- 任何 IO 操作
- 任何耗时计算
- 直接操作 UI 控件
- 持有 Service 的具体实现（依赖接口）

### 5.2 串行事件队列

State 内部维护一个串行消费队列，**所有输入源**（V 层 Intent + Service Event）都通过同一个队列进入：

```
[WPF鼠标事件] ──┐
[手柄按键]    ──┤
[实体按钮]    ──┼──→  Channel<Intent>  ──→  State（单线程消费）
[Service完成] ──┤
[定时器触发]  ──┘
```

**意义**：状态转移是原子的，不会因并发输入导致内部状态撕裂。这是简化版的 **Actor 模型**。

### 5.3 StateSnapshot

Snapshot 是 State 暴露给 V 层的**只读数据快照**，使用 C# `record` 定义以保证不可变性：

```csharp
public record StateSnapshot
{
    // 所有字段均为 init-only，外部无法修改
    public bool   IsBusy        { get; init; }
    public string StatusMessage { get; init; } = "";
    // ... 其他字段
}
```

State 内部通过 `with` 表达式生成新 Snapshot：

```csharp
Commit(_snapshot with { IsBusy = true, StatusMessage = "处理中..." });
```

### 5.4 Intent 与 Command 的区别

| | Intent | Command |
|---|---|---|
| **方向** | V → State | State → Service |
| **语义** | 无（描述事实） | 有（描述意图） |
| **产生者** | V 层适配器 | State 状态机 |
| **消费者** | State | Service |
| **示例** | `MouseClickedIntent(x, y)` | `StartSimulationCommand(config)` |

**同一个 Command 可以由多种 Intent 触发**，这是多输入源统一的关键。

---

## 6. S 层：服务层

### 6.1 职责定义

Service 负责所有"重活"：

- 设备驱动与通信协议
- 集群节点调度
- 数据持久化与查询
- 耗时计算与文件 IO
- 网络请求

Service 的核心特征：**有自己独立的生命周期和内部状态机**，不受 State 控制，只响应 Command。

### 6.2 Command 自描述原则

Command 应携带执行所需的**完整上下文**，Service 执行时无需回调 State 查询任何信息：

```csharp
// ❌ 错误设计：Service 执行到一半还要问 State
service.Execute(cmd);
// Service 内部：var config = state.GetCurrentConfig(); ← 违反原则

// ✅ 正确设计：Command 是自描述的
service.Execute(new StartSimulationCommand
{
    NodeCount      = 8,
    ConfigSnapshot = currentConfig,   // 执行时的完整上下文快照
    ResultEvent    = typeof(SimulationStartedEvent)  // 完成后回传的 Event 类型
});
```

**意义**：当某个 Service 大到需要拆分为独立微服务时，Command 的接口几乎不需要修改——这是架构可扩展性的基础。

### 6.3 Service 与 State 解耦

```
State → Service：Command（State 主动发起）
Service → State：Event（Service 异步回传）

Service 持有 State 的 Dispatch 委托（或事件总线）
Service 不持有 State 的引用
State 持有 Service 的接口引用
Service 完全不知道 V 层的存在
```

---

## 7. 层间通信协议

### 7.1 完整数据流

```
[用户操作 / 外设信号]
        ↓
IInputAdapter.OnXxx()
        ↓ dispatch(Intent)
State 的串行 Channel<Intent>
        ↓ HandleIntent(intent)
State 内部状态转移
        ↓ Commit(newSnapshot)
IOutputAdapter.AcceptSnapshot()     ← 可能在非 UI 线程
        ↓ （各适配器自行处理线程安全）
屏幕渲染 / LED 更新 / 面板刷新      ← 各自的驱动时钟
```

### 7.2 线程模型

| 操作 | 线程 |
|---|---|
| `IInputAdapter` 事件采集 | 各自线程（WPF UI 线程 / 后台轮询线程） |
| `State.Dispatch(Intent)` | 任意线程，线程安全（Channel 写入是原子的） |
| `State` 内部消费循环 | 单一后台线程 |
| `IOutputAdapter.AcceptSnapshot()` | State 的消费线程（非 UI 线程） |
| `WpfPresenter.PushToView()` | WPF UI 线程（由 `CompositionTarget.Rendering` 保证） |
| `HmiOutputAdapter.OnTick()` | Timer 回调线程 |

**核心结论**：整个架构中**只有 WpfPresenter 需要关心 UI 线程**，其余所有层次天然线程安全，无需到处写 `Dispatcher.Invoke`。

### 7.3 组装示例

```csharp
// Program / App.xaml.cs
var window = new MainWindow();

// 组装 V 层
var view = new ViewLayer()
    .Add(new WpfInputAdapter(window))
    .Add(new HmiInputAdapter(hmiDriver))
    .Add(new WpfPresenter(window))
    .Add(new HmiOutputAdapter(hmiDriver));

// 组装 Service 层
var excelService   = new ExcelService();
var printerService = new PrinterService();
// ...

// 组装 State
var state = new AppState(view.AcceptSnapshot, excelService, printerService);

// 启动
view.Start(state.Dispatch);
window.Show();
```

---

## 8. 与 MVVM 的对比

| 维度 | MVVM | VSS |
|---|---|---|
| **核心驱动** | 数据变化驱动视图 | 输入驱动状态，状态驱动视图 |
| **View 的角色** | 双向绑定的参与者 | 纯粹的 I/O 适配层 |
| **逻辑入口** | ICommand 散落在 ViewModel | 统一的 Intent → State |
| **多输入源** | 无原生支持，需自行处理 | 天然归一化到 State |
| **ViewModel/State 职责** | 常膨胀为上帝类 | State 只做状态转移决策 |
| **业务复杂度承载** | Model 层，无规范 | Service 层，有明确生命周期规范 |
| **线程模型** | 需大量 Dispatcher.Invoke | 只有 Presenter 需要关心 UI 线程 |
| **View 可替换性** | View 和 VM 生命周期耦合 | View 层可任意替换或完全关闭 |
| **无头运行** | 困难 | 天然支持（移除 V 层即可） |
| **适合场景** | 纯前端、纯数据展示类应用 | 工业上位机、仿真集群、多 HMI 软件 |

---

## 9. 适用场景

### ✅ VSS 强烈推荐

- 工业上位机（需要驱动硬件设备）
- 多输入源 HMI（鼠标 + 触摸 + 手柄 + 实体按钮）
- 仿真计算软件（既可单机也可集群）
- 需要"无头模式"运行的软件（自动化测试、远程节点）
- 某个功能模块已大到可以单独拆出做服务的软件

### ⚠️ MVVM 仍然适用

- 纯数据展示（Dashboard、报表工具）
- 纯前端业务（表单填写、内容管理）
- 业务逻辑简单，Model 没有独立生命周期

### ❌ 两者都不适合

- 分布式系统的后端服务（应使用 DDD / Clean Architecture）
- 微服务内部逻辑（应使用领域模型 + CQRS）

---


*本文档随项目演进持续更新。架构思想的提出与设计版权归 Nitload-NSI 所有。*