using StgSharp.Graphics;
using StgSharp.Logic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Stg
{
    public class CollisionTexture : glRenderStream
    {
        string vertexShader = "";

        string fragmentShader = "";

        public CollisionTexture(RenderStreamConstructArgs args) : base(args)
        {
        }

        private FrameBuffer fbo;

        protected override string[] InputPortsName =>
            [
            "InstanceBufferList",
            ];

        protected override string[] OutputPortsName => 
            [
            "CollisionTextureOutput"
            ];


        protected override void Deinit()
        {
            throw new NotImplementedException();
        }

        protected override void Init()
        {
            throw new NotImplementedException();
        }

        protected override void Main(
            in Dictionary<string, BlueprintPipeline> input, 
            in Dictionary<string, BlueprintPipeline> output)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessInput()
        {
            throw new NotImplementedException();
        }
    }
}
