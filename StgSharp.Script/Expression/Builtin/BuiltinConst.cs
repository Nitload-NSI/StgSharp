using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Script.Expression
{

    internal sealed class ExpBuiltinConst : ExpConstSource
    {
        private readonly float _value;

        public ExpBuiltinConst(string name, float value)
            : base(name, null) { }

        public override sealed void Analyse()
        {
            return;
        }

    }

}
