﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpCollectionIndexNode.cs"
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
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public class ExpCollectionIndexNode : ExpSyntaxNode
    {

        private ExpCollectionInstanceBase _collection;

        private ExpSyntaxNode _indexExpression;

        private ExpCollectionIndexNode(
                Token source,
                ExpCollectionInstanceBase instance,
                ExpSyntaxNode indexExpression )
            : base( source )
        {
            _collection = instance;
            _indexExpression = indexExpression;
            CodeConvertTemplate = @"{0}[{1}]";
        }

        public override ExpSyntaxNode Left => _collection;

        public override ExpSyntaxNode Right => _indexExpression;

        public override IExpElementSource EqualityTypeConvert => _collection.EqualityTypeConvert;

        public static ExpCollectionIndexNode Create(
                                             Token source,
                                             ExpCollectionInstanceBase instance,
                                             ExpSyntaxNode indexExpression )
        {
            if( !indexExpression.IsNumber ) {
                throw new ExpInvalidTypeException(
                    source, "Number", indexExpression.EqualityTypeConvert.Name );
            }
            return new ExpCollectionIndexNode( source, instance, indexExpression );
        }

    }
}
