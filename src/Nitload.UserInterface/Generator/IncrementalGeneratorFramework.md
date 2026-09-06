# 增量生成器框架说明

本文档说明当前 Generator 文件夹里这套增量生成器骨架为什么这样拆，以及每一层现在承担什么职责。

这里讲的是“结构设计”，不是“最终 VSS 业务代码怎么生成”。

---

## 1. 当前结论

当前这套结构不再是“一个字段触发一个小生成器”，而是：

1. 一个统一入口生成器
2. 一个类型级候选识别管线
3. 一个共享中间模型
4. 三个内部分析模块
5. 一个统一发射器
6. 一个诊断层预留点

也就是说，设计目标已经从“节点枚举小功能”升级成“围绕一个状态类型进行多维分析”。

---

## 2. 当前文件与职责

当前 Generator 文件夹中的核心文件是：

1. `PanelStateIncrementalGenerator.cs`
2. `PanelStateIncrementalGenerator.Pipeline.cs`
3. `PanelStateIncrementalGenerator.Models.cs`
4. `PanelStateIncrementalGenerator.Analysis.cs`
5. `PanelStateIncrementalGenerator.StateSymbols.cs`
6. `PanelStateIncrementalGenerator.StateChangeBindings.cs`
7. `PanelStateIncrementalGenerator.StateCommandHandlers.cs`
8. `PanelStateIncrementalGenerator.Emitter.cs`
9. `PanelStateIncrementalGenerator.Diagnostics.cs`

它们的职责可以概括成下面这张表：

| 文件 | 角色 | 负责什么 |
| --- | --- | --- |
| `PanelStateIncrementalGenerator.cs` | 入口层 | 把 Roslyn 的增量管线接起来 |
| `PanelStateIncrementalGenerator.Pipeline.cs` | 候选管线层 | 识别类型级候选，并收集已知标记成员 |
| `PanelStateIncrementalGenerator.Models.cs` | 中间模型层 | 定义共享输入、目标、分析结果模型 |
| `PanelStateIncrementalGenerator.Analysis.cs` | 协调层 | 驱动三个内部分析模块并汇总输出 |
| `PanelStateIncrementalGenerator.StateSymbols.cs` | 分析模块 1 | 分析状态符号相关成员 |
| `PanelStateIncrementalGenerator.StateChangeBindings.cs` | 分析模块 2 | 分析状态变化绑定相关成员 |
| `PanelStateIncrementalGenerator.StateCommandHandlers.cs` | 分析模块 3 | 基于前两部分结果派生状态变化命令处理结果 |
| `PanelStateIncrementalGenerator.Emitter.cs` | 发射层 | 把分析结果输出成 `.g.cs` |
| `PanelStateIncrementalGenerator.Diagnostics.cs` | 诊断层 | 预留统一诊断定义位置 |

一句话总结现在的结构：

> 一个入口识别类型声明，进入统一模型，再在内部拆成三个并列分析部分。

---

## 3. 入口层

对应文件：

`PanelStateIncrementalGenerator.cs`

它的职责仍然很单纯：

1. 建立 SyntaxProvider
2. 收集类型级候选
3. 与 Compilation 合并
4. 调用统一分析器
5. 把结果交给发射器

它不应该承担这些事情：

1. 不应该写复杂业务规则
2. 不应该直接拼大段生成代码
3. 不应该依赖某一块具体的状态逻辑

入口层越薄，后面三部分演化时越安全。

---

## 4. Pipeline 层

对应文件：

`PanelStateIncrementalGenerator.Pipeline.cs`

这层的重点已经从“字段筛选”切换成“类型筛选”。

### 4.1 `IsCandidate`

这一步只做便宜的语法级判断。

当前策略是：

1. 只看 `TypeDeclarationSyntax`
2. 该类型内部至少存在一个带 Attribute 的字段、属性或方法

这一步允许误报，但必须便宜。

