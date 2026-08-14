// -----------------------------------------------------------------------------
// file="TextRegex"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    public abstract class TextRegex
    {

        private const int WordCategoriesMask =
    (1 << (int)UnicodeCategory.UppercaseLetter) | (1 << (int)UnicodeCategory.LowercaseLetter) | (1 << (int)UnicodeCategory.TitlecaseLetter) | 1 << (int)UnicodeCategory.ModifierLetter | 1 << (int)UnicodeCategory.OtherLetter | 1 << (int)UnicodeCategory.NonSpacingMark | 1 << (int)UnicodeCategory.DecimalDigitNumber | 1 << (int)UnicodeCategory.ConnectorPunctuation;

        public abstract MatchResult Match(
                                    ReadOnlySpan<char> text
        );

        #region is char a word

        /*
         * Semantic reference:
         *
         *   .NET regular-expression word character: \w
         *
         * A character is treated as a word character when its Unicode general
         * category is one of:
         *
         *   Lu  UppercaseLetter
         *   Ll  LowercaseLetter
         *   Lt  TitlecaseLetter
         *   Lm  ModifierLetter
         *   Lo  OtherLetter
         *   Mn  NonSpacingMark
         *   Nd  DecimalDigitNumber
         *   Pc  ConnectorPunctuation
         *
         * The behavior and the ASCII-fast-path design are based on:
         *
         *   dotnet/runtime
         *   System.Text.RegularExpressions.RegexCharClass.IsWordChar
         *
         * Source:
         * https://github.com/dotnet/runtime/blob/main/src/libraries/
         * System.Text.RegularExpressions/src/System/Text/RegularExpressions/
         * RegexCharClass.cs
         *
         * dotnet/runtime is licensed under the MIT License.
         *
         * This implementation is written for NGRA rather than copied verbatim,
         * but the source is recorded here because its intended semantics are
         * explicitly compatible with .NET Regex.
         */

        /// <summary>
        ///   Bit mask containing every Unicode general category accepted by the default .NET
        ///   regular-expression <c> \w </c> character class.
        /// </summary>
        /// <remarks>
        ///   <see cref="UnicodeCategory" /> values are small integer identifiers, so each category
        ///   can be represented by one bit.
        /// </remarks>
        private const uint WordCategoryMask =
            (1u << (int)UnicodeCategory.UppercaseLetter) | (1u << (int)UnicodeCategory.LowercaseLetter) | (1u << (int)UnicodeCategory.TitlecaseLetter) | (1u << (int)UnicodeCategory.ModifierLetter) | (1u << (int)UnicodeCategory.OtherLetter) | (1u << (int)UnicodeCategory.NonSpacingMark) | (1u << (int)UnicodeCategory.DecimalDigitNumber) | (1u << (int)UnicodeCategory.ConnectorPunctuation);

        /// <summary>
        ///   Determines whether one UTF-16 code unit belongs to the default .NET regular-expression
        ///   <c> \w </c> character class.
        /// </summary>
        /// <param name="value">
        ///   The UTF-16 code unit to classify.
        /// </param>
        /// <returns>
        ///   <see langword="true" /> when <paramref name="value" /> is a word character; otherwise,
        ///   <see langword="false" />.
        /// </returns>
        /// <remarks>
        ///   <para> /// This method operates on a single <see cref="char" />, and therefore /// on
        ///   a single UTF-16 code unit rather than on a complete Unicode scalar /// value.
        ///   ///</para> <para> /// Characters in the Unicode Basic Multilingual Plane are
        ///   classified /// normally. A supplementary-plane character is represented by a ///
        ///   surrogate pair and is therefore presented to this method as two /// separate surrogate
        ///   code units. Neither surrogate is independently /// treated as a word character. This
        ///   follows the char-based behavior of /// the corresponding .NET Regex helper. ///</para>
        ///   <para> /// For ASCII input, the Unicode-category lookup is avoided. In ASCII, /// the
        ///   accepted set is exactly: /// <c> [A-Za-z0-9_] </c>. ///</para> <para> /// This method
        ///   implements ordinary .NET <c> \w </c> semantics. It does /// not implement the ASCII-
        ///   oriented behavior enabled by /// <c> RegexOptions.ECMAScript </c>. ///</para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool IsCharWord(
                              char value
        )
        {
            /*
             * ASCII fast path.
             *
             * Unsigned subtraction combines the lower-bound and upper-bound
             * checks into one comparison:
             *
             *   value in ['A', 'Z']
             *       => value - 'A' is in [0, 25]
             *
             * Values below 'A' underflow after conversion to uint and therefore
             * also fail the comparison.
             */
            if (value <= '\x7F')
            {
                return
                    (uint)(value - 'A') <= 'Z' - 'A' ||
                    (uint)(value - 'a') <= 'z' - 'a' ||
                    (uint)(value - '0') <= '9' - '0' ||
                    value == '_';
            }

            /*
             * Non-ASCII path.
             *
             * CharUnicodeInfo classifies the current UTF-16 code unit using
             * Unicode general categories. The resulting category selects one
             * bit from WordCategoryMask.
             */
            UnicodeCategory category =
                CharUnicodeInfo.GetUnicodeCategory(value);

            return
                (WordCategoryMask & (1u << (int)category)) != 0;
        }

        #endregion
    }

    public ref struct MatchResult { }
}
