//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="DataBinding.cs"
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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.MVVM.ViewModel
{
#nullable enable

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate ref object BindingDataGetter();
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void BindingDataSetter(ref object value);

    public unsafe struct DataBindingEntry
    {
        private IntPtr getterPtr, setterPtr;

        public DataBindingEntry(
            [NotNull] BindingDataGetter gettingCallback,
            [NotNull] BindingDataSetter settingCallback)
        {
            getterPtr = Marshal.GetFunctionPointerForDelegate(gettingCallback);
            setterPtr = Marshal.GetFunctionPointerForDelegate(settingCallback);
        }

        public static DataBindingEntry NullBinding
        {
            get => new DataBindingEntry();
        }

        public bool ExactEqual(DataBindingEntry other)
        {
            return (getterPtr == other.getterPtr) && (setterPtr == other.setterPtr);
        }

        public ref object GetValue()
        {
            return ref ((delegate* unmanaged[Cdecl]<ref object>)getterPtr)();
        }

        public void SetData(ref object value)
        {
            ((delegate* unmanaged[Cdecl]<ref object, void>)setterPtr)(ref value);
        }

    }

#nullable restore
}
