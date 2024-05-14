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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Logic
{
    public delegate void BlueprintNodeAction(
        in Dictionary<string, BlueprintPipeline> inpput,
        in Dictionary<string, BlueprintPipeline> output);



    /// <summary>
    /// 
    /// </summary>
    /// <risk>
    /// Reset pipeline has risk, this operation is a direct operation to port
    /// in current node, the port on the other node cannot get the change.
    /// </risk>
    public class BlueprintNode : IComparable<BlueprintNode>
    {

        private Blueprint local;
        private BlueprintNodeAction mainOperation;

        private bool levelAvailabel, nativeRequested;
        private int level;
        private string name;

        protected Dictionary<string, BlueprintPipeline> inputInterfaces;
        protected Dictionary<string, BlueprintPipeline> outputInterfaces;

        internal BlueprintNode(string name,
            BlueprintNodeAction operation, bool isNative,
            string[] inputPorts,
            string[] outputPorts)
        {
            this.levelAvailabel = false;
            this.name = name;
            nativeRequested = isNative;
            mainOperation = operation;
            inputInterfaces = new Dictionary<string, BlueprintPipeline>();
            outputInterfaces = new Dictionary<string, BlueprintPipeline>();
            if (inputPorts.Length * outputPorts.Length == 0)
            {
                throw new Exception();
            }
            foreach (var portName in inputPorts)
            {
                inputInterfaces.Add(portName, null);
            }
            foreach (var portName in outputPorts)
            {
                outputInterfaces.Add(portName, null);
            }
        }


        public BlueprintNode(string name, IConvertableToBlueprintNode convertable)
        {
            this.levelAvailabel = false;
            this.name = name;
            inputInterfaces = new Dictionary<string, BlueprintPipeline>();
            outputInterfaces = new Dictionary<string, BlueprintPipeline>();
            mainOperation = convertable.MainExecution;
            foreach (var soket in convertable.InputInterfacesName)
            {
                inputInterfaces.Add(soket, null);
            }
            foreach (var soket in convertable.OutputInterfacesName)
            {
                outputInterfaces.Add(soket, null);
            }
        }

        public BlueprintNode(string name,
            string[] inputPorts,
            string[] outputPorts
            )
        {
            this.levelAvailabel = false;
            this.name = name;
            inputInterfaces = new Dictionary<string, BlueprintPipeline>();
            outputInterfaces = new Dictionary<string, BlueprintPipeline>();
            if (inputPorts.Length * outputPorts.Length == 0)
            {
                throw new Exception();
            }
            foreach (var portName in inputPorts)
            {
                inputInterfaces.Add(portName, null);
            }
            foreach (var portName in outputPorts)
            {
                outputInterfaces.Add(portName, null);
            }
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
                if (levelAvailabel)
                {
                    return level;
                }
                level = 0;
                foreach (var item in inputInterfaces)
                {
                    if (item.Value == null)
                    {
                        continue;
                    }
                    if (level <= item.Value.Level)
                    {
                        level = item.Value.Level;
                    }
                }
                level += 1;
                return level;
            }
        }

        public string Name => name;

        internal Dictionary<string, BlueprintPipeline> InputInterfaces => inputInterfaces;
        internal Dictionary<string, BlueprintPipeline> OutputInterfaces => outputInterfaces;


        public void AddAfter(
            BlueprintNode former,
            string formerOutputPortName,
            string thisInputPortName)
        {
            this.levelAvailabel = false;
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
            this.levelAvailabel = false;
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


        public void Run()
        {
            try
            {
                //Console.WriteLine(name);
                BlueprintPipeline.WaitAll(inputInterfaces);
                mainOperation(in inputInterfaces, in outputInterfaces);
            }
            catch (Exception)
            {
                throw;
            }
        }


        int IComparable<BlueprintNode>.CompareTo(BlueprintNode other)
        {
            return this.Level.CompareTo(other.Level);
        }

        #region port operation

        public BlueprintPipeline GetCertainInputPort(string name)
        {
            if (!inputInterfaces.ContainsKey(name))
            {
                throw new Exception($"Cannot find the interface named {name} in node {Name}.");
            }
            return inputInterfaces[name];
        }


        public BlueprintPipeline GetCertainOutputPort(string name)
        {
            if (!outputInterfaces.ContainsKey(name))
            {
                throw new Exception($"Cannot find the interface named {name} in node {Name}.");
            }
            return outputInterfaces[name];
        }

        public void SetCertainInputPort(string name, BlueprintPipeline pipeline)
        {
            if (!inputInterfaces.ContainsKey(name))
            {
                throw new Exception($"Cannot find the interface named {name} in node {Name}.");
            }
            inputInterfaces[name] = pipeline;
        }

        public void SetCertainOutputPort(string name, BlueprintPipeline pipeline)
        {
            if (!outputInterfaces.ContainsKey(name))
            {
                throw new Exception($"Cannot find the interface named {name} in node {Name}.");
            }
            outputInterfaces[name] = pipeline;
        }

        #endregion


    }//--------------------------------- end of class -----------------------------

    public interface IConvertableToBlueprintNode
    {

        public string[] InputInterfacesName { get; }
        public BlueprintNodeAction MainExecution { get; }
        public string[] OutputInterfacesName { get; }

    }

    public class BlueprintNodeRunException : Exception
    {

        public BlueprintNodeRunException(Exception ex) : base(string.Empty, ex)
        {
        }

        public BlueprintNodeRunException(string message, Exception ex) : base(message, ex)
        {
        }

    }

}
