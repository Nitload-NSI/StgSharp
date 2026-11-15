// -----------------------------------------------------------------------------
// file="UnsafeCompute"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Mathematics.Numeric;
using System.Runtime.CompilerServices;

namespace Nitload.Mathematics.Internal
{
    public static class UnsafeCompute
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int CityHashSimplify(
                                 char* span,
                                 int length
        )
        {
            return NumericalModule.GlobalContext.city_hash_simplify(span, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int CityHashSimplify(
                                 char* span,
                                 int begin,
                                 int length
        )
        {
            return NumericalModule.GlobalContext.city_hash_simplify(span + begin, length);
        }

    }
}
