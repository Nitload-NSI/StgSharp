//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpType.cs"
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
using StgSharp.Script;

using System;
using System.Collections.Generic;

namespace StgSharp.Script.Express
{
    public class ExpType : ExpElementInstanceBase
    {

        public ExpType( string name, ExpSchema context )
            : base( name, context ) { }

        public override ExpBaseNode Left => throw new NotImplementedException();

        public override ExpBaseNode Right => throw new NotImplementedException();

        public override IExpElementSource EqualityTypeConvert => throw new NotImplementedException(
            );

        public override string TypeName => throw new NotImplementedException();

    }

    public class ExpTypeSource : IExpElementSource
    {

        private ScriptSourceTransmitter _transmitter;
        private string _name;

        public ExpTypeSource( string name, ScriptSourceTransmitter transmitter )
        {
            _name = name;
            _transmitter = transmitter;
        }

        public ExpElementType ElementType => ExpElementType.Type;

        public IScriptSourceProvider SourceProvider => _transmitter;

        public string Name => _name;

        public virtual void Analyse()
        {
            throw new NotImplementedException();
        }

        public bool IsConvertable( IExpElementSource targetType )
        {
            throw new NotImplementedException();
        }

    }
}