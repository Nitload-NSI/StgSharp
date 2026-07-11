# SIMDID (x86-64) 64-bit Specification (Completed per fixed layout)

This document completes the **64-bit SIMDID** bit layout as specified, with emphasis on the design of the **AVX512 Feature section ([31..16])**, so it can cover the messy AVX-512 subsets while keeping branching logic compact. It also allocates an **AVX Feature byte** adjacent to the AVX512/AMX regions for AVX-family capabilities, and an **AVX10 Feature byte** for the converged AVX10 ISA.

> Convention: bit0 is the least significant bit (LSB). Only x86-64 interpreter semantics are defined here; the header's Root ISA allows extension to other architectures.

---

## 1) ASCII Bit Layout (Final)

```
SIMDID (64 bits)  [63 ........................................................ 0]
+----------+-----------+-----------+----------------+----------------+----------------+--------+-------+
| [63..56] | [55..48]  | [47..40]  | [39..32]       | [31..16]       | [15..8]        | [7..4] | [3..0]|
| uArchHi  | AVX10Feat | AVX Feat  | AMX Future     | AVX512 Feature | MainLevel      | Manuf  | Root  |
+----------+-----------+-----------+----------------+----------------+----------------+--------+-------+
```

- **[3..0] Root ISA**: Root ISA type (selects interpreter)
- **[7..4] Manufacture**: Vendor info (Intel/AMD for x86)
- **[15..8] MainLevel**: Main ISA level (ascending, shared bit semantics)
- **[31..16] AVX512 Feature**: AVX-512 features/subsets/implementation form (key part)
- **[39..32] AMX Future**: AMX features (few bits for now)
- **[47..40] AVX Feature**: AVX-family capabilities (base requires AVX+AVX2; key extensions include FMA/F16C)
- **[55..48] AVX10 Feature**: AVX10 converged ISA version and vector width support
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
| 4 | RISC-V |
| 5 | LoongArch |
| 6..15 | Reserved |

> Note: Apple Silicon is architecturally AArch64 and uses `Root ISA = 2`. Apple vs Qualcomm vs other AArch64 vendors are distinguished via the **Manufacture** nibble under the AArch64 interpreter, not via a separate Root ISA value.

---

## 3) Manufacture ([7..4])

For **x86-64**:

| Value | Manufacture |
|---:|-------------|
| 0 | Unknown/Other |
| 1 | Intel |
| 2 | AMD |
| 3 | VIA/Zhaoxin |
| 4 | Hygon (海光) |
| 5..15 | Reserved |

For **AArch64**:

| Value | Manufacture |
|---:|-------------|
| 0 | Unknown/Other |
| 1 | Apple |
| 2 | Qualcomm |
| 3 | MediaTek |
| 4 | Ampere |
| 5..15 | Reserved |

For other Root ISAs, this nibble is defined by their interpreters.

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
| 7 | AVX10 (check [55..48]) |
| 8..255 | Reserved |

> Scheduling suggestion: `MainLevel` is the primary switch; only when `MainLevel >= AVX512` should `AVX512 Feature` be parsed; only when `MainLevel >= AMX` should `AMX Future` be parsed; only when `MainLevel >= AVX10` should `AVX10 Feature` be parsed. AVX Feature sits beside AMX/AVX512 and is meaningful when AVX or higher is present.

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
- **AVX512_IMPL ([23..22], 2-bit)**: 512-bit execution width policy — not port count, not frequency behaviour.

| Value | Meaning |
|---:|------|
| 0 | Unknown — cannot determine; use conservative 256-bit kernels |
| 1 | DUAL-PUMP — single 256-bit ALU executes 512-bit ops as two consecutive 256-bit micro-ops; ZMM throughput halved (example: AMD Zen4, Family 0x19) |
| 2 | NATIVE — genuine 512-bit execution unit(s); full ZMM throughput (Intel all models including SKX; AMD Zen5 Family 0x1A and later) |
| 3 | Reserved |

> **Note**: Intel SKX/CLX/CPX have two 512-bit FMA ports (port-0 + port-5) and are **NATIVE** — genuine 512-bit throughput. Intel downclocking on heavy ZMM workloads is a separate concern not encoded here. DUAL-PUMP applies only where a single 256-bit unit fires twice per 512-bit op (AMD Zen4-style).

- **VBMI/VBMI2/VNNI/BF16/FP16 ([29..25])**: Commonly needed niche or AI-related small-type extensions; set as needed.
- **R/AI (bit 24)**: Reserved for future AI or other non-crypto extensions.
- **[31..30], [21], [19..16]**: Reserved for future allocation.

---

## 7) AVX Feature Byte ([47..40], 8 bits)

Purpose: compact AVX-family capability encoding adjacent to AVX512/AMX. Base requirement: AVX **and** AVX2 present. Key extensions: FMA and F16C. Additional bits reserved for other extensions.

AVX Feature (bits 47..40)

| bit | 47 | 46 | 45 | 44 | 43 | 42 | 41 | 40 |
|-----|----|----|----|----|----|----|----|----|
| use | R  | R  | R  | DUAL | F16C | FMA | AVX2 | AVX |

