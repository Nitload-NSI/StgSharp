// -----------------------------------------------------------------------------
// file="Cordinate"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Geometries
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PartialPlainCoordinate
    {
        /*
        internal Matrix32 axis;

        /// <summary>
        ///   The _origin of this coordinate.
        /// </summary>
        public Vec3 origin;

        public PartialPlainCoordinate( Vec3 origin, Vec3 horiziontial, Vec3 vertical )
        {
            this.origin = origin;
            axis = new Matrix32();
            Unsafe.SkipInit( out axis.mat );
            axis.colum0 = horiziontial;
            axis.colum1 = vertical;
        }

        /// <summary>
        ///   The horizontial axis of the coordinate. Point to the right direction.
        /// </summary>
        public Vec3 axis_H => axis.colum0;

        /// <summary>
        ///   The vertical axis of the coordinate. Point to the top direction.
        /// </summary>
        public Vec3 axis_V => axis.colum1;

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public Vec3 GetRealLocate( Vec2 raletiveLocation )
        {
            return origin + axis.VerticalVecMultiply( raletiveLocation );
        }
        */

    }
}
