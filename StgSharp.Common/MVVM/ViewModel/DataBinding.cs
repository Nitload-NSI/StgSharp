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
using StgSharp.Internal.Intrinsic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.MVVM.ViewModel
{
#nullable enable

[UnmanagedFunctionPointer( CallingConvention.Cdecl )] 
    public delegate ref object BindingDataGetter();
[UnmanagedFunctionPointer( CallingConvention.Cdecl )] 
    public delegate void BindingDataSetter( ref object value );
[UnmanagedFunctionPointer( CallingConvention.Cdecl )] 
    public delegate bool MethodLookUpDelegate(
        string name,
        out Action callback );

    public abstract partial class ViewModelBase
    {

        public bool MethodLookup( string methodName, out Action callback )
        {
            MethodInfo method = GetType().GetMethod( methodName )!;
            if( method == null ) {
                callback = null!;
                return false;
            }
            try {
                callback = ( Action )Delegate.CreateDelegate(
                    typeof( Action ), this, methodName );
                return true;
            }
            catch( MissingMethodException ) {
                callback = null!;
                return false;
            }
            catch( MethodAccessException ) {
                callback = null!;
                return false;
            }
            catch( Exception ) {
                throw;
            }
        }

    }

    [StructLayout( LayoutKind.Explicit )]
    public unsafe struct DataBindingEntry
    {

        [FieldOffset( 0 )] private IntPtr getterPtr;
        [FieldOffset( 8 )] private IntPtr setterPtr;
        [FieldOffset( 0 )] private M128 _mask;

        public DataBindingEntry(
            [NotNull] BindingDataGetter gettingCallback,
            [NotNull] BindingDataSetter settingCallback )
        {
            getterPtr = Marshal.GetFunctionPointerForDelegate( gettingCallback );
            setterPtr = Marshal.GetFunctionPointerForDelegate( settingCallback );
        }

        public static DataBindingEntry NullBinding
        {
            get => new DataBindingEntry();
        }

        public bool Equals( DataBindingEntry other )
        {
            return ( _mask == other._mask );
        }

        public override bool Equals( object obj )
        {
            if( obj is not DataBindingEntry other ) {
                return false;
            }
            return Equals( other );
        }

        public bool ExactEqual( DataBindingEntry other )
        {
            return ( getterPtr == other.getterPtr ) && ( setterPtr == other.setterPtr );
        }

        public override int GetHashCode()
        {
            return _mask.GetHashCode();
        }

        public ref object GetValue()
        {
            return ref ( ( delegate* unmanaged[Cdecl]<ref object> )getterPtr )();
        }

        public void SetValue( ref object value )
        {
            ( ( delegate* unmanaged[Cdecl]<ref object, void> )setterPtr )(
                ref value );
        }

        public static bool operator !=(
            DataBindingEntry left,
            DataBindingEntry right )
        {
            return left._mask != right._mask;
        }

        public static bool operator ==(
            DataBindingEntry left,
            DataBindingEntry right )
        {
            return left._mask == right._mask;
        }

    }

#nullable restore
}
