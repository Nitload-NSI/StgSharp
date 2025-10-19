//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepEntityBase"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using Microsoft.CodeAnalysis.CSharp.Syntax;

using StgSharp.Mathematics;
using StgSharp.Model.Step;
using StgSharp.PipeLine;
using StgSharp.Script.Express;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StgSharp.Model.Step
{
    public abstract class StepEntityBase : IExpConvertableFrom<StepEntityBase>
    {

        public IEnumerable<int> Dependencies { get; private protected set; }

        public int Id
        {
            get => Label.Id;
            protected set { Label = new StepEntityLabel(value); }
        }

        public StepEntityLabel Label { get; protected set; }

        public abstract StepItemType ItemType { get; }

        public StepModel Context { get; init; }

        public string Name { get; protected set; }

        protected string ExpEntityTypeName { get; set; }

        public void FromInstance(StepEntityBase entity)
        {
            this.Id = entity.Id;
            this.Label = entity.Label;
            this.ExpEntityTypeName = entity.ExpEntityTypeName;
            this.Name = entity.Name;
        }

        public unsafe string[] GetEntityDescription(int depth)
        {
            List<string> ret = new();
            string thisDescription = $"Entity Id: {Id}, type: {ExpEntityTypeName}";
            if (depth <= 0) {
                return[ thisDescription ];
            }
            switch (Dependencies.Count())
            {
                case 0:
                    return[ thisDescription ];
                case 1:
                    ret.Add(thisDescription);
                    int only = Dependencies.First();
                    string[] dep = Context[only]!.GetEntityDescription(depth - 1);
                    ret.Add($"  ©¸-{dep[0]}");
                    for (int i = 1; i < dep.Length; i++) {
                        ret.Add($"    {dep[i]}");
                    }
                    return ret.ToArray();
                default:
                    ret.Add(thisDescription);
                    int first = Dependencies.First();
                    dep = Context[first]!.GetEntityDescription(depth - 1);
                    ret.Add($"  ©À-{dep[0]}");
                    int index = 0;
                    for (int i = 1; i < dep.Length; i++) {
                        ret.Add($"  | {dep[i]}");
                    }
                    foreach (int item in Dependencies)
                    {
                        index++;
                        if (index == 1 || index >= Dependencies.Count())
                        {
                            continue;
                        }
                        dep = Context[item]!.GetEntityDescription(depth - 1);
                        ret.Add($"  ©À-{dep[0]}");
                        for (int i = 1; i < dep.Length; i++) {
                            ret.Add($"  | {dep[i]}");
                        }
                    }
                    dep = Context[Dependencies.Last()].GetEntityDescription(depth - 1);
                    ret.Add($"  ©¸-{dep[0]}");
                    for (int i = 1; i < dep.Length; i++) {
                        ret.Add($"    {dep[i]}");
                    }
                    return ret.ToArray();
            }
        }

        public bool IsConvertableTo(string entityName)
        {
            return false;
        }

    }
}
