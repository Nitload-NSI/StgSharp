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
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Script.Express
{
    public abstract class ExpNode : IASTNode<ExpNode, IExpElementSource>
    {

        protected internal ExpNodeFlag _nodeFlag;
        protected internal string _name;

        protected ExpNode( string name )
        {
            _name = name;
            Next = this;
            Previous = this;
        }

        public bool IsNumber
        {
            get { return ( _nodeFlag | ExpNodeFlag.BuiltinType_Number ) != 0; }
        }

        public abstract ExpNode Left
        {
            get;
        }

        public static ExpNode Empty
        {
            get => ExpEmptyNode._only;
        }

        public ExpNode Next
        {
            get;
            set;
        }

        public abstract ExpNode Right
        {
            get;
        }

        public ExpNode Previous
        {
            get;
            set;
        }

        public abstract IExpElementSource EqualityTypeConvert
        {
            get;
        }

        public long NodeFlag => ( long )_nodeFlag;

        public string CodeConvertTemplate
        {
            get;
            init;
        }

        public string Name => _name;

        public void AppendNode( ExpNode nextNode )
        {
            Next.Previous = nextNode;
            nextNode.Next = Next;
            Next = nextNode;
            Next.Previous = this;
        }

        public ExpNodeNextEnumerator GetEnumerator()
        {
            return new ExpNodeNextEnumerator( this );
        }

        public static bool IsNullOrEmpty( ExpNode node )
        {
            return  node == ExpEmptyNode._only || node == null;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static ExpNode NonOperation( string name )
        {
            return new ExpEmptyNode( name );
        }

        public void PrependNode( ExpNode previousNode )
        {
            Previous.Next = previousNode;
            previousNode.Previous = Previous;
            Previous = previousNode;
            previousNode.Next = this;
        }

        public static bool VerifyTypeConvertable( ExpNode left, ExpNode right )
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

        private class ExpEmptyNode : ExpNode
        {

            internal static readonly ExpEmptyNode _only = new();

            public ExpEmptyNode() : base( string.Empty ) { }

            public ExpEmptyNode( string name ) : base( name ) { }

            public override ExpInstantiableElement EqualityTypeConvert => ExpInstantiableElement.Void;

            public override ExpNode Left => _only;

            public override ExpNode Right => _only;

        }

    }
}
