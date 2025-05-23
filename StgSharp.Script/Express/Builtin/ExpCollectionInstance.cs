﻿//-----------------------------------------------------------------------
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
    public abstract class ExpCollectionInstanceBase : ExpSyntaxNode
    {

        protected internal (int min, int max) _range;
        protected internal ExpElementType _memberType;

        protected ExpCollectionInstanceBase( Token source ) : base( source ) { }

        public abstract ExpCollectionBase CollectionValue { get; }

        public ExpElementType MemberType
        {
            get => _memberType;
        }

        public override ExpSyntaxNode Left => null!;

        public override ExpSyntaxNode Right => null!;

        public int MinCapacity => _range.min;

        public int MaxCapacity => _range.max;

    }

    public sealed class ExpSetInstance : ExpCollectionInstanceBase
    {

        private ExpSet _collection;

        public ExpSetInstance( Token source, ExpSet set )
            : base( source )
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

        public ExpBagInstance( Token source, ExpBag bag )
            : base( source )
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

        public ExpListInstance( Token source, ExpList list )
            : base( source )
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

        public ExpArrayInstance( Token source, ExpArray array )
            : base( source )
        {
            _collection = array;
        }

        public override ExpCollectionBase CollectionValue => _collection;

        public override ExpInstantiableElement EqualityTypeConvert => _collection;

    }
}
