// -----------------------------------------------------------------------------
// file="PipelineNodePort"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.PipeLine
{
    public class PipelineNodeInPort
    {

        private HashSet<PipelineNodeOutPort> _linkerSet = new HashSet<PipelineNodeOutPort>();

        public PipelineNodeInPort(
               PipelineNode container,
               string label
        )
        {
            Container = container;
            Label = label;
        }

        public IEnumerable<PipelineNodeOutPort> LinkedPorts => _linkerSet;

        public PipelineNode Container { get; init; }

        public SemaphoreSlim Waiter { get; init; }

        public string Label { get; init; }

        internal HashSet<PipelineNodeOutPort> Linkers => _linkerSet;

        public void Connect(
                    PipelineNodeOutPort port
        )
        {
            ArgumentNullException.ThrowIfNull(port);
            if (_linkerSet.Add(port))
            {
                port.Linkers.Add(this);
                Container.Previous.Add(port.Container);
                port.Container.Next.Add(Container);
            }
        }

        public IEnumerator<PipelineNode> GetFormerNodes()
        {
            foreach (PipelineNodeOutPort port in _linkerSet) {
                yield return port.Container;
            }
        }

        public void Wait()
        {
            PipelineNodeOutPort.WaitAll(_linkerSet);
        }

        public static void WaitAll(
                           IDictionary<string, PipelineNodeInPort> ports
        )
        {
            if (ports is null) {
                return;
            }
            foreach (PipelineNodeInPort port in ports.Values) {
                port.Wait();
            }
        }

        public static void WaitAll(
                           IEnumerable<PipelineNodeInPort> ports
        )
        {
            if (ports is null) {
                return;
            }
            foreach (PipelineNodeInPort port in ports) {
                port.Wait();
            }
        }

    }

    public class PipelineNodeOutPort
    {

        private HashSet<PipelineNodeInPort> _linkerSet = new HashSet<PipelineNodeInPort>();
        private volatile int _passCount;
        private SemaphoreSlim formerCompleteSemaphore;

        public PipelineNodeOutPort(
               PipelineNode container,
               string label
        )
        {
            Container = container;
            Label = label;
            formerCompleteSemaphore = new SemaphoreSlim(initialCount:0, maxCount:1);
        }

        public PipelineNode Container { get; init; }

        public string Label { get; init; }

        internal HashSet<PipelineNodeInPort> Linkers => _linkerSet;

        private SemaphoreSlim Waiter => formerCompleteSemaphore;

        public void Connect(
                    PipelineNodeInPort port
        )
        {
            ArgumentNullException.ThrowIfNull(port);
            if (_linkerSet.Add(port))
            {
                port.Linkers.Add(this);
                Container.Next.Add(port.Container);
                port.Container.Previous.Add(Container);
            }
        }

        public void Release()
        {
            if (_passCount == 0)
            {
                Waiter.Release();
            } else
            {
                return;
            }
        }

        public static void SkipAll(
                           Dictionary<string, PipelineNodeOutPort> ports
        )
        {
            if (ports is null) {
                return;
            }
            foreach (PipelineNodeOutPort port in ports.Values) {
                port.Release();
            }
        }

        public void TransmitValue(
                    IPipeLineConnectionPayload value
        )
        {
            foreach (PipelineNodeInPort item in _linkerSet) { }
        }

        public static void WaitAll(
                           IEnumerable<PipelineNodeOutPort> ports
        )
        {
            if (ports is null) {
                return;
            }
            foreach (PipelineNodeOutPort port in ports) {
                port.Wait();
            }
        }

        internal void Wait()
        {
            Waiter.Wait();
            if (_passCount >= _linkerSet.Count)
            {
                Interlocked.Exchange(ref _passCount, 0);
            } else
            {
                Interlocked.Increment(ref _passCount);
                Waiter.Release();
            }
        }

    }
}