Rules:
- **AVX (bit 40)**: AVX present.
- **AVX2 (bit 41)**: AVX2 present. For the AVX feature byte to be considered valid, both AVX and AVX2 must be set.
- **FMA (bit 42)**: FMA present.
- **F16C (bit 43)**: F16C present.
- **DUAL (bit 44)**: YMM execution is **glued/split-lane** (two fused 128-bit units rather than a native 256-bit path). Detection: AMD Zen1/Zen+ (Family 0x17, model < 0x30), Hygon (all, Zen1-based), VIA/Zhaoxin (all). When set, code using YMM operations may incur split-register overhead; prefer 128-bit SSE paths on these targets.
- **[45..47]**: Reserved for other AVX-family extensions (e.g., IFMA, XOP) as needed.

---

## 8) AVX10 Feature Byte ([55..48], 8 bits)

Purpose: encode the converged AVX10 ISA version and supported vector widths. AVX10 unifies and replaces the fragmented AVX-512 subset model; a single version number implies a defined set of instructions (e.g., AVX10.1 implies F+CD+VL+DQ+BW+VBMI+VBMI2+VNNI+BF16+FP16 converged). The width bits indicate what maximum vector width the hardware actually implements.

**CPUID detection**: `CPUID leaf 0x24, subleaf 0`.
- EBX[7:0] = AVX10 converged ISA version number (≥ 1 if supported).
- EBX[16] = 128-bit vector support (always 1 when AVX10 is present).
- EBX[17] = 256-bit vector support.
- EBX[18] = 512-bit vector support.

AVX10 Feature (bits 55..48)

| bit | 55 | 54 | 53 | 52   | 51   | 50 | 49 | 48   |
|-----|----|----|----| ---- | ---- |----|----|------|
| use | R  | R  | R  | 512W | 256W | V2 | V1 | AVX10 |

Rules:
- **AVX10 (bit 48)**: CPU declares AVX10 converged ISA support (leaf 0x24 reports a valid version ≥ 1).
- **V1 (bit 49)**: AVX10 version ≥ 1 (AVX10.1). Set when `EBX[7:0] >= 1`.
- **V2 (bit 50)**: AVX10 version ≥ 2 (AVX10.2). Set when `EBX[7:0] >= 2`.
- **256W (bit 51)**: Hardware supports 256-bit vector width for AVX10 instructions (`EBX[17]` == 1).
- **512W (bit 52)**: Hardware supports 512-bit vector width for AVX10 instructions (`EBX[18]` == 1).
- **[53..55]**: Reserved for future AVX10 versions (V3, V4...) or additional width/feature bits.

> Note: When `AVX10 == 1`, the 128-bit width is always implied and does not need a dedicated bit. `V1` and `V2` are cumulative — if `V2` is set, `V1` must also be set.

**Relationship with AVX512 Feature**:
- AVX10 is a superset specification. When `MainLevel == AVX10`, the `AVX512 Feature` field may still be populated for backward-compatible code paths (e.g., checking `AVX512_IMPL` to select DUAL vs NATIVE 512-bit policy).
- AVX10/256 hardware does **not** support 512-bit operations; code should check `512W` before using zmm registers.

---

## 9) "AVX512BASE" Determination (Single Check)

- **Rule**: `BASE=1` implies `F+CD+VL+DQ+BW` are all present; no need for per-subset checks. Detection must keep this consistent; if not all subsets are present, set to 0.
- **Runtime branching**:
  - If `MainLevel < AVX512`: fall back to AVX2/FMA path.
  - Otherwise:
    - Check `BASE`; if 0, fall back to AVX2/FMA (or treat as detection failure).
    - Check `AVX512_IMPL`: `DUAL` uses 256-bit/downclock-safe path, `NATIVE` uses 512-bit path.
    - When small-type/AI capabilities are required, consult `VNNI/BF16/FP16/VBMI/VBMI2` bits.
  - If `MainLevel == AVX10`: prefer AVX10 Feature for capability checks; `AVX512 Feature` remains valid for legacy branching.

---

## 10) AMX Future ([39..32], 8 bits)

A single byte is reserved; define three common bits for now, leave the rest reserved:

| bit (relative to [39..32]) | Name | Note |
|---:|------|------|
| 0 | AMX_TILE | Tile base |
| 1 | AMX_INT8 | INT8 tile |
| 2 | AMX_BF16 | BF16 tile |
| 3..7 | Reserved | |

When `MainLevel < AMX`, this byte can be all zeros.

---

## 11) uArchHi ([63..56], 8 bits)

Purpose: **bitmask** byte encoding core topology classification and SIMD execution-width policy for the detected core. All bits default to 0.

uArchHi bitmask layout:

| bit (relative to [63..56]) | Name | Note |
|---:|------|------|
| 0 | HYBRID_E | E-core (efficiency/small core) in a hybrid-topology processor (e.g., Intel Alder Lake+ E-core) |
| 1 | HYBRID_P | P-core (performance/big core) in a hybrid-topology processor (e.g., Intel Alder Lake+ P-core) |
| 2 | SIMD_DUAL | SIMD execution is glued/dual (not full-width). Summary flag: set when `AVX_DUAL=1` **or** `AVX512_IMPL=DUAL`. |
| 3..7 | Reserved | |

