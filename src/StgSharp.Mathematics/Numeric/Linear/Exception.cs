// -----------------------------------------------------------------------------
// file="Exception"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public class MatrixDimensionMismatchException<T> : Exception where T : unmanaged, INumber<T>
    {

        public MatrixDimensionMismatchException(
               Matrix<T> right,
               Matrix<T> ans
        )
            : base($"""
                    Matrix dimensions must agree but current:
                    right({right?.ColumnLength ?? 0}×{right?.RowLength ?? 0}),
                    ans({ans?.ColumnLength ?? 0}×{ans?.RowLength ?? 0})
                    """
                ) { }

        public MatrixDimensionMismatchException(
               Matrix<T> left,
               Matrix<T> right,
               Matrix<T> ans
        )
            : base($"""
                    Matrix dimensions must agree but current:
                    left({left?.ColumnLength ?? 0}×{left?.RowLength ?? 0}),
                    right({right?.ColumnLength ?? 0}×{right?.RowLength ?? 0}),
                    ans({ans?.ColumnLength ?? 0}×{ans?.RowLength ?? 0})
                    """
                ) { }

    }
}
