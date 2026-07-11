# NGRA `.ngra` 描述文件设计草案

## 1. 定位

**NGRA**，即 **Nitload Generalized Regulation Analysis**，是一个广义序列分析器框架。  
其实现位于StgSharp.RegularAnalysis项目/命名空间下  

NGRA 的目标不是只处理字符串正则，而是处理任意“可分类线性序列”，例如：

- 字符串；
- 字节流；
- 比特流；
- token 序列；
- AST 子节点序列；
- 游戏事件流；
- 玩家行为流；
- 二进制协议流。

`.ngra` 文件是 NGRA 接收的描述文件，用于描述如何从输入流中提取目标序列元素，以及如何通过规则递归地识别、分组、压缩出更高层的岛屿结构。

NGRA 读取 `.ngra` 文件后，会进行解析器/分析器的源生成。使用者需要根据场景补充必要的序列元素定义、分类逻辑以及树构造方法。

---

## 2. 基本设计原则

`.ngra` 文件应当保持声明式，主要负责描述：

1. 输入是什么；
2. 分析的目标元素是什么；
3. 规则如何从输入中提取目标元素；
4. 规则之间如何调用、组合、分层；
5. 分析器运行模式是什么。

`.ngra` 文件不应当承担过多语义任务，例如：

- 类型推导；
- 作用域解析；
- 复杂数据流分析；
- IL 生成；
- 任意 C# 代码嵌入；
- 复杂 tree builder 逻辑。

这些应当交给 NGRA 生成器外部的使用者代码完成。

---

## 3. `.ngra` 文件结构

一个 `.ngra` 文件由两部分组成：

1. 文件头部指令；
2. NGRAP 表达式集合。

示例：

```ngra
#input string
#element token
#import std_text.ngra
#run scanning

Identifier := Regex(/[ \t]*(?<id>[A-Za-z_][A-Za-z0-9_]*)/, id);
Number     := Regex(/[ \t]*(?<num>[0-9]+)/, num);
NewLine    := Regex/(?<nl>\r\n|\n|\r/, nl);

IfExpression :=
    <r"if">
    <r"(">
    (?<condition>: <Expression>)
    <r")">
    (?<then>: <Block>)
;
```

---

## 4. 文件头部指令

### 4.1 `#input`

声明输入流类型。

```ngra
#input string
#input byte
#input bit
#input token
#input event
#input custom
```

含义：

| 指令 | 含义 |
| --- | --- |
| `#input string` | 输入为字符串/文本流 |
| `#input byte` | 输入为字节流 |
| `#input bit` | 输入为比特流 |
| `#input token` | 输入为已有 token 序列 |
| `#input event` | 输入为事件流 |
| `#input custom` | 输入为用户自定义序列 |

`#input` 会影响哪些内联函数可用。

例如：

- `#input string` 下可用 `Regex(...)`、`Char(...)`；
- `#input byte` 下可用 `Byte(...)`；
- `#input bit` 下可用 `Bit(...)`；
- `#input token` / `#input event` 通常不使用原始提取函数，而直接使用 `<...>` 规则匹配。

---

### 4.2 `#element`

声明规则左值对应的目标元素类型。

```ngra
#element token
#element syntax
#element event
#element custom
```

示例：

```ngra
#input string
#element token
```

表示：

- 输入是字符串；
- `.ngra` 规则主要生成和处理 token 类型的目标元素。

又如：

```ngra
#input event
#element event
```

表示：

- 输入是事件流；
- 规则处理的基本单元就是事件元素。

`#element` 可对应具体宿主语言类型，例如 C# 中的：

```csharp
SyntaxNode
PlayerEvent
TerrainUnit
```

具体映射方式由 NGRA 源生成器配置或用户代码提供。

---

### 4.3 `#import`

导入其他 `.ngra` 文件作为当前上下文。

```ngra
#import std_text.ngra
#import std_binary.ngra
#import magicspinner_common.ngra
```

被导入文件可以提供：

- 通用 token 定义；
- 通用规则；
- 公共正则片段；
- 标准库式规则；
- 共享的序列描述。

