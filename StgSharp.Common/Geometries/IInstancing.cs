//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="IInstancing.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Mathematics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Geometries
{
    public interface IInstancing
    {

        public float Rotation { get { return GlobalBuffer.CoordAndRotationList[BufferId].W; } set
                                                                                              {
                                                                                                  Vec4 temp = GlobalBuffer.CoordAndRotationList[
                                                                                                      BufferId];
                                                                                                  temp.W = value;
                                                                                                  GlobalBuffer.CoordAndRotationList[
                                                                                                      BufferId] = temp;
                                                                                              } }

        public float Scale { get { return GlobalBuffer.ScalingList[BufferId]; } set { GlobalBuffer.ScalingList[
                                                                                          BufferId] = value; } }

        public GeometryMotion Motion { get; set; }

        public IInstancingBuffer GlobalBuffer { get; internal set; }

        public int BufferId { get; internal set; }

        public Vec3 CenterPositionGlobal { get; internal set; }

        public Vec3 Coord { get { return GlobalBuffer.CoordAndRotationList[BufferId].XYZ; } set { GlobalBuffer.CoordAndRotationList[
                                                                                                      BufferId] = new Vec4(
                                                                                                      value.reg); } }

        public void Move()
        {
            (Vec3 coord, float rotation) = (Coord, Rotation);
            Motion.RunMotion(ref coord, ref rotation);
            (Coord, Rotation) = (coord, rotation);
        }

    }
}
