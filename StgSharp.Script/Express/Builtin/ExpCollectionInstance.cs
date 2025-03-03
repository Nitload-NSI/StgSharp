//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpCollectionInstance.cs"
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
    public abstract class ExpCollectionInstanceBase : ExpNode
    {

        protected internal (int min, int max) _range;
        protected internal ExpElementType _memberType;

        protected ExpCollectionInstanceBase( string name ) : base( name ) { }

        public abstract ExpCollectionBase CollectionValue
        {
            get;
        }

        public ExpElementType MemberType
        {
            get => _memberType;
        }

        public override ExpNode Left => null!;

        public override ExpNode Right => null!;

        public int MinCapacity => _range.min;

        public int MaxCapacity => _range.max;

    }

    public sealed class ExpSetInstance : ExpCollectionInstanceBase
    {

        private ExpSet _collection;

        public ExpSetInstance( string name, ExpSet set, ExpSchema context )
            : base( name )
        {
            _collection = set;
        }

        public override ExpCollectionBase CollectionValue => _collection;

        public override ExpInstantiableElement EqualityTypeConvert => throw new NotImplementedException(
            );

    }

    public sealed class ExpBagInstance : ExpCollectionInstanceBase
    {

        private ExpBag _collection;

        public ExpBagInstance( string name, ExpBag bag, ExpSchema context )
            : base( name )
        {
            _collection = bag;
        }

        public override ExpCollectionBase CollectionValue => _collection;

        public override ExpInstantiableElement EqualityTypeConvert => throw new NotImplementedException(
            );

    }

    public sealed class ExpListInstance : ExpCollectionInstanceBase
    {

        private ExpList _collection;

        public ExpListInstance( string name, ExpList list, ExpSchema context )
            : base( name )
        {
            _collection = list;
        }

        public override ExpCollectionBase CollectionValue => _collection;

        public override ExpInstantiableElement EqualityTypeConvert => throw new NotImplementedException(
            );

    }

    public sealed class ExpArrayInstance : ExpCollectionInstanceBase
    {

        private ExpArray _collection;

        public ExpArrayInstance(
                       string name,
                       ExpArray array,
                       ExpSchema context )
            : base( name )
        {
            _collection = array;
        }

        public override ExpCollectionBase CollectionValue => _collection;

        public override ExpInstantiableElement EqualityTypeConvert => _collection;

    }
}
