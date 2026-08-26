// -----------------------------------------------------------------------------
// file="OpenGLFunction.Diagnostics"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StgSharp.Graphics.OpenGL
{
    public unsafe partial class OpenGLFunction
    {

        [Conditional("DEBUG")]
        public void Assert(
                    bool shouldReportWhenNormal
        )
        {
            uint code = _context->glGetError();
            if (code != glConst.NO_ERROR) {
                throw new GlExecutionException(code);
            }
            if (shouldReportWhenNormal) {
                Console.WriteLine("OpenGL runs normally");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearError()
        {
            uint code = 0;
            for (int i = 0; i < 1024; i++)
            {
                code = _context->glGetError();
                if (code == 0) {
                    return;
                }

                // Console.WriteLine(code);
            }
            foreach (ProcessThread thread in Process.GetCurrentProcess().Threads) {
                Debugger.Break();
            }
            throw new GlErrorOverflowException(1024, code);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void ClearGraphicError()
        {
            while (_context->glGetError() != 0) { }
        }

        public unsafe int GetMaskInteger(
                          uint statusCode
        )
        {
            int ret = 0;
            _context->glGetIntegerv(statusCode, &ret);
            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WaitAccomplishment()
        {
            _context->glFinish();
        }

    }
}
