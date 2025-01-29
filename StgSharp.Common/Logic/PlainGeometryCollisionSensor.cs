//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PlainGeometryCollisionSensor.cs"
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
using StgSharp.Geometries;

using System;

namespace StgSharp
{
    public class CollisionEventArgs:EventArgs
    {
        public CollisionEventArgs(IGeometry target, IGeometry boarder)
        {
            Target = target;
            Boarder = boarder;
        }

        public IGeometry Target { get; set; }
        public IGeometry Boarder { get; set; }
    }

    public class PlainGeometryCollisionSensor<TBoarder,TTarget>
        where TBoarder : IGeometry
        where TTarget : IGeometry
    {
        internal PlainGeometry _boarder;
        internal PlainGeometry _target;
        internal EventHandler<CollisionEventArgs> _rule;

        public event EventHandler<CollisionEventArgs> Rule
        {
            add { _rule += value; }
            remove 
            {
                if (value == null)
                {
                    return;
                }
#pragma warning disable CS8601
                _rule -= value;
#pragma warning restore CS8601
            }
        }

        public PlainGeometry boarder => _boarder;

        public PlainGeometry target
        {
            get => _target;
            set => _target = value;
        }

    }
}
