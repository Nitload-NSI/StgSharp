using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Logic
{

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class BpNodeActionAttribute : Attribute
    {
        private string[] _input;
        private string[] _output;
        private int _operationSpan;

        public string[] InputPort => _input;
        public string[] OutputPort => _output;
        public int OperationSpan => _operationSpan;

        public BpNodeActionAttribute(string[] inputPort, string[] outputPort, int operationSpan)
        {
            _input = inputPort;
            _output = outputPort;
            this._operationSpan = operationSpan;
        }
        
        public BpNodeActionAttribute(string[] inputPort, string[] outputPort)
        {
            _input = inputPort;
            _output = outputPort;
            this._operationSpan = 1;
        }

        public BpNodeActionAttribute(int operationSpan)
        {
            _input = Array.Empty<string>();
            _output = Array.Empty<string>();
            _operationSpan = operationSpan;
        }

    }
}
