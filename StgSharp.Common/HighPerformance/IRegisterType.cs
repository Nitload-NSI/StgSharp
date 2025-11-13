//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="IRegisterType"
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
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance
{
    /// <summary>
    ///   Abstraction for a wide SIMD register or equivalent packed numeric storage. Provides raw
    ///   reference access, broadcast initialization and element level access. Typical
    ///   implementations map to 128/256/512-bit hardware registers or software emulated blocks.
    /// </summary>

    public interface IRegisterPresentation
    {

        /// <summary>
        ///   Gets a reference to the first element of the register reinterpreted as type <typeparamref
        ///   name="T" />.
        /// </summary>
        /// <typeparam name="T">
        ///   Unmanaged numeric element type implementing <see cref="INumber{T}" />.
        /// </typeparam>
        /// <returns>
        ///   Writable reference to element 0 (lowest lane / lowest bits).
        /// </returns>
        /// <remarks>
        ///   Intended for treating the register as a contiguous block of elements. The returned reference
        ///   must remain valid for the lifetime of the register and be identical to <see
        ///   cref="Member{T}(int)" /> with index 0. Caller must choose a type layout compatible with the
        ///   actual register representation.
        /// </remarks>
        ref T AsRef<T>() where T: unmanaged, INumber<T>;

        /// <summary>
        ///   Fills all elements (lanes) of the register with the provided scalar <paramref name="value" />.
        /// </summary>
        /// <typeparam name="T">
        ///   Unmanaged numeric element type implementing <see cref="INumber{T}" />.
        /// </typeparam>
        /// <param name="value">
        ///   Scalar value to replicate to every element.
        /// </param>
        /// <remarks>
        ///   Should map to a single broadcast/shuffle instruction where possible. After completion all
        ///   lanes read back the same value.
        /// </remarks>
        void BroadCastFrom<T>(T value) where T: unmanaged, INumber<T>;

        /// <summary>
        ///   Gets a writable reference to the element at the specified <paramref name="index" />.
        /// </summary>
        /// <typeparam name="T">
        ///   Unmanaged numeric element type implementing <see cref="INumber{T}" />.
        /// </typeparam>
        /// <param name="index">
        ///   Zero-based lane index (range defined by implementation).
        /// </param>
        /// <returns>
        ///   Writable reference to the lane.
        /// </returns>
        /// <remarks>
        ///   Enables scalar lane edits without unpacking the entire register. Implementations may omit
        ///   bounds checks for performance. Caller is responsible for passing a valid index.
        /// </remarks>
        ref T Member<T>(int index) where T: unmanaged, INumber<T>;

    }
}