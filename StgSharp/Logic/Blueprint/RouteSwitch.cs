//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="RouteSwitch.cs"
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
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace StgSharp.Logic
{
    public class RouteSwitch : BlueprintNode
    {
        private protected Dictionary<string, (BlueprintPipeline pipeline, Func<bool> openingRule)> outputDictionary;

        public RouteSwitch(string name, bool isNative, string inputPorts) :
            base(name, isNative, [inputPorts], [string.Empty])
        {
            outputDictionary = new Dictionary<string, (BlueprintPipeline pipeline, Func<bool> openingRule)>();
        }

        public new int Level
        {
            get
            {
                if (levelAvailable)
                {
                    return _level;
                }
                _level = 0;
                KeyValuePair<string, BlueprintPipeline> item = _inputInterfaces.First();
                if (item.Value == null)
                {
                    throw new InvalidOperationException();
                }
                if (_level <= item.Value.Level)
                {
                    _level = item.Value.Level;
                }
                return _level;
            }
        }

        public new void AddBefore(
            BlueprintNode after,
            string thisOutputPortName,
            string afterInputPortName)
        {
            this.AppendNode(after, afterInputPortName, () => true);
        }

        public void AppendNode(BlueprintNode node, string portName, Func<bool> openningRule)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            string indexName = $"{node.Name}.{portName}";
            BlueprintPipeline p = new BlueprintPipeline(this, node);
            node.InputInterfaces[portName] = p;
            outputDictionary.Add(indexName, (p, openningRule));
        }

        public new void Run()
        {
            try
            {
                BlueprintPipeline input = InputInterfaces.First().Value;
                input.WaitForStart();
                foreach (KeyValuePair<string, (BlueprintPipeline pipeline, Func<bool> openingRule)> item in outputDictionary)
                {
                    if (item.Value.openingRule())
                    {
                        item.Value.pipeline.CompleteAndWriteData(input.Args);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
