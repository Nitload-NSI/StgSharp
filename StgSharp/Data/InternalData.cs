//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="InternalData.cs"
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
using StgSharp;

using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp
{
    internal partial class InternalIO
    {

        #region ControllingItemRender

        internal const string ControlItemRenderVertexShader = """
            


            """;

        #endregion

        #region CollisionTexture

        internal const string CollisionTextureVertexShader = """
            #version 430 core
            layout(location = 0) in vec2 localCollisionCoord;
            layout(location = 1) in vec2 spaceCoord;
            layout(location = 2) in float rotation;
            layout(location = 3) in float scaling;
            
            uniform vec2 target;
            uniform mat2 projection;
            
            void main()
            {
                mat2 scaling = mat2(
                scaling,0,
                0,scaling );

                mat2 rotation = mat2(
                cos(rotation),-sin(rotation),
                sin(rotation),cos(rotation) );

                vec2 realCoord = (rotation * scaling * localCollisionCoord) + spaceCoord - target;
                realCoord = projection * realCoord;
                float dist = distance(realCoord, vec2(0,0));
                float angle = atan(realCoord.y, realCoord.x);
                gl_Position = vec4(angle, realCoord.y, dist, 1);
            }
            """;
        internal const string CollisionTextureFragmentShader = """
            #version 430 core
            out vec4 Color;
            
            uniform float range;
            void main()
            {
                float dist = gl_FragCoord.z;
                if(dist > range)
                {
                    discard;
                }
                dist /= range;
                gl_FragDepth = min(dist, gl_FragDepth);
                Color = vec4(dist,0,0,1);
            }
            """;

    #endregion
    }
}
