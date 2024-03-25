//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PlainGeometry.cs"
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
using StgSharp.Graphics;
using StgSharp.Math;

namespace StgSharp.Geometries
{
    /// <summary>
    /// 平面几何体。包含三角形，对称四边形，自由多边形等。最多可由16个点参数定义。
    /// </summary>
    public abstract class PlainGeometry
    {

        protected TimeSpanProvider time;

        internal PartialPlainCoordinate coordinate;
        internal Matrix2x4 textureFrame;

        public PlainGeometry()
        {
        }

        /// <summary>
        /// Tick time this geometry eas crested
        /// </summary>
        public int BornTime => time;

        public PartialPlainCoordinate ReferenceCoordinate => this.coordinate;

        internal abstract int[] Indices { get; }

        /// <summary>
        /// Get all sides of this <see cref="PlainGeometry"/>.
        /// </summary>
        /// <returns>A array contains all straight <see cref="Line"/>s of current geometry item.</returns>
        public abstract Line[] GetAllSides();

        /// <summary>
        /// Get a flat <see cref="Plain"/> cincide with current geometry.
        /// </summary>
        /// <returns></returns>
        public abstract Plain GetPlain();


        internal abstract void UpdateCoordinate(int tick);

        //public abstract Rectangle GetRenderFrame();
    }
}