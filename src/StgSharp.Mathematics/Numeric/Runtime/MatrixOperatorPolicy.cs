// -----------------------------------------------------------------------------
// file="MatrixOperatorPolicy"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric.Runtime
{
    internal enum MatrixCachePolicy : short
    {

        None = 0,

        #region usage

        Left = 1,
        Right = 2,
        Ans = 3,

        #endregion

        #region index direction

        RowMajor = 4,
        ColMajor = 5,
        Sequential = 6,

        #endregion

        #region shape

        Square = 8,
        UpperTriangle = 9,
        LowerTriangle = 10,

        #endregion
    }
}
