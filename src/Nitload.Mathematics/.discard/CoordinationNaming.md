# Matrix Coordinate & Naming Conventions (StgSharp)

This document defines the coordinate terminology and naming rules used across `Matrix<T>`, `MatrixKernel<T>`, and packed/triangular storage layouts.  
Goal: eliminate ambiguity around `column`/`row` names (e.g., “index **of** a column” vs “index **along** a column”).

---

## 1) The key rule: encode prepositions into identifiers (`Of` vs `In`)

### `...OfX` — “which X” (object identity / ordinal)
Use **`Of`** when the value identifies **which object** (which column, which row, which kernel, etc.).

Examples:
- `columnOfElement`: the element is located in **which column**
- `rowOfKernel`: the kernel block is located in **which row**

### `...InX` / `...WithinX` — “inside X” (local coordinate)
Use **`In`** (or `Within`) when the value is a **local coordinate inside** another object.

Examples:
- `columnInKernel`: local column coordinate **inside** a kernel block
- `rowInKernel`: local row coordinate **inside** a kernel block

> Quick mental model: **`Of` = identity**, **`In` = inside**.

---

## 2) Coordinate levels: `Element`, `KernelGrid`, `InKernel`

StgSharp commonly uses three coordinate levels:

1. **Element-level**: final matrix element coordinates (`Matrix<T>[col,row]`)
2. **Kernel-grid level**: coordinates of a kernel block in the kernel grid
3. **In-kernel level**: local element coordinates within a kernel (e.g., 4×4)

Recommended names:

| Level | Column name | Row name | Meaning |
|---|---|---|---|
| Element | `columnOfElement` | `rowOfElement` | element located in which column/row (0-based) |
| Kernel grid | `columnOfKernel` | `rowOfKernel` | kernel block located in which kernel-column/kernel-row (0-based) |
| In-kernel | `columnInKernel` | `rowInKernel` | local coordinate inside the kernel (0..KernelSize-1) |

---

## 3) Index base: 0-based (mandatory)

Unless explicitly documented otherwise, all coordinates are **0-based**:

- `columnOfElement ∈ [0, ColumnLength-1]`
- `rowOfElement ∈ [0, RowLength-1]`
- `columnOfKernel ∈ [0, KernelColumnLength-1]`
- `rowOfKernel ∈ [0, KernelRowLength-1]`
- `columnInKernel ∈ [0, KernelSize-1]`
- `rowInKernel ∈ [0, KernelSize-1]`

---

## 4) Mapping element coordinates to kernel coordinates (KernelSize = 4 example)

For a fixed kernel size `KernelSize = 4`:

```csharp
int columnOfKernel = columnOfElement / 4;
int rowOfKernel    = rowOfElement    / 4;

int columnInKernel = columnOfElement % 4;
int rowInKernel    = rowOfElement    % 4;
```

If `columnOfElement` and `rowOfElement` are guaranteed non-negative, bit operations can be used:

```csharp
int columnOfKernel = columnOfElement >> 2;
int rowOfKernel    = rowOfElement    >> 2;

int columnInKernel = columnOfElement & 3;
int rowInKernel    = rowOfElement    & 3;
```

---

## 5) Packed storage: distinguish `Index` from `Offset`

Packed layouts do **not** store “empty blocks”; stored blocks are placed contiguously in a packed buffer.

### Use the correct terms
- **`packedKernelIndex`**: “the N-th kernel
