//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Camera.cs"
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
using StgSharp.Graphics.OpenGL;
using StgSharp.Graphics.ShaderEdit;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{
    public sealed class Camera : IglConvertable
    {

        private bool _isLookAtAvailable;
        internal Matrix44 _lookAt;
        internal Matrix44 _projection;
        internal Matrix44 cameraAtt;
        internal Matrix44 rotation;
        internal Matrix44 rotationAtt;
        internal Radius _pitch;
        internal Radius _row;
        internal Radius _yaw;
        internal Uniform<Matrix44> convertedUniform;
        internal Vec3 _target, up;

        public Camera()
        {
            cameraAtt = Matrix44.Unit;
            _target = Vec3.Zero;
            up = Vec3.Zero;
            _isLookAtAvailable = false;
            _lookAt = Matrix44.Unit;
            rotationAtt = Matrix44.Unit;
            _projection = new Matrix44();
        }

        public Camera( Vec3 position, Vec3 target, Vec3 up )
            : this()
        {
            rotationAtt = Matrix44.Unit;
            cameraAtt = new Matrix44();
            SetViewDirection( position, target, up );
            _pitch = Radius.Zero;
            _row = Radius.Zero;
            _yaw = Radius.Zero;
        }

        public Matrix44 Projection => _projection;

        public Matrix44 View
        {
            get
            {
                if( _isLookAtAvailable ) {
                    return _lookAt;
                }
                Matrix44 move = Matrix44.Unit;
                move.colum3.vec -= cameraAtt.colum3.vec;
                _lookAt = rotationAtt.Transpose * move;
                _isLookAtAvailable = true;
                return _lookAt;
            }
        }

        public void DisplayGLtypeDefinition()
        {
            Console.WriteLine( "uniform matrix4x4 cameraName;" );
        }

        /// <summary>
        /// Get all uniform related to this <see cref="Camera" />.
        /// </summary>
        /// <param name="source"> Shader program requires this camera. </param>
        /// <param name="uniformName">
        /// ContextName of all related uniforms. If you fallow form from <see
        /// cref="IglConvertable.DisplayGLtypeDefinition" />
        /// </param>
        /// <exception cref="ArgumentException"></exception>
        public unsafe void GainAllUniforms(
            ShaderProgram source,
            params string[] uniformName )
        {
            if( uniformName.Length != 1 ) {
                throw new ArgumentException(
                    "Camera needs only one uniform.", nameof( uniformName ) );
            }
            convertedUniform = source.GetUniform<Matrix44>( uniformName[ 0 ] );
        }

        public void MoveNear( float distance )
        {
            cameraAtt.colum3.Z -= distance;
            _isLookAtAvailable = false;
        }

        public void MoveRight( float distance )
        {
            cameraAtt.colum3.X -= distance;
            _isLookAtAvailable = false;
        }

        public void MoveUp( float distance )
        {
            cameraAtt.colum3.Y -= distance;
            _isLookAtAvailable = false;
        }

        public unsafe void SetAllUniforms()
        {
            //Console.WriteLine(Projection * ViewBase);
            OpenGL.OpenGLFunction.CurrentGL
                .SetUniformValue( convertedUniform, Projection * View );
        }

        public void SetViewDirection( Vec3 position, Vec3 target, Vec3 up )
        {
            Vec3 direction = position - target;
            if( ( position.vec == cameraAtt.colum3.vec ) && ( this.up == up ) && ( cameraAtt.colum2.vec == direction.vec ) ) {
                //no difference with precious value
                return;
            }
            if( direction.GetLength() == 0 ) {
                //deadlock
                throw new ArgumentException();
            }
            if( Vec3.IsParallel( direction, up ) ) {
                throw new ArgumentException();
            }
            _target = direction;
            this.up = up;

            _isLookAtAvailable = false;

            Linear.Orthogonalize( ref direction, ref up );

            Vec3 right = Linear.Orthogonalize( Linear.Cross( up, direction ) );

            cameraAtt.colum0.vec = right.vec;
            cameraAtt.colum1.vec = up.vec;
            cameraAtt.colum2.vec = direction.vec;
            cameraAtt.colum3.vec = position.vec;

            InternalPitch();
            InternalRow();
            InternalYaw();
        }

        public void SetViewRange(
            Radius fovRadius,
            Vec2 size,
            Vec2 offset,
            (float front, float back) dephRange )
        {
            float
                distance = _target.GetLength(),
                near = distance - dephRange.front,
                far = distance + dephRange.back,
                offsetX = offset.X,
                offsetY = offset.Y,
                width = Scaler.Abs(
                GeometryScaler.Tan( fovRadius / 2 ) * near * 2 ),
                height = ( width * size.Y ) / size.X;

            _projection.colum0.X = ( 2 * near ) / width;
            _projection.colum1.Y = ( 2 * near ) / height;
            _projection.colum2.X = ( 2 * offsetX ) / width;
            _projection.colum2.Y = ( 2 * offsetY ) / height;
            _projection.colum2.Z = ( far + near ) / ( near - far );
            _projection.colum2.W = -1;
            _projection.colum3.Z = ( 2 * near * far ) / ( near - far );
        }

        public void Test( params Vec4[] vec )
        {
            Console.WriteLine( View );
            Console.WriteLine( Projection );
            Console.WriteLine( Projection * View );
            Console.WriteLine( View );
            foreach( Vec4 item in vec ) {
                Console.Write( $"{Projection * View * item};" );
            }
        }

        ShaderStruct IglConvertable.GetConvertedGLtype()
        {
            throw new NotImplementedException();
        }

        ~Camera() { }

        #region rotation

        public void Yaw( Radius r )
        {
            _yaw -= r;

            //Console.WriteLine(_yaw._radius);
            //Console.WriteLine(rotationAtt);
            InternalYaw();
        }

        public void Pitch( Radius r )
        {
            _pitch -= r;

            //Console.WriteLine(_pitch._radius);
            //Console.WriteLine(rotationAtt);
            InternalPitch();
        }

        public void Row( Radius r )
        {
            _row -= r;

            //Console.WriteLine(_row._radius);
            //Console.WriteLine(rotationAtt);
            InternalRow();
        }

        internal void InternalPitch()
        {
            _isLookAtAvailable = false;

            Matrix32 partialCoord = new Matrix32(
                cameraAtt.colum1.vec, cameraAtt.colum2.vec );
            float angle = _pitch._radius;

            Matrix22 rotation = new Matrix22(
                Scaler.Cos( angle ), -Scaler.Sin( angle ), Scaler.Sin( angle ),
                Scaler.Cos( angle ) );

            partialCoord *= rotation;
            rotationAtt.colum1.vec = partialCoord.colum0.vec;
            rotationAtt.colum2.vec = partialCoord.colum1.vec;
            rotationAtt.isTransposed = false;
        }

        internal void InternalRow()
        {
            _isLookAtAvailable = false;

            Matrix32 partialCoord = new Matrix32(
                cameraAtt.colum0.vec, cameraAtt.colum1.vec );
            float angle = _row._radius;

            Matrix22 rotation = new Matrix22(
                Scaler.Cos( angle ), -Scaler.Sin( angle ), Scaler.Sin( angle ),
                Scaler.Cos( angle ) );

            partialCoord *= rotation;
            rotationAtt.colum0.vec = partialCoord.colum0.vec;
            rotationAtt.colum1.vec = partialCoord.colum1.vec;
            rotationAtt.isTransposed = false;
        }

        internal void InternalYaw()
        {
            _isLookAtAvailable = false;

            Matrix32 partialCoord = new Matrix32(
                cameraAtt.colum0.vec, cameraAtt.colum2.vec );
            float angle = _yaw._radius;
            Matrix22 rotation = new Matrix22(
                Scaler.Cos( angle ), -Scaler.Sin( angle ), Scaler.Sin( angle ),
                Scaler.Cos( angle ) );

            partialCoord *= rotation;
            rotationAtt.colum0.vec = partialCoord.colum0.vec;
            rotationAtt.colum2.vec = partialCoord.colum1.vec;
            rotationAtt.isTransposed = false;
        }

        #endregion
    }
}