建议规则：

1. 重复 import 忽略；
2. import 循环需要检测；
3. 同名符号默认报错；
4. 后续可以考虑 namespace 或显式覆盖机制。

---

### 4.4 `#run`

声明分析器运行模式。

```ngra
#run scanning
#run stepping
```

#### `scanning`

一次性扫描模式。

```ngra
#run scanning
```

含义：

> 分析器读取完整输入，尽可能完成全部分析，产出最终结果，然后结束。

适合：

- 编译器前端；
- 源码文件解析；
- 二进制文件解析；
- 静态日志分析；
- 一次性数据处理。

#### `stepping`

增量步进模式。

```ngra
#run stepping
```

含义：

> 分析器保持内部状态，每次接收一个或一批输入元素，执行一步或若干步分析，不自动结束。

适合：

- 游戏事件流；
- 玩家行为流；
- 网络流；
- 实时日志；
- 交互式 CLI；
- MagicSpinner 运行时施法流；
- 增量编译。

`stepping` 模式下，分析器通常需要提供类似接口：

```csharp
Feed(...)
Step()
Flush()
Reset()
Complete()
```

并支持：

- pending match；
- partial result；
- 状态保留；
- 用户显式结束输入。

---

## 5. NGRAP 表达式

`.ngra` 文件主体由一组 NGRAP 表达式组成。

每条 NGRAP 表达式的根操作是赋值：

```ngra
Symbol := Pattern;
```

其中：

- 左值 `Symbol` 必须是单个符号；
- 左值表示一个目标元素或目标元素序列；
- 右值 `Pattern` 是得到该目标元素/序列的规则描述；
- 左值符号可以在其他右值中被调用；
- 这种调用可以递归发生；
- NGRA 通过分层调用和岛屿压缩生成分析结构。

示例：

```ngra
Identifier := Regex(/[A-Za-z_][A-Za-z0-9_]*/);

Expression :=
      <Identifier>
    | <Number>
    | <String>
    | <IfExpression>
;

IfExpression :=
    <r"if">
    <r"(">
    (?<condition>: <Expression>)
    <r")">
    (?<then>: <Block>)
;
```

---

## 6. 单条 NGRAP 表达式与整体 `.ngra` 的关系

一条 NGRAP 表达式的右值可以视为一种增强型正则表达式。

它支持：

- 顺序拼接；
- 选择；
- 分组；
- 捕获；
- 计数；
- 条件；
- 局部规则调用；
- 类型化序列元素匹配。

但整个 `.ngra` 文件不是一条正则表达式，而是一个规则集合。

可以概括为：

> 单条规则是增强型序列正则；  
> 整个 `.ngra` 文件是分层序列压缩规则系统。

或者：

> Rule-level regex, system-level grammar.

---

## 7. 调用算符 `<...>`

### 7.1 普通规则调用

```ngra
<Identifier>
<Expression>
<IfExpression>
```

表示调用已有规则或匹配已有目标元素类型。

例如：

```ngra
Expression :=
      <Identifier>
    | <Number>
    | <IfExpression>
;
```

### 7.2 带值匹配简写

```ngra
<Identifier "if">
<Token "(">
<Symbol "{">
```

表示匹配某个类型并要求其 `value` 属性满足给定字面量。

这是一种常用简写，等价于：

```ngra
<Identifier>{value="if"}
<Token>{value="("}
<Symbol>{value="{"}
```

例如：

```ngra
IfExpression :=
    <Identifier "if">
    <Symbol "(">
    <Expression>
    <Symbol ")">
    <Block>
;
```

### 7.3 带属性约束匹配

```ngra
<Token>{kind=Identifier}
<Token>{value="if"}
<Height>{value < 500}
<Event>{type="Damage" && amount > 50}
```

表示先调用某个目标元素或规则，再对该元素属性施加约束。

约束写在 `{...}` 中，约定如下：

- 元素属性使用裸属性名，例如 `value`、`kind`、`type`；
- NGRA 匹配上下文仍保留 `@count`、`@size` 这类 `@...` 名称；
- `<T "...">` 继续保留，作为 `<T>{value="..."}` 的简写。

