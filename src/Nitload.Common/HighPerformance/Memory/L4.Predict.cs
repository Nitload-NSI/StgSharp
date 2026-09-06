// -----------------------------------------------------------------------------
// file="L4.Predict"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using Nitload.Common.Collections;
using Nitload.Common.HighPerformance.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitload.Common.HighPerformance.Memory
{
    public partial class L4
    {

        private IL4Predict CurrentPredict { get; set; }

        public void ResetPredict(
                    IL4Predict predict
        )
        {
            if (CurrentPredict is null)
            {
                CurrentPredict = predict;
                return;
            }

            // TODO: write back all and register new predictor
        }

    }
}
