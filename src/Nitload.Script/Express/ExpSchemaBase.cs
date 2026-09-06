//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ExpSchemaBase"
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
using StgSharp.Collections;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

namespace StgSharp.Script.Express
{
    public abstract class ExpSchema
    {

        public static ExpSchema BuiltinSchema
        {
            get => ExpSchema_Builtin.Only;
        }

        public string Name { get; internal set; }

        public abstract void LoadFromSource(ExpSchemaSource source);

        public abstract bool TryGetConst(string name, out ExpElementInstance c);

        public abstract bool TryGetEntity(string name, out ExpEntitySource e);

        public abstract bool TryGetFunction(string name, out ExpFunctionSource f);

        public abstract bool TryGetProcedure(string name, out ExpProcedureSource p);

        public abstract bool TryGetRule(string name, out ExpRuleSource r);

        public abstract bool TryGetSchemaInclude(string name, out ExpSchema include);

        public abstract bool TryGetType(string name, out ExpTypeSource t);

        #pragma warning disable CS8618
        protected FrozenDictionary<string, ExpElementInstance> ConstDict { get; set; }

        public IEnumerable<ExpSchema> IncludedSchema => IncludedSchema;

        protected FrozenDictionary<string, ExpEntitySource> EntityDict { get; set; }

        protected FrozenDictionary<string, ExpFunctionSource> FunctionDict { get; set; }

        protected ConcurrentStringHashMultiplexer StringMultiplexer => ExpressCompile.Multiplexer;

        protected ConcurrentDictionary<string, ExpProcedureSource> ProcedureDict { get; set; }

        protected FrozenDictionary<string, ExpRuleSource> RuleDict { get; set; }

        protected FrozenDictionary<string, ExpSchema> SchemaInclude { get; set; }

        protected FrozenDictionary<string, ExpTypeSource> TypeDict { get; set; }
#pragma warning restore CS8618
    }
}
