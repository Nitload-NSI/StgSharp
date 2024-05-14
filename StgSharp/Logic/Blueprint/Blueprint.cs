//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Blueprint.cs"
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
using StgSharp;
using StgSharp.Logic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StgSharp.Logic
{
    public class Blueprint : IConvertableToBlueprintNode
    {
        private BeginningNode beginNode;
        private Dictionary<string, BlueprintNode> allNode;
        private EndingNode endNode;
        private Func<bool> terminate;

        private int currentNodeIndex;
        private List<Task> allThread;
        private List<BlueprintNode> nativeNodeList;
        private List<BlueprintNode> nodeList;

        public Blueprint()
        {
            allNode = new Dictionary<string, BlueprintNode>();
            allThread = new List<Task>();
            nodeList = new List<BlueprintNode>();
            nativeNodeList = new List<BlueprintNode>();
            terminate = () => true;
            beginNode = new BeginningNode();
            endNode = new EndingNode();
        }

        public BlueprintNode BeginLayer => beginNode;
        public BlueprintNode EndLayer => endNode;

        public bool IsRunning => allThread.Any(t => t.Status == TaskStatus.Running);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (string name, BlueprintPipelineArgs arg)[] GetOutput()
        {
            return endNode.OutputData;
        }

        public void Init(int threadCount)
        {
            if (threadCount > nodeList.Count)
            {
                InternalIO.InternalWriteLog(
                    "Thread setting error: amount of threads of this blueprint is larger than amount of nodes.", LogType.Warning);
                threadCount = nodeList.Count;
            }
            if (threadCount!=0)
            {
                foreach (var item in allNode)
                {
                    if (item.Value.IsNative)
                    {
                        nativeNodeList.Add(item.Value);
                    }
                    else
                    {
                        nodeList.Add(item.Value);
                    }
                }
                nativeNodeList.Sort();
                PrepareTasks(threadCount);
            }
            else
            {
                foreach (var item in allNode)
                {
                    nodeList.Add(item.Value);
                }
            }
            nodeList.Sort();
        }

        public void PrepareTasks(int count)
        {
            for (int i = 0; i < count; i++)
            {
                allThread.Add(new Task(IndividualTask));
            }
        }

        private void IndividualTask()
        {
            do
            {
                try
                {
                    while (true)
                    {
                        RequestNextNode().Run();
                    }
                }
                catch (NodeClearException) { }
            } while (!terminate());
        }

        /// <summary>
        /// Run all nodes repeatedly until it is instructed to terminate.
        /// </summary>
        /// <param name="terminateInstructor">Instructor to terminate all tasks. Returns <see langword="true"/> if tasks should terminate</param>
        public void RunRepeatedly(Func<bool> terminateInstructor)
        {
            if (allThread.Count != 0)
            {
                MultiThreadRun(terminateInstructor);
                return;
            }
            SingleThreadRun(terminateInstructor);
        }

        private void MultiThreadRun(Func<bool> terminateInstructor)
        {
            if (terminate != terminateInstructor)
            {
                terminate = terminateInstructor;
                if (allThread.Count != 0)
                {
                    foreach (var t in allThread)
                    {
                        t.Start();
                    }
                }
            }
            beginNode.Run();
            foreach (var node in nativeNodeList)
            {
                node.Run();
            }
            EndLayer.Run();
        }

        private void SingleThreadRun(Func<bool> terminateInstructor)
        {
            beginNode.Run();
            foreach (var item in nodeList)
            {
                item.Run();
            }
            EndLayer.Run();
        }

        public void AddNativeNode(BlueprintNode node)
        {
            node.IsNative = true;
            allNode.Add(node.Name, node);
        }

        public void AddAsyncNode(BlueprintNode node)
        {
            node.IsNative = false;
            allNode.Add(node.Name, node);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetInput(params (string name, BlueprintPipelineArgs arg)[] parameters)
        {
            beginNode.InputData = parameters;
        }

        private BlueprintNode RequestNextNode()
        {
            if (currentNodeIndex < nodeList.Count)
            {
                BlueprintNode ret = nodeList[currentNodeIndex];
                Interlocked.Increment(ref currentNodeIndex);
                return ret;
            }
            Interlocked.Exchange(ref currentNodeIndex, 0);
            throw new NodeClearException();
        }

        string[] IConvertableToBlueprintNode.InputInterfacesName
        {
            get 
            {
                return beginNode.OutputInterfaces.Select(
                    p => p.Key).ToArray();
            }
        }

        BlueprintNodeAction IConvertableToBlueprintNode.MainExecution
        {
            get 
            {
                return (in Dictionary<string, BlueprintPipeline> input,
                    in Dictionary<string, BlueprintPipeline> output)
                    =>
                {
                    beginNode.Run();
                    foreach (var node in nativeNodeList)
                    {
                        node.Run();
                    }
                    endNode.Run();
                };
            }
        }

        string[] IConvertableToBlueprintNode.OutputInterfacesName
        {
            get
            {
                return beginNode.OutputInterfaces.Select(
                    p => p.Key).ToArray();
            }
        }
    }

    internal class NodeClearException : Exception
    {

    }
}
