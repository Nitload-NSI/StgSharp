# VSS 架构设计文档

> **作者**：Nitload-NSI  
> **创建日期**：2026-03-17  
> **所属项目**：StgSharp 框架 / 工业上位机架构预演  
> **文档状态**：精简版草案

---

## 1. 文档定位

本文档用于说明 VSS 的整体架构思想、三层职责划分与层间关系。  
具体类型定义、命名与边界，以《VSS 三层类型基座设计草案》为准。

当前版本需要特别强调一点：  
State 层不再被描述为几个并列的中枢对象，而是对外统一表现为一个单例 `StateHost`。

---

## 2. 设计评估

把整个 State 层对外收敛为 `StateHost` 单例，是比旧描述更合理的做法。

原因有三点：

- 它明确了唯一中枢，避免旧版文档中的多个中枢概念同时被读成并列核心。
- 它明确了所有权，说明命令接入、状态抽象和服务管理都属于同一个运行时边界。
- 它更符合宿主型系统的实际组织方式，对外暴露一个主机，对内再展开结构，比暴露多个概念性中枢更稳定。

这个设计也有一个边界需要保留：  
虽然 `ServiceManager` 现在被放入 `StateHost` 的下层，但这并不意味着状态与能力本体被混为一层。  
`AppState` 仍然是状态抽象，`ServiceManager` 仍然只是服务管理结构，具体服务对象依然属于能力层。

另外，前侧命令链路只定义命令而不定义结果也是不完整的。  
如果 `FrontCommandHost` 已经区分 `StateChangeCommand` 与 `ExecuteCommand`，那么还必须存在统一的 `CommandResult`，用于把执行完成状态回传给 View。

---

## 3. 架构总览

VSS 当前建议的核心结构如下：

View / Electron / HMI / 外设 / 其他宿主  
↓  
StateHost（单例）  
├─ 上层：FrontCommandHost  
├─ 中层：AppState  
└─ 下层：ServiceManager  
↓  
Service / ServiceHost / 具体能力实例

这个结构表达的是：

1. 对外，State 层只有一个统一入口，即 `StateHost`。
2. 对内，`StateHost` 全息展开为上中下三层。
3. 服务能力通过 `ServiceManager` 被纳入统一调度，而不是直接散落在外部结构中。

其基本流程可以概括为：

1. 外部宿主产生交互或控制请求。
2. 请求进入 `StateHost`。
3. `StateHost` 的上层 `FrontCommandHost` 接收并区分 `StateChangeCommand` 与 `ExecuteCommand`。
4. `StateHost` 的中层 `AppState` 参与状态判断与状态更新。
5. 当命令涉及能力执行时，`StateHost` 的下层 `ServiceManager` 负责调度服务。
6. 命令处理完成后，`StateHost` 统一生成 `CommandResult` 回传给 View。
7. 状态变化与结果通知分别承担状态同步和执行完成通知两类职责。

---

## 4. View 层

### 4.1 定位

View 层是前侧交互来源的统一抽象。  
它不等同于传统窗口或页面，而是所有外部交互入口与宿主环境的集合。

View 层只负责两件事：

- 承接交互
- 将交互转化为命令并送入 `StateHost`

同时，View 还需要能够接收由 `StateHost` 回传的 `CommandResult`，从而知道某次执行型命令是否已经完成。

它不再承担中枢职责，也不直接持有服务调度责任。

### 4.2 基础类型

#### ViewHost

`ViewHost` 是前侧宿主的统一抽象，用于表示任何可以承接界面表现、输入来源或外部交互来源的宿主对象。

#### StateChangeCommand

`StateChangeCommand` 用于表达对系统状态的直接修改请求。

#### ExecuteCommand

`ExecuteCommand` 用于表达需要进入执行流程的命令请求。

#### CommandResult

`CommandResult` 用于表达某次前侧命令处理完成后的统一结果通知。

### 4.3 小结

View 层的职责不是承载业务逻辑，而是把复杂、多样、异构的交互来源收敛成稳定命令，然后统一送入 `StateHost`。

对于执行类命令，View 不能只依赖状态变化判断是否结束，还应通过 `CommandResult` 获得明确的完成通知。

---

## 5. State 层

### 5.1 定位

State 层是 VSS 的唯一状态与调度中枢。  
它对外统一表现为 `StateHost` 单例，对内展开为三层结构。

### 5.2 内部结构

#### StateHost

`StateHost` 是 State 层唯一对外暴露的运行时主机。

它负责：

- 接收来自外部宿主的命令
- 统一组织命令接入、状态抽象和服务管理
- 统一处理状态同步、事件广播、服务结果回流和 `CommandResult` 回传

#### 上层：FrontCommandHost

`FrontCommandHost` 是 `StateHost` 的上层，用于承接前侧命令输入。  
它属于 State 内部结构，不再被视为 View 层自己的中介中心。

在命令协议上，`FrontCommandHost` 至少要面对两类前向命令：

- `StateChangeCommand`
- `ExecuteCommand`

对应地，State 在处理完成后还需要回传统一的 `CommandResult`。

#### 中层：AppState

`AppState` 是 `StateHost` 的中层状态抽象，是全局状态树根。  
它代表状态本体，而不是状态宿主。

#### 下层：ServiceManager

