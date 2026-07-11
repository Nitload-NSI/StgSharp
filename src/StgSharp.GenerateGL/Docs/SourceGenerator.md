# StgSharp.GenerateGL 源生成器技术文档

## 目标与范围
- **输入**：Khronos OpenGL Registry `gl.xml`（Core Profile 为主，版本 3.3→4.6，可选扩展）。
- **输出**：分版本 partial `glContext`（`delegate*` 字段）与 `OpenGLFunction` 包装方法（调用 `Context.glXxx`），以及新增常量/枚举补全文件，写入 `StgSharp.Common\Graphics\OpenGL\Generated\`。不改动现有手写文件。
- **风格**：保持现有函数指针字段 + 薄封装方法模式；兼容现有命名和类型定义（如 `GlHandle`）。

---

## 阶段 1：输入解析（XML 读取与过滤）
- 将 `gl.xml` 作为 AdditionalFile 或 EmbeddedResource，使用 `XmlReader` 流式解析。
- 解析节点：
  - `<commands><command>`：函数名、返回类型、参数。
  - `<enums>`：常量。
  - `<feature profile="core">`：按 `version` 收录命令/常量。
  - `<extensions>`：可选扩展（ARB/KHR/EXT/NV）。
- 过滤策略：仅 Core Profile 且版本 ≥ 3.3（3.3→4.6 递进）；扩展按启用列表追加。

---

## 阶段 2：类型映射与句柄规则
- 整数：`GLenum`/`GLbitfield`→`uint`；`GLint`/`GLsizei`/`GLfixed`→`int`；`GLuint`→`uint`；`GLboolean`→`byte`（包装层可 bool→byte）。
- 浮点：`GLfloat`→`float`；`GLdouble`→`double`。
- 指针与尺寸：`void*`/`const void*`→`void*`；`GLintptr`→`nint`/`IntPtr`；`GLsizeiptr`/`size_t`→`nuint`/`UIntPtr`；`GLsync`→现有 `GLsync`/`IntPtr` 包装。
- 字符串：`const GLchar*`→`byte*`（UTF-8），包装层可另行提供 string/Span<byte> 重载。
- 调用约定：OpenGL APIENTRY 通常 stdcall（Windows）；保持与现有 `glContext` 字段一致（`delegate* unmanaged[Stdcall]` 或 `unmanaged[Cdecl]`，需与 loader 对齐）。
- 句柄识别：包含 program/shader/buffer/texture/framebuffer/renderbuffer/vertexarray/query/sampler/transformfeedback 等对象名的 `GLuint` 参数 → 包装层使用 `GlHandle`，内部传 `handle.Value`。
- glGet* 输出：保留指针签名，或另行生成 unsafe Span/stackalloc 适配器；核心先生成指针版。

---

## 阶段 3：版本分组与文件组织
- 输出目录：`StgSharp.Common\Graphics\OpenGL\Generated\`
- 每个版本生成独立 partial：
  - `glContext.v33.cs`, `glContext.v40.cs`, …, `glContext.v46.cs`：`delegate*` 字段。
  - `OpenGLFunction.v33.cs`, …：包装方法 `public void Foo(...) => Context.glFoo(...);`
- 常量/枚举：`glConst.generated.cs`（或分版本补丁），避免与现有重复，先查已有定义再生成。
- 命名空间：`StgSharp.Graphics.OpenGL`；类声明：`partial struct/partial class glContext`（按现有定义），`partial class OpenGLFunction`。

---

## 阶段 4：生成规则（函数表与包装）
- `glContext` 字段：`internal unsafe delegate*<converted_sig> glFoo;`
- `OpenGLFunction` 包装：薄封装直接转发；句柄参数用 `GlHandle`，内部传 `handle.Value`；`bool` 参数按底层 `GLboolean` 需要转换为 `byte/uint`。
- 去重：若同名字段/方法已存在，跳过或比对签名避免重复生成。

---

## 阶段 5：增量源生成器结构
- 步骤：
  1. 收集 AdditionalFiles（`gl.xml`）。
  2. 解析→建模（CommandModel, EnumModel, FeatureModel）。
  3. 按版本/profile 过滤。
  4. Emit：分文件输出（`SourceProductionContext.AddSource`）。
- 配置建议：`LangVersion=preview`（支持 `delegate* unmanaged[Stdcall]`）；仅引用必要的 XML 解析库。

---

## 阶段 6：兼容性与发布注意
- 不改动现有手写文件，仅追加 partial。
- 常量值需与现有 `glConst` 保持一致，生成前检查重复。
- 大 `gl.xml` 使用流式解析避免内存压力。
- Loader（`wglGetProcAddress/glXGetProcAddress/eglGetProcAddress`）不在本轮生成，字段调用约定需与 loader 对齐。

---

## 下一步实施建议
1. 将 `gl.xml` 添加为 AdditionalFile。
2. 搭建增量生成器骨架：Parse → Filter → Emit。
3. 先生成 3.3 子集验证编译，再扩展到 4.6。
4. 按需追加扩展（ARB/KHR），保持可配置启用列表。
