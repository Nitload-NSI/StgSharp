﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepComplexEntityNode.cs"
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
using StgSharp.Script.Express;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Model.Step
{
    public class StepComplexEntityNode : ExpSyntaxNode
    {

        private ExpSyntaxNode _begin;

        private StepComplexEntityNode( ExpSyntaxNode node )
            : base( node.Source )
        {
            _begin = node;
        }

        public override ExpSyntaxNode Left => Empty;

        public override ExpSyntaxNode Right => _begin;

        public override IExpElementSource EqualityTypeConvert => StepEntityType.Source;

        public static StepComplexEntityNode FromTuple( ExpSyntaxNode node )
        {
            if( node is not ExpTupleNode tuple ) {
                throw new ExpInvalidSyntaxException( "Node to convert is not a complex" );
            }
            return new StepComplexEntityNode( tuple.Right );
        }

    }
}
