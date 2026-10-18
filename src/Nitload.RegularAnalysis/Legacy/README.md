# Frozen Text Regex implementation

Text Regex is archived following the [NGRA transformation plan](../../../future/NGRA_Transformation_Plan.md).
It is no longer NGRA's execution architecture or an active development target.

## Retained results and future string matching

Freezing development does not diminish the value of the work already completed. The project
produced reusable AST/token infrastructure, the five-link tree model and its normalization,
rotation, and merging techniques, as well as tested fixed-width COUNT retry logic. The archived
generator also records useful engineering lessons about continuations, group boundaries, label
ownership, and rollback. These are reference material for the new architecture, not a claim that
the old matcher is feature-complete.

Future string-regex functionality will use .NET `GeneratedRegex`. Further research into custom
`Span<char>` and SIMD optimizations for the Text Regex matcher is discontinued. This boundary
applies to the string-regex implementation, not to SIMD work elsewhere in Nitload.

## Contents

- `Text/`: the text tokenizer/parser, regex-specific AST and optimizers, matcher contract,
  attribute, and all source-generator files.
- `GroupIndex.cs`: the former group index/name record.
- `SourceGenerate.cs`: the unused source-generation entry stub.
- [Archived debug routines](../../../test/StgSharpDebug/Legacy/TextRegex): the original
  AST inspection and generated-source preview programs.

Source contents and namespaces are preserved. The generic infrastructure in `../Abstraction/`
remains active, including the five-link node interface, generic AST, token abstractions,
compilation stack, and sequence emitter. Text-specific tree transformations remain here
as source material for future Pattern-to-NFA lowering.

## Build boundary

The project excludes `Legacy/**/*.cs` from `Compile` and retains the files as `None` items.
They are not copied to build/publish output or packed. The active project no longer references
Roslyn or packages its assembly as a source generator. Debug routines have the same exclusion
in their own project and are not called from `Main`.

To investigate this implementation again, use a separate experimental project that explicitly
includes the archived files and supplies the former Roslyn dependencies:
`Microsoft.CodeAnalysis.CSharp` 4.13.0 and `Microsoft.CodeAnalysis.Analyzers` 3.11.0.
The debug preview also required Roslyn at runtime and the `StgSharpDebug` friend assembly.
Re-enabling files is not sufficient to make the unfinished matcher complete.

Historical notes remain in [IR_RETIRED.md](../IR_RETIRED.md) and
[regex_node_transaction.md](../regex_node_transaction.md). They describe earlier experiments,
not the new NFA execution contract. Older `.discard` material is unchanged.

The next runtime is planned around generic sequence input and Boolean ε-NFA execution;
weighted automata and NGRA-Tensor remain later development/research stages.
