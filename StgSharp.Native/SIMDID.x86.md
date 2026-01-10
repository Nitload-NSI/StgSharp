# SIMDID (x86-64) 64-bit Specification (Completed per fixed layout)

This document completes the **64-bit SIMDID** bit layout as specified, with emphasis on the design of the **AVX512 Feature section ([31..16])**, so it can cover the messy AVX-512 subsets while keeping branching logic compact. It also allocates an **AVX Feature byte** adjacent to the AVX512/AMX regions for AVX-family capabilities.

> Convention: bit0 is the least significant bit (LSB). Only x86-64 interpreter semantics are defined here; the header’s Root ISA allows extension to other architectures.

---

## 1) ASCII Bit Layout (Final)

```
SIMDID (64 bits)  [63 ........................................................ 0]
+----------+-----------+----------------+----------------+----------------+----------------+----------------+
| [63..56] | [47..40]  | [39..32]       | [31..16]       | [15..8]        | [7..4] | [3..0] |
| uArchHi  | AVX Feat  | AMX Future     | AVX512 Feature | MainLevel      | Manuf | Root   |
+----------+-----------+----------------+----------------+----------------+----------------+----------------+
```

- **[3..0] Root ISA**: Root ISA type (selects interpreter)
- **[7..4] Manufacture**: Vendor info (Intel/AMD for x86)
- **[15..8] MainLevel**: Main ISA level (ascending, shared bit semantics)
- **[31..16] AVX512 Feature**: AVX-512 features/subsets/implementation form (key part)
- **[39..32] AMX Future**: AMX features (few bits for now)
- **[47..40] AVX Feature**: AVX-family capabilities (base requires AVX+AVX2; key extensions include FMA/F16C)
- **[63..56] uArchHi**: Optional microarchitecture/policy hints (default 0, no functional impact).

---

## 2) Root ISA ([3..0])

Suggested encoding (minimal usable set, expandable if needed):

| Value | Root ISA |
|---:|----------|
| 0 | Unknown |
| 1 | x86-64 |
| 2 | AArch64 |
| 3 | PPC64LE |
| 4 | Apple Silicon (distinct value as required) |
| 5 | RISC-V |
| 6 | LoongArch |
| 7..15 | Reserved |

> Note: Architecturally Apple Silicon is AArch64; a distinct value is provided here because the Root ISA layer may distinguish it. Interpreter choice is up to implementation.

---

## 3) Manufacture ([7..4])

For **x86-64**:

| Value | Manufacture |
|---:|-------------|
| 0 | Unknown/Other |
| 1 | Intel |
| 2 | AMD |
| 3 | VIA/Zhaoxin |
| 4..15 | Reserved |

For other Root ISAs, this nibble is defined by their interpreters (e.g., Apple/Qualcomm/MediaTek for ARM).

---

## 4) MainLevel ([15..8], ascending)

"Shared bits and ascending" means higher values indicate higher capability (or later scheduling order). Suggested values:

| Value | MainLevel |
|---:|-----------|
| 0 | NONE |
| 1 | SSE (full SSE family: SSE1..SSE4.2) |
| 2 | AVX2 |
| 3 | AVX2_FMA |
| 4 | AVX2_FMA_F16C (from this level onward, 16-bit types are supported) |
| 5 | AVX512 (check [31..16]) |
| 6 | AMX |
| 7 | AVX10 |
| 8..255 | Reserved |

> Scheduling suggestion: `MainLevel` is the primary switch; only when `MainLevel >= AVX512` should `AVX512 Feature` be parsed; only when `MainLevel >= AMX` should `AMX Future` be parsed. AVX Feature sits beside AMX/AVX512 and is meaningful when AVX or higher is present.

---

## 5) AVX512 Feature ([31..16]) Design Goals (Compact)

1) **Base AVX512 one-shot check**: Use 1 bit for the bundle `F + CD + VL + DQ + BW`, avoiding multiple subset checks.
2) **Glue vs native**: Use an implementation field to distinguish `AVX512-DUAL` (glue/downclock risk) from `AVX512-NATIVE` (full 512-bit ALU).
3) **Niche/AI extensions separated**: VNNI, BF16/FP16, and similar extensions each get a dedicated bit so the base path stays simple.

---

## 6) AVX512 Feature Bit Layout ([31..16] = 16 bits, compact)

AVX512 Feature (bits 31..16)

| **bit**   | 31  | 30  | 29   | 28   | 27   | 26    | 25   | 24   | 23    | 22    | 21 | 20   | 19 | 18 | 17 | 16 |
|-----------|-----|-----|------|------|------|-------|------|------|-------|-------|----|------|----|----|----|----|
| **Usage** | R   | R   | FP16 | BF16 | VNNI | VBMI2 | VBMI | R/AI | Impl1 | Impl0 | R  | BASE | R  | R  | R  | R  |

Notes:

- **BASE (bit 20)**: Base AVX512 bundle, indicating `F + CD + VL + DQ + BW` are all present. If set to 1, the detecting side must guarantee these subsets are all available.
- **AVX512_IMPL ([23..22], 2-bit)**: Implementation form (width policy).

