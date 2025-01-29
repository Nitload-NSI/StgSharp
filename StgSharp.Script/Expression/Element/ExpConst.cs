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
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;

namespace StgSharp.Script.Expression
{
    public class ExpConst : ExpElementInstanceBase
    {

        private ExpConstSource _source;
        private string _target;

        public ExpConst( string name, ExpSchema context ):
            base( name, context )
        {
        }

        public override object? Value => _source;

        public override string TypeName => _source.Name;
    }

    public class ExpConstSource : IExpElementSource
    {

        private static Lazy<ReadOnlyDictionary<string, ExpConstSource>> _builtinConst
        = new(
            () => ReadOnlyDictionary(
                new Dictionary<string, ExpConstSource> {
            { "PI", new ExpConstSource( "PI",  ) },
        } ) );

        private IScriptSourceProvider _sourceProvider;

        private string _name;

        public ExpConstSource(
                       string name,
                       IScriptSourceProvider sourceProvider )
        {
            _name = name;
            _sourceProvider = sourceProvider;
        }

        public ExpElementType ElementType => ExpElementType.Const;

        public IScriptSourceProvider SourceProvider => _sourceProvider;

        public static ReadOnlyDictionary<string, ExpConstSource> BuiltinConst => _builtinConst.Value;

        public string Name
        {
            get => _name;
        }

        public virtual void Analyse()
        {
            throw new NotImplementedException();
        }

    }
}