### 4.2 `Transform`

这一步开始做语义级确认。

当前逻辑是：

1. 拿到类型符号 `INamedTypeSymbol`
2. 遍历该类型成员
3. 识别已知 Attribute
4. 收集所有命中的成员
5. 构造一个类型级目标 `PanelStateIncrementalGeneratorTypeTarget`

这一步的核心意义是：

1. 根输入单位是“类型”
2. 成员只是这个类型上的分析素材
3. 后面的三部分都共享同一份素材，而不是各自重新扫一遍类型

---

## 5. Models 层

对应文件：

`PanelStateIncrementalGenerator.Models.cs`

中间模型的作用，是把 Roslyn 的语义对象整理成稳定的数据结构，供后面多个模块共享。

当前关键模型包括：

1. `PanelStateIncrementalGeneratorMarkedMember`
2. `PanelStateIncrementalGeneratorTypeTarget`
3. `PanelStateIncrementalGeneratorInput`
4. `PanelStateIncrementalGeneratorTypeAnalysis`
5. `PanelStateIncrementalGeneratorOutput`

以及三个内部分析结果：

1. `PanelStateSymbolPartAnalysis`
2. `StateChangeBindingPartAnalysis`
3. `StateChangeCommandHandlerPartAnalysis`

其中第三个结果不再对应一个额外的用户声明 attribute，而是前两部分分析完成后自动推导出来的调度结果。

这意味着现在共享的不是某个模块实例里的私有字段，而是显式模型。

这点很重要，因为源生成器里真正稳定的共享方式就是：

1. 共享模型
2. 共享编译上下文
3. 共享归一化后的分析结果

而不是模块之间互相暴露运行时对象。

---

## 6. Analysis 层

对应文件：

1. `PanelStateIncrementalGenerator.Analysis.cs`
2. `PanelStateIncrementalGenerator.StateSymbols.cs`
3. `PanelStateIncrementalGenerator.StateChangeBindings.cs`
4. `PanelStateIncrementalGenerator.StateCommandHandlers.cs`

这一层是当前结构里最关键的变化。

以前如果只做一个节点枚举生成器，那么 Pipeline 之后可以直接进 Emitter。

但现在不行，因为你已经明确有三块并列需求：

1. StateSymbols
2. StateChangeBindings
3. StateChangeCommandHandlers

所以中间必须有一个统一分析层。

它负责：

1. 对每个类型目标进行统一调度
2. 先调用前两个显式分析模块
3. 再基于前两部分结果运行第三个派生调度阶段
4. 把三个分析结果装配成一个类型级总分析结果
5. 保证最终发射器看到的是完整上下文

这就是“一入口，三部分”的真正落点。

---

## 7. Emitter 层

对应文件：

`PanelStateIncrementalGenerator.Emitter.cs`

发射器当前还是一个骨架，但它已经不再只是输出“发现了哪些字段”，而是输出：

1. 发现了哪些类型
2. 每个类型在三部分里各命中了多少成员

这类 manifest 的作用不是业务交付，而是结构校验：

1. 证明统一入口已经接通
2. 证明类型级分析结果已经被汇总
3. 证明发射器能看到三部分的完整结果

等后面要真正生成 enum、binding 路由、handler 路由时，这个文件会自然长大。

---

## 8. Diagnostics 层

对应文件：

`PanelStateIncrementalGenerator.Diagnostics.cs`

当前它依然很薄，这是正常的。

它的存在价值是为后续统一诊断留一个稳定出口，例如：

1. 标记位置不合法
2. 同一类型内声明冲突
3. 某个 handler 签名不符合规则
4. 某个 binding 指向不存在的状态路径
5. 同一路由被重复生成

真正重要的不是现在有没有很多诊断，而是不要等逻辑长大后再到处散着写诊断定义。

---

## 9. 为什么必须是“一入口三部分”

如果把这三块拆成三个彼此独立的生成器，会立刻遇到两个问题：

