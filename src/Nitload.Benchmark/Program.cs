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
using Nitload.Benchmark;

if (args.Length > 0 && args[0] == "--disasm-swiss")
{
    IConfig disassemblyConfig = ManualConfig.Create(DefaultConfig.Instance)
                                               .AddLogger(ConsoleLogger.Default)
                                               .AddJob(Job.ShortRun.WithWarmupCount(6));
    BenchmarkRunner.Run<SwissX86DisassemblyBench>(disassemblyConfig);
    return;
}

IConfig config = ManualConfig.Create(DefaultConfig.Instance)
                             .AddLogger(ConsoleLogger.Default)
                             .AddJob(Job.Default
                                        .WithLaunchCount(1)
                                        .WithWarmupCount(6)
                                        .WithIterationCount(15)
                                        .WithInvocationCount(3)
                                        .WithUnrollFactor(1));
BenchmarkSwitcher.FromAssembly(typeof(TlsfAllocatorBench).Assembly).Run(args, config);
