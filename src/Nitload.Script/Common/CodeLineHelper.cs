//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="CodeLineHelper"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using System.Text;

namespace StgSharp.Script
{
    public static class CodeLineHelper
    {

        public static string SliceAndTrim(this string line, int startIndex)
        {
            if (line == null) {
                throw new ArgumentNullException(nameof(line));
            }

            if (startIndex < 0 || startIndex >= line.Length) {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            ReadOnlySpan<char> sourceSpan = line.AsSpan();
            ReadOnlySpan<char> subSpan = sourceSpan.Slice(startIndex);

            int start = 0;
            while (start < subSpan.Length && char.IsWhiteSpace(subSpan[start])) {
                start++;
            }
            int end = subSpan.Length - 1;
            while (end >= start && char.IsWhiteSpace(subSpan[end])) {
                end--;
            }
            if (start > end) {
                return string.Empty;
            }
            int finalLength = end - start + 1;

            return new string(subSpan.Slice(start, finalLength));
        }

        public static string SliceAndTrim(this string line, int startIndex, int length)
        {
            if (line == null) {
                throw new ArgumentNullException(nameof(line));
            }

            if (startIndex < 0 || startIndex >= line.Length) {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (length < 0 || startIndex + length > line.Length) {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            ReadOnlySpan<char> sourceSpan = line.AsSpan();
            ReadOnlySpan<char> subSpan = sourceSpan.Slice(startIndex, length);

            int start = 0;
            while (start < subSpan.Length && char.IsWhiteSpace(subSpan[start])) {
                start++;
            }
            int end = subSpan.Length - 1;
            while (end >= start && char.IsWhiteSpace(subSpan[end])) {
                end--;
            }
            if (start > end) {
                return string.Empty;
            }
            int finalLength = end - start + 1;

            return new string(subSpan.Slice(start, finalLength));
        }

    }
}
