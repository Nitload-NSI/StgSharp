//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepTriple.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the ¡°Software¡±), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED ¡°AS IS¡±, 
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
using StgSharp.Math;

using StgSharp.Model.Step;
using StgSharp.Script;
using StgSharp.Script.Express;

using System.Collections.Generic;

namespace StgSharp.Model.Step
{
    public abstract class StepTriple : StepPoint
    {

        private Vec3 _coord;

        protected StepTriple() : this( 0.0f, 0.0f, 0.0f ) { }

        protected StepTriple( float x, float y, float z )
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X
        {
            get => _coord.X;
            set => _coord.X = value;
        }

        public float Y
        {
            get => _coord.Y;
            set => _coord.Y = value;
        }

        public float Z
        {
            get => _coord.Z;
            set => _coord.Z = value;
        }

        protected abstract int MinimumValueCount { get; }

        public bool Equals( StepTriple other )
        {
            if( other is null ) {
                return false;
            }

            return ItemType == other.ItemType && X == other.X && Y == other.Y && Z == other.Z && Name ==
                   other.Name;
        }

        public override bool Equals( object obj )
        {
            return ( obj is StepTriple triple ) && Equals( triple );
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        internal static StepTriple AssignTo( StepTriple triple, ExpSyntaxNode values )
        {
            ExpNodePresidentEnumerator enumerator = values.ToPresidentEnumerator();
            enumerator.AssertEnumeratorCount( 2 );
            triple.Name = ( enumerator[ 0 ]as ExpStringNode )!.Value;
            ExpNodePresidentEnumerator pointValues = enumerator[ 1 ].ToPresidentEnumerator();
            pointValues.AssertEnumeratorCount( triple.MinimumValueCount, 3 );
            triple.X = ( pointValues[ 0 ]as ExpRealNumberNode )!.Value;
            triple.Y = ( pointValues[ 1 ]as ExpRealNumberNode )!.Value;
            triple.Z = ( pointValues[ 2 ]as ExpRealNumberNode )!.Value;
            return triple;
        }

        public static bool operator !=( StepTriple left, StepTriple right )
        {
            return !( left == right );
        }

        public static bool operator ==( StepTriple left, StepTriple right )
        {
            if( ReferenceEquals( left, right ) ) {
                return true;
            }

            if( left is null || right is null ) {
                return false;
            }

            return left.Equals( right );
        }

    }
}

