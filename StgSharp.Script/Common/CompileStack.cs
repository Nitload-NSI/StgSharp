﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="CompileStack.cs"
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
using StgSharp.Collections;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script
{
    public partial class CompileStack<TNode, TType> where TNode: ISyntaxNode<TNode, TType>
        where TType: ITypeSource<TType>
    {

        private Token? _lastToken;

        private TNode[] _nodeArray = new TNode[4];
        private Token[] _operatorArray = new Token[4];
        private Token[] _tokenArray = new Token[4];

        private int _nodeCount,_tokenCount, _operatorCount;
        private RandomAccessibleStack<CompileDepthMark> _depthStack = new RandomAccessibleStack<CompileDepthMark>(
            );

        private RandomAccessibleStack<OperandIndex> _isNode = new RandomAccessibleStack<OperandIndex>(
            8 );
        private Stack<Stack<TNode>> _statementCache = new Stack<Stack<TNode>>();
        private TNode _lastNode;

        public CompileStack()
        {
            _depthStack.Push( new CompileDepthMark( 0, 0, 0, null! ) );
            _statementCache.Push( new Stack<TNode>() );
        }

        public bool IsLastAddedOperator { get; private set; }

        public CompileDepthMark CurrentDepthMark
        {
            get => _depthStack.Peek();
        }

        public int Depth => _depthStack.Count;

        public int OperandCount => _isNode.Count;

        public int OperatorCount => _operatorCount;

        public int OperatorAheadOfDepth
        {
            get => OperatorCount - _depthStack.Peek().OperatorBegin;
        }

        public int OperandAheadOfDepth
        {
            get => OperandCount - _depthStack.Peek().OperandBegin;
        }

        public Stack<TNode> StatementsInDepth
        {
            get => _statementCache.Peek();
        }

        public void DecreaseDepth()
        {
            CompileDepthMark mark = _depthStack.Pop();
            Stack<TNode> statementsInDepth = _statementCache.Pop();
        }

        public bool GetOperandAt( Index i, out TNode operandNode, out Token operandToken )
        {
            OperandIndex index = _isNode[ i ];
            if( index.IsNode )
            {
                operandNode = _nodeArray[ index.Index ];
                operandToken = Token.Empty;
                return true;
            } else
            {
                operandToken = _tokenArray[ index.Index ];
                operandNode = default!;
                return false;
            }
        }

        public bool GetOperandAt(
                    CompileDepthMark depth,
                    Index i,
                    out Token operandToken,
                    out TNode operandNode )
        {
            OperandIndex index = _isNode[ i ];

            _depthStack.Contains( depth );
            if( index.IsNode )
            {
                operandNode = _nodeArray[ index.Index ];
                operandToken = Token.Empty;
                return true;
            } else
            {
                operandToken = _tokenArray[ index.Index ];
                operandNode = default!;
                return false;
            }
        }

        public CompileDepthMark IncreaseDepth( int usage )
        {
            CompileDepthMark mark =
                new CompileDepthMark( OperatorCount, OperandCount, usage );

            _depthStack.Push( mark );
            _statementCache.Push( new Stack<TNode>() );
            return mark;
        }

        public TNode PackAllStatements()
        {
            if( StatementsInDepth.Count == 0 ) {
                return TNode.Empty;
            }
            TNode end = StatementsInDepth.Peek();
            TNode first = end;
            while( StatementsInDepth.Count > 0 )
            {
                first = StatementsInDepth.Pop();
                end.PrependNode( first );
                end = first;
            }
            return first;
        }

        public bool PeekOperand( out Token t, out TNode n )
        {
            if( OperandAheadOfDepth <= 0 )
            {
                t = Token.Empty;
                n = TNode.Empty;
                return false;
            }
            if( _isNode.Pop().IsNode )
            {
                t = Token.Empty;
                n = _nodeArray[ _nodeCount - 1 ];
                return true;
            } else
            {
                t = _tokenArray[ _tokenCount - 1 ];
                n = TNode.Empty;
                return false;
            }
        }

        public Token PeekOperator()
        {
            if( OperatorAheadOfDepth > 0 ) {
                return _operatorArray[ _operatorCount - 1 ];
            }
            return Token.Empty;
        }

        public bool PopOperand( out Token t, out TNode n )
        {
            if( OperandAheadOfDepth <= 0 )
            {
                t = Token.Empty;
                n = TNode.Empty;
                return false;
            }
            if( _isNode.Pop().IsNode )
            {
                t = Token.Empty;
                n = _nodeArray[ --_nodeCount ];
                return true;
            } else
            {
                t = _tokenArray[ --_tokenCount ];
                n = TNode.Empty;
                return false;
            }
        }

        public Token PopOperator()
        {
            if( OperatorAheadOfDepth > 0 )
            {
                _operatorCount--;
                return _operatorArray[ _operatorCount ];
            }
            return Token.Empty;
        }

        public void PushOperand( TNode node )
        {
            if( _nodeArray.Length == _nodeCount ) {
                Array.Resize( ref _nodeArray, _nodeArray.Length * 2 );
            }
            _nodeArray[ _nodeCount ] = node;
            _isNode.Push( new( true, _nodeCount ) );
            _nodeCount++;
            IsLastAddedOperator = false;
        }

        public void PushOperand( Token token )
        {
            if( _tokenArray.Length == _tokenCount ) {
                Array.Resize( ref _tokenArray, _tokenArray.Length * 2 );
            }
            _tokenArray[ _tokenCount ] = token;
            _isNode.Push( new( false, _tokenCount ) );
            _tokenCount++;
            IsLastAddedOperator = false;
        }

        public void PushOperator( Token token )
        {
            if( _operatorArray.Length == _operatorCount ) {
                Array.Resize( ref _operatorArray, _operatorArray.Length * 2 );
            }
            _operatorArray[ _operatorCount ] = token;
            _operatorCount++;
            IsLastAddedOperator = true;
        }

        public T StateOfCurrentDepth<T>() where T: class, ICompileDepthState
        {
            return ( CurrentDepthMark.StateCache  as T )!;
        }

        public bool TryPeekOperand( out bool isNode, out Token t, out TNode n )
        {
            if( OperandAheadOfDepth <= 0 )
            {
                t = Token.Empty;
                n = TNode.Empty;
                isNode = false;
                return false;
            }
            if( _isNode.Pop().IsNode )
            {
                t = Token.Empty;
                n = _nodeArray[ _nodeCount - 1 ];
                isNode = true;
                return true;
            } else
            {
                t = _tokenArray[ _tokenCount - 1 ];
                n = TNode.Empty;
                isNode = false;
                return false;
            }
        }

        public bool TryPeekOperator( out Token op )
        {
            if( OperatorAheadOfDepth > 0 )
            {
                op = _operatorArray[ _operatorCount - 1 ];
                return true;
            }
            op = Token.Empty;
            return false;
        }

        public bool TryPopOperand( out bool isNode, out Token t, out TNode n )
        {
            if( OperandAheadOfDepth <= 0 )
            {
                t = Token.Empty;
                n = TNode.Empty;
                isNode = false;
                return false;
            }
            if( _isNode.Pop().IsNode )
            {
                t = Token.Empty;
                n = _nodeArray[ --_nodeCount ];
                isNode = true;
                return true;
            } else
            {
                t = _tokenArray[ --_tokenCount ];
                n = TNode.Empty;
                isNode = false;
                return true;
            }
        }

        public bool TryPopOperator( out Token op )
        {
            if( OperatorAheadOfDepth > 0 )
            {
                op = _operatorArray[ --_operatorCount ];
                return true;
            }
            op = Token.Empty;
            return false;
        }

        private record struct OperandIndex( bool IsNode, int Index );

    }
}
