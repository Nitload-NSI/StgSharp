﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Connector.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
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

        private protected PipeLineNode after;
        private protected PipeLineNode former;
        private protected PipeLineConnectorArgs args;
        private protected SemaphoreSlim formerCompleteSemaphore;

        public PipelineConnector( PipeLineNode former, PipeLineNode after )
        {
            this.former = former;
            this.after = after;
            formerCompleteSemaphore = new SemaphoreSlim(
                initialCount: 0, maxCount: 1 );
        }

        public PipeLineNode After => after;

        public PipeLineNode Former => former;

        public PipeLineConnectorArgs Args
        {
            get => args;
            set => args = value;
        }

        public int Level => former.Level;

        public static void SkipAll(
                                   Dictionary<string, PipelineConnector> ports )
        {
            if( ports == null ) {
                return;
            }
            foreach( KeyValuePair<string, PipelineConnector> item in ports ) {
                if( item.Value != null ) {
                    item.Value.CompleteAndSkip();
                }
            }
        }

        public static void WaitAll(
                                   Dictionary<string, PipelineConnector> ports )
        {
            if( ports == null ) {
                return;
            }
            foreach( KeyValuePair<string, PipelineConnector> item in ports ) {
                if( item.Value != null ) {
                    item.Value.WaitForStart();
                }
            }
        }

        public virtual void WaitForStart()
        {
            //Console.WriteLine($"Waiting   \t{former.ContextName}\t->\t{after.ContextName}");
            formerCompleteSemaphore.Wait();
        }

        ~PipelineConnector()
        {
            formerCompleteSemaphore.Dispose();
        }

        #region pipline op

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public void CompleteAndSkip()
        {
            //Console.WriteLine($"Complete\t{former.Name}\t->\t{after.Name}");
            formerCompleteSemaphore.Release();
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public void CompleteAndWriteData( PipeLineConnectorArgs args )
        {
            this.args = args;
            CompleteAndSkip();
        }

        #endregion
    }

    public abstract class PipeLineConnectorArgs
    {

        public object? Value
        {
            get;
            set;
        }

        public object? ValueDefault
        {
            get;
        }

    }

    public sealed class SealedBlueprintPiipeline : PipelineConnector
    {

        internal SealedBlueprintPiipeline(
                         PipeLineNode former,
                         PipeLineNode after )
            : base( former, after ) { }

        public static SealedBlueprintPiipeline SealInput(
                                                       PipeLineNode node,
                                                       string portName )
        {
            return new SealedBlueprintPiipeline( PipeLineNode.Empty, node );
        }

        public static SealedBlueprintPiipeline SealOutput(
                                                       PipeLineNode node,
                                                       string portName )
        {
            return new SealedBlueprintPiipeline( node, PipeLineNode.Empty );
        }

        public override sealed void WaitForStart() { }

    }
}
