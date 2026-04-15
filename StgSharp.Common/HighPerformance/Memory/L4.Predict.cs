//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="L4.Predict"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Common.Collections;
using StgSharp.Mathematics.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.HighPerformance.Memory
{
    public partial class L4
    {

        private ConcurrentBufferStack<int> _idReuse = new(32);

        private ConcurrentFlexibleArray<IL4Predict> _predictors = new(32);

        private ConcurrentQueue<int> _predictionQueue = new();

        private int _largestId ;

        private IL4Predict CurrentPredict { get; set; }

        private int CurrentPredictIndex { get; set; }

        public int RegisterPredict(
                   IL4Predict predict
        )
        {
            if (predict is null) {
                return -1;
            }
            if (!_idReuse.TryPop(out int id)) {
                id = _largestId++;
            }
            _predictors[id] = predict;
            _predictionQueue.Enqueue(id);
            return id;
        }

        public IL4Predict? UnregisterPredict(
                           int id
        )
        {
            if (id < 0) {
                return null;
            }
            IL4Predict p = _predictors[id];
            if (p == null) {
                return p;
            }
            _predictors[id] = null!;
            _idReuse.Push(id);
            return p;
        }

        private bool TryGetNextPredict(
        )
        {
            if (_predictionQueue.IsEmpty) {
                return false;
            }
            if (_predictionQueue.TryDequeue(out int id))
            {
                IL4Predict predict = _predictors[id] ??
                    throw new L4PredictUnregisteredException(id);
                CurrentPredict = predict;
                CurrentPredictIndex = id;
                return true;
            }
            return false;
        }

    }
}
