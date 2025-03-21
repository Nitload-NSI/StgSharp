﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="IConvertableToBlueprintNode.cs"
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
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.PipeLine
{
    public delegate void BlueprintNodeOperation(
                                 in Dictionary<string, PipelineConnector> input,
                                 in Dictionary<string, PipelineConnector> output );

    public interface IConvertableToBlueprintNode
    {

        BlueprintNodeOperation Operation
        {
            get;
        }

        public IEnumerable<string> InputInterfacesName
        {
            get;
        }

        public IEnumerable<string> OutputInterfacesName
        {
            get;
        }

    }

    internal class DefaultConvertableToBlueprintNode : IConvertableToBlueprintNode
    {

        private string[] _input;
        private string[] _output;
        private BlueprintNodeOperation _execution;

        public DefaultConvertableToBlueprintNode(
                       BlueprintNodeOperation execution,
                       string[] input,
                       string[] output )
        {
            _input = input;
            _output = output;
            _execution = execution;
        }

        public BlueprintNodeOperation Operation => _execution;

        public IEnumerable<string> InputInterfacesName => _input;

        public IEnumerable<string> OutputInterfacesName => _output;

    }
}