---

## 8. 匿名局部正则调用 `<r"...">`

为了避免过度声明语法胶水，可以使用：

```ngra
<r"if">
<r"(">
<r")">
<r"{">
<r"}">
<r",">
<r"(if|when|unless)">
<r"[A-Za-z_][A-Za-z0-9_]*">
```

`<r"...">` 表示：

> 使用局部正则进行拼接匹配，但该片段默认 discard，不作为后续语义分析中的显式目标元素。

在当前输入类型支持正则的前提下，`<r"...">` 内允许书写任何可用的正则表达式，而不只限字面量。
由于其匹配结果默认 discard，因此不提供类似 `Regex(expr, group)` 的局部取组语义。

示例：

```ngra
IfExpression :=
    <r"if">
    <r"(">
    (?<condition>: <Expression>)
    <r")">
    (?<then>: <Block>)
;
```

`<r"...">` 适合表示：

- 关键字和字面量胶水；
- 括号；
- 逗号；
- 分号；
- 冒号；
- 局部正则片段；
- 其他纯语法胶水。

它应当参与 source span 与 diagnostics，但默认不成为 AST 子节点。

---

## 9. 捕获组

建议使用接近 PCRE / .NET Regex 的命名捕获形式：

```ngra
(?<name>: pattern)
```

示例：

```ngra
IfExpression :=
    <r"if">
    <r"(">
    (?<condition>: <Expression>)
    <r")">
    (?<then>: <Block>)
    (<r"else"> (?<else>: <Block>))?
;
```

捕获引用使用 `$name`：

```ngra
LengthPrefixed :=
    (?<len>: <Byte>)
    <Byte>{=$len.value}
;
```

命名空间建议：

| 类型 | 示例 |
| --- | --- |
| 捕获定义 | `(?<name>: pattern)` |
| 捕获引用 | `$name` |
| NGRA 元属性 | `@count`, `@size`, `@line`, `@column` |
| 元素属性 | `value`, `kind`, `type` |

不建议使用 `@name:` 作为捕获语法，以免和 `@count` 等元属性混淆。

---

## 10. 内联函数

`.ngra` 内联函数应保持克制，主要分为两类：

1. 输入提取函数；
2. 规则定义辅助函数。

目前建议首发保留少量输入提取函数和少量正则组合函数。

---

### 10.1 `Regex(expr)`

从字符串输入中提取匹配正则的目标元素。

```ngra
Identifier := Regex(/[A-Za-z_][A-Za-z0-9_]*/);
Number     := Regex(/[0-9]+/);
```

要求：

- 输入必须是字符串或兼容文本流；
- 匹配应当锚定在当前位置；
- 匹配失败不消费输入。

---

### 10.2 `Regex(expr, group)`

从字符串输入中匹配完整正则，但只把指定捕获组赋值给左值。

```ngra
Identifier := Regex(/[ \t]*(?<id>[A-Za-z_][A-Za-z0-9_]*)/, id);
```

语义：

1. 整个 `expr` 负责匹配和消费输入；
2. 只有 `group` 指定的捕获组用于生成左值元素；
3. group 外的内容不进入主序列；
4. group 外内容可以视实现保留为 trivia/span 信息；
5. 匹配必须从当前位置开始。

这使得 `.ngra` 可以局部决定哪些空白有用、哪些空白只是分隔符。

例如：

```ngra
Identifier := Regex(/[ \t]*(?<id>[A-Za-z_][A-Za-z0-9_]*)/, id);
NewLine    := Regex/(?<nl>\r\n|\n|\r/, nl);
IndentRaw  := Regex/(?m)^(?<indent>[ \t]+)/, indent);
```

这样普通水平空白可以被 identifier 消费掉，但换行和行首缩进仍然可以作为有意义元素保留。

这对于 Python、YAML、Makefile、Markdown 等空白敏感语言很重要。

---

### 10.3 `Byte(int)`

从字节流中读取指定数量的字节。

```ngra
Magic  := Byte(4);
Height := Byte(2);
```

要求：

```ngra
#input byte
```

