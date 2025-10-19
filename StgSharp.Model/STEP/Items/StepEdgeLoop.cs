//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepEdgeLoop"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using StgSharp.Model.Step;
using StgSharp.Script;
using StgSharp.Script.Express;

using System.Collections.Generic;
using System.Linq;

namespace StgSharp.Model.Step
{
    public class StepEdgeLoop : StepLoop, IExpConvertableFrom<StepEdgeCurve>
    {

        public StepEdgeLoop(StepModel model, params StepOrientedEdge[] edgeList)
            : this(model, (IEnumerable<StepOrientedEdge>)edgeList) { }

        public StepEdgeLoop(StepModel model, IEnumerable<StepOrientedEdge> edgeList)
            : base(model)
        {
            EdgeList = new List<StepOrientedEdge>(edgeList);
        }

        public List<StepOrientedEdge> EdgeList { get; private set; }

        public override StepItemType ItemType => StepItemType.EdgeLoop;

        public void FromInstance(StepEdgeCurve entity)
        {
            base.FromInstance(entity);
        }

        internal static StepEdgeLoop FromSyntax(StepModel binder, ExpSyntaxNode syntaxList)
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator(syntaxList);
            enumerator.AssertEnumeratorCount(2);
            ExpNodePresidentEnumerator edgeSyntaxList = enumerator[1].TupleToPresidentEnumerator();
            StepEdgeLoop edgeLoop = new StepEdgeLoop(
                binder, new StepOrientedEdge[edgeSyntaxList.Count]);
            edgeLoop.Name = (enumerator[0]as ExpStringNode)!.Value;
            for (int i = 0; i < edgeSyntaxList.Count; i++)
            {
                int j = i; // capture to avoid rebinding
                edgeLoop.EdgeList[j] = binder[edgeSyntaxList[j]] as StepOrientedEdge;
            }

            return edgeLoop;
        }

    }
}

