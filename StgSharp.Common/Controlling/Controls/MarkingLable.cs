//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="MarkingLable.cs"
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
using StgSharp.Controls;
using StgSharp.Geometries;
using StgSharp.Graphics;
using StgSharp.Math;
using StgSharp.Timing;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StgSharp.Controls
{
    public class MarkingLabel : ControllingItem
    {

        private PlainGeometryMesh marker;

        private TimeFunction _movement;

        private Vec2 _currentPosition, _movementVec;

        public MarkingLabel( PlainGeometryMesh marker, TimeFunction movement )
        {
            this.marker = marker;
            _movement = movement;
        }

        public bool IsEntity => true;

        public ReadOnlySpan<Vec4> TextureBox
        {
            get => marker.TextureCoord;
        }

        public Rectangle BoundingBox { get; set; }

        public Vec2 Position { get; set; }

        public IEnumerator<PlainGeometryMesh> GetEnumerator()
        {
            yield return marker;
        }

        public void Move( Vec2 movement )
        {
            if( !_movement.IsComplete ) {
                Position = _currentPosition + ( _movement.Calculate() * _movementVec );
            }
        }

        public void MoveTo( Vec2 target )
        {
            Move( target - Position );
        }

    }
}
