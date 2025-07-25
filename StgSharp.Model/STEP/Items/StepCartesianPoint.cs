//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepCartesianPoint.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the ��Software��), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED ��AS IS��, 
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
using StgSharp.Model.Step;
using StgSharp.Script;
using StgSharp.Script.Express;

namespace StgSharp.Model.Step
{
    public class StepCartesianPoint : StepTriple, IExpConvertableFrom<StepCartesianPoint>
    {

        private StepCartesianPoint( StepModel model ) : base( model ) { }

        public StepCartesianPoint( StepModel model, float x, float y, float z )
            : base( model, x, y, z ) { }

        public override StepItemType ItemType => StepItemType.CartesianPoint;

        protected override int MinimumValueCount => 1;

        public void FromInstance( StepCartesianPoint entity )
        {
            base.FromInstance( entity );
        }

        internal static StepCartesianPoint FromSyntax( StepModel binder, ExpSyntaxNode syntaxList )
        {
            return AssignTo( new StepCartesianPoint( binder ), syntaxList );
        }

    }
}

