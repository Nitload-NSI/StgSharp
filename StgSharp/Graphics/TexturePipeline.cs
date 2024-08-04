using StgSharp.Logic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public class TexturePipeline:BlueprintPipelineArgs
    {
        public TexturePipeline(Image i)
        {
            _value = i;
        }

        private Image _value;

        public new Image Value
        {
            get => _value;
            set => _value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
