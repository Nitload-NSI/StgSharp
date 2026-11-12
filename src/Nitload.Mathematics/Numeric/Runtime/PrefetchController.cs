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
             L4.Handle cache)
        {
            
        }

    }
}