| Value | Meaning |
|---:|------|
| 0 | Unknown (strategy decided by Manufacture/uArch) |
| 1 | AVX512-DUAL (glue/downclock risk; prefer 256-bit path) |
| 2 | AVX512-NATIVE (full 512-bit ALU; 512-bit path is usable) |
| 3 | Reserved |

- **VBMI/VBMI2/VNNI/BF16/FP16 ([29..26])**: Commonly needed niche or AI-related small-type extensions; set as needed.
- **R/AI (bit 24)**: Reserved for future AI or other non-crypto extensions.
- **[31..30], [21], [19..16]**: Reserved for future allocation.

---

## 7) AVX Feature Byte ([47..40], 8 bits)

Purpose: compact AVX-family capability encoding adjacent to AVX512/AMX. Base requirement: AVX **and** AVX2 present. Key extensions: FMA and F16C. Additional bits reserved for other extensions.

AVX Feature (bits 47..40)

| bit | 47 | 46 | 45 | 44 | 43 | 42 | 41 | 40 |
|-----|----|----|----|----|----|----|----|----|
| use | R  | R  | R  | R  | F16C | FMA | AVX2 | AVX |

Rules:
- **AVX (bit 40)**: AVX present.
- **AVX2 (bit 41)**: AVX2 present. For the AVX feature byte to be considered valid, both AVX and AVX2 must be set.
- **FMA (bit 42)**: FMA present.
- **F16C (bit 43)**: F16C present.
- **[44..47]**: Reserved for other AVX-family extensions (e.g., BMI/BMI2, IFMA, XOP) as needed.

---

## 8) "AVX512BASE" Determination (Single Check)

- **Rule**: `BASE=1` implies `F+CD+VL+DQ+BW` are all present; no need for per-subset checks. Detection must keep this consistent; if not all subsets are present, set to 0.
- **Runtime branching**:
  - If `MainLevel < AVX512`: fall back to AVX2/FMA path.
  - Otherwise:
    - Check `BASE`; if 0, fall back to AVX2/FMA (or treat as detection failure).
    - Check `AVX512_IMPL`: `DUAL` uses 256-bit/downclock-safe path, `NATIVE` uses 512-bit path.
    - When small-type/AI capabilities are required, consult `VNNI/BF16/FP16/VBMI/VBMI2` bits.

---

## 9) AMX Future ([39..32], 8 bits)

A single byte is reserved; define three common bits for now, leave the rest reserved:

| bit (relative to [39..32]) | Name | Note |
|---:|------|------|
| 0 | AMX_TILE | Tile base |
| 1 | AMX_INT8 | INT8 tile |
| 2 | AMX_BF16 | BF16 tile |
| 3..7 | Reserved | |

When `MainLevel < AMX`, this byte can be all zeros.

---

## 10) uArchHi ([63..56], 8 bits)

Purpose: optional high-byte strategy field to address issues like early Intel/AMD "glue" AVX512 that are not expressible purely via CPUID subsets. Defaults to 0.

Suggested minimal use:
- `0`: unused
- `1`: ForceDual (treat as AVX512-DUAL even if impl is unknown)
- `2`: ForceNative (treat as AVX512-NATIVE even if impl is unknown)
- `3..`: Implementation-maintained uArch enumeration (e.g., SKX/ICL/SPR/ZEN4/ZEN5…)

Practical strategy for a single maintainer:
- AMD: detection side sets `AVX512_IMPL = DUAL` directly (no uArch needed)
- Intel: enumerate only selected models of interest; otherwise Unknown follows default policy

---

## 11) Minimal Detection-Side Fill Rules (x86-64)

1) Set `RootISA = x86-64`, `Manufacture = Intel/AMD`.
2) Compute `MainLevel` (per ascending rule).
3) If `MainLevel >= AVX512`:
   - If `F+CD+VL+DQ+BW` are all present, set `BASE = 1`; otherwise `BASE = 0`.
   - Set `AVX512_IMPL`:
     - AMD: DUAL (per definition)
     - Intel: Decide DUAL/NATIVE based on family/model or policy table; if uncertain, use Unknown
   - When VNNI/BF16/FP16/VBMI/VBMI2 are detected, set the corresponding bits.
4) If `MainLevel >= AMX`: fill the AMX byte.
5) When AVX/AVX2 are present, populate the AVX Feature byte: set AVX and AVX2; set FMA and F16C when detected; leave reserved bits for future extensions.
6) Use uArchHi only when additional policy is needed; otherwise leave as 0.

---

## 12) Choice for High Bits Reservation

Currently `[31..24]` is reserved for AVX512 extensions/AI. `[47..44]` is reserved for AVX-family extensions beside the AVX byte. uArchHi `[63..56]` remains available for minimal policy hints. If IFMA or other extensions are more important, repurpose the reserved bits while keeping `BASE` and `Impl` unchanged.