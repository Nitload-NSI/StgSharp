# IR 层退役说明

2026-08-12。本项目的正则源生成路线从「AST → IR → 优化 → 前缀化 → 源码」
改为「AST → 树上规范化 → 直接递归生成源码」。
所有 `*.cs.old` 文件是退役的 IR 层实现，保留作参考，不参与编译。

## 为什么不要 IR 了

一句话：**前缀 IR 的整套机制都在补偿线性化亲手制造的问题，而源生成端最后又把树形还原了回来**。压平再还原，两头各付一遍成本。

逐项对照，前缀 IR 的每个机制在树上的对应物：

| 前缀机制 | 存在原因 | 树上的对应物 |
| --- | --- | --- |
| `Prev_Fail` / `Prev_Success` | 压平后失败链丢失 | ALT 节点的 case 链，天生就是 if/else |
| `Observe_Line` / `ObserveSource` | 平坦流里"读谁的结果"需显式编号 | 结构化控制流，结果就在作用域里 |
| `Pop_xxx` + region id | `break` 要知道跳出哪个作用域 | 树的作用域嵌套本身 |
| `RegionExtent` / 套山 | 切片找不到子表达式边界 | 子树边界白送 |

IR 窗口优化器（`ScanAndOptimizeIR`）为了在平坦序列上做局部重写，
搭了 8 条滑窗、`IsFullyOptimized` 防重扫、`goto phase_zero` 重启、
`CommandCount` 重算一整套脚手架——这些在树上是单节点替换。

历史讨论结论：.NET 官方 RegexGenerator 同样直接展开 `RegexNode` 树生成源码；
线性字节码的正当用途是**解释器**（`#run stepping` 模式），不是源生成。
IR 层代码因此归档而非删除：将来做 stepping 解释器时是字节码路线的起点。

## 需要在 AST 上补什么

### 节点重设计（先于一切）

`RegexAstNode` 不再挂载 `Token<RegexElementLabel>`：token 在 AST 构建时被**消费掉**，
转换为结构化载荷。节点只剩三样东西：`Label`（原 `Source.Flag`）、`IPayload`、拓扑。
这把 IR 层真正有价值的部分（类型化语义）请了回来，线性形态留在 `.old` 里。

| Label | Payload | 转换时机 |
| --- | --- | --- |
| `UNIT` / `UNIT_SPAN` | `LiteralPayload(Text)`，**转义已消解** | AST 构建 |
| `UNIT_SET` | `CharSetPayload(Set)` | AST 构建 |
| `COUNT` | `CountPayload(Min, Max, IsGreedy)`，即 `{m,n}` 的唯一解析点 | AST 构建 |
| `GROUP_BEGIN` | `GroupPayload(Name, Index)` | AST 构建 |
| `CONCAT` / `ALT` | 无（null） | — |
| `FIND`（新） | `FindPayload(Anchor, IsNearest)` | phase 6 融合 pass |

规则：

1. 载荷全部是不可变 record，深拷贝共享引用不复制
2. `ReplaceBy` / `RemoveAndAsChild` 搬运 `Label + Payload`，不再拷 Source
3. mask 决断载荷类型，取用走 `PayloadAs<T>()`，类型不符当场抛
4. pass 局部属性（`FirstAtom`/`LastAtom`）不住这个槽，放融合 pass 的 side table

改造点：`ASTGenerate` 构造处（token→payload 转换）、`ASTOptimize` 里读
`Value`/`Source` 的消费点、两个变形 API、`EmptyRegexAstNode`（Label=NONE）。

### 基础件（不存在，先补）

1. **子树深拷贝**——`(ab){2,3}` 展开需要克隆 body；配合破坏性移动可省一份；载荷共享不拷
2. **FIND 节点标签**——`RegexElementLabel` 新增 `FIND`，语义为"搜索连接"
   （替代 IR 的 `FIND_COMPLEX`，见下）；合成节点直接 `new(label, payload)`，
   无需伪造 token

### 树上规范化 pass（`OptimizeTree` 扩到六阶段）

| # | Pass | 状态 | 说明 |
| --- | --- | --- | --- |
| 0 | 清孤儿 | 已有 | 不变 |
| 1 | `FlattenAlt` + `RotateConcat` | 已有 | 不变 |
| 2 | **COUNT 展开族** | 待写 | `a+`→`CONCAT(a, a*)`、`a{1}`→`a`、`a{m,n}`→前缀展开+余数；产物是右脊 CONCAT，天然归一 |
| 3 | `MergeAlt` | 已有 | 不变 |
| 4 | `MergeConcat` | 已有 | 顺带吞掉 phase2 克隆出的相邻字面量 |
| 5 | **`FirstAtom`/`LastAtom` 综合** | 待写 | 一趟自底向上；GROUP 透传、COUNT 不透传、ALT 不一致则 null。必须在所有结构重写之后、字面量合并之后（锚要极大） |
| 6 | **FIND 融合** | 待写 | 后序遍历，`CONCAT(左.LastAtom==dot-star, 右.FirstAtom==字面量)` → 原地替换为 FIND 节点；覆盖有锚/穿 GROUP/无锚吞尾三种形态 |

