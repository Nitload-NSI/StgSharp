﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ParticleOnPlain.cs"
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
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Geometries
{
    public class ParticleOnPlain : IInstancing, IPlainEntity
    {

        public ParticleOnPlain( PlainInstancingBuffer<ParticleOnPlain> buffer )
        {
            if( buffer == null ) {
                throw new ArgumentNullException( paramName: nameof( buffer ) );
            }
            GlobalBuffer = buffer;
            BufferId = buffer.CreateInstanceID();
            ( ( IInstancingBuffer )buffer ).InstanceList.Add( this );
        }

        public GeometryMotion Motion { get; set; }

        public IInstancingBuffer GlobalBuffer { get; set; }

        public int BufferId { get; set; }

        Vec3 IInstancing.CenterPositionGlobal { get; set; }

        bool IPlainEntity.CollideWith( IPlainEntity entity )
        {
            throw new NotImplementedException();
        }

        Vec2 IPlainEntity.CenterPosition
        {
            get => GlobalBuffer.CoordAndRotationList[ BufferId ].XY;
            set => throw new NotImplementedException();
        }

    }
}
