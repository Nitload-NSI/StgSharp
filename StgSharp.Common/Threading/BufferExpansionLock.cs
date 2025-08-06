//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="BufferExpansionLock.cs"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//     
// The above copyright notice and 
// this permission notice shall be included in all copies 
// or substantial portions of the Software.
//     
// THE SOFTWARE IS PROVIDED “AS IS”, 
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Threading
{
    /// <summary>
    ///   Provide a lock for managing concurrent access to a buffer that can be expanded and use a
    ///   common two-pase-copy expanding mechanism.
    /// </summary>
    public class BufferExpansionLock : IDisposable
    {

        private readonly ReaderWriterLockSlim _copyLock;
        private readonly ReaderWriterLockSlim _expansionLock;

        public BufferExpansionLock()
        {
            _expansionLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            _copyLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        public bool IsThreadReadingMetaData
        {
[MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _expansionLock.IsReadLockHeld;
        }

        public void Dispose()
        {
            _expansionLock?.Dispose();
            _copyLock?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnterBufferCopy()
        {
            _copyLock.EnterWriteLock();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnterBufferRead()
        {
            _copyLock.EnterReadLock();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnterExpansionProcess()
        {
            _expansionLock.EnterWriteLock();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnterMetaDataRead()
        {
            _expansionLock.EnterReadLock();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExitBufferCopy()
        {
            _copyLock.ExitWriteLock();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExitBufferRead()
        {
            _copyLock.ExitReadLock();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExitExpansionProcess()
        {
            _expansionLock.ExitWriteLock();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExitMetaDataRead()
        {
            _expansionLock.ExitReadLock();
        }

    }
}
