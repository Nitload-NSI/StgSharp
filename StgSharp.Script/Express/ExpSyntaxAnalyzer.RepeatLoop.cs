//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpSyntaxAnalyzer.RepeatLoop.cs"
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
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.Keyword;

namespace StgSharp.Script.Express
{
    public partial class ExpSyntaxAnalyzer
    {

        private Dictionary<string, ExpElementInstance> _tempVariableInLoops { get; } 
            = new Dictionary<string, ExpElementInstance>();

        private bool AppendToken_RepeatLoop( Token t )
        {
            RepeatLoopState state = _cache.StateOfCurrentDepth<RepeatLoopState>();
            if( state == null )
            {
                state = new RepeatLoopState( t );
                _cache.CurrentDepthMark.StateCache = state;
                return true;
            }
            switch( state.CurrentState )
            {
                case ExpKeyword.Repeat:
                    return TryParseRepeatVariable( state, t );
                case ExpKeyword.To:
                    return TryParseRepeatEnding( state, t );
                case ExpKeyword.By:
                    return TryParseRepeatVariable( state, t );
                case ExpKeyword.Until:
                    return TryParseRepeatUntilRegulation( state, t );
                case ExpKeyword.Assignment:
                    return TryParseRepeatBeginning( state, t );
                case ExpKeyword.While:
                    return TryParseRepeatWhileRegulation( state, t );
                case ExpKeyword.Semicolon:
                    if( t.Value != ExpKeyword.EndRepeat )
                    {
                        AppendToken_common( t );
                        return true;
                    }
                    break;
                default:
                    AppendToken_common( t );
                    return false;
            }
            ExpNode root = _cache.PackAllStatements();
            state.Body = root;
            _cache.DecreaseDepth();
            root = ExpRepeatNode.Create(
                state.Position, state.Body, state.Begin, state.End, state.IncrementVariable,
                state.UntilExpression, state.WhileExpression );
            _cache.StatementsInDepth.Push( root );
        }

        private bool TryParseRepeatBeginning( RepeatLoopState state, Token t )
        {
            switch( t.Value )
            {
                case ExpKeyword.To:
                    state.CurrentState = ExpKeyword.To;
                    while( _cache.TryPopOperator( out Token op ) ) {
                        ConvertAndPushOneOperator( op );
                    }
                    _cache.PopOperand( out _, out ExpNode? root );
                    state.Begin = root;
                    return true;
                default:
                    AppendToken_common( t );
                    return true;
            }
        }

        private bool TryParseRepeatEnding( RepeatLoopState state, Token t )
        {
            switch( t.Value )
            {
                case ExpKeyword.By:
                    state.CurrentState = ExpKeyword.By;
                    break;
                case ExpKeyword.Until:
                    state.CurrentState = ExpKeyword.Until;
                    break;
                case ExpKeyword.While:
                    state.CurrentState = ExpKeyword.While;
                    break;
                default:
                    AppendToken_common( t );
                    return true;
            }
            while( _cache.TryPopOperator( out Token op ) ) {
                ConvertAndPushOneOperator( op );
            }
            _cache.PopOperand( out _, out ExpNode? root );
            state.End = root;
            return true;
        }

        private bool TryParseRepeatIncrement( RepeatLoopState state, Token t )
        {
            switch( t.Value )
            {
                case ExpKeyword.Semicolon:
                    state.CurrentState = ExpKeyword.Semicolon;
                    break;
                case ExpKeyword.Until:
                    state.CurrentState = ExpKeyword.Until;
                    break;
                case ExpKeyword.While:
                    state.CurrentState = ExpKeyword.While;
                    break;
                default:
                    AppendToken_common( t );
                    return true;
            }
            while( _cache.TryPopOperator( out Token op ) ) {
                ConvertAndPushOneOperator( op );
            }
            _cache.PopOperand( out _, out ExpNode? root );
            state.IncrementVariable = root;
            return true;
        }

        private bool TryParseRepeatUntilRegulation( RepeatLoopState state, Token t )
        {
            switch( t.Value )
            {
                case ExpKeyword.Semicolon:
                    state.CurrentState = ExpKeyword.Semicolon;
                    break;
                default:
                    AppendToken_common( t );
                    return true;
            }
            while( _cache.TryPopOperator( out Token op ) ) {
                ConvertAndPushOneOperator( op );
            }
            _cache.PopOperand( out _, out ExpNode? root );
            state.UntilExpression = root;
            return true;
        }

        private bool TryParseRepeatVariable( RepeatLoopState state, Token t )
        {
            switch( t.Value )
            {
                case ExpKeyword.Integer:
                    state.IncrementVariableType = ExpressCompile.ExpInt;
                    return true;
                case
                    ExpKeyword.Real:
                    state.IncrementVariableType = ExpressCompile.ExpReal;
                    return true;
                case ExpKeyword.Assignment:
                    state.CurrentState = ExpKeyword.Assignment;
                    return true;
                default:
                    if( ExpTypeSource.IsNullOrVoid( state.IncrementVariableType ) )
                    {
                        throw new ExpInvalidTypeException( "Numerical", t.Value );
                    } else
                    {
                        state.IncrementVariable = state.IncrementVariableType
                                                       .CreateInstanceNode( t );
                        return true;
                    }
            }
        }

        private bool TryParseRepeatWhileRegulation( RepeatLoopState state, Token t )
        {
            switch( t.Value )
            {
                case ExpKeyword.Until:
                    state.CurrentState = ExpKeyword.Until;
                    break;
                default:
                    AppendToken_common( t );
                    return true;
            }
            while( _cache.TryPopOperator( out Token op ) ) {
                ConvertAndPushOneOperator( op );
            }
            _cache.PopOperand( out _, out ExpNode? root );
            state.WhileExpression = root;
            return true;
        }

        private class RepeatLoopState : ICompileDepthState
        {

            public RepeatLoopState( Token t )
            {
                Position = t;
            }

            public ExpNode IncrementVariable { get; set; }

            public ExpNode Begin { get; set; }

            public ExpNode End { get; set; }

            public ExpNode UntilExpression { get; set; }

            public ExpNode WhileExpression { get; set; }

            public ExpNode Body { get; set; }

            public ExpTypeSource IncrementVariableType { get; set; }

            public string CurrentState { get; set; }

            public Token Position { get; set; }

        }

    }
}
