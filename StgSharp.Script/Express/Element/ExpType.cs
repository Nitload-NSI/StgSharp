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
    public class ExpTypeSource : ExpInstantiableElement
    {

        private ScriptSourceTransmitter _transmitter;
        private string _name;
        protected bool _isBuiltinType;

        protected ExpTypeSource( string name )
        {
            _name = name;
            _transmitter = ScriptSourceTransmitter.Empty;
            _isBuiltinType = true;
        }

        public ExpTypeSource( string name, ScriptSourceTransmitter transmitter )
        {
            _name = name;
            _transmitter = transmitter;
            _isBuiltinType = false;
        }

        public bool IsLanguageBuiltinType
        {
            get => _isBuiltinType;
        }

        public override ExpElementType ElementType => ExpElementType.Type;

        public ExpTypeSource Void
        {
            get => ExpVoidType.Only;
        }

        public override IScriptSourceProvider SourceProvider => _transmitter;

        public override string Name => _name;

        public override void Analyse()
        {
            throw new NotImplementedException();
        }

        public override ExpNode CreateInstanceNode()
        {
            throw new NotImplementedException();
        }

        public static bool IsNullOrVoid( IExpElementSource type )
        {
            return type == null || type == ExpVoidType.Only;
        }

        private class ExpVoidType : ExpTypeSource
        {

            private static readonly ExpVoidType _onlyInstance = new ExpVoidType(
                );

            private ExpVoidType() : base( string.Empty ) { }

            public static ExpVoidType Only
            {
                get => _onlyInstance;
            }

            public override void Analyse()
            {
                return;
            }

        }

    }
}