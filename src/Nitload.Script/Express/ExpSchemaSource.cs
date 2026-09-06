//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ExpSchemaSource"
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
using StgSharp.Graphics.OpenGL;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Text;
using System.Text.RegularExpressions;

namespace StgSharp.Script.Express
{
    public partial class ExpSchemaSource
    {

        private static Regex 
            _constDefineBegin = GetConstDefineBegin(),
            _constDefineEnd = GetConstDefineEnd(),
            _schemaBegin = GetSchemaBegin(),
            _schemaEnd = GetSchemaEnd(),
            _entityDefineBegin = GetEntityDefineBegin(),
            _entityDefineEnd = GetEntityDefineEnd(),
            _typeDefineBegin = GetTypeDefineBegin(),
            _typeDefineEnd = GetTypeDefineEnd(),
            _functionDefineBegin = GetFunctionDefineBegin(),
            _functionDefineEnd = GetFunctionDefineEnd(),
            _ruleDefineBegin = GetRuleDefineBegin(),
            _ruleDefineEnd = GetRuleDefineEnd();

        private readonly GeneratedExpSchema _compileResult = new GeneratedExpSchema();

        private List<IExpElementSource> _sourceCache = [];

        private string name;

        public ExpSchemaSource() { }

        public List<IExpElementSource> Cache
        {
            get => _sourceCache;
        }

        public string Name
        {
            get => _compileResult.Name;
            internal set => _compileResult.Name = value;
        }

        public ExpSchema Organize()
        {
            _compileResult.LoadFromSource(this);
            return _compileResult;
        }

        public void ReadSchema(ExpFileSource reader)
        {
            string line = reader.ReadLine();
            while (!_schemaBegin.IsMatch(line)) {
                line = reader.ReadLine();
            }
            name = _schemaBegin.Match(line).Groups["namespace"].Value;

            while (!_schemaEnd.IsMatch(line))
            {
                if (TryReadConst(line, reader, out IExpElementSource source))
                {
                    _sourceCache.Add(source);
                } else if (TryReadType(line, reader, out source))
                {
                    _sourceCache.Add(source);
                } else if (TryReadEntity(line, reader, out source))
                {
                    _sourceCache.Add(source);
                } else if (TryReadRule(line, reader, out source))
                {
                    _sourceCache.Add(source);
                } else if (TryReadFunction(line, reader, out source)) {
                    _sourceCache.Add(source);
                }
                line = reader.ReadLine();
            }
        }

        public static bool TryReadSchema(string firstLine, ExpFileSource reader, out ExpSchemaSource source)
        {
            if (!_schemaBegin.IsMatch(firstLine))
            {
                source = null!;
                return false;
            }
            ExpSchemaSource _source = new ExpSchemaSource();
            _source.name = _schemaBegin.Match(firstLine).Groups["namespace"].Value;

            while (!_schemaEnd.IsMatch(firstLine))
            {
                if (_source.TryReadConst(firstLine, reader,
                                          out IExpElementSource _elementSource))
                {
                    _source._sourceCache.Add(_elementSource);
                } else if (_source.TryReadType(firstLine, reader, out _elementSource))
                {
                    _source._sourceCache.Add(_elementSource);
                } else if (_source.TryReadEntity(firstLine, reader, out _elementSource))
                {
                    _source._sourceCache.Add(_elementSource);
                } else if (_source.TryReadRule(firstLine, reader, out _elementSource))
                {
                    _source._sourceCache.Add(_elementSource);
                } else if (_source.TryReadFunction(firstLine, reader, out _elementSource)) {
                    _source._sourceCache.Add(_elementSource);
                }
                firstLine = reader.ReadLine();
            }
            source = _source;
            return true;
        }

        [GeneratedRegex(@"\s*CONSTANT")]
        private static partial Regex GetConstDefineBegin();

        [GeneratedRegex(@"\s*END_CONSTANT;")]
        private static partial Regex GetConstDefineEnd();

        [GeneratedRegex(@"\s*ENTITY\s*(?<TypeName>[A-Za-z0-9_]+)\s*", RegexOptions.Compiled)]
        private static partial Regex GetEntityDefineBegin();

        [GeneratedRegex(@"\s*END_ENTITY;\s*--\s*(?<TypeName>[A-Za-z0-9_]+)\s*", RegexOptions.Compiled)]
        private static partial Regex GetEntityDefineEnd();

        [GeneratedRegex(@"\s*FUNCTION\s*\s*(?<FunctionName>[a-zA-Z0-9_]+)\s*(?<Remaining>.*)", RegexOptions.Compiled)]
        private static partial Regex GetFunctionDefineBegin();

        [GeneratedRegex(@"\s*END_FUNCTION\s*;\s*--\s*(?<FunctionName>[a-zA-Z0-9_]+)", RegexOptions.Compiled)]
        private static partial Regex GetFunctionDefineEnd();

        [GeneratedRegex(
                @"\s*RULE\s*(?<RuleName>[a-zA-Z0-9_]+)\s*FOR\s*\((?<Target>[a-zA-Z0-9_]*(?:\s*\)\s*;)?)",
                RegexOptions.Compiled)]
        private static partial Regex GetRuleDefineBegin();

        [GeneratedRegex(@"\s*END_RULE\s*;\s*--\s*(?<RuleName>[A-Za-z0-9_]+)", RegexOptions.Compiled)]
        private static partial Regex GetRuleDefineEnd();

        [GeneratedRegex(@"\s*SCHEMA\s*(?<namespace>[A-Za-z0-9_]+)\s?;")]
        private static partial Regex GetSchemaBegin();

        [GeneratedRegex(@"END_SCHEMA;\s*--\s*(?<namespace>[A-Za-z0-9_]+)", RegexOptions.Compiled)]
        private static partial Regex GetSchemaEnd();

