// -----------------------------------------------------------------------------
// file="ManagedApiModel"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Generation
{
    internal sealed record ManagedMethodDefinition(
        string Name,
        string ReturnType,
        IReadOnlyList<ManagedParameterDefinition> Parameters,
        GlCommandDefinition NativeCommand,
        ManagedInvocation Invocation,
        bool WrapHandleReturn = false,
        bool IsEnumReturn = false,
        string? DocumentationPage = null
    );

    internal sealed record ManagedParameterDefinition(
        string Type,
        string Name,
        GlParameterDefinition? NativeParameter = null,
        bool IsHandle = false,
        bool IsHandleSpan = false,
        bool IsEnum = false
    );

    internal abstract record ManagedInvocation;

    internal sealed record DirectInvocation(
        IReadOnlyList<ManagedArgument> Arguments
    ) : ManagedInvocation;

    internal sealed record PinnedSpanInvocation(
        IReadOnlyList<ManagedArgument> PrefixArguments,
        int SpanParameterIndex,
        string ElementType
    ) : ManagedInvocation;

    internal sealed record PinnedVectorSpanInvocation(
        IReadOnlyList<ManagedArgument> PrefixArguments,
        int SpanParameterIndex,
        string ComponentType,
        string VectorType
    ) : ManagedInvocation;

    internal sealed record Matrix4Invocation(
        IReadOnlyList<ManagedArgument> PrefixArguments,
        int TransposeParameterIndex,
        int MatrixParameterIndex,
        bool IsSpan
    ) : ManagedInvocation;

    internal sealed record QuerySpanInvocation(
        IReadOnlyList<ManagedArgument> PrefixArguments,
        int SpanParameterIndex,
        string ElementType
    ) : ManagedInvocation;

    internal abstract record ManagedArgument;

    internal sealed record ParameterArgument(
        int ParameterIndex
    ) : ManagedArgument;

    internal sealed record VectorComponentArgument(
        int ParameterIndex,
        string Component
    ) : ManagedArgument;

    internal sealed record SpanLengthArgument(
        int ParameterIndex
    ) : ManagedArgument;

}
