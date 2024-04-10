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
using StgSharp.Graphics.ShaderEdit;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphic
{
    public sealed class Camera : IglConvertable
    {

        private Uniform<Matrix44> convertedUniform ;

        internal bool _isLookAtAvailable;
        internal Matrix44 cameraAtt;
        internal vec3d _target, up, cameraCoord;
        internal Matrix44 _lookAt;
        internal Matrix44 _projection;

        public Camera()
        {
            cameraAtt = Matrix44.Unit;
            _target = Vec3d.Zero;
            up = Vec3d.Zero;
            cameraCoord = new vec3d(0,0,1);
            _isLookAtAvailable = false;
            _lookAt = Matrix44.Unit;
            _projection = new Matrix44();
        }

        public Camera(vec3d position, vec3d target, vec3d up) : this()
        {
            SetViewDirection(position, target, up);
        }

        public Matrix44 View
        {
            get
            {
                if (_isLookAtAvailable)
                {
                    return _lookAt;
                }
                Matrix44 move = Matrix44.Unit;
                move.Colum3.vec -= cameraCoord.vec;
                _lookAt = cameraAtt.Transpose * move;
                _isLookAtAvailable = true;
                return _lookAt;
            }
        }

        public Matrix44 Projection
=> _projection;

        public void MoveNear(float distance)
        {
            cameraCoord.Z -= distance;
        }

        public void MoveRight(float distance)
        {
            cameraCoord.X -= distance;
        }

        public void MoveUp(float distance)
        {
            cameraCoord.Y -= distance;
        }

        public void SetViewRange(Radius fovRadius, vec2d size, vec2d offset, (float front, float back) dephRange)
        {
            float 
                distance = _target.GetLength(),
                near = distance - dephRange.front,
                far = distance + dephRange.back,
                offsetX = offset.X,
                offsetY = offset.Y,
                width = Scaler.Abs(GeometryScaler.Tan(fovRadius / 2) * near *2),
                height = width * size.Y / size.X;

            _projection.Colum0.X = 2 * near / width;
            _projection.Colum1.Y = 2 * near / height;
            _projection.Colum2.X = 2 * offsetX / width;
            _projection.Colum2.Y = 2 * offsetY / height;
            _projection.Colum2.Z = (far + near) / (near - far);
            _projection.Colum2.W = -1;
            _projection.Colum3.Z = 2 * near * far / (near - far);

        }

        public void Test(params vec4d[] vec)
        {
            Console.WriteLine(View);
            Console.WriteLine(Projection);
            Console.WriteLine(Projection * View);
            foreach (var item in vec)
            {
                Console.Write($"{Projection * View * item};");
            }
        }

        public void Pitch(Radius r)
        {
            Matrix32 partialCoord = new Matrix32(
                cameraAtt.Colum1.vec,
                cameraAtt.Colum2.vec
                );
            Matrix22 rotation = new Matrix22(
                GeometryScaler.Cos(r), -GeometryScaler.Sin(r),
                GeometryScaler.Sin(r), -GeometryScaler.Cos(r)
                );

            partialCoord *= rotation;
            cameraAtt.Colum1.vec = partialCoord.colum0.vec;
            cameraAtt.Colum2.vec = partialCoord.colum1.vec;
        }

        public void SetViewDirection(vec3d position, vec3d target, vec3d up)
        {
            vec3d direction = position - target;
            if (position == cameraCoord && this.up == up && cameraAtt.Colum2.vec == direction.vec)
            {
                //no difference with precious value
                return;
            }
            if (direction.GetLength() == 0)
            {
                //deadlock
                throw new ArgumentException();
            }
            if (Vec3d.IsParallel(direction, up))
            {
                throw new ArgumentException();
            }
            _target = direction;
            cameraCoord = position;
            this.up = up;

            _isLookAtAvailable = false;

            Linear.Normalize(ref direction, ref up);

            vec3d right = Linear.Normalize(Linear.Cross(up,direction));

            cameraAtt.Colum0.vec = right.vec;
            cameraAtt.Colum1.vec = up.vec;
            cameraAtt.Colum2.vec = direction.vec;

        }

        public void DisplayGLtypeDefinition()
        {
            Console.WriteLine("uniform matrix4x4 cameraName;");
        }

        /// <summary>
        /// Get all uniform related to this <see cref="Camera"/>.
        /// </summary>
        /// <param name="source">Shader program requires this camera.</param>
        /// <param name="uniformName">
        /// Name of all related uniforms.
        /// If you fallow form from <see cref="IglConvertable.DisplayGLtypeDefinition"/>
        /// </param>
        /// <exception cref="ArgumentException"></exception>
        public unsafe void GainAllUniforms(ShaderProgram source, params string[] uniformName)
        {
            if (uniformName.Length!=1)
            {
                throw new ArgumentException();
            }
            convertedUniform = source.GetUniform<Matrix44>(uniformName[0]);
        }

        ShaderStruct IglConvertable.GetConvertedGLtype()
        {
            throw new NotImplementedException();
        }

        public unsafe void SetAllUniforms()
        {
            GL.SetValue(convertedUniform, Projection * View);
        }

        ~Camera()
        {
        }

    }
}
