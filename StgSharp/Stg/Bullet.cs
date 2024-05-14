//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Bullet.cs"
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

using StgSharp.Geometries;
using StgSharp.Math;

using System;

namespace StgSharp.Gaming
{
    public abstract class Bullet : IInstancing<Bullet>, IPlainEntity
    {
        protected Bullet
            (
            PlainInstancingBuffer<Bullet> buffer,
            GeometryMotion movement,
            vec2d beginPosition,
            float angle,
            float scale
            )
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }
            GlobalBuffer = buffer;
            BufferId = buffer.CreateInstanceID();
            ((IInstancing<Bullet>)this).Scale = scale;
            ((IInstancing<Bullet>)this).Coord = new vec3d( beginPosition,0);
            ((IInstancing<Bullet>)this).Rotation = angle;
            Motion = movement;
        }

        public int BufferId
        {
            get;
            set;
        }

        public vec2d CenterPosition
        {
            get;
            set;
        }

        public PlainInstancingBuffer<Bullet> GlobalBuffer
        {
            get;
            set;
        }

        public GeometryMotion Motion 
        {
            get; 
            set; 
        }

        vec3d IInstancing<Bullet>.CenterPositionGlobal 
        { 
            get;
            set; 
        }

        public bool CollideWith(IPlainEntity entity)
        {
            throw new System.NotImplementedException();
        }

    }
}