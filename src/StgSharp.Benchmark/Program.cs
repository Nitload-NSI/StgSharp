// -----------------------------------------------------------------------------
// file="Program"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using StgSharp.Benchmark;

IConfig config = ManualConfig.Create(DefaultConfig.Instance)
                             .AddLogger(ConsoleLogger.Default)
                             .AddJob(Job.Default
                                        .WithToolchain(InProcessEmitToolchain.Instance)
                                        .WithLaunchCount(1)
                                        .WithWarmupCount(6)
                                        .WithIterationCount(15)
                                        .WithInvocationCount(3)
                                        .WithUnrollFactor(1));
_ = BenchmarkRunner.Run<TlsfAllocatorBench>(config);
