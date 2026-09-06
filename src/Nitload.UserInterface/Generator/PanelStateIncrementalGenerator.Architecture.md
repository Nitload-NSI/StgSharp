# PanelStateIncrementalGenerator 结构草案

本文档说明当前 `PanelStateIncrementalGenerator` 的设计目标：

1. 只有一个增量生成器入口点
2. 入口识别的是“类型声明”，不是单个字段
3. 类型进入统一上下文后，内部拆成三个独立分析部分
4. 三部分不互相依赖实例成员，只共享同一份中间模型

---

## 1. 为什么入口要识别类型声明

VSS 后续的生成需求不再只是“某个字段生成 enum”，而是围绕某个状态类型展开：

1. 该类型有哪些公开状态符号
2. 该类型有哪些状态变化绑定
3. 基于前两者可以推导出哪些 command handler 路由入口

因此，最合适的根单位不是单个字段，而是一个类型声明。

也就是说，入口阶段做的事情应该是：

1. 找出值得参与 VSS 生成的类型
2. 收集该类型上所有已知标记成员
3. 把这份类型级目标交给前两部分分析，再进入第三部分派生调度

---

## 2. 三部分内部分析

当前骨架把内部分析拆成三块：

1. `StateSymbols`
2. `StateChangeBindings`
3. `StateChangeCommandHandlers`

### 2.1 StateSymbols

这一部分负责状态符号层。

目标是为后续生成这些东西打基础：

1. 状态节点枚举
2. 状态项元数据
3. state 向量上的属性节点
4. 节点到属性的静态映射

### 2.2 StateChangeBindings

这一部分负责绑定层。

目标是分析：

1. 哪些状态变化需要绑定到 handler
2. 哪些声明使用了 `StateChangeBinding` 之类的标记
3. 后续需要生成怎样的静态接线代码

### 2.3 StateChangeCommandHandlers

这一部分负责调度层。

它不是一个额外的 attribute 输入层，而是前两部分完成后自动运行的派生阶段。

目标是分析：

1. 哪些状态符号和 binding 结果可以组成 command handler 路由入口
2. 后续如何生成状态变化命令到 handler 的静态路由
3. 如何在不依赖运行时字符串解释器的前提下构建调度表

---

## 3. 为什么三部分不能互相暴露实例成员

原因很简单：

1. Roslyn 生成器不是运行时 IOC 容器
2. 不应该设计成多个生成器实例互相通信
3. 也不应该让一个分析模块依赖另一个模块的内部字段状态

正确方式是：

1. 统一入口构建共享中间模型
2. 三部分都基于这份中间模型分析
3. 最终统一做诊断和发射

也就是说，共享的应该是：

1. 类型级目标
2. 已知标记成员
3. 编译上下文
4. 归一化后的分析结果

而不是：

1. 某个模块里的字典字段
2. 某个模块先运行出来的实例状态
3. 依赖执行顺序的临时对象

---

## 4. 当前骨架中的文件分工

当前相关文件如下：

1. `PanelStateIncrementalGenerator.cs`
2. `PanelStateIncrementalGenerator.Pipeline.cs`
3. `PanelStateIncrementalGenerator.Models.cs`
4. `PanelStateIncrementalGenerator.Analysis.cs`
5. `PanelStateIncrementalGenerator.StateSymbols.cs`
6. `PanelStateIncrementalGenerator.StateChangeBindings.cs`
7. `PanelStateIncrementalGenerator.StateCommandHandlers.cs`
8. `PanelStateIncrementalGenerator.Emitter.cs`
9. `PanelStateIncrementalGenerator.Diagnostics.cs`

它们各自的职责是：

1. `PanelStateIncrementalGenerator.cs`
负责总入口接线

2. `PanelStateIncrementalGenerator.Pipeline.cs`
负责从语法树中找出类型级候选，并收集已知标记成员

3. `PanelStateIncrementalGenerator.Models.cs`
负责共享中间模型和三部分分析结果模型

4. `PanelStateIncrementalGenerator.Analysis.cs`
负责协调三部分分析模块，并汇总出统一输出上下文

5. `PanelStateIncrementalGenerator.StateSymbols.cs`
负责状态符号层分析

6. `PanelStateIncrementalGenerator.StateChangeBindings.cs`
负责状态变化绑定层分析

7. `PanelStateIncrementalGenerator.StateCommandHandlers.cs`
负责基于前两部分结果派生状态变化命令处理层

8. `PanelStateIncrementalGenerator.Emitter.cs`
负责真正输出当前轮次的生成代码

9. `PanelStateIncrementalGenerator.Diagnostics.cs`
负责后续统一挂诊断定义

---

## 5. 当前状态

现在这套代码还只是骨架，业务逻辑没有真正接进去。

当前它已经具备的结构能力是：

1. 入口已经从字段级切到类型级
2. 已知标记成员会在 Pipeline 阶段被收集
3. 内部已经形成“前两部分显式分析 + 第三部分派生调度”的结构
4. 发射器已经能看到每个类型在三部分里各自命中了多少成员

也就是说，现在已经完成的是“结构立住”，还没有进入“具体生成 VSS 代码”的阶段。

---

## 6. 后续最自然的实现顺序

如果继续往下做，建议顺序是：

1. 先完善 `StateSymbols`
2. 再完善 `StateChangeBindings`
3. 最后完善 `StateChangeCommandHandlers`

原因是：

1. 状态符号层是基础设施
2. 绑定层建立在状态符号层之上
3. command handler 调度层由前两层自动派生

所以三部分虽然是并列模块，但实现优先级并不是并列的。

---

## 7. 一句话总结

当前设计不是“三个生成器共享上下文”，而是：

一个 `PanelStateIncrementalGenerator` 统一入口，识别类型声明，先完成前两部分分析，再自动进入第三部分派生调度。
