// -----------------------------------------------------------------------------
// file="NumericModuleInitProfile"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.HighPerformance.Memory;

namespace Nitload.Mathematics.Numeric
{
    public record class NumericModuleInitProfile : IModuleInitializeConfiguration
    {

        public L4ManagerCapacityLayout CacheSizeLayout { get; set; }

        public L4ManagerRetirePolicy RuntimeRole { get; set; }

        public static NumericModuleInitProfile Default { get; } = new()
        {
            CacheSizeLayout = L4ManagerCapacityLayout.Default,
            CacheSize = 1024 * 1024,
            RuntimeRole = L4ManagerRetirePolicy.Auto,
        };

        public uint CacheSize { get; set; }

    }
}
