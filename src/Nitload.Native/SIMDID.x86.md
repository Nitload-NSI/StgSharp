# SIMDID (x86-64) 64-bit Specification (Completed per fixed layout)

This document defines the compact **64-bit SIMDID** layout, including AVX/AVX-512/AVX10 capabilities and per-core execution-policy hints. AMX is not represented by the current policy.

> Convention: bit0 is the least significant bit (LSB). Only x86-64 interpreter semantics are defined here; the header's Root ISA allows extension to other architectures.

---

## 1) ASCII Bit Layout (Final)

```
SIMDID (64 bits)  [63 ........................................................ 0]
+----------+-----------+-----------+----------------+----------------+----------------+--------+-------+
| [63..56] | [55..48]  | [47..40]  | [39..32]       | [31..16]       | [15..8]        | [7..4] | [3..0]|
| Reserved | uArchHi   | AVX10Feat | AVX Feature    | AVX512 Feature | MainLevel      | Manuf  | Root  |
+----------+-----------+-----------+----------------+----------------+----------------+--------+-------+
```

- **[3..0] Root ISA**: Root ISA type (selects interpreter)
- **[7..4] Manufacture**: Vendor info (Intel/AMD for x86)
- **[15..8] MainLevel**: Main ISA level (ascending, shared bit semantics)
- **[31..16] AVX512 Feature**: AVX-512 features/subsets/implementation form (key part)
- **[39..32] AVX Feature**: AVX-family capabilities (base requires AVX+AVX2; key extensions include FMA/F16C)
- **[47..40] AVX10 Feature**: AVX10 version and tracked discrete features
- **[55..48] uArchHi**: Optional microarchitecture/policy hints
- **[63..56] Reserved**: tail space for future extension

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
| 3 | AVX512 (check [31..16]) |
| 4 | AVX10 (check [47..40]) |
| 5..255 | Reserved |

> `MainLevel` is the primary switch. Parse AVX512 Feature at AVX512 or later and AVX10 Feature only at AVX10.

---

## 5) AVX512 Feature ([31..16]) Design Goals (Compact)

1) **Base AVX512 one-shot check**: Use 1 bit for the bundle `F + CD + VL + DQ + BW`, avoiding multiple subset checks.
2) **Glue vs native**: Use an implementation field to distinguish `AVX512-DUAL` (glue/downclock risk) from `AVX512-NATIVE` (full 512-bit ALU).
3) **Minimal extension tracking**: VNNI is the only AVX-512 extension currently retained.

---

## 6) AVX512 Feature Bit Layout ([31..16] = 16 bits, compact)

AVX512 Feature (bits 31..16)

| **bit**   | 31  | 30  | 29   | 28   | 27   | 26    | 25   | 24   | 23    | 22    | 21 | 20   | 19 | 18 | 17 | 16 |
|-----------|-----|-----|------|------|------|-------|------|------|-------|-------|----|------|----|----|----|----|
| **Usage** | R   | R   | R    | R    | VNNI | R     | R    | R    | Impl1 | Impl0 | R  | BASE | R  | R  | R  | R  |

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

- **VNNI (bit 27)**: the only AVX-512 extension currently tracked; the surrounding extension bits are reserved.
- **R/AI (bit 24)**: Reserved for future AI or other non-crypto extensions.
- **[31..30], [21], [19..16]**: Reserved for future allocation.

---

## 7) AVX Feature Byte ([39..32], 8 bits)

Purpose: compact AVX-family capability encoding. Base requirement: AVX **and** AVX2 present. Key extensions: FMA and F16C.

AVX Feature (bits 39..32)

| bit | 39 | 38 | 37 | 36 | 35 | 34 | 33 | 32 |
|-----|----|----|----|----|----|----|----|----|
| use | R  | R  | R  | DUAL | F16C | FMA | AVX2 | AVX |

Rules:
- **AVX (bit 32)**: AVX present.
- **AVX2 (bit 33)**: AVX2 present. For the AVX feature byte to be considered valid, both AVX and AVX2 must be set.
- **FMA (bit 34)**: FMA present.
- **F16C (bit 35)**: F16C present.
- **DUAL (bit 36)**: YMM execution is **glued/split-lane** (two fused 128-bit units rather than a native 256-bit path). Detection: AMD Zen1/Zen+ (Family 0x17, model < 0x30), Hygon (all, Zen1-based), VIA/Zhaoxin (all). When set, code using YMM operations may incur split-register overhead; prefer 128-bit SSE paths on these targets.
- **[37..39]**: Reserved for other AVX-family extensions as needed.

---

## 8) AVX10 Feature Byte ([47..40], 8 bits)

Purpose: encode the converged AVX10 ISA version and the discrete features relevant to this runtime. Under the current specification every AVX10 processor supports 128-, 256-, and 512-bit vector lengths, so vector width is not encoded.

**CPUID detection**: `CPUID leaf 0x24, subleaf 0`.
- EBX[7:0] = AVX10 converged ISA version number (≥ 1 if supported).
- EBX[18:16] are reserved at 1. Earlier specifications used them for VL128/VL256/VL512; they must not be interpreted as width capabilities.
- `CPUID.24H.1:ECX[2]` enumerates `AVX10_VNNI_INT` when subleaf 1 exists.

AVX10 Feature (bits 47..40)

| bit | 47       | 46..40  |
|-----|----------|---------|
| use | VNNI_INT | Version |

