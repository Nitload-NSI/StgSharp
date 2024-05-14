//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="IInstancing.cs"
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
    public interface IInstancing<T>  where T : IInstancing<T>
    {

        public int BufferId
        {
            get;
            set;
        }

        public vec3d CenterPositionGlobal
        {
            get;
            set;
        }

        public vec3d Coord
        {
            get { return GlobalBuffer.CoordList[BufferId].XYZ; }
            set { GlobalBuffer.CoordList[BufferId] = new vec4d( value.vec); }
        }

        public PlainInstancingBuffer<T> GlobalBuffer
        {
            get;
            internal set;
        }

        public float Rotation
        {
            get { return GlobalBuffer.CoordList[BufferId].W; }
            set
            {
                vec4d temp = GlobalBuffer.CoordList[BufferId];
                temp.W = value;
                GlobalBuffer.CoordList[BufferId] = temp;
            }
        }

        public float Scale
        {
            get { return GlobalBuffer.ScalingList[BufferId]; }
            set { GlobalBuffer.ScalingList[BufferId] = value; }
        }

        public GeometryMotion Motion
        {
            get; set;
        }

        public void Move()
        {
            (vec3d coord , float rotation) =(Coord, Rotation);
            Motion.RunMotion(ref coord, ref rotation);
            (Coord,Rotation) = (coord, rotation);
        }

    }

    public class ParticleOnPlain : IInstancing<ParticleOnPlain>
    {

        public ParticleOnPlain(PlainInstancingBuffer<ParticleOnPlain> buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(paramName: nameof(buffer));
            }
            GlobalBuffer = buffer;
            BufferId = buffer.CreateInstanceID();
            buffer.InstanceList.Add(this);
        }

        public int BufferId
        {
            get;
            set;
        }

        public PlainInstancingBuffer<ParticleOnPlain> GlobalBuffer
        {
            get;
            set;
        }
        public GeometryMotion Motion 
        { 
            get; 
            set;
        }

        vec3d IInstancing<ParticleOnPlain>.CenterPositionGlobal
        {
            get;
            set;
        }

    }
}
