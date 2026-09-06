// -----------------------------------------------------------------------------
// file="GraphicVector"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Mathematics.Numeric.Graphics
{
    internal static class GraphicVector
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalVectorElement<T>() where T : unmanaged, INumber<T>
        {
            return typeof(T) == typeof(float) ||
                   typeof(T) == typeof(int) ||
                   typeof(T) == typeof(uint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWideVectorElement<T>() where T : unmanaged, INumber<T>
        {
            return typeof(T) == typeof(double) ||
                   typeof(T) == typeof(long) ||
                   typeof(T) == typeof(ulong);
        }

        public static NotSupportedException ThrowUnsupportedTypeException<T>()
        {
            return new NotSupportedException($"Operation is not supported for type {typeof(T)}");
        }

    }
}
