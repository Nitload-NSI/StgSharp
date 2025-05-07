//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PipelineNodeLabel.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
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
        public PipelineNodeStringLabel( string name )
        {
            Name = name;
        }

        public string Name { get; }

        public override int ComputeHashCode()
        {
            return string.GetHashCode( Name );
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
