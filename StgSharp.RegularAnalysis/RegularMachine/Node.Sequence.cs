using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis
{
    internal sealed class RegularSequenceNode : RegularStateNode
    {

        private string _sequence;

        public override NodeType Type => NodeType.SEQUENCE;

        public override bool IsValidateFrom(
                             RegularStateNode former,
                             RegularContext context
        )
        {
            int pos = context.Position + 1;
            return context.CurrentCharSpan[pos.._sequence.Length].SequenceEqual(_sequence);
        }

    }
}
