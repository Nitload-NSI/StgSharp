//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="glParamQueue.cs"
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
using StgSharp.Graphics;
using StgSharp.Math;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Logic
{
    internal unsafe class GLparamQueue
    {

        private ConcurrentQueue<M128> paramQueue;
        private ConcurrentQueue<glArrayHandle> streamQueue;
        internal GLcontext* context;

        public GLparamQueue()
        {
            paramQueue = new ConcurrentQueue<M128>();
            streamQueue = new ConcurrentQueue<glArrayHandle>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(Vec4d v)
        {
            paramQueue.Enqueue(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(M128 m)
        {
            paramQueue.Enqueue(m);
            Console.WriteLine(paramQueue.Count + streamQueue.Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(uint u0)
        {
            M128 m = default;
            m.Write<uint>(0, u0);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(IntPtr p)
        {
            M128 m = default;
            m.Write<ulong>(0, (ulong)p);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(glHandle handle)
        {
            M128 m = default;
            m.Write<ulong>(0, (ulong)handle.valuePtr);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(int i0)
        {
            M128 m = default;
            m.Write<int>(0, i0);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(string str)
        {
            byte[] code = Encoding.UTF8.GetBytes(str);
            streamQueue.Enqueue(glArrayHandle.Alloc<byte>(code));
            Console.WriteLine(paramQueue.Count + streamQueue.Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue<T>(T[] array) where T : struct
        {
            streamQueue.Enqueue(glArrayHandle.Alloc<T>(array));
            Console.WriteLine(paramQueue.Count + streamQueue.Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(uint u0, uint u1)
        {
            M128 m = default;
            m.Write<uint>(0, u0);
            m.Write<uint>(1, u1);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(ulong u0, ulong u1)
        {
            M128 m = default;
            m.Write<ulong>(0, u0);
            m.Write<ulong>(1, u1);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(IntPtr p0, IntPtr p1)
        {
            M128 m = default;
            m.Write<ulong>(0, (ulong)p0);
            m.Write<ulong>(1, (ulong)p1);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(glHandle handle, glHandle handle1)
        {
            M128 m = default;
            m.Write<ulong>(0, (ulong)handle.valuePtr);
            m.Write<ulong>(1, (ulong)handle1.valuePtr);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(int i0, int i1)
        {
            M128 m = default;
            m.Write<int>(0, i0);
            m.Write<int>(1, i1);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(uint u0, uint u1, uint u2)
        {
            M128 m = default;
            m.Write<uint>(0, u0);
            m.Write<uint>(1, u1);
            m.Write<uint>(2, u2);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(float f0, float f1, float f2)
        {
            M128 m = default;
            m.Write<float>(0, f0);
            m.Write<float>(1, f1);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(int i0, int i1, int i2)
        {
            M128 m = default;
            m.Write<int>(0, i0);
            m.Write<int>(1, i1);
            m.Write<int>(2, i2);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(float f0, float f1, float f2, float f3)
        {
            M128 m = default;
            m.Write<float>(0, f0);
            m.Write<float>(1, f1);
            m.Write<float>(2, f2);
            m.Write<float>(3, f3);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(int i0, int i1, int i2, int i3)
        {
            M128 m = default;
            m.Write<int>(0, i0);
            m.Write<int>(1, i1);
            m.Write<int>(2, i2);
            m.Write<int>(3, i3);
            this.Enqueue(m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out glArrayHandle arrayHandle)
        {
            if (!streamQueue.TryDequeue(out arrayHandle))
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function:", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDequeue(out M128 m)
        {
            bool ret = paramQueue.TryDequeue(out m);
            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out Vec4d v)
        {
            M128 m;
            if (this.TryDequeue(out m))
            {
                v = *(Vec4d*)&m;
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue<T>(out T[] array)
        {
            glArrayHandle output = default;
            if (streamQueue.TryDequeue(out output))
            {
                array = output.a as T[];
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out int i0)
        {
            M128 ret;
            if (this.TryDequeue(out ret))
            {
                i0 = ret.Read<int>(0);
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out IntPtr p)
        {
            M128 ret;
            if (this.TryDequeue(out ret))
            {
                p = (IntPtr)ret.Read<ulong>(0);
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out uint u0)
        {
            M128 ret;
            if (this.TryDequeue(out ret))
            {
                u0 = ret.Read<uint>(0);
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out int i0, out int i1)
        {
            M128 ret;
            if (this.TryDequeue(out ret))
            {
                i0 = ret.Read<int>(0);
                i1 = ret.Read<int>(1);
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out IntPtr p0, out IntPtr p1)
        {
            M128 ret;
            if (this.TryDequeue(out ret))
            {
                p0 = (IntPtr)ret.Read<ulong>(0);
                p1 = (IntPtr)ret.Read<ulong>(1);
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out uint u0, out uint u1)
        {
            M128 ret;
            if (this.TryDequeue(out ret))
            {
                u0 = ret.Read<uint>(0);
                u1 = ret.Read<uint>(1);
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out int i0, out int i1, out int i2)
        {
            M128 ret;
            if (this.TryDequeue(out ret))
            {
                i0 = ret.Read<int>(0);
                i1 = ret.Read<int>(1);
                i2 = ret.Read<int>(2);
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out float f0, out float f1, out float f2)
        {
            M128 ret;
            if (this.TryDequeue(out ret))
            {
                f0 = ret.Read<uint>(0);
                f1 = ret.Read<uint>(1);
                f2 = ret.Read<uint>(2);
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out uint u0, out uint u1, out uint u2)
        {
            M128 ret;
            if (this.TryDequeue(out ret))
            {
                u0 = ret.Read<uint>(0);
                u1 = ret.Read<uint>(1);
                u2 = ret.Read<uint>(2);
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out int i0, out int i1, out int i2, out int i3)
        {
            M128 ret;
            if (this.TryDequeue(out ret))
            {
                i0 = ret.Read<int>(0);
                i1 = ret.Read<int>(1);
                i2 = ret.Read<int>(2);
                i3 = ret.Read<int>(3);
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out float f0, out float f1, out float f2, out float f3)
        {
            M128 ret;
            if (this.TryDequeue(out ret))
            {
                f0 = ret.Read<uint>(0);
                f1 = ret.Read<uint>(1);
                f2 = ret.Read<uint>(2);
                f3 = ret.Read<uint>(3);
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TryDequeue(out uint u0, out uint u1, out uint u2, out uint u3)
        {
            M128 ret;
            if (this.TryDequeue(out ret))
            {
                u0 = ret.Read<uint>(0);
                u1 = ret.Read<uint>(1);
                u2 = ret.Read<uint>(2);
                u3 = ret.Read<uint>(3);
            }
            else
            {
                InternalIO.InternalWriteLog("No param in buffer when calling gl function.", LogType.Error);
                throw new Exception();
            }
        }

        /*
        internal void SetContext(GLcontext* p)
        {
            context = p;
        }
        /**/
    }
}