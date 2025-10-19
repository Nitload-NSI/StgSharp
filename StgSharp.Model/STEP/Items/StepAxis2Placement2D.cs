//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepAxis2Placement2D"
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
    public class StepAxis2Placement2D : StepAxis2Placement, IExpConvertableFrom<StepAxis2Placement2D>
    {

        private StepAxis2Placement2D(StepModel model) : base(model) { }

        public StepAxis2Placement2D(StepModel model, StepCartesianPoint location, StepDirection direction)
            : base(model)
        {
            Location = location;
            RefDirection = direction;
        }

        public override StepItemType ItemType => StepItemType.AxisPlacement2D;

        public void FromInstance(StepAxis2Placement2D entity)
        {
            base.FromInstance(entity);
            Location = entity.Location;
            RefDirection = entity.RefDirection;
        }

        internal static StepAxis2Placement2D FromSyntax(StepModel binder, ExpSyntaxNode syntaxList)
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator(syntaxList);
            StepAxis2Placement2D axis = new StepAxis2Placement2D(binder);
            enumerator.AssertEnumeratorCount(3);
            axis.Name = (enumerator[0]as ExpStringNode)!.Value;
            axis.Location = binder[enumerator[1]] as StepCartesianPoint;
            axis.RefDirection = binder[enumerator[2]] as StepDirection;
            return axis;
        }

    }
}