`ServiceManager` 是 `StateHost` 的下层服务管理结构。  
它负责把命令向下转化为具体能力调度，并将结果回流状态体系。

#### 配套结构

State 层还包括：

- `PanelState`，用于表示局部状态单元

此外，observer 语义仍然存在，但在 `StgSharp.UserInterface` 中不再通过 `OnStateChanged` / `StateChangeObserver` 这类手写公共类型表达。

这里更准确的说法是：状态写入后的轻量 observer 逻辑，改为由源生成器围绕具体状态节点直接分布式织入。

在传统 MVVM 中，observer 往往同时承担：

- 内部逻辑回调
- VM 内部广播变化
- 通知 View 重绘
- 通知 Data / 业务执行

而在 VSS 中，这四项职责被拆开处理：

- 内部逻辑回调：由 Service 或具体业务实现承担
- VM 内部广播变化：由源生成器生成的轻量 observer hook 承担
- 通知 View 重绘：由 command/result 与同步符号链路承担
- 通知 Data / 业务执行：由 command/result 与 service 调度承担

其中 `CommandResult` 仍用于通知某次命令已经处理完成。

### 5.3 小结

State 层现在应被理解为一个统一主机，而不是多个同级中心。  
`StateHost` 对外是单例边界，对内则由 `FrontCommandHost`、`AppState` 和 `ServiceManager` 三层共同构成。

---

## 6. Service 层

### 6.1 定位

Service 层仍然是系统能力的承载层，但其管理入口已经被明确收敛到 `StateHost` 的下层 `ServiceManager`。

因此，Service 层关注的重点是：

- 被调度的具体服务对象
- 服务装载与完成事件
- 服务运行宿主与能力实例

Service 内部完成事件可以回流到 `StateHost`，但前侧最终接收到的仍应是统一的 `CommandResult`，而不是直接暴露服务内部事件。

### 6.2 基础类型

#### ServiceLoadCommand

`ServiceLoadCommand` 用于表达服务装载请求。

#### ServiceLoadAccomplishmentEvent

`ServiceLoadAccomplishmentEvent` 用于表达服务装载完成后的结果通知。

#### ServiceHost

`ServiceHost` 用于表达某一类服务实例的直接宿主或执行载体。

### 6.3 小结

`ServiceManager` 属于 State 的下层管理结构，而 `ServiceHost` 与具体服务实例才属于能力层主体。  
这样可以在统一调度和能力解耦之间保持清晰边界。

---

## 7. 层间关系

### 7.1 View → State

View 层通过命令把交互送入 `StateHost`。  
命令首先由 `FrontCommandHost` 接入，再由 `StateHost` 统一协调后续动作。  
当前前侧协议至少应包含：

- `StateChangeCommand`
- `ExecuteCommand`
- `CommandResult`

### 7.2 State 内部关系

State 内部的关系是：

- 上层 `FrontCommandHost` 负责接命令
- 中层 `AppState` 负责持状态
- 下层 `ServiceManager` 负责管服务

三者共同组成一个统一的 State 运行时主机。

### 7.3 State → Service

当命令涉及能力执行时，`StateHost` 通过下层 `ServiceManager` 调度服务对象。  
服务执行结果再统一回流 `StateHost`，由其决定状态更新与通知广播。  
对于 View 而言，某次执行是否完成，应以 `CommandResult` 为准，而不是直接依赖服务内部事件。

---

## 8. 与 MVVM 的区别

相较于 MVVM，VSS 更强调统一运行时主机，而不是围绕界面绑定建立系统中心。

主要区别在于：

- MVVM 通常以界面和 ViewModel 配合为中心，VSS 以 `StateHost` 为唯一中枢
- MVVM 更适合界面驱动型应用，VSS 更适合多输入源、多宿主、强状态调度的软件
- VSS 明确区分状态本体、命令接入和服务管理，但又把它们统一收束在同一个 State 边界内

---

## 9. 适用场景

VSS 更适合以下场景：

- 工业上位机
- 多输入源 HMI
- 仿真与控制软件
- 支持无头运行的软件
- 需要多宿主接入的系统

对于纯数据展示、轻量表单或前端页面驱动型应用，MVVM 仍可能是更直接的选择。

---

## 10. 当前建议保留的最小类型清单

### View 层

- `ViewHost`
- `StateChangeCommand`
- `ExecuteCommand`
- `CommandResult`

### State 层

- `StateHost`
- `FrontCommandHost`
- `AppState`
- `PanelState`
- `ServiceManager`

### Service 层

- `ServiceLoadCommand`
- `ServiceLoadAccomplishmentEvent`
- `ServiceHost`

---

## 11. 结论

VSS 的重点不在于先定义某一种具体界面实现，而在于先定义稳定的运行时骨架。  
在当前阶段，更准确的表达应当是：

- View 层负责交互来源
- State 层对外统一表现为 `StateHost` 单例
- `StateHost` 对内全息展开为 `FrontCommandHost`、`AppState`、`ServiceManager` 三层
- 前侧命令协议由 `StateChangeCommand`、`ExecuteCommand`、`CommandResult` 构成闭环
- Service 层负责承载被调度的具体能力对象

这一定义比旧版本更清楚地表达了唯一中枢、运行时边界和内部层次关系。