Core topology encoding (bits [1..0], numerically comparable — higher = stronger):

| bits [1..0] | Value | Meaning |
|:-----------:|:-----:|----------|
| 00 | 0 | Unknown — undetected hybrid type or pre-CPUID.1A hardware |
| 01 | 1 | **Hybrid-E** — efficiency/small core (a3) |
| 10 | 2 | **Hybrid-P** — performance/big core (a2) |
| 11 | 3 | **Unified** — confirmed non-hybrid; full-topology core (a1) |

The 2-bit field is directly comparable as an unsigned integer: `unified(3) > P-core(2) > E-core(1) > unknown(0)`.

Detection:
- **Non-hybrid (unified)**: if `CPUID.7.0 EDX[15]` (HYBRID) is **not** set, the CPU is confirmed non-hybrid → set both `HYBRID_P` and `HYBRID_E` (topology = 11).
- **Hybrid P-core**: HYBRID flag set and `CPUID.0x1A.0 EAX[31:24] == 0x40` → set only `HYBRID_P` (topology = 10).
- **Hybrid E-core**: HYBRID flag set and `CPUID.0x1A.0 EAX[31:24] == 0x20` → set only `HYBRID_E` (topology = 01).
- **Unknown hybrid** (flag set but core type unrecognised): leave topology at 00.
- **SIMD_DUAL**: set when any of the following: `AVX Feature DUAL=1` (glued YMM), or `AVX512_IMPL=DUAL`.

`sn_get_unite_simd` computes topology bits [1..0] with bitwise **AND** (conservative intersection — weaker topology wins) and SIMD_DUAL with bitwise **OR** (dual if either core is dual). `sn_compare_simd` compares topology value numerically as step 6 and SIMD_DUAL as the final (step 7) tie-break.

---

## 12) Minimal Detection-Side Fill Rules (x86-64)

1) Set `RootISA = x86-64`, `Manufacture = Intel / AMD / Hygon / VIA`.
2) Compute `MainLevel` (per ascending rule).
3) If `MainLevel >= AVX512`:
   - If `F+CD+VL+DQ+BW` are all present, set `BASE = 1`; otherwise `BASE = 0`.
   - Set `AVX512_IMPL`:
     - **Intel (all)**: NATIVE — all Intel AVX-512 CPUs have genuine 512-bit execution units
     - **AMD Zen4 (Family 0x19)**: DUAL — single 256-bit ALU, dual-pump
     - **AMD Zen5+ (Family 0x1A+)**: NATIVE — native full-width 512-bit units
     - **AMD other/future**: UNKNOWN (conservative)
     - **Hygon**: UNKNOWN (AVX-512 characteristics unclear)
     - **VIA/other**: UNKNOWN
   - When VNNI/BF16/FP16/VBMI/VBMI2 are detected, set the corresponding bits.
4) If `MainLevel >= AMX`: fill the AMX byte.
5) When AVX/AVX2 are present, populate the AVX Feature byte: set `AVX` and `AVX2`; set `FMA` and `F16C` when detected; set `DUAL` when the YMM execution path is split/glued (AMD Zen1/Zen+ Family 0x17 model < 0x30, Hygon all models, VIA/Zhaoxin all models).
6) **AVX10 detection** (requires `CPUID leaf 7 subleaf 1, EDX[19] == 1` as the AVX10 convergence bit):
   - If AVX10 is indicated, query `CPUID leaf 0x24, subleaf 0`:
     - Read `EBX[7:0]` for version number; set `AVX10`, `V1`, and optionally `V2`.
     - Read `EBX[17]` for 256-bit support → set `256W`.
     - Read `EBX[18]` for 512-bit support → set `512W`.
   - Promote `MainLevel` to `AVX10` (value 7).
   - The `AVX512 Feature` field should still be filled for backward compatibility.
7) Populate uArchHi:
   - If `CPUID.7.0 EDX[15]` (HYBRID) **not** set: set `HYBRID_P | HYBRID_E` (unified = 11).
   - If HYBRID is set, read `CPUID.0x1A.0 EAX[31:24]`:
     - `0x40` (P-core) → set only `HYBRID_P` (topology = 10).
     - `0x20` (E-core) → set only `HYBRID_E` (topology = 01).
     - Otherwise leave topology at 00 (unknown hybrid).
   - If `AVX Feature DUAL=1` or `AVX512_IMPL=DUAL`, set `SIMD_DUAL`.

---

## 13) Choice for High Bits Reservation

`[31..24]` is reserved for AVX512 extensions/AI. `[47..45]` is reserved for AVX-family extensions (bit 44 is now `DUAL`). `[55..53]` is reserved for future AVX10 versions. uArchHi `[63..56]` bits [3..7] are reserved; bits [0..2] encode hybrid topology and SIMD-dual policy.