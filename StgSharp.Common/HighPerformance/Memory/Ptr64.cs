//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Ptr64"
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
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
#pragma warning disable CA2225

    public readonly struct Ptr64 : IEquatable<Ptr64>, IComparable<Ptr64>, IFormattable
    {

        public Ptr64(
               ulong value
        )
        {
            Value = value;
        }

        public ulong Value { get; }

        public int CompareTo(
                   Ptr64 other
        )
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(
                    Ptr64 other
        )
        {
            return Value == other.Value;
        }

        public override bool Equals(
                             object? obj
        )
        {
            return obj is Ptr64 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public string ToString(
                      string? format,
                      IFormatProvider? formatProvider
        )
        {
            return Value.ToString(format, formatProvider);
        }

        public static Ptr64 operator -(
                                     Ptr64 left,
                                     ulong right
        )
        {
            return new(left.Value - right);
        }

        public static ulong operator -(
                                     Ptr64 left,
                                     Ptr64 right
        )
        {
            return left.Value - right.Value;
        }

        public static bool operator !=(
                                    Ptr64 left,
                                    Ptr64 right
        )
        {
            return left.Value != right.Value;
        }

        public static Ptr64 operator &(
                                     Ptr64 left,
                                     ulong mask
        )
        {
            return new(left.Value & mask);
        }

        public static Ptr64 operator |(
                                     Ptr64 left,
                                     ulong mask
        )
        {
            return new(left.Value | mask);
        }

        public static Ptr64 operator +(
                                     Ptr64 left,
                                     ulong right
        )
        {
            return new(left.Value + right);
        }

        public static bool operator <(
                                    Ptr64 left,
                                    Ptr64 right
        )
        {
            return left.Value < right.Value;
        }

        public static Ptr64 operator <<(
                                     Ptr64 left,
                                     int shift
        )
        {
            return new Ptr64(left.Value << shift);
        }

        public static bool operator <=(
                                    Ptr64 left,
                                    Ptr64 right
        )
        {
            return left.Value <= right.Value;
        }

        public static bool operator ==(
                                    Ptr64 left,
                                    Ptr64 right
        )
        {
            return left.Value == right.Value;
        }

        public static bool operator >(
                                    Ptr64 left,
                                    Ptr64 right
        )
        {
            return left.Value > right.Value;
        }

        public static bool operator >=(
                                    Ptr64 left,
                                    Ptr64 right
        )
        {
            return left.Value >= right.Value;
        }

        public static Ptr64 operator >>(
                                     Ptr64 left,
                                     int shift
        )
        {
            return new Ptr64(left.Value >> shift);
        }

        public static implicit operator Ptr64(
                                        ulong value
        )
        {
            return new(value);
        }

        public static implicit operator ulong(
                                        Ptr64 value
        )
        {
            return value.Value;
        }

    }

    public readonly struct SPtr64 : IEquatable<SPtr64>, IComparable<SPtr64>, IFormattable
    {

        public SPtr64(
               long value
        )
        {
            Value = value;
        }

        public long Value { get; }

        public int CompareTo(
                   SPtr64 other
        )
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(
                    SPtr64 other
        )
        {
            return Value == other.Value;
        }

        public override bool Equals(
                             object? obj
        )
        {
            return obj is SPtr64 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public string ToString(
                      string? format,
                      IFormatProvider? formatProvider
        )
        {
            return Value.ToString(format, formatProvider);
        }

        public static SPtr64 operator -(
                                      SPtr64 left,
                                      long right
        )
        {
            return new(left.Value - right);
        }

        public static long operator -(
                                    SPtr64 left,
                                    SPtr64 right
        )
        {
            return left.Value - right.Value;
        }

        public static bool operator !=(
                                    SPtr64 left,
                                    SPtr64 right
        )
        {
            return left.Value != right.Value;
        }

        public static SPtr64 operator &(
                                      SPtr64 left,
                                      long mask
        )
        {
            return new(left.Value & mask);
        }

        public static SPtr64 operator |(
                                      SPtr64 left,
                                      long mask
        )
        {
            return new(left.Value | mask);
        }

        public static SPtr64 operator +(
                                      SPtr64 left,
                                      long right
        )
        {
            return new(left.Value + right);
        }

        public static bool operator <(
                                    SPtr64 left,
                                    SPtr64 right
        )
        {
            return left.Value < right.Value;
        }

        public static SPtr64 operator <<(
                                      SPtr64 left,
                                      int shift
        )
        {
            return new SPtr64(left.Value << shift);
        }

        public static bool operator <=(
                                    SPtr64 left,
                                    SPtr64 right
        )
        {
            return left.Value <= right.Value;
        }

        public static bool operator ==(
                                    SPtr64 left,
                                    SPtr64 right
        )
        {
            return left.Value == right.Value;
        }

        public static bool operator >(
                                    SPtr64 left,
                                    SPtr64 right
        )
        {
            return left.Value > right.Value;
        }

        public static bool operator >=(
                                    SPtr64 left,
                                    SPtr64 right
        )
        {
            return left.Value >= right.Value;
        }

        public static SPtr64 operator >>(
                                      SPtr64 left,
                                      int shift
        )
        {
            return new SPtr64(left.Value >> shift);
        }

        public static implicit operator long(
                                        SPtr64 value
        )
        {
            return value.Value;
        }

        public static implicit operator SPtr64(
                                        long value
        )
        {
            return new(value);
        }

    }

#pragma warning disable CA2225
}
