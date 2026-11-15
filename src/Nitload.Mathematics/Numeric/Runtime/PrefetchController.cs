// -----------------------------------------------------------------------------
// file="PrefetchController"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.HighPerformance.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Mathematics.Numeric.Runtime
{
    public interface IPrefetchController
    {

        int Id { get; }

        void Prefetch(
             L4.AddressSource source,
             L4.Handle cache
        );

    }

    internal class PrefetchController
    {

        public int Id { get; }

        public void Prefetch(
                    L4.AddressSource source,
                    L4.Handle cache
        )
        {
            // for highest byte of source.description must being type code ranging 1-10, so negative value or say 1 at highest bit can be used as mark for "prefetch accomplishment"
            if (source.Description < 0)
            {
                return;
            }

            // for prefetch op set as 0, which means NoOp, skip and mark as "prefetch accomplished"
            uint profile = source.Description;
            ushort exe_mask = (ushort)profile;
            MatrixPackType exe_handle = (MatrixPackType)exe_mask;
            if (exe_handle != MatrixPackType.NoOp)
            {
                // we need a "cpu core local"
            }

            // otherwise, we need a prefetch operation, so we mark the source as "prefetch accomplished" by setting the highest bit of source.description to 1
        }

    }
}
