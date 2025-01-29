//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="CrossFrameBlueprintNode.cs"
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
using StgSharp.Blueprint;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Blueprint
{
    public class CrossFrameOperation : IConvertableToBlueprintNode
    {
        private int currentCount;
        private readonly int cycleCount;
        private Action _mainExecution;

        public CrossFrameOperation( Action startup, int cycleCount )
        {
            MainExecution = startup;
            this.cycleCount = ( cycleCount > 1 ) ?
            cycleCount : throw new ArgumentException(
                $"{$"The {typeof(CrossFrameOperation).Name} crosses only one cycle, "}{$"please use a {typeof(BlueprintNode).Name} instead."}" );
            this.cycleCount = cycleCount;
            currentCount = cycleCount;
        }

        public Action MainExecution
        {
            get => _mainExecution;
            private protected set => _mainExecution = value;
        }

        private static string[] _inputName = ["TaskIn"];
        private static string[] _outputName = ["TaskOut"];

        public BlueprintNodeOperation Operation
        {
            get => ExecuteMain;
        }

        public void ExecuteMain(
            in Dictionary<string, BlueprintPipeline> input,
            in Dictionary<string, BlueprintPipeline> output )
        {
            if( currentCount < cycleCount ) {
                Interlocked.Increment( ref currentCount );
            } else {
                Interlocked.Exchange( ref currentCount, 0 );
                Task.Run( MainExecution );
            }

            //Console.WriteLine(currentCount);
            BlueprintPipeline.SkipAll( output );
        }

        public ReadOnlySpan<string> InputInterfacesName => _inputName;

        public ReadOnlySpan<string> OutputInterfacesName => _outputName;

    }
}
