//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="BlueprintNode.cs"
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


using StgSharp.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Logic
{

    /// <summary>
    /// 
    /// </summary>
    /// <risk>
    /// Reset pipeline has risk, this operation is a direct operation to port
    /// in current node, the port on the other node cannot get the change.
    /// </risk>
#pragma warning disable CA1036
    public class BlueprintNode : IComparable<BlueprintNode>
#pragma warning restore CA1036
    {

        private protected bool levelAvailable, nativeRequested;
        private protected Dictionary<string, BlueprintPipeline> _inputInterfaces;
        private protected Dictionary<string, BlueprintPipeline> _outputInterfaces;
        private protected int _level;
        private IConvertableToBlueprintNode mainOperation;
        private string name;

        internal BlueprintNode(string name,
            IConvertableToBlueprintNode node, bool isNative)
        {
            this.levelAvailable = false;
            this.name = name;
            nativeRequested = isNative;
            mainOperation = node;
            _inputInterfaces = new Dictionary<string, BlueprintPipeline>();
            _outputInterfaces = new Dictionary<string, BlueprintPipeline>();
            foreach (string portName in node.InputInterfacesName)
            {
                _inputInterfaces.Add(portName, null!);
            }

            foreach (string portName in node.OutputInterfacesName)
            {
                _outputInterfaces.Add(portName, null!);
            }
        }


#pragma warning disable CS8618
        internal BlueprintNode(string name, bool isNative,
            string[] inputPorts,
            string[] outputPorts
            )
#pragma warning restore CS8618
        {
            if (inputPorts == null || inputPorts.Length == 0)
            {
                throw new ArgumentNullException(nameof(inputPorts));
            }
            if (outputPorts == null || outputPorts.Length == 0)
            {
                throw new ArgumentNullException(nameof(outputPorts));
            }
            levelAvailable = false;
            this.name = name;
            IsNative = isNative;
            _inputInterfaces = new Dictionary<string, BlueprintPipeline>();
            _outputInterfaces = new Dictionary<string, BlueprintPipeline>();
            foreach (string portName in inputPorts)
            {
                _inputInterfaces.Add(portName, null!);
            }
            foreach (string portName in outputPorts)
            {
                _outputInterfaces.Add(portName, null!);
            }
        }

        internal IConvertableToBlueprintNode Value
        {
            get => mainOperation;
        }

        public bool IsNative
        {
            get => nativeRequested;
            internal set => nativeRequested = value;
        }

        public int Level
        {
            get
            {
                if (levelAvailable)
                {
                    return _level;
                }
                _level = 0;
                foreach (KeyValuePair<string, BlueprintPipeline> item in _inputInterfaces)
                {
                    if (item.Value == null)
                    {
                        continue;
                    }
                    if (_level <= item.Value.Level)
                    {
                        _level = item.Value.Level;
                    }
                }
                _level += 1;
                return _level;
            }
        }

        public string Name => name;

        internal Dictionary<string, BlueprintPipeline> InputInterfaces => _inputInterfaces;
        internal Dictionary<string, BlueprintPipeline> OutputInterfaces => _outputInterfaces;


        public void AddAfter(
            BlueprintNode former,
            string formerOutputPortName,
            string thisInputPortName)
        {
            this.levelAvailable = false;
            BlueprintPipeline pipeline = new BlueprintPipeline(former, this);
            if (former is BeginningNode bnode)
            {
                bnode.SetCertainOutputPort(formerOutputPortName, pipeline);
            }
            else
            {
                former.SetCertainOutputPort(formerOutputPortName, pipeline);
            }
            SetCertainInputPort(thisInputPortName, pipeline);
        }


        public void AddBefore(
            BlueprintNode after,
            string thisOutputPortName,
            string afterInputPortName)
        {
            this.levelAvailable = false;
            BlueprintPipeline pipeline = new BlueprintPipeline(this, after);
            if (after is EndingNode enode)
            {
                enode.SetCertainInputPort(afterInputPortName, pipeline);
            }
            else
            {
                after.SetCertainInputPort(afterInputPortName, pipeline);
            }
            SetCertainOutputPort(thisOutputPortName, pipeline);
        }

        public int CompareTo(BlueprintNode other)
        {
#pragma warning disable CA1062
            return this.Level.CompareTo(other.Level);
#pragma warning restore CA1062
        }


        public virtual void Run()
        {
            try
            {
                //Console.WriteLine(name);
                BlueprintPipeline.WaitAll(_inputInterfaces);
                if (mainOperation != null)
                {
                    mainOperation.ExecuteMain(in _inputInterfaces, in _outputInterfaces);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region port operation

        public BlueprintPipeline GetCertainInputPort(string name)
        {
            BlueprintPipeline ret;
            if (!_inputInterfaces.TryGetValue(name, out ret))
            {
                throw new Exception($"Cannot find the interface named {name} in node {Name}.");
            }
            return ret;
        }


        public BlueprintPipeline GetCertainOutputPort(string name)
        {
            BlueprintPipeline ret;
            if (!_outputInterfaces.TryGetValue(name, out ret))
            {
                throw new Exception($"Cannot find the interface named {name} in node {Name}.");
            }
            return ret;
        }

        public void SetCertainInputPort(string name, BlueprintPipeline pipeline)
        {
            if (!_inputInterfaces.ContainsKey(name))
            {
                throw new Exception($"Cannot find the interface named {name} in node {Name}.");
            }
            _inputInterfaces[name] = pipeline;
        }

        public void SetCertainOutputPort(string name, BlueprintPipeline pipeline)
        {
            if (!_outputInterfaces.ContainsKey(name))
            {
                throw new Exception($"Cannot find the interface named {name} in node {Name}.");
            }
            _outputInterfaces[name] = pipeline;
        }

        #endregion

        
    }//--------------------------------- end of class -----------------------------------

    

    public class BlueprintNodeRunException : Exception
    {

        protected BlueprintNodeRunException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BlueprintNodeRunException()
        {
        }

        public BlueprintNodeRunException(Exception ex) : base(string.Empty, ex)
        {
        }

        public BlueprintNodeRunException(string message) : base(message)
        {
        }

        public BlueprintNodeRunException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }

}
