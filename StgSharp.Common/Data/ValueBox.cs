//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ValueBox"
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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Data
{
    public interface IValueBox
    {

        ref object ValueReference { get; }

        static virtual IValueBox<T> AsGeneric<T>(IValueBox box) where T: unmanaged
        {
            if (box is not IValueBox<T> genericBox) {
                return null;
            }
            return genericBox;
        }

        static virtual bool IsGeneric<T>(IValueBox box) where T: unmanaged
        {
            return box is IValueBox<T>;
        }

    }

    public interface IValueBox<T> where T: unmanaged
    {

        T Value { get; set; }

    }

    /// <summary>
    ///   A class attempt to reduce cost of boxing, though still a bit slower than direct access to
    ///   _origin value type instance.
    /// </summary>
    /// <typeparam name="T">
    ///   Type of instance to be boxed.
    /// </typeparam>
    public class ValueBox<T> : IValueBox, IValueBox<T> where T: unmanaged
    {

        private T _value;

        public ValueBox(T value)
        {
            _value = value;
        }

        public ref object ValueReference => ref Unsafe.As<T, object>(ref _value);

        public T Value
        {
            get => _value;
            set => _value = value;
        }

    }
}
