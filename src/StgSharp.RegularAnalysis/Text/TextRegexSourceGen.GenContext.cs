// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenContext"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.RegularAnalysis.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {

        #region var naming

        private class SourceGenContext
        {

            public bool Need_char_cur { get; set; }

            public bool Need_remain_cur { get; set; }

            public bool Need_find_result { get; set; }

            private const string __char_cur = "__char_cur";
            private const string __remain_cur = "__remain_cur";
            private const string __find_result = "__find_result";

            private int _min_region_begin_index;

            public string MakeNewRegionBegin()
            {
                string name = $"__region_begin_{_min_region_begin_index}";
                _min_region_begin_index++;
                return name;
            }

            public string CurChar
            {
                get
                {
                    Need_char_cur = true;
                    return __char_cur;
                }
            }

            public string FindResult
            {
                get
                {
                    Need_find_result = true;
                    return __find_result;
                }
            }

            public string CurRemain
            {
                get
                {
                    Need_remain_cur = true;
                    return __remain_cur;
                }
            }

            public SequenceEmitter<string> GenVarDefine(
                                           SequenceEmitter<string> se
            )
            {
                if (se is null) {
                    return null!;
                }
                _ = se.AppendLine($@"int {__char_cur}", Need_char_cur)
                      .AppendLine($@"int {__remain_cur}", Need_remain_cur);

                return se;
            }

        }

        #endregion
    }
}
