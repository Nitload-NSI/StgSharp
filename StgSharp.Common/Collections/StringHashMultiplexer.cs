//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StringHashMultiplexer"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace StgSharp.Collections
{
    public sealed class StringHashMultiplexer : IHashMultiplexer<string>
    {

        private int _initCount, _remainedCount, _hashCache, _hashLast;
        private string _source, _sourceLast;

        public int RemainedMultiplexCount
        {
            get => _remainedCount;
            set => _initCount = value;
        }

        public void CacheHash(string obj, int count)
        {
            _hashCache = ForceComputeHash(obj);
            _initCount = count;
            _remainedCount = _initCount;
            _source = obj;
        }

        public bool Equals(string x, string y)
        {
            return x == y;
        }

        public void ExtendCache(int count)
        {
            _initCount += count;
            _remainedCount += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe int ForceComputeHash([NotNull] string obj)
        {
            int hash;
            fixed (char* cPtr = obj.AsSpan()) {
                hash = InternalIO.Intrinsic.city_hash_simplify(cPtr, obj.Length);
            }
            return hash;
        }

        public int GetHashCode(string obj)
        {
            if (_remainedCount > 0 && ReferenceEquals(_source, obj))
            {
                _remainedCount--;
                return _hashCache;
            }
            if (ReferenceEquals(_sourceLast, obj)) {
                return _hashLast;
            }
            _sourceLast = obj;
            _hashLast = ForceComputeHash(obj);
            return _hashLast;
        }

    }

    public sealed class ConcurrentStringHashMultiplexer : IHashMultiplexer<string>, IDisposable
    {

        private bool disposedValue;
        private readonly ThreadLocal<StringHashMultiplexer> _multiplexers = 
            new(() => new StringHashMultiplexer());

        public int RemainedMultiplexCount
        {
            get => _multiplexers.Value.RemainedMultiplexCount;
            set => _multiplexers.Value.RemainedMultiplexCount = value;
        }

        public void CacheHash(string obj, int count)
        {
            throw new NotImplementedException();
        }

        // ~ConcurrentStringHashMultiplexer()
        // {
        // Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        public bool Equals(string x, string y)
        {
            return x == y;
        }

        public void ExtendCache(int count)
        {
            _multiplexers.Value.ExtendCache(count);
        }

        public unsafe int ForceComputeHash([NotNull] string obj)
        {
            int hash;
            fixed (char* cPtr = obj.AsSpan()) {
                hash = InternalIO.Intrinsic.city_hash_simplify(cPtr, obj.Length);
            }
            return hash;
        }

        public int GetHashCode(string obj)
        {
            return _multiplexers.Value.GetHashCode(obj);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) {
                    _multiplexers.Dispose();
                }

                disposedValue = true;
            }
        }

    }
}
