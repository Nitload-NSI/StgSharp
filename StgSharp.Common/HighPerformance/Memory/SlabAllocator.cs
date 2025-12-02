//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="SlabAllocator"
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

namespace StgSharp.HighPerformance.Memory
{
    public abstract class SlabAllocator<T> : IDisposable where T : unmanaged
    {

        public abstract nuint Allocate();

        /// <summary>
        ///   Creates a new SLAB allocator instance with the specified configuration.
        /// </summary>
        /// <param name="count">
        ///   Initial number of objects that can be allocated.
        /// </param>
        /// <param name="layout">
        ///   Memory layout strategy for the SLAB allocator.
        /// </param>
        /// <param name="concurrentSupport">
        ///   Whether to enable concurrent access support.
        /// </param>
        /// <returns>
        ///   A new SLAB allocator instance.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown when an invalid layout is specified.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///   Thrown when requesting unsupported configurations.
        /// </exception>
        /// <remarks>
        ///   <para><strong> Important Threading Considerations: </strong></para> <para> While SLAB
        ///   ///   allocators themselves may be designed for single-threaded use, allocated objects
        ///   are ///   frequently shared across multiple threads in real-world applications. Due to
        ///   the ///   special implementation of expandable concurrent sequential SLAB allocators,
        ///   they rely ///   on internal  expansion locks to ensure that memory buffer expansions
        ///   (which involve ///   copying the entire  buffer to a new location) do not occur while
        ///   other threads are ///   reading from allocated regions.</para> <para><strong>
        ///   Sequential Layout Limitations: /// </strong><br /> Sequential layout SLAB allocators
        ///   cannot provide a single-threaded ///   version because:<list
        ///   type="bullet"><item><description> They use contiguous memory ///   layout that
        ///   requires buffer expansion when capacity is exceeded  ///
        ///   </description></item><item><description> Buffer expansion involves copying all  /// 
        ///   existing data to a new memory location </description></item><item><description>///  
        ///   Without proper synchronization, concurrent access to allocated objects during  ///  
        ///   expansion would cause memory corruption </description></item><item><description> The 
        ///   ///   expansion lock mechanism is essential even if allocation itself is single- ///  
        ///   threaded</description></item></list></para> <para><strong> Chunked Layout: ///
        ///   </strong><br /> Chunked layout allocators can optionally provide single-threaded ///  
        ///   versions since they don't  require buffer expansion and copying. Each chunk is ///  
        ///   allocated independently, avoiding the  memory relocation issues present in sequential
        ///   ///   layouts.</para>
        /// </remarks>
        public static SlabAllocator<T> Create(
                                       nuint count,
                                       SlabBufferLayout layout,
                                       bool concurrentSupport = true)
        {
            if (concurrentSupport) {
                return layout switch
                {
                    SlabBufferLayout.Sequential => new ConcurrentSequentialSlabAllocator<T>(count),
                    SlabBufferLayout.Chunked => new ConcurrentChunkedSlabAllocator<T>(count),
                    _ => throw new ArgumentOutOfRangeException(nameof(layout), layout, null)
                };
            }
            return layout switch
            {
                SlabBufferLayout.Sequential => throw new NotSupportedException("Non-concurrent version of sequential layout SLAB is not supported due to expansion lock requirements for memory safety during buffer expansion."),
                SlabBufferLayout.Chunked => new ChunkedSlabAllocator<T>(count),
                _ => throw new ArgumentOutOfRangeException(nameof(layout), layout, null)
            };
        }

        public abstract void Dispose();

        public abstract void EnterBufferReading();

        public abstract void ExitBufferReading();

        public abstract void Free(nuint index);

        public abstract SlabAllocationHandle<T> ReadAllocation();

    }

    public enum SlabBufferLayout
    {

        Sequential,
        Chunked

    }
}
