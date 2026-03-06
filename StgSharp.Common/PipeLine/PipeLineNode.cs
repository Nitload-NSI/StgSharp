//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="PipelineNode"
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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.PipeLine
{
    /// <summary>
    ///
    /// </summary>
    /// <risk>
    ///   Reset pipeline has risk, this operation is a direct operation to port in current node, the
    ///   port on the other node cannot get the change.
    /// </risk>
#pragma warning disable CA1036
    public class PipelineNode
    #pragma warning restore CA1036
    {

        private static readonly Dictionary<string, PipelineConnector> _emptyLayer = [];
        private static PipelineNode _emptyNode = new PipelineNode(
            PipelineNodeLabel.Empty, null!, false);
        private IConvertableToPipelineNode _mainOperation;
        private PipelineNodeLabel _label;
        private protected bool _nativeRequested;
        private protected Dictionary<string, PipelineNodeInPort> _input;
        private protected Dictionary<string, PipelineNodeOutPort> _output;
        private protected int _level, _count;

        internal PipelineNode(PipelineNodeLabel label, IConvertableToPipelineNode node, bool isNative)
        {
            _label = label;
            _nativeRequested = isNative;
            _mainOperation = node;
            InputPorts = new();
            OutputPorts = new();
            foreach (string port in node.InputPortName) {
                InputPorts.Add(port, new PipelineNodeInPort(this, port));
            }
            foreach (string port in node.OutputPortName) {
                OutputPorts.Add(port, new PipelineNodeOutPort(this, port));
            }
        }

        #pragma warning disable CS8618
        internal PipelineNode(
                 PipelineNodeOperation operation,
                 PipelineNodeLabel name,
                 bool isNative,
                 string[] inputNames,
                 string[] outputNames)
        #pragma warning restore CS8618
        {
            if ((inputNames == null) || (inputNames.Length == 0)) {
                throw new ArgumentNullException(nameof(inputNames));
            }
            if ((outputNames == null) || (outputNames.Length == 0)) {
                throw new ArgumentNullException(nameof(outputNames));
            }
            _label = name;
            _nativeRequested = isNative;
            if (operation == null) {
                operation = DefaultOperation;
            }
            _mainOperation = new DefaultConvertableToBlueprintNode(
                operation, inputNames, outputNames);
            InputPorts = new Dictionary<string, PipelineNodeInPort>();
            OutputPorts = new Dictionary<string, PipelineNodeOutPort>();
            foreach (string port in inputNames) {
                InputPorts.Add(port, new PipelineNodeInPort(this, port));
            }
            foreach (string port in outputNames) {
                OutputPorts.Add(port, new PipelineNodeOutPort(this, port));
            }
        }

        public bool IsNative
        {
            get => _nativeRequested;
            internal set => _nativeRequested = value;
        }

        public static Dictionary<string, PipelineConnector> EmptyPortLayer => _emptyLayer;

        public IConvertableToPipelineNode NodeOperation => _mainOperation;

        public int Level
        {
            get => _level;
            private set => _level = value;
        }

        public static PipelineNode Empty => _emptyNode;

        public PipelineNodeLabel Label => _label;

        internal Dictionary<string, PipelineNodeInPort> InputPorts
        {
            get => _input;
            private set => _input = value;
        }

        internal Dictionary<string, PipelineNodeOutPort> OutputPorts
        {
            get => _output;
            private set => _output = value;
        }

        internal HashSet<PipelineNode> Previous { get; } = [];

        internal HashSet<PipelineNode> Next { get; } = [];

        internal IConvertableToPipelineNode Value => _mainOperation;

        internal int RemainingIncreaseCount => Previous.Count - _count;

        public static void Connect(PipelineNode front, string frontNodePort, string backNodePort, PipelineNode back)
        {
            ArgumentNullException.ThrowIfNull(front);
            ArgumentNullException.ThrowIfNull(back);
            if (!front.OutputPorts
                      .TryGetValue(frontNodePort,
                                                out PipelineNodeOutPort? frontPort)) {
                throw new ArgumentOutOfRangeException($"Cannot find port named {frontNodePort}");
            }
            if (!back.InputPorts.TryGetValue(backNodePort, out PipelineNodeInPort? backPort)) {
                throw new ArgumentOutOfRangeException($"Cannot find port named {backNodePort}");
            }
            backPort.Connect(frontPort);
            frontPort.Connect(backPort);
        }

        public IEnumerator<PipelineNode> GetFormerNodes()
        {
            foreach (PipelineNodeInPort port in InputPorts.Values)
            {
                IEnumerator<PipelineNode> enumerator = port.GetFormerNodes();
                while (enumerator.MoveNext()) {
                    yield return enumerator.Current;
                }
            }
        }

        public virtual void Run()
        {
            try
            {
                // Console.WriteLine(_label);
                PipelineNodeInPort.WaitAll(InputPorts);
                if (_mainOperation != null) {
                    _mainOperation.NodeMain(in _input, in _output);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal bool TryIncreaseLevelTo(int level)
        {
            _count++;
            if (_count > Previous.Count) {
                return false;
            }
            Level = int.Max(level, this.Level);
            return true;
        }

        protected static void DefaultOperation(
                              in Dictionary<string, PipelineNodeInPort> input,
                              in Dictionary<string, PipelineNodeOutPort> output)
        {
            PipelineNodeOutPort.SkipAll(output);
        }

        #region port operation

        public PipelineNodeInPort GetInputPort(string lable)
        {
            if (!InputPorts.TryGetValue(lable, out PipelineNodeInPort? ret)) {
                throw new ArgumentOutOfRangeException(
                    $"Cannot find the interface named {lable} in node {Label}.");
            }
            return ret;
        }

        public PipelineNodeOutPort GetOutputPort(string lable)
        {
            if (!OutputPorts.TryGetValue(lable, out PipelineNodeOutPort? ret)) {
                throw new ArgumentOutOfRangeException(
                    $"Cannot find the interface named {lable} in node {Label}.");
            }
            return ret;
        }

        public PipelineNodeInPort? DefineCertainInputPort(string label)
        {
            if (InputPorts.ContainsKey(label)) {
                return null;
            }
            PipelineNodeInPort port = new(this, label);
            InputPorts[label] = port;
            return port;
        }

        public PipelineNodeOutPort? DefineCertainOutputPort(string label)
        {
            if (OutputPorts.ContainsKey(label)) {
                return null;
            }
            PipelineNodeOutPort port = new(this, label);
            OutputPorts[label] = port;
            return port;
        }

        #endregion
    }//--------------------------------- end of class -----------------------------------

    public class PipelineNodeRunException : Exception
    {

        public PipelineNodeRunException() { }

        public PipelineNodeRunException(Exception ex) : base(string.Empty, ex) { }

        public PipelineNodeRunException(string message) : base(message) { }

        public PipelineNodeRunException(string message, Exception innerException) : base(message, innerException) { }

    }
}
