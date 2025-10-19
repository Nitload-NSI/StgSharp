//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Bullet"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
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
using StgSharp.Geometries;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphic;

using System;

namespace StgSharp.Gaming
{
    public abstract class Bullet : IInstancing, IPlainEntity
    {

        protected Bullet(
                  PlainInstancingBuffer<Bullet> buffer,
                  GeometryMotion movement,
                  Vec2 beginPosition,
                  float angle,
                  float scale)
        {
            ArgumentNullException.ThrowIfNull(buffer);
            GlobalBuffer = buffer;
            BufferId = buffer.CreateInstanceID();
            ((IInstancing)this).Scale = scale;
            ((IInstancing)this).Coord = new Vec3(beginPosition, 0);
            ((IInstancing)this).Rotation = angle;
            Motion = movement;
        }

        public GeometryMotion Motion { get; set; }

        public IInstancingBuffer GlobalBuffer { get; set; }

        public int BufferId { get; set; }

        public Vec2 CenterPosition { get; set; }

        public bool CollideWith(IPlainEntity entity)
        {
            throw new System.NotImplementedException();
        }

        Vec3 IInstancing.CenterPositionGlobal
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

    }
}