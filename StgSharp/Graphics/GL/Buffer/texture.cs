using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public unsafe class Texture
    {
        internal uint[] textureId;

        public Texture(int n)
        {
            textureId = new uint[n];
            fixed (uint* id = textureId)
            {
                GL.gl.GenTextures(n, id);
            }
        }

        public void Bind(int index)
        {
            GL.gl.BindTexture(GLconst.PROXY_TEXTURE_2D, textureId[index]);
        }

    }
}
