//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="SwissBenchmark"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using BenchmarkDotNet.Attributes;
using StgSharp.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StgSharp.Benchmark
{
    /// <summary>
    ///   Benchmark comparing SwissTable against <see cref="Dictionary{TKey, TValue}" /> under
    ///   realistic L4 software cache address-mapping workloads. <para> ///   L4 scenario
    ///   characteristics: ///   - Keys are HLSF/malloc pointer addresses (large, 1024-byte aligned)
    ///   ///   - Access pattern is Zipfian-skewed (20% hot cache lines receive 80% of lookups) /// 
    ///    - Steady state: high hit-rate lookups dominate, with rare evict+refill on miss ///   -
    ///   Working set: 512–4096 active cache line mappings ///</para>
    /// </summary>
    [MemoryDiagnoser]
    public class SwissTableBench
    {

        /// <summary>
        ///   L4 cache line size in bytes (per L4_design.md).
        /// </summary>
        private const int CacheLineSize = 1024;

        /// <summary>
        ///   Simulated heap base address for pointer-like keys.
        /// </summary>
        private const long HeapBase = 0x7FFF_0000_0000L;

        /// <summary>
        ///   Number of lookup operations per Lookup benchmark call.
        /// </summary>
        private const int LookupIterations = 8192;

        /// <summary>
        ///   Fraction of steady-state ops that are cache misses.
        /// </summary>
        private const double MissRate = 0.05;

        /// <summary>
        ///   Number of steady-state operations per call. Each op is either a lookup (hit) or an
        ///   evict+insert+lookup (miss).
        /// </summary>
        private const int SteadyStateOps = 4096;

        /// <summary>
        ///   Indices into _residentKeys for eviction targets (cold entries, tail region).
        /// </summary>
        private int[] _evictOrder = null!;

        /// <summary>
        ///   Additional keys not in the table, for triggering misses.
        /// </summary>
        private nuint[] _externalKeys = null!;

        // ── Pre-generated key arrays (pointer-like, 1024-byte aligned) ──

        /// <summary>
        ///   Keys for initial table fill (simulates cached addresses).
        /// </summary>
        private nuint[] _residentKeys = null!;

        /// <summary>
        ///   Pre-built steady-state operation script. Each entry: positive index → lookup
        ///   _residentKeys[idx], negative value → cache miss trigger (evict + insert from
        ///   _externalKeys).
        /// </summary>
        private int[] _steadyStateScript = null!;

        /// <summary>
        ///   Uniform random lookup sequence (baseline comparison).
        /// </summary>
        private nuint[] _uniformLookups = null!;

        /// <summary>
        ///   Zipfian-skewed lookup sequence (hot 20% accessed 80% of time).
        /// </summary>
        private nuint[] _zipfianLookups = null!;
        private Dictionary<nuint, nuint> _dictLookup = null!;
        private Dictionary<nuint, nuint> _freshDict = null!;

        // ── Fresh instances for insert/mixed (rebuilt each iteration) ──

        private SwissTable _freshSwiss = null!;

        // ── Pre-filled lookup targets ──

        private SwissTable _swissLookup = null!;

        /// <summary>
        ///   Active cache line count. Matches L4 capacity range: 5 MB / 1024B ≈ 5120,  10 MB /
        ///   1024B ≈ 10240. 512 and 1024 test low-occupancy; 4096 tests near-capacity.
        /// </summary>
        [Params(512, 1024, 4096)]
        public int KeyCount;

        [GlobalCleanup]
        public void Cleanup()
        {
            _dictLookup.Clear();
            _dictLookup.TrimExcess();
        }

        // ════════════════════════════════════════════════════════════
        // Insert — bulk fill with scattered pointer addresses
        // Simulates L4 initial cache warm-up phase.
        // ════════════════════════════════════════════════════════════

        [Benchmark]
        public void Dictionary_Insert()
        {
            Dictionary<nuint, nuint> dict = _freshDict;
            nuint[] keys = _residentKeys;
            for (int i = 0; i < keys.Length; i++) {
                dict[keys[i]] = keys[i] ^ unchecked((nuint)0xDEAD_BEEF);
            }
        }

        // ════════════════════════════════════════════════════════════
        // Lookup — Uniform (baseline comparison)
        // ════════════════════════════════════════════════════════════

        [Benchmark]
        public nuint Dictionary_Lookup_Uniform()
        {
            nuint sum = 0;
            nuint[] keys = _uniformLookups;
            Dictionary<nuint, nuint> dict = _dictLookup;
            for (int i = 0; i < keys.Length; i++)
            {
                if (dict.TryGetValue(keys[i], out nuint value)) {
                    sum += value;
                }
            }
            return sum;
        }

        // ════════════════════════════════════════════════════════════
        // Lookup — Zipfian (L4 hot path, skewed access)
        // 20% of cache lines receive 80% of accesses.
        // This is the most representative L4 benchmark.
        // ════════════════════════════════════════════════════════════

        [Benchmark(Baseline = true)]
        public nuint Dictionary_Lookup_Zipfian()
        {
            nuint sum = 0;
            nuint[] keys = _zipfianLookups;
            Dictionary<nuint, nuint> dict = _dictLookup;
            for (int i = 0; i < keys.Length; i++)
            {
                if (dict.TryGetValue(keys[i], out nuint value)) {
                    sum += value;
                }
            }
            return sum;
        }

        // ════════════════════════════════════════════════════════════
        // SteadyState — the KEY L4 benchmark
        // Pre-filled table at capacity, then 4096 operations:
        // ~95% are lookups (cache hits)
        // ~5%  are miss → evict cold entry → insert new entry → lookup new
        // This directly simulates L4's main runtime loop.
        // ════════════════════════════════════════════════════════════

        [Benchmark]
        public nuint Dictionary_SteadyState()
        {
            nuint sum = 0;
            Dictionary<nuint, nuint> dict = _freshDict;
            nuint[] resident = _residentKeys;
            nuint[] external = _externalKeys;
            int[] script = _steadyStateScript;
            int[] evict = _evictOrder;
            int evictCursor = 0;
            int externalCursor = 0;

            for (int i = 0; i < script.Length; i++)
            {
                int op = script[i];
                if (op >= 0)
                {
                    // Cache hit: lookup existing entry
                    if (dict.TryGetValue(resident[op], out nuint value))
                    {
                        sum += value;
                    }
                } else
                {
                    // Cache miss: evict a cold entry, insert a new one, then lookup
                    int evictIdx = evict[evictCursor % evict.Length];
                    evictCursor++;
                    dict.Remove(resident[evictIdx]);

                    int extIdx = externalCursor % external.Length;
                    externalCursor++;
                    nuint newKey = external[extIdx];
                    nuint newVal = newKey ^ unchecked((nuint)0xDEAD_BEEF);
                    dict[newKey] = newVal;

                    if (dict.TryGetValue(newKey, out nuint v)) {
                        sum += v;
                    }
                }
            }
            return sum;
        }

        [IterationSetup(Targets = [
            nameof(SwissTable_Insert),
            nameof(SwissTable_SteadyState),
            nameof(Dictionary_Insert),
            nameof(Dictionary_SteadyState)
        ])]
        public void IterationSetup()
        {
            _freshSwiss = SwissTable.Create();
            _freshDict = new Dictionary<nuint, nuint>(KeyCount);

            // Pre-fill for steady-state benchmarks
            for (int i = 0; i < KeyCount; i++)
            {
                nuint key = _residentKeys[i];
                nuint val = key ^ unchecked((nuint)0xDEAD_BEEF);
                _freshSwiss.TryAddOrSet(key, val, out _);
                _freshDict[key] = val;
            }
        }

        // ════════════════════════════════════════════════════════════
        // Setup / Teardown
        // ════════════════════════════════════════════════════════════

        [GlobalSetup]
        public void Setup()
        {
            Random rng = new Random(42);

            // ── Generate pointer-like keys (HeapBase + random_offset * 1024) ──
            _residentKeys = GeneratePointerKeys(KeyCount, rng);
            _externalKeys = GeneratePointerKeys(KeyCount, rng); // disjoint set for miss simulation

            // ── Build lookup sequences ──
            _zipfianLookups = GenerateZipfianLookups(_residentKeys, LookupIterations, rng);
            _uniformLookups = GenerateUniformLookups(_residentKeys, LookupIterations, rng);

            // ── Build steady-state operation script ──
            _steadyStateScript = GenerateSteadyStateScript(KeyCount, SteadyStateOps, MissRate, rng);

            // ── Build eviction order (cold tail entries first) ──
            _evictOrder = new int[KeyCount];
            for (int i = 0; i < KeyCount; i++) {
                _evictOrder[i] = i;
            }

            // Shuffle so eviction isn't just sequential
            Shuffle(_evictOrder, rng);

            // ── Pre-fill lookup tables ──
            _swissLookup = SwissTable.Create();
            _dictLookup = new Dictionary<nuint, nuint>(KeyCount);

            for (int i = 0; i < KeyCount; i++)
            {
                nuint key = _residentKeys[i];
                nuint val = key ^ unchecked((nuint)0xDEAD_BEEF); // simulated cache pointer
                _swissLookup.TryAddOrSet(key, val, out _);
                _dictLookup[key] = val;
            }
        }

        [Benchmark]
        public void SwissTable_Insert()
        {
            SwissTable swiss = _freshSwiss;
            nuint[] keys = _residentKeys;
            for (int i = 0; i < keys.Length; i++) {
                swiss.TryAddOrSet(keys[i], keys[i] ^ unchecked((nuint)0xDEAD_BEEF), out _);
            }
        }

        [Benchmark]
        public nuint SwissTable_Lookup_Uniform()
        {
            nuint sum = 0;
            nuint[] keys = _uniformLookups;
            SwissTable swiss = _swissLookup;
            for (int i = 0; i < keys.Length; i++)
            {
                if (swiss.TryGet(keys[i], out nuint value)) {
                    sum += value;
                }
            }
            return sum;
        }

        [Benchmark]
        public nuint SwissTable_Lookup_Zipfian()
        {
            nuint sum = 0;
            nuint[] keys = _zipfianLookups;
            SwissTable swiss = _swissLookup;
            for (int i = 0; i < keys.Length; i++)
            {
                if (swiss.TryGet(keys[i], out nuint value)) {
                    sum += value;
                }
            }
            return sum;
        }

        [Benchmark]
        public nuint SwissTable_SteadyState()
        {
            nuint sum = 0;
            SwissTable swiss = _freshSwiss;
            nuint[] resident = _residentKeys;
            nuint[] external = _externalKeys;
            int[] script = _steadyStateScript;
            int[] evict = _evictOrder;
            int evictCursor = 0;
            int externalCursor = 0;

            for (int i = 0; i < script.Length; i++)
            {
                int op = script[i];
                if (op >= 0)
                {
                    // Cache hit: lookup existing entry
                    if (swiss.TryGet(resident[op], out nuint value))
                    {
                        sum += value;
                    }
                } else
                {
                    // Cache miss: evict a cold entry, insert a new one, then lookup
                    int evictIdx = evict[evictCursor % evict.Length];
                    evictCursor++;
                    swiss.TryRemove(resident[evictIdx], out _);

                    int extIdx = externalCursor % external.Length;
                    externalCursor++;
                    nuint newKey = external[extIdx];
                    nuint newVal = newKey ^ unchecked((nuint)0xDEAD_BEEF);
                    swiss.TryAddOrSet(newKey, newVal, out _);

                    if (swiss.TryGet(newKey, out nuint v)) {
                        sum += v;
                    }
                }
            }
            return sum;
        }

        // ════════════════════════════════════════════════════════════
        // Data generation helpers
        // ════════════════════════════════════════════════════════════

        /// <summary>
        ///   Generate <paramref name="count" /> unique pointer-like keys. Each key = HeapBase +
        ///   randomOffset * CacheLineSize, simulating HLSF-allocated 1024-byte aligned addresses
        ///   scattered across a ~1 GB virtual address range.
        /// </summary>
        private static nuint[] GeneratePointerKeys(
                               int count,
                               Random rng
        )
        {
            nuint[] keys = new nuint[count];
            HashSet<long> used = new HashSet<long>(count);
            for (int i = 0; i < count; i++)
            {
                long offset;
                do
                {
                    // Random offset within ~1M cache lines (1 GB range)
                    offset = (long)rng.Next(0, 1 << 20) * CacheLineSize;
                } while (!used.Add(offset));
                keys[i] = (nuint)(HeapBase + offset);
            }
            return keys;
        }

        /// <summary>
        ///   Generate a steady-state operation script. Each entry: non-negative value = index into
        ///   _residentKeys (lookup/hit), negative value (-1) = cache miss trigger (evict → insert →
        ///   lookup). Lookup indices are Zipfian-skewed.
        /// </summary>
        private static int[] GenerateSteadyStateScript(
                             int keyCount,
                             int opCount,
                             double missRate,
                             Random rng
        )
        {
            int hotSetSize = Math.Max(1, keyCount / 5);
            int[] script = new int[opCount];
            for (int i = 0; i < opCount; i++)
            {
                if (rng.NextDouble() < missRate)
                {
                    script[i] = -1; // miss
                } else
                {
                    // Zipfian hit
                    if (rng.NextDouble() < 0.8)
                    {
                        script[i] = rng.Next(0, hotSetSize);
                    } else
                    {
                        script[i] = rng.Next(hotSetSize, keyCount);
                    }
                }
            }
            return script;
        }

        /// <summary>
        ///   Generate uniform random lookup sequence (baseline for comparison with Zipfian).
        /// </summary>
        private static nuint[] GenerateUniformLookups(
                               nuint[] sourceKeys,
                               int count,
                               Random rng
        )
        {
            nuint[] lookups = new nuint[count];
            for (int i = 0; i < count; i++) {
                lookups[i] = sourceKeys[rng.Next(0, sourceKeys.Length)];
            }
            return lookups;
        }

        /// <summary>
        ///   Generate Zipfian-skewed lookup sequence: 80% of accesses hit the hottest 20% of keys
        ///   (sorted by index as "hotness proxy"), 20% of accesses hit the remaining 80%. This
        ///   models matrix computation where a small panel working set is reused heavily.
        /// </summary>
        private static nuint[] GenerateZipfianLookups(
                               nuint[] sourceKeys,
                               int count,
                               Random rng
        )
        {
            int hotSetSize = Math.Max(1, sourceKeys.Length / 5);
            nuint[] lookups = new nuint[count];
            for (int i = 0; i < count; i++)
            {
                if (rng.NextDouble() < 0.8)
                {
                    lookups[i] = sourceKeys[rng.Next(0, hotSetSize)];
                } else
                {
                    lookups[i] = sourceKeys[rng.Next(hotSetSize, sourceKeys.Length)];
                }
            }
            return lookups;
        }

        /// <summary>
        ///   Fisher-Yates shuffle.
        /// </summary>
        private static void Shuffle(
                            int[] array,
                            Random rng
        )
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rng.Next(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

    }
}