1. 元数据重复扫描
2. 诊断信息无法自然共享

更严重的是，多个生成器之间并不适合靠实例成员互相通信。

因此更合理的结构是：

1. 一个根入口
2. 一个共享模型
3. 三个内部分析部分
4. 一个统一发射出口

这就是现在这套结构背后的核心原则。

---

## 10. 当前阶段的定位

现在这套代码还只是“结构骨架”，不是“业务生成完成版”。

它已经完成的事情是：

1. 根入口已经统一
2. 输入粒度已经切到类型级
3. 三部分内部分析已经独立成模块
4. 发射器已经能看到完整汇总结果

还没有完成的事情是：

1. 真实的状态符号生成
2. 真实的状态变化绑定生成
3. 真实的基于前两部分推导出的状态命令路由生成
4. 真实的统一诊断规则

也就是说，当前目标不是“把功能做完”，而是先把以后不会推翻的结构搭稳。

但从工程实现看，它其实是几件不同的事情：

1. Roslyn 入口接线
2. 低成本筛候选
3. 高成本做语义确认
4. 把结果整理成稳定模型
5. 输出代码
6. 在不合法时报诊断

如果这几件事全写在一个文件里，短期看更短，长期一定更难改。

所以你可以把这套拆分理解成：

> 它不是为了“显得标准”，而是为了让后面真正加业务逻辑时，不需要每次都从一坨 Roslyn 接线代码里翻东西。

---

## 9. 一个更直观的心智模型

你可以把当前框架理解成一条流水线：

1. 入口层：把流水线接起来
2. Pipeline：把矿石挑出来
3. Models：把矿石做成标准坯料
4. Emitter：把坯料加工成成品代码
5. Diagnostics：发现废品时给出报错

如果写成一句更短的话：

1. 找目标
2. 抽模型
3. 发代码
4. 报问题

---

## 10. 你后面最可能怎么继续扩展

如果按你现在的 VSS 方向继续，这套框架下一步通常会往下面长：

### 第一步：把 Target 变得更像“节点描述”

例如不只是保存字段名，还保存：

1. 节点名
2. 所属 PanelState
3. 路径前缀
4. 可生成成员集合

### 第二步：在 Emitter 中真正生成 enum

例如：

1. 每个 PanelState 对应一个 `xxxNode` enum
2. enum 成员由被标记字段自动生成

### 第三步：继续生成路由代码

例如：

1. string path -> enum
2. enum -> setter/handler

### 第四步：再接入受控状态写入与分布式 observer hook

例如：

1. pre-change handler 调用点
2. set property
3. 由源生成器织入的轻量 post-change observer hook
4. 局部状态广播或内部同步

这里的 observer hook 只负责很薄的一层内部广播，不负责 view 重绘或业务执行。
在 VSS 中，这两部分分别由 command/result 同步链路与 service 逻辑承担。

---

## 11. 对你当前项目的最简理解

如果你觉得这套骨架有点“过工程化”，可以先只记住下面这五句话：

1. `IncrementalGenerator` 文件只是总入口，不负责细活。
2. `Pipeline` 文件负责找出哪些代码需要生成。
3. `Models` 文件负责把识别结果整理成稳定的数据结构。
4. `Emitter` 文件负责把这些数据真正输出成 `.g.cs`。
5. `Diagnostics` 文件负责以后统一放错误和警告定义。

你后面要改业务逻辑，主要会集中在 `Pipeline`、`Models`、`Emitter` 这三层。

---

## 12. 最后一句话

当前这套框架不是“生成器已经很复杂了”，而是“先把复杂度分栏放好”。

你后面真正写逻辑时，应该优先记住这个原则：

1. Pipeline 决定“找谁”
2. Models 决定“怎么表示它”
3. Emitter 决定“怎么生成它”

只要这三个边界不乱，后面的业务逻辑就算加很多，也还会比较稳。
