//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="BpNodeActionAttribute.cs"
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
using System.Text;

namespace StgSharp.BluePrint
{
    [AttributeUsage( AttributeTargets.Method )]
    public sealed class BpNodeActionAttribute : Attribute
    {

        private string[] _input;
        private string[] _output;
        private int _operationSpan;

        public BpNodeActionAttribute( int operationSpan )
        {
            _input = Array.Empty<string>();
            _output = Array.Empty<string>();
            _operationSpan = operationSpan;
        }

        public BpNodeActionAttribute( string[] inputPort, string[] outputPort )
        {
            _input = inputPort;
            _output = outputPort;
            this._operationSpan = 1;
        }

        public BpNodeActionAttribute(
            string[] inputPort,
            string[] outputPort,
            int operationSpan )
        {
            _input = inputPort;
            _output = outputPort;
            this._operationSpan = operationSpan;
        }

        public string[] InputPort => _input;

        public string[] OutputPort => _output;

        public int OperationSpan => _operationSpan;

    }
}
