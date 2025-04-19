//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="_ExpNode.cs"
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
using CommunityToolkit.HighPerformance.Buffers;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Script.Express
{
    public abstract class ExpSyntaxNode : ISyntaxNode<ExpSyntaxNode, IExpElementSource>
    {

        protected internal ExpNodeFlag _nodeFlag;
        protected internal Token _source;

        protected ExpSyntaxNode( Token source )
        {
            _source = source;
            Next = this;
            Previous = this;
        }

        public bool IsNumber
        {
            get { return ( _nodeFlag | ExpNodeFlag.BuiltinType_Number ) != 0; }
        }

        public abstract ExpSyntaxNode Left { get; }

        public static ExpSyntaxNode Empty
        {
            get => ExpEmptyNode._only;
        }

        public ExpSyntaxNode Next { get; set; }

        public abstract ExpSyntaxNode Right { get; }

        public ExpSyntaxNode Previous { get; set; }

        public abstract IExpElementSource EqualityTypeConvert { get; }

        public int Line => _source.Line;

        public int Column => _source.Column;

        public long NodeFlag => ( long )_nodeFlag;

        public string CodeConvertTemplate { get; init; }

        public Token Source
        {
            get => _source;
        }

        public void AppendNode( ExpSyntaxNode nextNode )
        {
            Next.Previous = nextNode;
            nextNode.Next = Next;
            Next = nextNode;
            Next.Previous = this;
        }

        public static bool IsNullOrEmpty( [AllowNull]ExpSyntaxNode node )
        {
            return  node == ExpEmptyNode._only || node == null;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static ExpSyntaxNode NonOperation( Token source )
        {
            return new ExpEmptyNode( source.Line, source.Column );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static ExpSyntaxNode NonOperation( int line, int column )
        {
            return new ExpEmptyNode( line, column );
        }

        public void PrependNode( ExpSyntaxNode previousNode )
        {
            Previous.Next = previousNode;
            previousNode.Previous = Previous;
            Previous = previousNode;
            previousNode.Next = this;
        }

        public static bool VerifyTypeConvertable( ExpSyntaxNode left, ExpSyntaxNode right )
        {
            IExpElementSource tLeft = left.EqualityTypeConvert,
                tRight = right.EqualityTypeConvert;
            if( tLeft is not ExpInstantiableElement tLeftIns || tRight is not ExpInstantiableElement tRightIns ) {
                return false;
            }
            if( ExpInstantiableElement.IsNullOrVoid( tLeftIns ) || ExpInstantiableElement.IsNullOrVoid(
                tLeftIns ) ) {
                return false;
            }
            return tLeft.IsConvertable( tRight );
        }

        private class ExpEmptyNode : ExpSyntaxNode
        {

            internal static readonly ExpEmptyNode _only = new();

            public ExpEmptyNode() : base( Token.Empty ) { }

            public ExpEmptyNode( int line, int column )
                : base( new Token( string.Empty, line, column, TokenFlag.None ) ) { }

            public override ExpInstantiableElement EqualityTypeConvert => ExpInstantiableElement.Void;

            public override ExpSyntaxNode Left => _only;

            public override ExpSyntaxNode Right => _only;

        }

    }
}
