//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpMetaRefNode.cs"
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
using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public class ExpMetaRefNode : ExpSyntaxNode
    {

        internal ExpMetaRefNode( Token source, ExpInstantiableElement element )
            : base( source )
        {
            SourceRef = element;
        }

        public ExpInstantiableElement SourceRef { get; init; }

        public override ExpSyntaxNode Left => Empty;

        public override ExpSyntaxNode Right => Empty;

        public override IExpElementSource EqualityTypeConvert => ExpInstantiableElement.Void;

        public static ExpMetaRefNode RefEntity( Token position, ExpEntitySource entity )
        {
            if( position.Value != entity.Name ) {
                throw new ExpCompileException(
                    position,
                    $"Meta data does not match, name of source to match is {entity.Name}, but token is {position.Value}" );
            }
            ExpMetaRefNode node = new ExpMetaRefNode( position, entity )
            {
                CodeConvertTemplate = entity.Name,
            };
            return node;
        }

        public static ExpMetaRefNode RefType( Token position, ExpTypeSource type )
        {
            if( position.Value != type.Name ) {
                throw new ExpCompileException(
                    position,
                    $"Meta data does not match, name of source to match is {type.Name}, but token is {position.Value}" );
            }
            ExpMetaRefNode node = new ExpMetaRefNode( position, type )
            {
                CodeConvertTemplate = type.Name,
            };
            ;
            return node;
        }

    }
}