或兼容字节流输入。

---

### 10.4 `Bit(int)`

从比特流中读取指定数量的比特。

```ngra
Flag := Bit(1);
Mode := Bit(3);
```

要求：

```ngra
#input bit
```

或兼容比特流输入。

---

### 10.5 `Char(int)`

根据字符集规则读取指定数量的字符。

```ngra
OneChar := Char(1);
FourChars := Char(4);
```

要求：

```ngra
#input string
```

或兼容文本输入。

注意：

- 需要明确字符单位；
- 在 C# 中 `char` 是 UTF-16 code unit；
- 后续可以考虑是否需要 `Rune(int)` 表示 Unicode scalar value。

---

### 10.6 `Append(expr1, expr2, ...)`

按顺序拼接多个正则片段，等价于正则级串接。

```ngra
Identifier := Regex(
    Append(/[ \t]*/, /(?<id>[A-Za-z_][A-Za-z0-9_]*)/),
    id
);
```

适合把较长的正则拆成多个可读片段，而不是把所有内容压在一个表达式里。

---

### 10.7 `Cases(expr1, expr2, ...)`

在多个正则片段之间做分支选择，等价于正则级 `|`。

```ngra
NewLine := Regex(Cases(/\r\n/, /\n/, /\r/));
```

适合把若干候选形式显式列出，避免在单个正则中写过长的分支链。

---

### 10.8 `Count(expr, count)` / `Count(expr, min, max)`

对正则片段施加重复次数约束，等价于正则级 `{n}` 或 `{min,max}`。

```ngra
HexByte := Regex(Count(/[0-9A-Fa-f]/, 2));
Indent  := Regex(Count(/[ \t]/, 1, 4));
```

这些函数用于构造传给 `Regex(...)` 的正则表达式本身，不引入宿主语言动作语义。

---

## 11. 空白处理策略

不建议默认引入类似：

```ngra
Whitespace := Regex(/\s+/) -> skip;
```

因为：

1. `-> skip` 类似 lexer action，会污染纯声明式设计；
2. 全局丢弃空白过于粗暴；
3. 无法很好支持缩进敏感语言；
4. 容易导致后续扩展成任意动作系统。

推荐使用：

```ngra
Regex(expr, group)
```

通过 group 外部内容控制哪些输入被消费但不产出。

例如：

```ngra
Identifier := Regex(/[ \t]*(?<id>[A-Za-z_][A-Za-z0-9_]*)/, id);
Number     := Regex(/[ \t]*(?<num>[0-9]+)/, num);
```

这表示：

- 水平空白被消费；
- 只有 `id` / `num` 捕获组成为 token；
- 换行可以单独声明为有意义 token。

如果后续确实需要全局忽略机制，可以另行考虑：

```ngra
#ignore Whitespace
#trivia Comment
```

但这不应作为首要机制。

---

## 12. 条件表达式与属性谓词

`<...>` 调用算符后的 `{...}` 应支持简单条件表达式。

示例：

```ngra
Plain := <Height>{value < 500}+;
```

表示匹配连续的高度小于 500 的 `Height` 元素。

可以支持：

```ngra
<Token>{kind=Identifier}
<Token>{value="if"}
<Height>{value < 500}
<Event>{type="Damage" && amount > 50}
```

建议支持的小型表达式能力：

- 字面量；
- 属性访问；
- 比较；
- 布尔运算；
- 简单算术；
- 捕获引用；
- 有限纯函数。

不建议支持任意内嵌 C#。

复杂逻辑应通过用户注册命名谓词或后续 semantic pass 实现。

---

## 13. 基本属性

NGRA 应内建一组 match context 基本属性。

### 13.1 `@count`

当前匹配片段中的序列元素数量。

```ngra
Plain :=
    <Height>{value < 500}+
    where @count >= 8;
```

### 13.2 `@size`

当前匹配片段覆盖的字节长度。

```ngra
Packet :=
    <Header>
    <Payload>
    where @size <= 4096;
```

如果当前输入不提供 byte span，则该属性不可用或需要由用户序列适配器提供。

### 13.3 `@line`

