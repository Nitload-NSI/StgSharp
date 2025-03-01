//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpCollection.cs"
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StgSharp.Script.Express
{
    public abstract class ExpCollectionBase : ExpInstantiableElementBase, IEnumerable<ExpElementInstanceBase>
    {

        protected internal string _accurateType;

        protected ExpCollectionBase( string accurateType )
        {
            _accurateType = accurateType;
        }

        public override ExpElementType ElementType => ExpElementType.Collection;

        public abstract int Count
        {
            get;
        }

        public override sealed IScriptSourceProvider SourceProvider => ScriptSourceTransmitter.Empty;

        public string MemberType
        {
            get => _accurateType;
        }

        public override void Analyse()
        {
            return;
        }

        public abstract IEnumerator<ExpElementInstanceBase> GetEnumerator();

        public override sealed bool IsConvertable(
                                            ExpInstantiableElementBase targetType )
        {
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    public class ExpArray : ExpCollectionBase
    {

        private ExpElementInstanceBase[] _collection;

        public ExpArray( string accurateType ) : base( accurateType ) { }

        public ExpElementInstanceBase this[ int index ]
        {
            get { return _collection[ index ]; }
            set
            {
                if( value.TypeName != MemberType ) {
                    throw new ExpInvalidCollectionMemberTypeException(
                        value, this );
                }
                _collection[ index ] = value;
            }
        }

        public override int Count => _collection.Length;

        public override string Name => ExpCompile.PoolString( "ARRAY" );

        public override ExpBaseNode CreateInstanceNode()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<ExpElementInstanceBase> GetEnumerator()
        {
            return ( ( IEnumerable<ExpElementInstanceBase> )_collection )
                .GetEnumerator();
        }

    }

    public class ExpBag : ExpCollectionBase
    {

        private ConcurrentBag<ExpElementInstanceBase> _collection = new ConcurrentBag<ExpElementInstanceBase>(
            );

        public ExpBag( string accurateType ) : base( accurateType ) { }

        public override int Count => _collection.Count;

        public override string Name => ExpCompile.PoolString( "BAG" );

        public override ExpBaseNode CreateInstanceNode()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<ExpElementInstanceBase> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

    }

    public sealed class ExpList : ExpCollectionBase
    {

        private List<ExpElementInstanceBase> _collection = new List<ExpElementInstanceBase>(
            );

        public ExpList( string accurateType ) : base( accurateType ) { }

        public override int Count => _collection.Count;

        public override string Name => ExpCompile.PoolString( "LIST" );

        public override ExpBaseNode CreateInstanceNode()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<ExpElementInstanceBase> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

    }

    public class ExpSet : ExpCollectionBase
    {

        private HashSet<ExpElementInstanceBase> _collection = new();

        public ExpSet( string accurateType ) : base( accurateType ) { }

        public override int Count => _collection.Count;

        public override string Name => ExpCompile.PoolString( "SET" );

        public void Add( ExpElementInstanceBase token )
        {
            if( token.TypeName != MemberType ) {
                throw new ExpInvalidCollectionMemberTypeException( token,
                                                                   this );
            }
            _collection.Add( token );
        }

        public override ExpBaseNode CreateInstanceNode()
        {
            throw new NotImplementedException();
        }

        public override sealed IEnumerator<ExpElementInstanceBase> GetEnumerator(
                                                                           )
        {
            return ( ( IEnumerable<ExpElementInstanceBase> )_collection )
                .GetEnumerator();
        }

    }
}
