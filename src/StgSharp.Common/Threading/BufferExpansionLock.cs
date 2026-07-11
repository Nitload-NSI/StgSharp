// -----------------------------------------------------------------------------
// file="BufferExpansionLock"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

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

        private static readonly ThreadLocal<int> Count = new(() => 0);

        private readonly ReaderWriterLockSlim _copyLock;
        private readonly ReaderWriterLockSlim _expansionLock;

        public BufferExpansionLock()
        {
            _expansionLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            _copyLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        public bool IsThreadReadingMetaData => _expansionLock.IsReadLockHeld;

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
            int c = Count.Value;
            if (c == 0) {
                _copyLock.EnterReadLock();
            }
            Count.Value = c + 1;
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
            int c = Count.Value - 1;
            Count.Value = c;
            if (c == 0) {
                _copyLock.ExitReadLock();
            }
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
