﻿//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="CoordinationBase"
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
using StgSharp.Mathematics.Graphic;

using System.Runtime.CompilerServices;

namespace StgSharp.Geometries
{
    public abstract class CoordinationBase
    {

        private bool globalCoordAvailable;
        private CoordinationBase localCoordination;
        private Matrix44 coordMat;

        public CoordinationBase(CoordinationBase localCoordination)
        {
            this.localCoordination = localCoordination;
            coordMat = new Matrix44();
        }

        public virtual Point LocalOrigin
        {
            get => new Point(coordMat.column3.XYZ);
            internal set => coordMat.column3.XYZ = value.Coord;
        }

        public virtual Vec3 LocalX
        {
            get => coordMat.column0.XYZ;
            internal set => coordMat.column0.XYZ = value;
        }

        public virtual Vec3 LocalY
        {
            get => coordMat.column1.XYZ;
            internal set => coordMat.column1.XYZ = value;
        }

        public virtual Vec3 LocalZ
        {
            get => coordMat.column2.XYZ;
            set => coordMat.column2.XYZ = value;
        }

        protected ref Matrix44 CoordMat
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref coordMat;
        }

    }
}
