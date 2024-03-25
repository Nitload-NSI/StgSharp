//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="GLhandle.cs"
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
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    /// <summary>
    /// A handle to an OpenGL object,
    /// must be indexed from a <see cref="glHandleSet"/>.
    /// Direct call will cause <see langword="InvalidOperationException"/>
    /// </summary>
    public unsafe struct glHandle
    {

        internal glHandleSet source;
        internal uint* valuePtr;

        private glHandle(IntPtr handle)
        {
            this.valuePtr = (uint*)handle;
            source = null;
        }

        internal glHandle(uint* val, glHandleSet set)
        {
            source = set;
            valuePtr = val;
        }

        [Obsolete("Cannot init instance of this type", true)]
        public glHandle()
        {
        }

        public IntPtr Handle
        {
            get
            {
                if (source.handle == (uint*)0)
                {
                    throw new InvalidOperationException($"Handle set {nameof(source)} is not pinned.");
                }
                return (IntPtr)valuePtr;
            }
        }

        public uint Value
        {
            get
            {
                if (source.handle == (uint*)0)
                {
                    throw new InvalidOperationException($"Handle set {nameof(source)} is not pinned.");
                }
                return *valuePtr;
            }
            set
            {
                if (source.handle == (uint*)0)
                {
                    throw new InvalidOperationException($"Handle set {nameof(source)} is not pinned.");
                }
                *valuePtr = value;
            }
        }

        public static glHandle Zero => Alloc(0);

        internal uint DirectValue => *valuePtr;

        public static glHandle Alloc(uint value)
        {
            glHandle ret = default;
            ret.source = new glHandleSet(1);
            ret.valuePtr = ret.source.handle;
            *ret.valuePtr = value;
            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static glHandle Alloc(IntPtr handle)
        {
            return new glHandle(handle);
        }

    }

    public unsafe struct glSHandle
    {

        internal glHandleSet source;
        internal int* valuePtr;

        internal glSHandle(int* val, glHandleSet set)
        {
            source = set;
            valuePtr = val;
        }

        public static glSHandle Zero => Alloc(0);

        internal int DirectValue => *valuePtr;

        public static glSHandle Alloc(int value)
        {
            glSHandle ret = default;
            ret.source = new glHandleSet(1);
            ret.valuePtr = (int*)ret.source.handle;
            *ret.valuePtr = value;
            return ret;
        }

    }

    public unsafe class glArrayHandle
    {

        internal readonly Array a;
        internal GCHandle handle;

        internal glArrayHandle(Array array)
        {
            this.a = array;
            handle = GCHandle.Alloc(array, GCHandleType.Pinned);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr AddressOfArray()
        {
            if (handle != default)
            {
                return handle.AddrOfPinnedObject();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static glArrayHandle Alloc<T>(T[] array) where T : struct
        {
            return new glArrayHandle(array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static glArrayHandle CopyAlloc<T>(T[] array) where T : struct
        {
            Array copy = new T[array.Length];
            array.CopyTo(copy, 0);
            return new glArrayHandle(copy);
        }

        public void Release()
        {
            handle.Free();
            handle = default;
        }

    }

    /// <summary>
    /// A handle to an OpenGL object.
    /// </summary>
    public unsafe class glHandleSet
    {

        private readonly uint[] _value;
        internal uint* handle;
        internal int length;

        public glHandleSet(int count)
        {
            _value = new uint[count];
            length = count;
            handle = (uint*)GCHandle.Alloc(_value, GCHandleType.Pinned).AddrOfPinnedObject();
        }

        public glHandle this[int index]
        {
            get
            {
                if (index > length)
                {
                    throw new IndexOutOfRangeException();
                }
                return new glHandle(handle + index, this);
            }
        }

        public glSHandle this[uint index]
        {
            get
            {
                if (index > length)
                {
                    throw new IndexOutOfRangeException();
                }
                return new glSHandle(((int*)handle) + index, this);
            }
        }

        public static void Copy(in glHandleSet source, glHandleSet target)
        {
            if ((source == null) || (target == null))
            {
                throw new NullReferenceException();
            }
            if (source.length != target.length)
            {
                throw new Exception($"Lengths of {nameof(source)} and {nameof(target)} are not equal.");
            }
            for (int i = 0; i < source.length; i++)
            {
                target._value[i] = source._value[i];
            }
        }

        /// <summary>
        /// temporarily release the handle of GLhandle sets.
        /// Please call <see cref="Pin"/> method before indexing.
        /// </summary>
        public void Free()
        {
            if (_value == null)
            {
                return;
            }
            GCHandle h = GCHandle.FromIntPtr((IntPtr)handle);
            h.Free();
            handle = (uint*)0;
        }

        public void Pin()
        {
            if (_value == null)
            {
                return;
            }
            handle = (uint*)GCHandle.Alloc(_value, GCHandleType.Pinned).AddrOfPinnedObject();
        }

        /// <summary>
        /// Realse all resource in this handle.
        /// This will release all resource in this <see cref="glHandle"/>.
        /// This <see langword="glHandle"/> will be not usable.
        /// </summary>
        public void Release()
        {
            if (_value == null)
            {
                return;
            }
            else
            {
                GCHandle.FromIntPtr((IntPtr)handle).Free();
                handle = (uint*)IntPtr.Zero;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(int index, uint value)
        {
            _value[index] = value;
        }

    }
}