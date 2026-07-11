// -----------------------------------------------------------------------------
// file="PipelineNodeLabel"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.PipeLine
{
    public abstract record class PipelineNodeLabel
    {

        public static PipelineNodeLabel Empty => PipelineNodeLabelEmpty.Only;

        public abstract int ComputeHashCode();

        public override int GetHashCode()
        {
            return ComputeHashCode();
        }

    }

    public record class PipelineNodeStringLabel : PipelineNodeLabel
    {

        public PipelineNodeStringLabel(
               string name
        )
        {
            Name = name;
        }

        public string Name { get; }

        public override int ComputeHashCode()
        {
            return string.GetHashCode(Name);
        }

    }

    internal record class PipelineNodeLabelEmpty : PipelineNodeLabel
    {

        public static PipelineNodeLabelEmpty Only { get; } = new PipelineNodeLabelEmpty();

        public override int ComputeHashCode()
        {
            return 0;
        }

    }
}
