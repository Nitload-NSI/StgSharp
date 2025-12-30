//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="MatrixCompute.Factorize"
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
using StgSharp.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Mathematics.Numeric
{
    public static partial class MatrixCompute
    {

        public static unsafe void FactorizePLU(
                                  Matrix<float> matrix,
                                  Span<int> pivot,
                                  Matrix<float> lower,
                                  Matrix<float> upper
        )
        {
            if (matrix.ColumnLength != matrix.RowLength) {
                throw new InvalidOperationException("Matrix must be square.");
            }
            if (matrix.Layout != MatrixLayout.DenseRectangle) {
                throw new NotSupportedException("Only dense square matrices are supported.");
            }
            int n = matrix.RowLength;
            if (pivot.Length < n) {
                throw new ArgumentException("Pivot span length is insufficient.", nameof(pivot));
            }
            if (lower.RowLength != n ||
                lower.ColumnLength != n ||
                lower.Layout != MatrixLayout.UpperTriangle) {
                throw new ArgumentException("Lower matrix must be lower-triangular with same dimensions as input.", nameof(lower));
            }
            if (upper.RowLength != n ||
                upper.ColumnLength != n ||
                upper.Layout != MatrixLayout.UpperTriangle) {
                throw new ArgumentException("Upper matrix must be upper-triangular with same dimensions as input.", nameof(upper));
            }

            int size = matrix.KernelColumnLength * matrix.KernelRowLength;
            if (size < 256)
            {
                for (int i = 0; i < n; i++)
                {
                    pivot[i] = i; // identity permutation
                }

                // Aggressive: always call native pivot to fetch four-column candidates
                ulong* pivots = stackalloc ulong[4];
                for (int kBlock = 0; kBlock < n; kBlock += 4)
                {
                    NativeIntrinsic.Context
                                   .mat[(int)MatrixElementType.F32].pivot(matrix.Buffer,
                                                                          (ulong)(kBlock >> 2),
                                                                          (ulong)(matrix.KernelRowLength - (kBlock >> 2)),
                                                                          (ulong)pivots);
                    int colsInBlock = Math.Min(4, n - kBlock);
                    for (int offset = 0; offset < colsInBlock; offset++)
                    {
                        int k = kBlock + offset;
                        int pivotRow = k;
                        float maxAbs;

                        int candidateRow = (int)pivots[offset];
                        int idx = -1;
                        for (int r = k; r < n; r++)
                        {
                            if (pivot[r] == candidateRow)
                            {
                                idx = r;
                                break;
                            }
                        }

                        if (idx != -1)
                        {
                            pivotRow = idx;
                            maxAbs = MathF.Abs(matrix.GetElementUnsafe(pivot[idx], k));
                        } else
                        {
                            maxAbs = MathF.Abs(matrix.GetElementUnsafe(pivot[k], k));
                            for (int r = k + 1; r < n; r++)
                            {
                                float val = MathF.Abs(matrix.GetElementUnsafe(pivot[r], k));
                                if (val > maxAbs)
                                {
                                    maxAbs = val;
                                    pivotRow = r;
                                }
                            }
                        }

                        if (maxAbs == 0f) {
                            throw new InvalidOperationException("Zero pivot encountered in PLU.");
                        }
                        if (pivotRow != k) {
                            (pivot[k], pivot[pivotRow]) = (pivot[pivotRow], pivot[k]);
                        }
                        lower[k, k] = 1.0f;    // unit diagonal for L

                        // Compute U[k, j] for j >= k
                        for (int j = k; j < n; j++)
                        {
                            float sum = 0f;
                            for (int s = 0; s < k; s++) {
                                sum += lower[k, s] * upper[s, j];
                            }
                            upper[k, j] = matrix.GetElementUnsafe(pivot[k], j) - sum;
                        }

                        // Compute L[i, k] for i > k
                        for (int i = k + 1; i < n; i++)
                        {
                            float sum = 0f;
                            for (int s = 0; s < k; s++) {
                                sum += lower[i, s] * upper[s, k];
                            }
                            float u = upper[k, k];
                            if (u == 0f) {
                                throw new InvalidOperationException("Zero pivot encountered in PLU.");
                            }
                            lower[i, k] = (matrix.GetElementUnsafe(pivot[i], k) - sum) / u;
                        }
                    }
                }

                // TODO: For larger kernels, consider calling native pivot helper to extract 4 pivots at once.
            } else if (size < 1024) { } else { }
        }

    }
}
