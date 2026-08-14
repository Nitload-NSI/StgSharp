// -----------------------------------------------------------------------------
// file="TextRegexSourceGen.GenClass.Prefix"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

namespace StgSharp.RegularAnalysis.Text
{
    internal partial class TextRegexSourceGen
    {

        /*
         * =======================================================================================
         * L5, prefix decoration
         * =======================================================================================
         *
         * A prefix consumes no IR of its own. It wraps whatever an L1 to L4 body generator
         * produced:
         *
         *     [prefix open ]   if (<observed result>) {
         *     [ body       ]       ...  sets __ok, advances __cur
         *     [ line slot  ]       __lK_ok = __ok;
         *     [ pop        ]       if (__ok) { __rN_ok = true; break; }
         *     [prefix close]   }
         *
         * Keeping this outside the body generators is what lets L2 and L4 stay small. They only
         * describe what an instruction does, never when it runs or who reads the outcome. Without
         * this layer the other layers cannot be assembled into correct control flow at all, so L5
         * is part of the correctness core rather than an optimisation.
         *
         * Checklist, RegexIRPrefixMask on the left:
         *
         *   L5.1   ExeCondition  none            run unconditionally
         *   L5.2   ExeCondition  Prev_Success    wrap in if (observed)
         *   L5.3   ExeCondition  Prev_Fail       wrap in if (!observed)
         *   L5.4   ExeCondition  Always          run unconditionally, deliberately detached from
         *                                        the surrounding condition chain
         *   L5.5   ObserveMethod none            read the result of the preceding instruction
         *   L5.6   ObserveMethod Observe_Line    read the slot named by ObserveSource
         *   L5.7   ObserveMethod Observe_Region  read the slot named by ObserveSource
         *   L5.8   PopCondition  Pop_Success     write the region result true, then leave it
         *   L5.9   PopCondition  Pop_Fail        write the region result false, then leave it
         *   L5.10  WorkingRegion                 selects which region scope the code lands in
         *
         * Rules the implementation has to hold:
         *
         *   - Observe_Line and Observe_Region must read the slot ObserveSource names. Neither may
         *     degrade into reading the previous instruction, which is what an absent observe
         *     method means and nothing else.
         *   - Line slots are allocated only for lines some prefix actually observes. That demand
         *     has to be collected in a pass before body generation, because an observer can be
         *     emitted before the line it reads, for instance when a nested region observes a line
         *     of its parent.
         *   - A pop lowers to break, which leaves the nearest enclosing do while(false) block.
         *     That block is the region the instruction works in, because every region is emitted
         *     as one and the whole match body is wrapped as region zero.
         *   - Open and close must be paired. A body generator that declines the window must be
         *     detected before the opening brace is emitted, so bodies are generated into a
         *     detached emitter first.
         *
         * The open call should report how many braces it opened, so the close call knows how many
         * to emit rather than recomputing the condition.
         */
    }
}