        [GeneratedRegex(@"\s*TYPE\s+(?<TypeName>[A-Za-z0-9_]+)\s*=\s*(?<Remaining>[A-Z]+)", RegexOptions.Compiled)]
        private static partial Regex GetTypeDefineBegin();

        [GeneratedRegex(@"\s*END_TYPE\s*;\s*--\s*(?<TypeName>[a-zA-Z0-9_]+)", RegexOptions.Compiled)]
        private static partial Regex GetTypeDefineEnd();

        private bool TryReadConst(string firstLine, ExpFileSource reader, out IExpElementSource source)
        {
            Match match = _constDefineBegin.Match(firstLine);
            if (match.Success)
            {
                ScriptSourceTransmitter transmitter = new ScriptSourceTransmitter(
                    "END_CONSTANT", reader.Location);
                ExpConstantCollectionSource _source = new ExpConstantCollectionSource(transmitter);
                string line = reader.ReadLine();
                while (!_constDefineEnd.IsMatch(line))
                {
                    transmitter.WriteLine(line);
                    line = reader.ReadLine();
                }
                transmitter.WriteLine("END_CONSTANT");
                source = _source;
                return true;
            }
            source = null!;
            return false;
        }

        private bool TryReadEntity(string firstLine, ExpFileSource reader, out IExpElementSource source)
        {
            Match match = _entityDefineBegin.Match(firstLine);
            if (match.Success)
            {
                ScriptSourceTransmitter transmitter = new ScriptSourceTransmitter(
                    "END_ENTITY", reader.Location);
                ExpEntitySource _source = new ExpEntitySource(
                    match.Groups["TypeName"].ToString(), transmitter);
                string line = reader.ReadLine();
                match = _entityDefineEnd.Match(line);
                while (!match.Success)
                {
                    transmitter.WriteLine(line);
                    line = reader.ReadLine();
                    match = _entityDefineEnd.Match(line);
                }
                string endedName = match.Groups["TypeName"].ToString();
                if (!string.Equals(endedName, _source.Name)) {
                    throw new ExpInvalidElementDeclareEndingExceptions(_source.Name, endedName);
                }
                transmitter.WriteLine("END_ENTITY");
                source = _source;
                return true;
            }
            source = null!;
            return false;
        }

        private bool TryReadFunction(string firstLine, ExpFileSource reader, out IExpElementSource source)
        {
            Match match = _functionDefineBegin.Match(firstLine);
            if (match.Success)
            {
                ScriptSourceTransmitter transmitter = new ScriptSourceTransmitter(
                    "END_FUNCTION", reader.Location);
                ExpFunctionSource _source = new ExpFunctionSource(
                    match.Groups["FunctionName"].ToString(), transmitter);
                transmitter.WriteLine(match.Groups["Remaining"].ToString());
                string line = reader.ReadLine();
                match = _functionDefineEnd.Match(line);
                while (!match.Success)
                {
                    transmitter.WriteLine(line);
                    line = reader.ReadLine();
                    match = _functionDefineEnd.Match(line);
                }
                string endedName = match.Groups["FunctionName"].ToString();
                if (!string.Equals(endedName, _source.Name)) {
                    throw new ExpInvalidElementDeclareEndingExceptions(_source.Name, endedName);
                }
                transmitter.WriteLine("END_FUNCTION");
                source = _source;
                return true;
            }
            source = null!;
            return false;
        }

        private bool TryReadRule(string firstLine, ExpFileSource reader, out IExpElementSource source)
        {
            Match match = _ruleDefineBegin.Match(firstLine);
            if (match.Success)
            {
                ScriptSourceTransmitter transmitter = new ScriptSourceTransmitter(
                    "END_RULE", reader.Location);
                ExpRuleSource _source = new ExpRuleSource(
                    match.Groups["RuleName"].ToString(), transmitter);
                string target = match.Groups["Target"].ToString();
                if (!string.IsNullOrEmpty(target)) {
                    transmitter.WriteLine(target);
                }
                string line = reader.ReadLine();
                match = _ruleDefineEnd.Match(line);
                while (!match.Success)
                {
                    transmitter.WriteLine(line);
                    line = reader.ReadLine();
                    match = _ruleDefineEnd.Match(line);
                }
                string endedName = match.Groups["RuleName"].ToString();
                if (!string.Equals(endedName, _source.Name)) {
                    throw new ExpInvalidElementDeclareEndingExceptions(_source.Name, endedName);
                }
                transmitter.WriteLine("END_RULE");
                source = _source;
                return true;
            }
            source = null!;
            return false;
        }

        private bool TryReadType(string firstLine, ExpFileSource reader, out IExpElementSource source)
        {
            Match match = _typeDefineBegin.Match(firstLine);
            if (match.Success)
            {
                ScriptSourceTransmitter transmitter = new ScriptSourceTransmitter(
                    "END_ENTITY", reader.Location);
                ExpTypeSource _source = new ExpTypeSource(
                    match.Groups["TypeName"].ToString(), transmitter);
                transmitter.WriteLine(match.Groups["Remaining"].ToString());
                string line = reader.ReadLine();
                match = _typeDefineEnd.Match(line);
                while (!match.Success)
                {
                    transmitter.WriteLine(line);
                    line = reader.ReadLine();
                    match = _typeDefineEnd.Match(line);
                }
                string endedName = match.Groups["TypeName"].ToString();
                if (!string.Equals(endedName, _source.Name)) {
                    throw new ExpInvalidElementDeclareEndingExceptions(_source.Name, endedName);
                }
                transmitter.WriteLine("END_ENTITY");
                source = _source;
                return true;
            }
            source = null!;
            return false;
        }

    }
}
