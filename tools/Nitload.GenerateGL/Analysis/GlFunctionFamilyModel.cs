// -----------------------------------------------------------------------------
// file="GlFunctionFamilyModel"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using StgSharp.GenerateGL.Registry;

namespace StgSharp.GenerateGL.Analysis
{
    internal sealed record GlFunctionNameShape(
        string FamilyName,
        string? Shape,
        int? Columns,
        int? Rows,
        string TypeSuffix,
        bool IsVectorForm
    );

    internal sealed record GlFunctionFamilyMember(
        GlCommandDefinition Command,
        GlFunctionNameShape? NameShape
    );

    internal sealed record GlFunctionRelation(
        string SourceName,
        string TargetName,
        GlFunctionRelationKind Kind,
        GlFunctionRelationState State,
        string Detail
    );

    internal sealed record GlFunctionFamily(
        string Name,
        IReadOnlyList<GlFunctionFamilyMember> Members,
        IReadOnlyList<GlFunctionRelation> Relations,
        IReadOnlyList<string> Diagnostics,
        GlFunctionShapeNamePolicy ShapeNamePolicy
    );

    internal sealed record GlFunctionFamilyAnalysis(
        IReadOnlyList<GlFunctionFamily> Families,
        IReadOnlyList<GlCommandDefinition> UnclassifiedCommands,
        IReadOnlyList<GlFunctionRelation> Relations
    );

    internal enum GlFunctionRelationKind
    {
        RegistryVectorEquivalent,
        InferredSingleValuePointer,
    }

    internal enum GlFunctionRelationState
    {
        Verified,
        MissingTarget,
        Rejected,
    }

    internal enum GlFunctionShapeNamePolicy
    {
        NotApplicable,
        CanRemove,
        MustRetain,
    }
}
