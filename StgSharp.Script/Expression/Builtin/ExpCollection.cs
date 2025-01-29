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

namespace StgSharp.Script.Expression
{
    public abstract class ExpCollectionBase : IEnumerable<ExpElementInstanceBase>
    {
        protected internal string _accurateType;

        protected ExpCollectionBase(
            string accurateType )
        {
            _accurateType = accurateType;
        }

        public string MemberType
        {
            get => _accurateType;
        }

        public abstract int Count { get; }

        public abstract IEnumerator<ExpElementInstanceBase> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    public sealed class ExpList : ExpCollectionBase
    {
        public ExpList(string accurateType ):base(accurateType)
        {
            
        }

        private List<ExpElementInstanceBase> _collection = new List<ExpElementInstanceBase>();

        public override int Count => _collection.Count;

        public override IEnumerator<ExpElementInstanceBase> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class ExpSet : ExpCollectionBase
    {

        private HashSet<ExpElementInstanceBase> _collection = new();

        public ExpSet( string accurateType )
            : base( accurateType ) { }

        public void Add( ExpElementInstanceBase token )
        {
            if( token.TypeName != MemberType ) {
                throw new ExpInvalidCollectionMemberTypeException( token,
                                                                   this );
            }
            _collection.Add( token );
        }

        public override int Count => _collection.Count;

        public override sealed IEnumerator<ExpElementInstanceBase> GetEnumerator()
        {
            return ( ( IEnumerable<ExpElementInstanceBase> )_collection ).GetEnumerator(
                );
        }

    }

    public class ExpArray : ExpCollectionBase
    {

        private ExpElementInstanceBase[] _collection;

        public ExpArray( string accurateType )
            : base( accurateType ) { }

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
        public override IEnumerator<ExpElementInstanceBase> GetEnumerator()
        {
            return ( ( IEnumerable<ExpElementInstanceBase> )_collection ).GetEnumerator(
                );
        }

    }

    public class ExpBag : ExpCollectionBase
    {

        private ConcurrentBag<ExpElementInstanceBase> _collection = new ConcurrentBag<ExpElementInstanceBase>(
            );

        public ExpBag( string accurateType )
            : base( accurateType ) { }

        public override int Count => _collection.Count;

        public override IEnumerator<ExpElementInstanceBase> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

    }
}
