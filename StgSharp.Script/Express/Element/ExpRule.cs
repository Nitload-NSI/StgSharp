//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpRule.cs"
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace StgSharp.Script.Express
{
    public class ExpRule : ExpElementInstance
    {

        private ExpRuleSource _source;

        public ExpRule( Token source ) : base( source ) { }

        public object? value => throw new NotImplementedException();

        public override ExpInstantiableElement EqualityTypeConvert => throw new NotImplementedException(
            );

        public override ExpSyntaxNode Left => throw new NotImplementedException();

        public override ExpSyntaxNode Right => throw new NotImplementedException();

        public override string TypeName => _source.Name;

    }

    public class ExpRuleSource : ExpImmutableElement
    {

        private ScriptSourceTransmitter _transmitter;
        private string _name;

        public ExpRuleSource( string name, ScriptSourceTransmitter transmitter )
        {
            _name = name;
            _transmitter = transmitter;
        }

        public override ExpElementType ElementType => ExpElementType.Rule;

        public override IScriptSourceProvider SourceProvider => _transmitter;

        public override string Name => _name;

        public override void Analyse()
        {
            throw new NotImplementedException();
        }

        public override ExpSyntaxNode MakeReference( params object[] options )
        {
            throw new NotImplementedException();
        }

        public override bool TryGetMember( string name, out ExpSyntaxNode memberNode )
        {
            memberNode = ExpSyntaxNode.Empty;
            return false;
        }

    }
}