当前匹配起始位置所在行。

```ngra
TopLevelOnly :=
    <Statement>
    where @line == 1;
```

如果设置为换行符无关序列，则 `@line` 可恒定为 `1`。

### 13.4 `@column`

当前匹配起始位置相对于行首的偏移量。

```ngra
Indented :=
    <Statement>
    where @column > 1;
```

对于无行列概念的输入，可以不可用，或由输入适配器定义。

建议：

- `@...` 命名空间保留给 NGRA match context；
- 用户元素属性使用裸属性名，并写在 `<...>{...}` 内；
- 捕获变量使用 `$name`。

---

## 14. 岛屿语法与分层压缩

NGRA 的核心机制之一是：

> 规则匹配成功后，可以将匹配到的序列片段压缩成左值类型的岛屿元素，供更高层规则继续分析。

例如：

```ngra
IfExpression :=
    <r"if">
    <r"(">
    (?<condition>: <Expression>)
    <r")">
    (?<then>: <Block>)
;
```

匹配成功后，可以生成：

```text
<IfExpression>
```

上层规则不必再关心其内部 token 细节。

这使得 NGRA 可以形成分层分析：

```text
raw string
  -> token sequence
  -> expression islands
  -> statement islands
  -> block/function/module islands
  -> AST / domain nodes
```

岛屿语法的优势：

1. 局部错误不影响全局；
2. 适合增量编译；
3. 同层岛屿可并行分析；
4. 支持先壳后体；
5. 有助于解决声明顺序问题；
6. 可以用于代码、二进制流、事件流等多种序列。

---

## 15. 递归调用与预压缩

`.ngra` 规则允许递归调用。

示例：

```ngra
Expression :=
      <Literal>
    | <CallExpression>
    | <IfExpression>
;

IfExpression :=
    <r"if">
    <r"(">
    (?<condition>: <Expression>)
    <r")">
    (?<then>: <Block>)
;
```

但为了避免卡死在递归识别中，NGRA 在编译 `.ngra` 文件时应当进行规则图分析。

需要检测：

- 递归调用；
- 左递归；
- 空匹配递归；
- 强连通规则组；
- 是否存在明确边界；
- 是否需要预压缩；
- 是否可以并行分析。

核心策略：

> 对被调用序列预先进行边界识别和压缩，使上层规则不必纠结内部细节。

例如：

```ngra
Block := <r"{"> ... <r"}">;
```

可以先识别 `{ ... }` 的边界并生成 `Block` island，再进入内部解析 statements。

---

## 16. Tree Builder 与用户代码

NGRA 可以提供默认树节点类型，例如：

```csharp
NgraNode
NgraToken
NgraMatch
NgraSpan
```

但实际使用中，用户通常需要根据领域定义自己的节点。

例如 MagicSpinner 可能需要：

```csharp
MagicSpinnerToken
MagicSpinnerSyntaxNode
SpellDeclarationNode
IfExpressionNode
VisionTagNode
```

因此 `.ngra` 文件只描述匹配和捕获，不应强行规定所有 AST 构造细节。

可选设计：

```ngra
IfExpression :=
    <r"if">
    <r"(">
    (?<condition>: <Expression>)
    <r")">
    (?<then>: <Block>)
;
```

生成器产生匹配结果后，调用用户 tree builder：

```csharp
BuildIfExpression(NgraMatch match)
```

如果用户不提供 builder，则使用默认 generic node：

```text
GenericNode {
    Type = "IfExpression",
    Captures = {
        condition,
        then
    },
    Span = ...
}
```

---

## 17. 与 ANTLR 的关系

ANTLR 的强项是：

- 单个 `.g4` 文件即可定义 lexer/parser；
- 工具链成熟；
- 生成 visitor/listener；
- 非常适合传统编译器前端。

NGRA 不应当在 ANTLR 最强的传统 parser generator 领域硬碰硬。

NGRA 的核心优势应当是：

- 输入序列泛化；
- 不局限于字符/token；
- 支持任意可分类线性序列；
- 支持岛屿压缩；
- 支持分层并行分析；
- 支持事件流、二进制流、AST 节点流；
- 规则右值使用增强型正则表达式风格；
- `scanning` / `stepping` 双运行模式。

