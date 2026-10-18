// -----------------------------------------------------------------------------
// file="L4Predictor"
// Project: StgSharp
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

    }

    public interface IL4Predictor<TMission> : IL4Predictor
    {

        bool TryGetNextMission(
             out TMission mission
        );

    }

}
