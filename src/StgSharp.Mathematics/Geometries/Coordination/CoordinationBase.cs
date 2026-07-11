// -----------------------------------------------------------------------------
// file="CoordinationBase"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics.Graphics;

using System.Runtime.CompilerServices;

namespace StgSharp.Geometries
{
    public abstract class CoordinationBase
    {

        private bool globalCoordAvailable;
        private CoordinationBase localCoordination;
        private GraphicsMatrix coordMat;

        public CoordinationBase(
               CoordinationBase localCoordination
        )
        {
            this.localCoordination = localCoordination;
            coordMat = new GraphicsMatrix();
        }

        public virtual Point LocalOrigin
        {
            get => new Point(coordMat.column[3].XYZ);
            internal set => coordMat.column[3].XYZ = value.Coord;
        }

        public virtual Vec3 LocalX
        {
            get => coordMat.column[3].XYZ;
            internal set => coordMat.column[3].XYZ = value;
        }

        public virtual Vec3 LocalY
        {
            get => coordMat.column[3].XYZ;
            internal set => coordMat.column[3].XYZ = value;
        }

        public virtual Vec3 LocalZ
        {
            get => coordMat.column[3].XYZ;
            set => coordMat.column[3].XYZ = value;
        }

        protected ref GraphicsMatrix CoordMat => ref coordMat;

    }
}