可以概括为：

> ANTLR 是传统语言前端生成器；  
> NGRA 是广义序列分析器源生成框架。

此外，`.ngra` 文件的解释器/解析器本身应当使用 NGRA 构建。
也就是说，NGRA 的实现路线默认采用自举：先落地最小可运行核心，再用 NGRA 描述 `.ngra` 自身语法并逐步替换过渡实现。
这可以保证语法设计、规则执行模型和解释器实现始终保持同一种表达体系。

---

## 18. 运行模式示例

### 18.1 scanning 模式

```ngra
#input string
#element token
#run scanning

Identifier := Regex(/[ \t]*(?<id>[A-Za-z_][A-Za-z0-9_]*)/, id);
Number     := Regex(/[ \t]*(?<num>[0-9]+)/, num);

Expression :=
      <Identifier>
    | <Number>
;
```

适合一次性输入：

```text
source.ngra input -> complete output
```

---

### 18.2 stepping 模式

```ngra
#input event
#element event
#run stepping

DangerPattern :=
    <DamageTaken>{amount > 50}
    <HealthBelow>{value < 20}
    <EnemyNearby>+
;
```

适合持续输入：

```text
event1 -> analyzer state
event2 -> analyzer state
event3 -> partial/full match
...
```

分析器不会自动关闭，直到用户显式结束。

---

## 19. 当前设计小结

当前 `.ngra` 设计可以概括为：

```text
.ngra file
  = header directives
  + NGRAP assignment expressions
```

头部指令：

```ngra
#input string|byte|bit|token|event|custom_name
#element token|syntax|event|custom_name
#import other.ngra
#run scanning|stepping
```

规则形式：

```ngra
Symbol := Pattern;
```

核心机制：

- 左值是目标元素或目标元素序列；
- 右值是增强型序列正则；
- `Regex(expr, group)` 支持消费输入但只产出指定组；
- `<...>` 调用目标元素或规则，`<T "...">` 是 `<T>{value="..."}` 的简写；
- `<...>{...}` 对元素属性施加约束；
- `<r"...">` 可直接书写完整可用正则，且默认 discard；
- `(?<name>: pattern)` 定义捕获组；
- `$name` 引用捕获；
- `Append`、`Cases`、`Count` 用于组合正则表达式；
- `@count`、`@size`、`@line`、`@column` 提供匹配上下文；
- 规则调用可递归；
- NGRA 通过预压缩和岛屿分层避免递归卡死；
- `scanning` 用于一次性分析；
- `stepping` 用于持续事件流分析；
- `.ngra` 解释器本身采用 NGRA 自举实现；
- 树构造由默认节点或用户 tree builder 完成。

---

## 20. 后续待定问题

以下内容可以后续继续设计：

1. 是否需要 `#start` 指令；
2. 是否需要 `#namespace`；
3. 是否需要 `#output`；
4. 是否需要 `#builder`；
5. 是否保留 `#ignore` / `#trivia`；
6. `<r"...">` 与 `Regex(...)` 共享哪一套正则方言与 flags 语义；
7. `Regex(expr, group)` 中 group 是否支持数字组；
8. `@offset`、`@endLine`、`@endColumn` 是否内建；
9. 用户自定义内联函数的注册方式；
10. 用户自定义谓词的纯度声明；
11. stepping 模式的窗口和缓存策略；
12. import 的 namespace 与符号冲突策略；
13. 默认 GenericNode 的具体结构；
14. tree builder 的源生成接口形式；
15. 自举阶段的 core grammar 与完整 grammar 应如何分层演化。

---

## 22. 一句话定义

NGRA 是一个广义序列分析器源生成框架；`.ngra` 文件通过头部指令声明分析环境，  
通过 `Symbol := Pattern;` 形式的 NGRAP 表达式描述  
如何从输入序列中提取、匹配、捕获、约束并压缩目标元素，  
最终生成可用于源码、二进制流、事件流或任意可分类线性序列的分析器。
