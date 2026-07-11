// -----------------------------------------------------------------------------
// file="PortArgs"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.PipeLine
{
    public class PipelineConnector
    {

        private protected IPipeLineConnectionPayload args;
        private protected PipelineNodeInPort output;
        private protected PipelineNodeOutPort input;
        private protected SemaphoreSlim formerCompleteSemaphore;

        public PipelineConnector(
               PipelineNodeOutPort former,
               PipelineNodeInPort after
        )
        {
            this.input = former;
            this.output = after;
            formerCompleteSemaphore = new SemaphoreSlim(initialCount:0, maxCount:1);
        }

        public IPipeLineConnectionPayload Args
        {
            get => args;
            set => args = value;
        }

        public PipelineNodeInPort Output => output;

        public PipelineNodeOutPort Input => input;

        public static void SkipAll(
                           Dictionary<string, PipelineConnector> ports
        )
        {
            if (ports == null) {
                return;
            }
            foreach (KeyValuePair<string, PipelineConnector> item in ports)
            {
                if (item.Value != null) {
                    item.Value.CompleteAndSkip();
                }
            }
        }

        public static void WaitAll(
                           Dictionary<string, PipelineConnector> ports
        )
        {
            if (ports == null) {
                return;
            }
            foreach (KeyValuePair<string, PipelineConnector> item in ports)
            {
                if (item.Value != null) {
                    item.Value.WaitForStart();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void WaitForStart()
        {
            // Console.WriteLine($"Waiting   \t{input.ContextName}\t->\t{output.ContextName}");
            formerCompleteSemaphore.Wait();
        }

        ~PipelineConnector()
        {
            formerCompleteSemaphore.Dispose();
        }

        #region pipline op

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompleteAndSkip()
        {
            // Console.WriteLine($"Complete\t{input.Name}\t->\t{output.Name}");
            formerCompleteSemaphore.Release();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CompleteAndWriteData(
                    IPipeLineConnectionPayload args
        )
        {
            this.args = args;
            CompleteAndSkip();
        }

        #endregion
    }

    public abstract class IPipeLineConnectionPayload { }
}
