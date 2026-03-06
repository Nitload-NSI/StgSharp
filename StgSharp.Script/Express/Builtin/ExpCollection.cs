//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ExpCollection"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StgSharp.Script.Express
{
    public abstract class ExpCollectionBase : ExpInstantiableElement, IEnumerable<ExpElementInstance>
    {

        protected internal string _accurateType;

        protected ExpCollectionBase(string accurateType)
        {
            _accurateType = accurateType;
        }

        public override ExpElementType ElementType => ExpElementType.Collection;

        public abstract int Count { get; }

        public sealed override IScriptSourceProvider SourceProvider => ScriptSourceTransmitter.Empty;

        public string MemberType
        {
            get => _accurateType;
        }

        public override void Analyse()
        {
            return;
        }

        public abstract IEnumerator<ExpElementInstance> GetEnumerator();

        public override bool TryGetMember(string name, out ExpSyntaxNode memberNode)
        {
            memberNode = ExpSyntaxNode.Empty;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    public class ExpArray : ExpCollectionBase
    {

        private ExpElementInstance[] _collection;

        public ExpArray(string accurateType) : base(accurateType) { }

        public ExpElementInstance this[int index]
        {
            get { return _collection[index]; }
            set
            {
                if (value.TypeName != MemberType) {
                    throw new ExpInvalidCollectionMemberTypeException(value, this);
                }
                _collection[index] = value;
            }
        }

        public override int Count => _collection.Length;

        public override string Name => ExpressCompile.PoolString("ARRAY");

        public override ExpSyntaxNode CreateInstanceNode(Token t)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<ExpElementInstance> GetEnumerator()
        {
            return ((IEnumerable<ExpElementInstance>)_collection)
                .GetEnumerator();
        }

    }

    public class ExpBag : ExpCollectionBase
    {

        private ConcurrentBag<ExpElementInstance> _collection = new ConcurrentBag<ExpElementInstance>(
            );

        public ExpBag(string accurateType) : base(accurateType) { }

        public override int Count => _collection.Count;

        public override string Name => ExpressCompile.PoolString("BAG");

        public override ExpSyntaxNode CreateInstanceNode(Token t)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<ExpElementInstance> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

    }

    public sealed class ExpList : ExpCollectionBase
    {

        private List<ExpElementInstance> _collection = new List<ExpElementInstance>();

        public ExpList(string accurateType) : base(accurateType) { }

        public override int Count => _collection.Count;

        public override string Name => ExpressCompile.PoolString("LIST");

        public override ExpSyntaxNode CreateInstanceNode(Token t)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<ExpElementInstance> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

    }

    public class ExpSet : ExpCollectionBase
    {

        private HashSet<ExpElementInstance> _collection = new();

        public ExpSet(string accurateType) : base(accurateType) { }

        public override int Count => _collection.Count;

        public override string Name => ExpressCompile.PoolString("SET");

        public void Add(ExpElementInstance token)
        {
            if (token.TypeName != MemberType) {
                throw new ExpInvalidCollectionMemberTypeException(token, this);
            }
            _collection.Add(token);
        }

        public override ExpSyntaxNode CreateInstanceNode(Token t)
        {
            throw new NotImplementedException();
        }

        public sealed override IEnumerator<ExpElementInstance> GetEnumerator()
        {
            return ((IEnumerable<ExpElementInstance>)_collection)
                .GetEnumerator();
        }

    }
}