Rules:
- **Version ([46..40])**: numeric AVX10 version, saturated to 7 bits. Zero means AVX10 is absent.
- **VNNI_INT (bit 47)**: `CPUID.24H.1:ECX[2]`.

> AVX10 version comparison is numeric. Merging two SIMDIDs keeps the lower version and intersects discrete feature bits.

**Relationship with AVX512 Feature**:
- AVX10 is a superset specification. When `MainLevel == AVX10`, the `AVX512 Feature` field may still be populated for backward-compatible code paths (e.g., checking `AVX512_IMPL` to select DUAL vs NATIVE 512-bit policy).
- Current AVX10 hardware is architecturally AVX10/512. OS support still requires the full XCR0 SSE/AVX/opmask/ZMM state.

---

## 9) "AVX512BASE" Determination (Single Check)

- **Rule**: `BASE=1` implies `F+CD+VL+DQ+BW` are all present; no need for per-subset checks. Detection must keep this consistent; if not all subsets are present, set to 0.
- **Runtime branching**:
  - If `MainLevel < AVX512`: fall back to AVX2/FMA path.
  - Otherwise:
    - Check `BASE`; if 0, fall back to AVX2/FMA (or treat as detection failure).
    - Check `AVX512_IMPL`: `DUAL` uses 256-bit/downclock-safe path, `NATIVE` uses 512-bit path.
    - Consult `VNNI` when the integer dot-product path requires it.
  - If `MainLevel == AVX10`: prefer AVX10 Feature for capability checks; `AVX512 Feature` remains valid for legacy branching.

---

## 10) uArchHi ([55..48], 8 bits)

Purpose: **bitmask** byte encoding core topology classification and SIMD execution-width policy for the detected core. All bits default to 0.

uArchHi bitmask layout:

| bit (relative to [55..48]) | Name | Note |
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

`nif_get_unite_simd` computes topology bits [1..0] with bitwise **AND** (conservative intersection — weaker topology wins) and SIMD_DUAL with bitwise **OR** (dual if either core is dual). `nif_compare_simd` compares topology value numerically as step 6 and SIMD_DUAL as the final (step 7) tie-break.

---

## 12) Minimal Detection-Side Fill Rules (x86-64)

1) Set `RootISA = x86-64`, `Manufacture = Intel / AMD / Hygon / VIA`.
2) Compute `MainLevel` (per ascending rule).
3) If `MainLevel >= AVX512`:
   - If `F+CD+VL+DQ+BW` are all present, set `BASE = 1`; otherwise `BASE = 0`.
   - Set `AVX512_IMPL`:
     - **Intel (all)**: NATIVE — all Intel AVX-512 CPUs have genuine 512-bit execution units
     - **AMD Zen4 (Family 0x19)**: DUAL — single 256-bit ALU, dual-pump
     - **AMD Family 0x1A models 00h..0Fh (EPYC 9005)**: NATIVE — full-width 512-bit path
     - **AMD Family 0x1A models 10h..1Fh**: UNKNOWN — this CPUID range is shared by full-width EPYC 9005 Zen5c and 256-bit dual-pumped EPYC 8005 products
     - **AMD other client/future models**: UNKNOWN unless their execution width can be identified without an ambiguous product-name heuristic
     - **Hygon**: UNKNOWN (AVX-512 characteristics unclear)
     - **VIA/other**: UNKNOWN
   - Set VNNI when detected; other AVX-512 extension bits remain reserved.
4) When AVX/AVX2 are present, populate the AVX Feature byte: set `AVX` and `AVX2`; set `FMA` and `F16C` when detected; set `DUAL` when the YMM execution path is split/glued (AMD Zen1/Zen+ Family 0x17 model < 0x30, Hygon all models, VIA/Zhaoxin all models).
5) **AVX10 detection** (requires `CPUID leaf 7 subleaf 1, EDX[19] == 1` as the AVX10 convergence bit):
   - If AVX10 is indicated, query `CPUID leaf 0x24, subleaf 0`:
     - Read `EBX[7:0]` as the numeric version; do not interpret `EBX[18:16]` as vector-width bits.
     - If subleaf 1 exists, read `ECX[2]` as `AVX10_VNNI_INT`.
   - Promote `MainLevel` to `AVX10` (value 4).
   - The `AVX512 Feature` field should still be filled for backward compatibility.
6) Populate uArchHi:
   - If `CPUID.7.0 EDX[15]` (HYBRID) **not** set: set `HYBRID_P | HYBRID_E` (unified = 11).
   - If HYBRID is set, read `CPUID.0x1A.0 EAX[31:24]`:
     - `0x40` (P-core) → set only `HYBRID_P` (topology = 10).
     - `0x20` (E-core) → set only `HYBRID_E` (topology = 01).
     - Otherwise leave topology at 00 (unknown hybrid).
   - If `AVX Feature DUAL=1` or `AVX512_IMPL=DUAL`, set `SIMD_DUAL`.

---

## 13) Choice for High Bits Reservation

`[31..24]` is reserved for AVX512 extensions/AI. `[39..37]` is reserved for AVX-family extensions. AVX10 `[47..40]` stores one tracked discrete feature plus a 7-bit version. uArchHi `[55..48]` bits [3..7] are reserved; bits [0..2] encode hybrid topology and SIMD-dual policy. `[63..56]` is reserved tail space.
