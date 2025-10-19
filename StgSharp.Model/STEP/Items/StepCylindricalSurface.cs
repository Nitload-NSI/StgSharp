//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepCylindricalSurface"
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
    public class StepCylindricalSurface : StepElementarySurface, IExpConvertableFrom<StepCylindricalSurface>
    {

        public StepCylindricalSurface(StepModel model) : base(model) { }

        public StepCylindricalSurface(StepModel model, StepAxis2Placement3D position, float radius)
            : base(model, position)
        {
            Radius = radius;
        }

        public float Radius { get; set; }

        public override StepItemType ItemType => StepItemType.CylindricalSurface;

        public void FromInstance(StepCylindricalSurface entity)
        {
            base.FromInstance(entity);
            Radius = entity.Radius;
        }

        internal static StepRepresentationItem FromSyntax(StepModel binder, ExpSyntaxNode syntaxList)
        {
            ExpNodePresidentEnumerator enumerator = new ExpNodePresidentEnumerator(syntaxList);
            enumerator.AssertEnumeratorCount(3);
            StepCylindricalSurface surface = new StepCylindricalSurface(binder);
            surface.Name = (enumerator[0] as ExpStringNode)!.Value;
            surface.Position = binder[enumerator[1]]as StepAxis2Placement3D;
            surface.Radius = (enumerator[2]as ExpRealNumberNode)!.Value;
            return surface;
        }

    }
}

