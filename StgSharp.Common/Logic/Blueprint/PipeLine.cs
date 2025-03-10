﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PipeLine.cs"
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

namespace StgSharp.Blueprint
{
    public class BlueprintPipeline
    {

        private protected BlueprintNode after;
        private protected BlueprintNode former;
        private protected BlueprintPipelineArgs args;
        private protected SemaphoreSlim formerCompleteSemaphore;

        public BlueprintPipeline( BlueprintNode former, BlueprintNode after )
        {
            this.former = former;
            this.after = after;
            formerCompleteSemaphore = new SemaphoreSlim(
                initialCount: 0, maxCount: 1 );
        }

        public BlueprintNode After => after;

        public BlueprintNode Former => former;

        public BlueprintPipelineArgs Args
        {
            get => args;
            set => args = value;
        }

        public int Level => former.Level;

        public static void SkipAll(
            Dictionary<string, BlueprintPipeline> ports )
        {
            if( ports == null ) {
                return;
            }
            foreach( KeyValuePair<string, BlueprintPipeline> item in ports ) {
                if( item.Value != null ) {
                    item.Value.CompleteAndSkip();
                }
            }
        }

        public static void WaitAll(
            Dictionary<string, BlueprintPipeline> ports )
        {
            if( ports == null ) {
                return;
            }
            foreach( KeyValuePair<string, BlueprintPipeline> item in ports ) {
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

        ~BlueprintPipeline()
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
        public void CompleteAndWriteData( BlueprintPipelineArgs args )
        {
            this.args = args;
            CompleteAndSkip();
        }

        #endregion
    }

    public abstract class BlueprintPipelineArgs
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

    public sealed class SealedBlueprintPiipeline:BlueprintPipeline
    {
        internal SealedBlueprintPiipeline(BlueprintNode former, BlueprintNode after):
            base(former, after)
        {
            
        }

        public static SealedBlueprintPiipeline SealOutput(BlueprintNode node, string portName)
        {
            return new SealedBlueprintPiipeline(node, BlueprintNode.Empty);
        }

        public static SealedBlueprintPiipeline SealInput(BlueprintNode node, string portName)
        {
            return new SealedBlueprintPiipeline(BlueprintNode.Empty, node);
        }

        public sealed override void WaitForStart()
        {

        }
    }
}
