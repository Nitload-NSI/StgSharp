// -----------------------------------------------------------------------------
// file="CoordinationBase"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics.Numeric.Graphics;

using System.Runtime.CompilerServices;

namespace Nitload.Geometries
{
    public abstract class CoordinationBase
    {

        private bool globalCoordAvailable;
        private CoordinationBase localCoordination;
        private GMatrix44<float> coordMat;

        public CoordinationBase(
               CoordinationBase localCoordination
        )
        {
            this.localCoordination = localCoordination;
            coordMat = new GMatrix44<float>();
        }

        public virtual Point LocalOrigin
        {
            get => new Point(new Vec3<float>(coordMat[3, 0], coordMat[3, 1], coordMat[3, 2]));
            internal set
            {
                coordMat[3, 0] = value.Coord.X;
                coordMat[3, 1] = value.Coord.Y;
                coordMat[3, 2] = value.Coord.Z;
            }
        }

        public virtual Vec3<float> LocalX
        {
            get => new Vec3<float>(coordMat[3, 0], coordMat[3, 1], coordMat[3, 2]);
            internal set
            {
                coordMat[3, 0] = value.X;
                coordMat[3, 1] = value.Y;
                coordMat[3, 2] = value.Z;
            }
        }

        public virtual Vec3<float> LocalY
        {
            get => new Vec3<float>(coordMat[3, 0], coordMat[3, 1], coordMat[3, 2]);
            internal set
            {
                coordMat[3, 0] = value.X;
                coordMat[3, 1] = value.Y;
                coordMat[3, 2] = value.Z;
            }
        }

        public virtual Vec3<float> LocalZ
        {
            get => new Vec3<float>(coordMat[3, 0], coordMat[3, 1], coordMat[3, 2]);
            set
            {
                coordMat[3, 0] = value.X;
                coordMat[3, 1] = value.Y;
                coordMat[3, 2] = value.Z;
            }
        }

        protected ref GMatrix44<float> CoordMat => ref coordMat;

    }
}
