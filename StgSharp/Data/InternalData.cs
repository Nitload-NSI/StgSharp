using StgSharp;

using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp
{
    internal partial class InternalIO
    {
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

        #region ControllingItemRender

        internal const string ControlItemRenderVertexShader="""
            


            """;

        #endregion
    }
}
