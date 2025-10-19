//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepOrientedEdge"
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
using Microsoft.Win32.SafeHandles;

using StgSharp.Model.Step;
using StgSharp.Script;
using StgSharp.Script.Express;

using System;
using System.Collections.Generic;

namespace StgSharp.Model.Step
{
    public class StepOrientedEdge : StepEdge, IExpConvertableFrom<StepOrientedEdge>
    {

        private StepEdge _edgeElement;

        private StepOrientedEdge(StepModel model) : base(model) { }

        public StepOrientedEdge(
               StepModel model,
               StepVertex edgeStart,
               StepVertex edgeEnd,
               StepEdge edgeElement,
               bool orientation)
            : base(model, edgeStart, edgeEnd)
        {
            EdgeElement = edgeElement;
            Orientation = orientation;
        }

        public bool Orientation { get; set; }

        public StepEdge EdgeElement
        {
            get { return _edgeElement; }
            set
            {
                if (value == null) {
                    throw new ArgumentNullException();
                }

                _edgeElement = value;
            }
        }

        public override StepItemType ItemType => StepItemType.OrientedEdge;

        public void FromInstance(StepOrientedEdge entity)
        {
            base.FromInstance(entity);
            EdgeElement = entity.EdgeElement;
            Orientation = entity.Orientation;
        }

        internal static StepOrientedEdge FromSyntax(StepModel binder, ExpSyntaxNode syntaxList)
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator(syntaxList);
            StepOrientedEdge orientedEdge = new StepOrientedEdge(binder);
            enumerator.AssertEnumeratorCount(5);
            orientedEdge.Name = (enumerator[0]as ExpStringNode)!.Value;
            orientedEdge.EdgeStart = binder[enumerator[1]]as StepVertex;
            orientedEdge.EdgeEnd = binder[enumerator[2]] as StepVertex;
            orientedEdge.EdgeElement = binder[enumerator[3]] as StepEdge;
            orientedEdge.Orientation = (enumerator[4]as ExpBoolNode)!.Value;
            return orientedEdge;
        }

    }
}

