//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpressStepUntil.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the ¡°Software¡±), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED ¡°AS IS¡±, 
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
using StgSharp.Modeling.Step;
using StgSharp.Modeling.Step;
using StgSharp.Script;
using StgSharp.Script.Express;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StgSharp.Modeling.Step
{
    public class ExpressStepUntil : ExpSchema
    {

        private FrozenDictionary<string, ExpTypeSource> entitiesDefine;

        public ExpressStepUntil()
        {
            Span<KeyValuePair<string, ExpTypeSource>> source = [
                new KeyValuePair<string, ExpTypeSource>( StepItemTypeExtensions.AdvancedFaceText,
                                                          ),
                ];
            Dictionary<string, ExpTypeSource> tIndex = new Dictionary<string, ExpTypeSource>();
        }

        internal StepModel Current { get; set; }

        public override void LoadFromSource( ExpSchemaSource source )
        {
            return;
        }

        public override bool TryGetConst( string name, out ExpElementInstance c )
        {
            c = null!;
            return false;
        }

        public override bool TryGetEntity( string name, out ExpEntitySource e )
        {
            throw new NotImplementedException();
        }

        public override bool TryGetFunction( string name, out ExpFunctionSource f )
        {
            throw new NotImplementedException();
        }

        public override bool TryGetProcedure( string name, out ExpProcedureSource p )
        {
            throw new NotImplementedException();
        }

        public override bool TryGetRule( string name, out ExpRuleSource r )
        {
            throw new NotImplementedException();
        }

        public override bool TryGetSchemaInclude( string name, out ExpSchema include )
        {
            throw new NotImplementedException();
        }

        public override bool TryGetType( string name, out ExpTypeSource t )
        {
            throw new NotImplementedException();
        }

    }
}