FIND 节点语义契约（继承自 IR 版 FIND+PACK 协议）：
候选按优先级排序（FURTHEST 从远端往前退 / NEAREST 从近端往后进），
右子树是候选验证器，候选耗尽才是真失败；
GROUP 的捕获终点落在锚起点**之前**。

### 代码生成（按节点 label 一对一）

| 节点 | 形状 |
| --- | --- |
| `UNIT` / `UNIT_SPAN` | `StartsWith` + 推进 |
| `UNIT_SET` | 列表模式 char 测试 `__rest is [var c, ..] && ...` |
| `CONCAT` | 顺序发射，失败短路 |
| `ALT` | case 链 → `if / else if / else`；复杂 case 落回滚作用域 |
| `GROUP` | 进入存 span，成功写捕获槽（PACK 职责回归 GROUP） |
| `COUNT`（叶） | 计数循环 / `IndexOfAnyExcept` 向量化 |
| `COUNT`（复杂体） | 循环包递归 body |
| `FIND` | 候选重试循环，验证器 = 右子树 |

生成约定（沿用已定结论）：

- **破坏性遍历**：谁消费谁切，下游拿到无主子树根，物理上无法偷看上下文；
  展开族用移动代替克隆省一份拷贝；生成后 `TextRegexSource.Ast` 是残骸，置空或立 flag
- **不变式契约**：进入时游标就位；离开时 `__ok` 必被赋值，
  成功则游标已推进、失败则游标复位
- **剩余 span 写法**：游标是 `ReadOnlySpan<char> __rest`，绝对偏移按需反算
- `do{}while(false)` + `break` 只在真需要回滚的节点出现（ALT 复杂 case、
  COUNT 复杂体、FIND 重试），不再每 region 强制
- 变量命名与槽位由 `SourceGenContext` 独有（该类保留，删掉 `RegionExtent` 即可）

## 当前编译错误（预期内，待重写）

| 文件 | 错误原因 | 处置 |
| --- | --- | --- |
| `RegexAnalyzer.cs` | `TextRegexSource` 记录携带 `List<RegexIR>` 字段，`Analyze` 调 `GenerateIR`/`ScanAndOptimizeIR`/`EmitPrefix` | 砍掉 IR 字段与三个调用，管线止于 `OptimizeTree` |
| `TextRegexSourceGen.GenClass.cs` | `GenerateSource` 读 `source.IRs`，`GenerateBlock`/`GenerateRegion`/`MeasureRegionExtents`/`FindRegionEnd` 全是 IR 机器 | 重写为读 `source.Ast` 的节点递归；保留 `FormatStringLiteral`/`FormatCharLiteral`；charset 解析迁入 `CharSetPayload` 转换 |

节点重设计落地时，`ASTGenerate`/`ASTOptimize` 会新增一批预期内错误（`Source`/`Value` 消费点），同批修复。

## `.old` 文件清单与参考价值

| 文件 | 参考价值 |
| --- | --- |
| `RegexIR.cs.old`、`RegexIR.Abstraction.cs.old`、`RegexIR.Prefix.cs.old`、`RegexIRGenerator.cs.old` | 字节码指令集定义，stepping 解释器的起点 |
| `RegexAnalyzer.IRGenerate.cs.old` | GROUP→TRY/POP/PACK 协议、COUNT 源文本解析（min/max/greedy 解析逻辑树上重写时照抄） |
| `RegexAnalyzer.IROptimize.cs.old` | 展开族与 FIND 合并的**语义定义**（树上 pass 的行为基准） |
| `RegexAnalyzer.PrefixEmit.cs.old` | region/观察/pop 语义的原始出处 |
| `TextRegexSourceGen.GenClass.SingleIR.cs.old` | L4 生成器 body（匹配逻辑可直接搬进节点生成器） |
| `TextRegexSourceGen.GenClass.MultiIR.cs.old`、`.Prefix.cs.old` | L1/L5 引导文档，FIND 重试循环与 POP 冲突的解法记录在此 |

配套文档：`future/ngra-regex-sourcegen-goldens.md` 的目标代码形状**全部继续有效**
（它记录的是生成产物的形状，与中间表示无关），
`test/SingleFile/regex.cs` 的手写样本同样有效。
`future/ngra-regex-ir.md` 的「优化策略」一节按旧认识写成，待按本文档修订。
