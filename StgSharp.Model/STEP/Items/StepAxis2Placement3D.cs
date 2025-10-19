//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepAxis2Placement3D"
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

using System;
using System.Collections.Generic;

namespace StgSharp.Model.Step
{
    public class StepAxis2Placement3D : StepAxis2Placement, IExpConvertableFrom<StepAxis2Placement3D>
    {

        private StepDirection _axis;

        private StepAxis2Placement3D(StepModel model) : base(model) { }

        public StepAxis2Placement3D(
               StepModel model,
               StepCartesianPoint location,
               StepDirection axis,
               StepDirection refDirection)
            : base(model)
        {
            Location = location;
            Axis = axis;
            RefDirection = refDirection;
        }

        public StepDirection Axis
        {
            get { return _axis; }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _axis = value;
            }
        }

        public override StepItemType ItemType => StepItemType.AxisPlacement3D;

        public override bool IsConvertableTo(string entityTypeName)
        {
            throw new NotImplementedException();
        }

        internal static StepAxis2Placement3D FromSyntax(StepModel binder, ExpSyntaxNode syntaxList)
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator(syntaxList);
            StepAxis2Placement3D axis = new StepAxis2Placement3D(binder);
            enumerator.AssertEnumeratorCount(4);
            axis.Name = (enumerator[0]as ExpStringNode)!.Value;
            axis.Location = binder[enumerator[1]] as StepCartesianPoint;
            axis.Axis = binder[enumerator[2]] as StepDirection;
            axis.RefDirection = binder[enumerator[3]] as StepDirection;
            return axis;
        }

        void IExpConvertableFrom<StepAxis2Placement3D>.FromInstance(StepAxis2Placement3D entity)
        {
            throw new NotImplementedException();
        }

    }
}

