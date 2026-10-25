// -----------------------------------------------------------------------------
// file="L4Predictor"
// Project: ['.] nFramework
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Common.HighPerformance.Memory
{
    public interface IL4Predictor
    {

        int Id { get; set; }

        int PredictMission(
            in Span<L4.AddressSource> RequiredBuffer
        );

        void Prefetch(
             L4.AddressSource RequiredBuffer,
             nuint allocBuffer,
             nuint bufferSize
        );

    }

    public interface IL4Predictor<TMission> : IL4Predictor
    {

        bool TryGetNextMission(
             out TMission mission
        );

    }
}
