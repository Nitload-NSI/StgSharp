//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpConst.cs"
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
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Script.Express
{
    public sealed class ExpConstantCollectionSource : ExpImmutableElement
    {

        private Dictionary<string, ExpElementInstance> _constants;
        private IScriptSourceProvider _sourceProvider;

        public ExpConstantCollectionSource( IScriptSourceProvider sourceProvider )
        {
            _sourceProvider = sourceProvider;
            _constants = new Dictionary<string, ExpElementInstance>( ExpressCompile.Multiplexer );
        }

        public override ExpElementType ElementType => ExpElementType.Const;

        public override IScriptSourceProvider SourceProvider => _sourceProvider;

        public override string Name
        {
            get => ExpressCompile.Keyword.Constant;
        }

        public override void Analyse()
        {
            throw new NotImplementedException();
        }

        public override ExpSyntaxNode MakeReference( params object[] options )
        {
            if( options.Length == 0 || options[ 0 ] is not string str ) {
                return null!;
            }
            if( _constants.Count == 0 ) {
                return null!;
            }
            if( _constants.TryGetValue( str, out ExpElementInstance? token ) ) {
                return token;
            }
            return null!;
        }

        public bool TryGetConstant( string name, out ExpElementInstance instance )
        {
            return _constants.TryGetValue( name, out instance );
        }

        public override bool TryGetMember( string name, out ExpSyntaxNode memberNode )
        {
            if( _constants.TryGetValue( name, out ExpElementInstance? node ) )
            {
                memberNode = node;
                return true;
            }
            memberNode = ExpSyntaxNode.Empty;
            return false;
        }

        internal void AddConstant( string name, ExpElementInstance value )
        {
            _constants.Add( name, value );
        }

    }
}
