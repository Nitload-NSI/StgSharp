//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepFaceBound"
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

namespace StgSharp.Model.Step
{
    public class StepFaceBound : StepTopologicalRepresentationItem, IExpConvertableFrom<StepFaceBound>
    {

        public StepFaceBound(StepModel model) : base(model) { }

        public StepFaceBound(StepModel model, StepLoop bound, bool orientation)
            : base(model)
        {
            Bound = bound;
            Orientation = orientation;
        }

        public bool Orientation { get; set; }

        public override StepItemType ItemType => StepItemType.FaceBound;

        public StepLoop Bound { get; set; }

        public void FromInstance(StepFaceBound entity)
        {
            base.FromInstance(entity);
            Bound = entity.Bound;
            Orientation = entity.Orientation;
        }

        internal static StepFaceBound FromSyntax(StepModel binder, ExpSyntaxNode syntaxList)
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator(syntaxList);
            enumerator.AssertEnumeratorCount(3);
            StepFaceBound faceBound = new StepFaceBound(binder);
            faceBound.Name = (enumerator[0]as ExpStringNode)!.Value;
            faceBound.Bound = binder[enumerator[1]] as StepLoop;
            faceBound.Orientation = (enumerator[2]as ExpBoolNode)!.Value;
            return faceBound;
        }

    }
}

