//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpSwitchNode.cs"
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
    public class ExpSwitchNode : ExpNode
    {

        private Dictionary<ExpNode, ExpNode> _cases;
        private ExpNode _default;
        private ExpNode _target;

        public ExpSwitchNode(
                       Token source,
                       ExpNode target,
                       ExpNode defaultCase,
                       params (ExpNode label, ExpNode op)[] cases )
            : base( source )
        {
            _target = target;
            _default = defaultCase;
            _cases = new Dictionary<ExpNode, ExpNode>();
            foreach( (ExpNode label, ExpNode op ) item in cases ) {
                if( item.op == null ) {
                    _cases.Add( item.label, null! );
                } else {
                    _cases.Add( item.label, item.op );
                }
            }
        }

        public ExpNode this[ ExpNode index ]
        {
            get
            {
                if( _cases.TryGetValue( index, out ExpNode? ret ) ) {
                    return ret;
                }
                return _default;
            }
        }

        public override ExpNode Left => throw new NotImplementedException();

        public override ExpNode Right => throw new NotImplementedException();

        public override IExpElementSource EqualityTypeConvert => null!;

        public void AddOperationToDefault( ExpNode expressionRoot )
        {
            if( expressionRoot != null ) {
                _default.AppendNode( expressionRoot );
            }
        }

        /// <summary>
        /// Adds an expression to certain <see langword="case" />. The added expression will be run
        /// at last.
        /// </summary>
        /// <param name="label"> Label of case to add operation </param>
        /// <param name="expressionRoot"> Root token of the expression to be added. </param>
        /// <exception cref="ExpCaseNotFoundException"> Cannot fine the case label in current witch expression. </exception>
        public void AppendOperationToCase(
                            ExpNode label,
                            ExpNode expressionRoot )
        {
            if( _cases.TryGetValue( label, out ExpNode? op ) ) {
                op.AppendNode( expressionRoot );
            } else {
                throw new ExpCaseNotFoundException( this, label );
            }
        }

        public bool ContainsCase( ExpNode token )
        {
            return _cases.ContainsKey( token );
        }

    }
}
