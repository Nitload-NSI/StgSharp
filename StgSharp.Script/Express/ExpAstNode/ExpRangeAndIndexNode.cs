//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpRangeAndIndexNode.cs"
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
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public class ExpIndexNode : ExpSyntaxNode
    {

        private const string _codeTemplate = @"[{0}]";

        private ExpSyntaxNode _index;

        internal ExpIndexNode( Token t, ExpSyntaxNode node )
            : base( t )
        {
            _index = node;
            CodeConvertTemplate = _codeTemplate;
        }

        public override ExpSyntaxNode Left => _index;

        public override ExpSyntaxNode Right => Empty;

        public override IExpElementSource EqualityTypeConvert => throw new NotImplementedException();

        public static ExpIndexNode IndexAt( Token position, ExpSyntaxNode expression )
        {
            ArgumentNullException.ThrowIfNull( expression );
            if( ExpNodeFlag.BuiltinType_Int.IsNotInFlag( expression.NodeFlag ) ) {
                throw new ExpInvalidTypeException(
                    position, "Integer", expression.EqualityTypeConvert.Name );
            }
            return new ExpIndexNode( position, expression );
        }

    }

    public class ExpRangeNode : ExpSyntaxNode
    {

        private const string _codeTemplate = "[{0}..{1}]";

        private ExpSyntaxNode _begin, _end;

        internal ExpRangeNode( Token t, ExpSyntaxNode begin, ExpSyntaxNode end )
            : base( t )
        {
            _begin = begin;
            _end = end;
            CodeConvertTemplate = _codeTemplate;
        }

        public override ExpSyntaxNode Left => _begin;

        public override ExpSyntaxNode Right => _end;

        public override IExpElementSource EqualityTypeConvert => ExpInstantiableElement.Void;

        public static ExpRangeNode RangeNode(
                                   Token position,
                                   ExpSyntaxNode beginNode,
                                   ExpSyntaxNode endNode )
        {
            ArgumentNullException.ThrowIfNull( beginNode );
            ArgumentNullException.ThrowIfNull( endNode );
            if( ExpNodeFlag.BuiltinType_Int.IsNotInFlag( beginNode.NodeFlag ) ) {
                throw new ExpInvalidTypeException(
                    position, "Integer", beginNode.EqualityTypeConvert.Name );
            }
            if( ExpNodeFlag.BuiltinType_Int.IsNotInFlag( endNode.NodeFlag ) ) {
                throw new ExpInvalidTypeException(
                    position, "Integer", endNode.EqualityTypeConvert.Name );
            }
            return new ExpRangeNode( position, beginNode, endNode );
        }

    }

    public class ExpCollectionExpressionNode : ExpSyntaxNode
    {

        internal ExpCollectionExpressionNode( Token t, ExpTupleNode tupleNode ) : base( t ) { }

        public override ExpSyntaxNode Left => throw new NotImplementedException();

        public override ExpSyntaxNode Right => throw new NotImplementedException();

        public override IExpElementSource EqualityTypeConvert => throw new NotImplementedException();

    }
}
