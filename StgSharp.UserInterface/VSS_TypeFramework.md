# VSS 三层类型基座设计草案

## 目录

### 总览
- [1. 设计目标](#1-设计目标)
- [2. 整体结构概览](#2-整体结构概览)
- [3. View 层类型基座](#3-view-层类型基座)
- [4. State 层类型基座](#4-state-层类型基座)
- [5. Service 层类型基座](#5-service-层类型基座)
- [6. 三层之间的关系](#6-三层之间的关系)
- [7. 当前建议保留的最小类型清单](#7-当前建议保留的最小类型清单)
- [8. 后续扩展建议](#8-后续扩展建议)
- [9. 结论](#9-结论)

### 已明确类型

#### View 层
- [ViewHost](#321-viewhost)
- [StateChangeCommand](#322-statechangecommand)
- [ExecuteCommand](#323-executecommand)
- [CommandResult](#324-commandresult)

#### State 层
- [StateHost](#421-statehost)
- [FrontCommandHost](#422-frontcommandhost)
- [AppState](#423-appstate)
- [PanelState](#424-panelstate)
- [ServiceManager](#425-servicemanager)

#### Service 层
- [ServiceLoadCommand](#521-serviceloadcommand)
- [ServiceLoadAccomplishmentEvent](#522-serviceloadaccomplishmentevent)
- [ServiceHost](#523-servicehost)

## 1. 设计目标

VSS 面向的是多宿主、多运行环境、多交互来源的系统，不应将整体架构绑定到单一 GUI 框架、单一线程模型或单一输入方式上。  
因此，VSS 的基础设计不以传统 ViewModel-View 对应关系为核心，而以统一状态中枢为运行时骨架。

在当前版本中，VSS 仍然保留 View、State、Service 三种职责域，但其运行时组织方式需要进一步明确：

- View 层负责承接外部交互来源
- State 层负责作为唯一状态与调度中枢
- Service 层负责承载系统能力与执行对象

其中，State 层不再被描述为若干并列对象的松散组合，而是对外统一表现为一个单例运行时主机：`StateHost`。

---

## 2. 整体结构概览

VSS 当前建议的核心运行结构如下：

View / Electron / HMI / 外设 / 其他宿主  
↓  
StateHost（单例）  
├─ 上层：FrontCommandHost  
├─ 中层：AppState  
└─ 下层：ServiceManager  
↓  
Service / ServiceHost / 具体能力实例

这个结构表达的是两层含义：

1. 对外，整个 State 层只暴露一个统一中枢，即 `StateHost`。
2. 对内，`StateHost` 全息展开为上中下三层：上层接命令，中层持状态，下层管服务。

其基本运行逻辑如下：

1. 外部宿主、界面、手柄、HMI 或进程外系统发出命令。
2. 命令进入 `StateHost` 的上层，即 `FrontCommandHost`。
3. `FrontCommandHost` 完成命令接入、规范化与前置转发。
4. `StateHost` 的中层 `AppState` 作为全局状态抽象参与状态判断与状态更新。
5. 当命令涉及能力执行时，`StateHost` 的下层 `ServiceManager` 负责服务管理与调度。
6. 服务结果回流 `StateHost`，再统一同步到状态体系与观察者体系。

---

# 3. View 层类型基座

## 3.1 设计定位

View 层表示前侧宿主与交互来源，而不是具体控件集合。  
它只负责承接输入、形成命令，并与 `StateHost` 建立稳定的交互接口。

因此，View 层不承担系统中枢职责，也不直接管理服务。

---

## 3.2 View 层基础类型

### 3.2.1 `ViewHost`

#### 定义
`ViewHost` 是 VSS 对前侧宿主的统一抽象，用于表示任何可以承接界面表现、输入来源或外部交互来源的宿主对象。

#### 职责
- 承载某一类前端或外部交互上下文；
- 接收宿主内产生的交互行为；
- 将交互行为转为标准命令；
- 将命令提交给 `StateHost`；
- 接收由 `StateHost` 回传的 `CommandResult`；
- 在必要时订阅状态变化并做宿主内部同步。

#### 说明
`ViewHost` 不是具体 UI 控件，也不是具体页面对象。  
它更偏向交互宿主，例如图形界面宿主、HMI 宿主、外设宿主或远程宿主。

---

### 3.2.2 `StateChangeCommand`

#### 定义
`StateChangeCommand` 用于表达对系统状态的直接修改请求。

#### 职责
- 描述某项状态应当如何被修改；
- 携带状态修改目标与修改参数；
- 由 `StateHost` 接收并执行；
- 执行后触发状态变更通知。

#### 说明
这类命令本质上是确定性状态修改命令，适合用于审计、回放和状态同步。

对于高频、值类型占比高的状态节点，不应长期停留在 `object` 中继模型上。

更合理的做法是：

- `StateChangeCommand` 继续承担统一路由 envelope；
- 具体节点更新优先落到类型化的 `StateValueCommand<TValue>`；
- 由源生成器直接生成对应的 typed handler，避免在热点路径中反复装箱、拆箱和类型判断。

如果一次命令需要协调多个状态节点的原子更新，则不应继续复用单节点 `StateChangeCommand`，而应单独设计“向量命令”家族，将批量节点写入、冲突检测、原子性与回滚语义显式建模。

---

### 3.2.3 `ExecuteCommand`

#### 定义
`ExecuteCommand` 用于表达需要进入执行流程的命令请求。

#### 职责
- 表达需要被执行而不是被直接改写为状态的动作；
- 以当前状态为上下文触发流程、能力调用或服务调度；
- 由 `StateHost` 判断是否需要下传到 `ServiceManager`；
- 在执行结束后对应产生一个 `CommandResult`。

#### 说明
这类命令更接近执行请求，而不是单一字段修改。

---

### 3.2.4 `CommandResult`

#### 定义
`CommandResult` 用于表达某次前侧命令处理完成后的统一结果通知。

#### 职责
- 作为 `StateHost` 回传给 `ViewHost` 的统一结果对象；
- 通知某次命令是否已经处理完成；
- 描述执行成功、失败、拒绝或中止等结果状态；
- 在必要时携带消息、结果数据或关联标识。

#### 说明
如果没有 `CommandResult`，View 只能看到状态变化，却无法明确知道某次命令是否已经结束。  
因此，`CommandResult` 应被视为前侧命令协议中必需的回向对象。

---

## 3.3 View 层总结

View 层的基础作用只有三点：

1. 提供统一的前侧宿主抽象；
2. 将复杂交互来源统一转换为命令；
3. 将命令稳定送入 `StateHost`。

当前基础类型包括：

- `ViewHost`
- `StateChangeCommand`
- `ExecuteCommand`
- `CommandResult`

---

# 4. State 层类型基座

## 4.1 设计定位

State 层是 VSS 的唯一状态与调度中枢。  
它对外统一表现为 `StateHost` 单例，而不是多个并列对象分别对外承担中枢职责。

`StateHost` 的内部结构分为三层：

- 上层：`FrontCommandHost`
- 中层：`AppState`
- 下层：`ServiceManager`

因此，State 层既是统一入口，也是统一协调边界。

---

## 4.2 State 层基础类型

### 4.2.1 `StateHost`

#### 定义
`StateHost` 是 VSS 对外暴露的唯一状态主机，建议以单例形式存在。

#### 职责
- 作为整个 State 层的唯一外观；
- 对外承接来自 `ViewHost` 或其他入口的命令；
- 对内组织 `FrontCommandHost`、`AppState` 与 `ServiceManager`；
- 统一管理状态判断、状态同步、服务调度与结果回流；
- 作为状态变化通知与观察者体系的汇聚点。

#### 说明
`StateHost` 是运行时主机概念，不等价于某一个状态对象。  
它的价值在于为整个 State 层提供唯一边界与唯一所有权。

---

### 4.2.2 `FrontCommandHost`

#### 定义
`FrontCommandHost` 是 `StateHost` 的上层结构，用于承接前侧命令输入。

#### 职责
- 接收来自不同 `ViewHost` 的命令；
- 统一前侧命令入口；
- 区分 `StateChangeCommand` 与 `ExecuteCommand` 两类命令；
- 对命令进行初步规范化；
- 执行必要的前置校验；
- 将命令转交给 `StateHost` 的中层与下层进行后续处理；
- 参与将处理结果组织为 `CommandResult` 回传前侧。

#### 说明
`FrontCommandHost` 是 State 的上边界，不再被视为独立于 State 层之外的中介对象。

---

- 统一向前侧回传 `CommandResult`；
### 4.2.3 `AppState`

#### 定义
`AppState` 是 `StateHost` 中层所持有的全局状态抽象，是 VSS 的唯一可信状态树根。

#### 职责
- 聚合多个 `PanelState`；
- 保存全局运行时配置；
- 保存当前会话状态；
- 保存系统共享上下文；
- 作为状态判断和状态变更的核心依据；
- 为状态广播与观察提供基础数据。

#### 说明
`AppState` 代表状态本体，而不是状态宿主。  
它由 `StateHost` 持有和管理。

---

### 4.2.4 `PanelState`

#### 定义
`PanelState` 是面板级状态单元，用于表示某个功能域的局部状态。

#### 职责
- 保存局部运行状态；
- 作为 `AppState` 的子状态节点；
- 为不同功能模块提供独立状态边界；
- 支持局部状态的独立演进与扩展。

#### 说明
`PanelState` 使状态结构具备模块化能力，避免所有字段直接堆叠在 `AppState` 根节点上。

如果沿用状态机术语，更准确的说法应当是：VSS 当前更接近“向量状态存储 + 受控变更路由”，而不是经典有限状态机。

这里的典型映射关系是：

- 状态节点值对应状态向量的各分量；
- `CanApply` 更接近 transition guard；
- 实际赋值是状态向量写入；
- `BroadcastChange` / observer hook 更接近 transition effect；
- route 或 command path 只是进入某条受控变更管线的路由键，不等于经典状态机中的单一状态迁移边。

---

当前版本仍然保留 observer 语义，但不再保留 `OnStateChanged` 与 `StateChangeObserver` 这两个手写的公开类型。

在 `StgSharp.UserInterface` 中，状态写入后的前置校验、实际赋值与后置 observer 联动，统一改为由源生成器围绕具体状态节点分布式生成。

在传统 MVVM 中，observer 往往同时承担：

- 内部逻辑回调
- VM 内部广播变化
- 通知 View 重绘
- 通知 Data / 业务执行

而在 VSS 中，这四项职责被拆开后分别落到不同位置：

- 内部逻辑回调：由 Service 或具体业务实现承担
- VM 内部广播变化：由源生成器织入的轻量 observer hook 承担
- 通知 View 重绘：由 command/result 与同步符号链路承担
- 通知 Data / 业务执行：由 command/result 与 service 调度承担

因此，observer 在 VSS 中并没有消失，只是从一个中心化、手写的公共类型，变成了围绕各状态节点分布实现的生成代码职责。

---

### 4.2.5 `ServiceManager`

#### 定义
`ServiceManager` 是 `StateHost` 的下层结构，用于管理服务体系。

#### 职责
- 管理服务注册、装载、卸载与生命周期；
- 维护服务实例与执行上下文；
- 接收 `StateHost` 下传的能力请求；
- 将服务执行结果回流给 `StateHost`；
- 作为 State 层通往具体能力层的下边界。

#### 说明
`ServiceManager` 虽然位于 `StateHost` 内部下层，但它管理的仍然是能力体系，而不是状态本体。  
这一点必须与 `AppState` 严格区分。

---

## 4.3 State 层总结

State 层应被理解为一个统一主机，而不是若干同级对象的松散拼装。  
`StateHost` 对外是单例中枢，对内全息展开为上中下三层：

- 上层负责命令接入
- 中层负责状态抽象
- 下层负责服务管理

同时，State 对前侧的完整协议应当至少包含：

- `StateChangeCommand`
- `ExecuteCommand`
- `CommandResult`

当前基础类型包括：

- `StateHost`
- `FrontCommandHost`
- `AppState`
- `PanelState`
- `ServiceManager`

此外，还存在由源生成器分布式织入的 observer 机制，用于承担轻量的状态节点内部广播，但它不再作为单独的公开基座类型出现。

---

# 5. Service 层类型基座

## 5.1 设计定位

Service 层是系统能力的承载层。  
这里的重点不再是把 `ServiceManager` 视为一个独立的外部中枢，而是将其视为 `StateHost` 下层所管理的能力入口。

因此，Service 层更关注的是被管理的服务对象、服务装载命令和服务完成事件。

---

## 5.2 Service 层基础类型

### 5.2.1 `ServiceLoadCommand`

#### 定义
`ServiceLoadCommand` 用于表达服务装载请求。

#### 职责
- 描述需要装载的服务；
- 描述装载目标与装载参数；
- 由 `StateHost` 下传给 `ServiceManager`；
- 驱动服务实例创建、初始化或激活。

---

### 5.2.2 `ServiceLoadAccomplishmentEvent`

#### 定义
`ServiceLoadAccomplishmentEvent` 用于表达服务装载动作完成后的结果通知。

#### 职责
- 通知服务是否完成装载；
- 通知装载是否成功；
- 承载失败信息或附加上下文；
- 支持结果回流到 `StateHost` 并同步状态。

---

### 5.2.3 `ServiceHost`

#### 定义
`ServiceHost` 用于表达某一类服务实例的直接宿主或执行载体。

#### 职责
- 承接某类服务的运行实例；
- 为服务提供执行上下文；
- 支持未来多宿主、多环境的服务承载模式；
- 作为 `ServiceManager` 管理的底层运行单元之一。

---

## 5.3 Service 层总结

Service 层负责真正承载系统能力。  
`ServiceManager` 属于 `StateHost` 的下层管理结构，而 `ServiceHost` 与具体服务实例才构成能力层主体。

当前基础类型包括：

- `ServiceLoadCommand`
- `ServiceLoadAccomplishmentEvent`
- `ServiceHost`

---

# 6. 三层之间的关系

## 6.1 View → State

View 层不直接管理服务，也不直接改写系统运行结构。  
其职责是通过命令将外部交互送入 `StateHost`。

即：

- `ViewHost` 产生命令；
- 命令分为 `StateChangeCommand` 与 `ExecuteCommand`；
- 命令进入 `StateHost`；
- `StateHost` 的上层 `FrontCommandHost` 负责接入和前置处理；
- 命令处理完成后由 `StateHost` 向 `ViewHost` 回传 `CommandResult`。

---

## 6.2 State 内部三层关系

`StateHost` 的内部关系如下：

- `FrontCommandHost` 负责接命令；
- `AppState` 负责持状态；
- `ServiceManager` 负责管服务。

这三者不是并列对外暴露，而是共同组成一个统一的 State 运行时主机。

---

## 6.3 State → Service

当命令涉及能力执行时，`StateHost` 通过下层 `ServiceManager` 调度具体服务对象。  
服务执行结果再统一回流 `StateHost`，由其决定状态更新与事件广播。

---

# 7. 当前建议保留的最小类型清单

## View 层
- `ViewHost`
- `StateChangeCommand`
- `ExecuteCommand`
- `CommandResult`

## State 层
- `StateHost`
- `FrontCommandHost`
- `AppState`
- `PanelState`
- `ServiceManager`

## Service 层
- `ServiceLoadCommand`
- `ServiceLoadAccomplishmentEvent`
- `ServiceHost`

---

# 8. 后续扩展建议

在当前基础类型稳定后，建议后续按以下顺序扩展：

## 第一阶段：固定唯一中枢边界
- 明确 `StateHost` 的单例角色；
- 明确 `FrontCommandHost` 是 `StateHost` 的上层；
- 明确 `AppState` 是 `StateHost` 的中层状态抽象；
- 明确 `ServiceManager` 是 `StateHost` 的下层服务管理结构。

## 第二阶段：扩展具体子类型

### View 侧
- 图形界面宿主
- HMI 宿主
- 外设宿主
- 远程宿主

### State 侧
- 更多 `PanelState` 子类型
- 更明确的状态变化事件类型
- 更完整的观察者与同步机制

### Service 侧
- 具体服务实例
- 具体 `ServiceHost` 子类型
- 更细化的服务装载与能力描述体系

## 第三阶段：补充运行时机制
- 命令上下文；
- 状态快照；
- 事件总线；
- 服务能力描述；
- 审计与回放；
- 服务结果同步协议。

---

# 9. 结论

VSS 当前不应先从具体界面技术切入，而应先定义稳定的运行时类型基座。  
在这一前提下，State 层需要被明确为唯一统一中枢，而不是若干对象之间的松散协作。

因此，当前版本更合适的表述是：

- View 层负责交互来源
- State 层对外统一表现为 `StateHost` 单例
- `StateHost` 对内全息展开为 `FrontCommandHost`、`AppState`、`ServiceManager` 三层
- 前侧命令协议由 `StateChangeCommand`、`ExecuteCommand`、`CommandResult` 共同构成
- Service 层负责承载被 `ServiceManager` 管理的具体能力对象

这一定义能更清楚地表达所有权、调度边界与运行时结构。