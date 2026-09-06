// -----------------------------------------------------------------------------
// file="Camera"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics.Numeric.Graphics;
using System;
using System.Numerics;

namespace Nitload.Mathematics.Numeric.Graphics
{
    public sealed class Camera
    {

        private bool _isLookAtAvailable;
        internal GraphicsMatrix _lookAt;
        internal GraphicsMatrix _projection;
        internal GraphicsMatrix cameraAtt;
        internal GraphicsMatrix rotation;
        internal GraphicsMatrix rotationAtt;
        internal Radius _pitch;
        internal Radius _row;
        internal Radius _yaw;
        internal Vec3<float> _target, up;

        public Camera()
        {
            cameraAtt = GraphicsMatrix.Unit;
            _target = Vec3<float>.Zero;
            up = Vec3<float>.Zero;
            _isLookAtAvailable = false;
            _lookAt = GraphicsMatrix.Unit;
            rotationAtt = GraphicsMatrix.Unit;
            _projection = new GraphicsMatrix();
        }

        public Camera(
               Vec3<float> position,
               Vec3<float> target,
               Vec3<float> up
        )
            : this()
        {
            rotationAtt = GraphicsMatrix.Unit;
            cameraAtt = new GraphicsMatrix();
            SetViewDirection(position, target, up);
            _pitch = Radius.Zero;
            _row = Radius.Zero;
            _yaw = Radius.Zero;
        }

        public GraphicsMatrix Projection => _projection;

        public GraphicsMatrix View
        {
            get
            {
                if (_isLookAtAvailable) {
                    return _lookAt;
                }
                GraphicsMatrix move = GraphicsMatrix.Unit;
                move.column[3] -= cameraAtt.column[3];
                _lookAt = rotationAtt.Transpose * move;
                _isLookAtAvailable = true;
                return _lookAt;
            }
        }

        public GraphicsMatrix CameraMatrix()
        {
            return Projection * View;
        }

        public void MoveNear(
                    float distance
        )
        {
            cameraAtt.column[3].Z -= distance;
            _isLookAtAvailable = false;
        }

        public void MoveRight(
                    float distance
        )
        {
            cameraAtt.column[3].X -= distance;
            _isLookAtAvailable = false;
        }

        public void MoveUp(
                    float distance
        )
        {
            cameraAtt.column[3].Y -= distance;
            _isLookAtAvailable = false;
        }

        public void SetViewDirection(
                    Vec3<float> position,
                    Vec3<float> target,
                    Vec3<float> up
        )
        {
            Vec3<float> direction = position - target;
            if ((position == cameraAtt.column[3].XYZ) &&
                (this.up == up) &&
                (cameraAtt.column[3].XYZ == direction)) {
                return;
            }
            if (direction.GetLength() == 0)
            {
                // deadlock
                throw new ArgumentException("Direction bust is not zero.");
            }
            if (Vec3.IsParallel(direction, up)) {
                throw new ArgumentException("Direction and UP is on on same way");
            }
            _target = direction;
            this.up = up;

            _isLookAtAvailable = false;

            direction.Orthogonalize(ref up);

            Vec3<float> right = Linear.Orthogonalize(Vec3.Cross(up, direction));

            cameraAtt.column[0].XYZ = right;
            cameraAtt.column[1].XYZ = up;
            cameraAtt.column[2].XYZ = direction;
            cameraAtt.column[3].XYZ = position;

            InternalPitch();
            InternalRow();
            InternalYaw();
        }

        public void SetViewRange(
                    Radius fovRadius,
                    Vec2<float> size,
                    Vec2<float> offset,
                    (float front, float back) depthRange
        )
        {
            float
                distance = _target.GetLength(),
                near = distance - depthRange.front,
                far = distance + depthRange.back,
                offsetX = offset.X,
                offsetY = offset.Y,
                width = MathF.Abs(GeometryScaler.Tan(fovRadius / 2) * near * 2),
                height = (width * size.Y) / size.X;

            _projection.column[0].X = (2 * near) / width;
            _projection.column[1].Y = (2 * near) / height;
            _projection.column[2].X = (2 * offsetX) / width;
            _projection.column[2].Y = (2 * offsetY) / height;
            _projection.column[2].Z = (far + near) / (near - far);
            _projection.column[2].W = -1;
            _projection.column[3].Z = (2 * near * far) / (near - far);
        }

        public void Test(
                    params Vec4<float>[] vec
        )
        {
            Console.WriteLine(View);
            Console.WriteLine(Projection);
            Console.WriteLine(Projection * View);
            Console.WriteLine(View);
            foreach (Vec4<float> item in vec)
            {
                // Console.Write($"{Projection * View * item};");
            }
        }

        #region rotation

        public void Yaw(
                    Radius r
        )
        {
            _yaw -= r;

            // Console.WriteLine(_yaw._radius);
            // Console.WriteLine(rotationAtt);
            InternalYaw();
        }

        public void Pitch(
                    Radius r
        )
        {
            _pitch -= r;

            // Console.WriteLine(_pitch._radius);
            // Console.WriteLine(rotationAtt);
            InternalPitch();
        }

        public void Row(
                    Radius r
        )
        {
            _row -= r;

            // Console.WriteLine(_row._radius);
            // Console.WriteLine(rotationAtt);
            InternalRow();
        }

        internal void InternalPitch()
        {
            /*
            _isLookAtAvailable = false;

            Matrix32 partialCoord = new Matrix32(cameraAtt.colum1.vec, cameraAtt.colum2.vec);
            float angle = _pitch._radius;

            Matrix22 rotation = new Matrix22(
                Scaler.Cos(angle), -Scaler.Sin(angle), Scaler.Sin(angle), Scaler.Cos(angle));

            partialCoord *= rotation;
            rotationAtt.colum1.reg = partialCoord.colum0.reg;
            rotationAtt.colum2.reg = partialCoord.colum1.reg;
            rotationAtt.isTransposed = false;
            /**/
        }

        internal void InternalRow()
        {
            /*
            _isLookAtAvailable = false;

            Matrix32 partialCoord = new Matrix32(cameraAtt.colum0.vec, cameraAtt.colum1.vec);
            float angle = _row._radius;

            Matrix22 rotation = new Matrix22(
                Scaler.Cos(angle), -Scaler.Sin(angle), Scaler.Sin(angle), Scaler.Cos(angle));

            partialCoord *= rotation;
            rotationAtt.colum0.reg = partialCoord.colum0.reg;
            rotationAtt.colum1.reg = partialCoord.colum1.reg;
            rotationAtt.isTransposed = false;
        */
        }

        internal void InternalYaw()
        {
            /*
            _isLookAtAvailable = false;

            Matrix32 partialCoord = new Matrix32(cameraAtt.colum0.vec, cameraAtt.colum2.vec);
            float angle = _yaw._radius;
            Matrix22 rotation = new Matrix22(
                Scaler.Cos(angle), -Scaler.Sin(angle), Scaler.Sin(angle), Scaler.Cos(angle));

            partialCoord *= rotation;
            rotationAtt.colum0.reg = partialCoord.colum0.reg;
            rotationAtt.colum2.reg = partialCoord.colum1.reg;
            rotationAtt.isTransposed = false;
        */
        }

        #endregion
    }
}
