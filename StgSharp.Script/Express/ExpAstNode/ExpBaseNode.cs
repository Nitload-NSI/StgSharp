//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpBaseNode.cs"
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
using System.Text;

namespace StgSharp.Script.Express
{
    public abstract class ExpBaseNode : IASTNode<ExpBaseNode, IExpElementSource>
    {

        protected internal ExpNodeFlag _nodeFlag;
        protected internal string _name;

        protected ExpBaseNode( string name, ExpSchema context )
        {
            _name = name;
            NameSpace = context;
            Next = this;
            Previous = this;
        }

        public bool IsNumber
        {
            get { return ( _nodeFlag | ExpNodeFlag.BuiltinType_Number ) != 0; }
        }

        public abstract ExpBaseNode Left
        {
            get;
        }

        public static ExpBaseNode Empty
        {
            get => ExpEmptyNode._only;
        }

        public ExpBaseNode Next
        {
            get;
            set;
        }

        public abstract ExpBaseNode Right
        {
            get;
        }

        public ExpBaseNode Previous
        {
            get;
            set;
        }

        public ExpSchema NameSpace
        {
            get;
            private set;
        }

        public abstract IExpElementSource EqualityTypeConvert
        {
            get;
        }

        public long NodeFlag => ( long )_nodeFlag;

        public string CodeConvertTemplate
        {
            get;
            internal set;
        }

        public string Name => _name;

        public virtual void AppendNode( ExpBaseNode nextToken )
        {
            Next.Previous = nextToken;
            nextToken.Next = Next;
            Next = nextToken;
            Next.Previous = this;
        }

        public ExpNodeNextEnumerator GetEnumerator()
        {
            return new ExpNodeNextEnumerator( this );
        }

        public static bool IsNullOrEmpty( ExpBaseNode node )
        {
            return  node == ExpEmptyNode._only || node == null;
        }

        public static bool VerifyTypeConvertable(
                                   ExpBaseNode left,
                                   ExpBaseNode right )
        {
            IExpElementSource tLeft = left.EqualityTypeConvert,
                tRight = right.EqualityTypeConvert;
            if( tLeft == null || tRight == null ) {
                return false;
            }
            return tLeft.IsConvertable( tRight );
        }

        protected void AssertSchemaInclude( ExpBaseNode expressionRoot )
        {
            if( expressionRoot.NameSpace != NameSpace && NameSpace.TryGetSchemaInclude(
                expressionRoot.NameSpace.Name, out _ ) ) {
                throw new ExpInvalidSchemaIncludeException(
                    NameSpace, expressionRoot.NameSpace );
            }
        }

        private class ExpEmptyNode : ExpBaseNode
        {

            internal static readonly ExpEmptyNode _only = new();

            public ExpEmptyNode()
                : base( string.Empty, ExpSchema.BuiltinSchema ) { }

            public override ExpBaseNode Left => _only;

            public override ExpBaseNode Right => _only;

            public override ExpInstantiableElementBase EqualityTypeConvert => ExpInstantiableElementBase.Void;

        }

    }
}
