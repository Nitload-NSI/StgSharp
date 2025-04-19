//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpEntity.cs"
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
using System.Text;

namespace StgSharp.Script.Express
{
    public class ExpEntitySource : ExpInstantiableElement
    {

        private AbstractSyntaxTree<ExpSyntaxNode, IExpElementSource> _expressionTree;
        private ScriptSourceTransmitter _source;
        private string _name;

        public ExpEntitySource( string name, ScriptSourceTransmitter provider )
        {
            _name = name;
            _source = provider;
        }

        public override ExpElementType ElementType => ExpElementType.Entity;

        public override IScriptSourceProvider SourceProvider => _source;

        public override string Name => _name;

        public override void Analyse()
        {
            throw new NotImplementedException();
        }

        public override ExpSyntaxNode CreateInstanceNode( Token t )
        {
            throw new NotImplementedException();
        }

        public override bool TryGetMember( string name, out ExpSyntaxNode memberNode )
        {
            throw new NotImplementedException();
        }

    }
}
