//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="TimeSpanAwaitingToken.cs"
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Timing
{
    #nullable enable

    public sealed class TimeSpanAwaitingToken : IDisposable
    {

        private static readonly ConcurrentStack<int> s_unusedID = new ConcurrentStack<int>(
            [0] );
        private static int s_maxID;
        private static readonly object s_lock = new object();

        private EventHandler? _refreshHandler,_missHandler;
        private int _size, _id;

        private object _participantObject = new object();
        private SemaphoreSlim _awaitingSemaphore;

        private TimeSpanProvider _provider;

        internal TimeSpanAwaitingToken( TimeSpanProvider provider )
        {
            if( s_unusedID.Count == 1 ) {
                lock( s_lock ) {
                    s_unusedID.TryPop( out int id );
                    TokenID = id;
                    s_unusedID.Push( s_maxID++ );
                }
            } else {
                s_unusedID.TryPop( out int id );
                TokenID = id;
            }
            _provider = provider;
            _size = 1;
            _awaitingSemaphore = new SemaphoreSlim( 0, 1 );
        }

        internal TimeSpanAwaitingToken( TimeSpanProvider provider, int count )
        {
            if( s_unusedID.Count == 1 ) {
                lock( s_lock ) {
                    s_unusedID.TryPop( out int id );
                    TokenID = id;
                    s_unusedID.Push( s_maxID++ );
                }
            } else {
                s_unusedID.TryPop( out int id );
                TokenID = id;
            }
            _provider = provider;
            _size = count;
            _awaitingSemaphore = new SemaphoreSlim( 0, _size );
        }

        public event EventHandler MissTimeSpanRefreshed
        {
            add { _missHandler += value; }
            remove { _missHandler -= value; }
        }

        public event EventHandler TimeSpanRefreshed
        {
            add { _refreshHandler += value; }
            remove { _refreshHandler -= value; }
        }

        public int TokenID
        {
            get => _id;
            private set => _id = value;
        }

        internal SemaphoreSlim AwaitingSemaphoreSlim
        {
            get => _awaitingSemaphore;
        }

        public void Dispose()
        {
            _provider.QuitSpanAwaiting( this );
            s_unusedID.Push( TokenID );
            _awaitingSemaphore.Dispose();
            GC.SuppressFinalize( this );
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public void Participant()
        {
            lock( _participantObject ) {
                _size++;
                SemaphoreSlim newSemaphore = new SemaphoreSlim(
                    _awaitingSemaphore.CurrentCount, _size );
                Interlocked.Exchange( ref _awaitingSemaphore, newSemaphore )
                    .Dispose();
            }
        }

        public void WaitNextSpan()
        {
            _awaitingSemaphore.Wait();
        }

        internal void MissRefresh()
        {
            _missHandler?.Invoke( this, EventArgs.Empty );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal void Refresh()
        {
            _refreshHandler?.Invoke( this, EventArgs.Empty );
        }

        ~TimeSpanAwaitingToken()
        {
            Dispose();
        }

    }
    #nullable restore
}
