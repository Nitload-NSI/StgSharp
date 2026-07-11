//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="Generator"
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
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace GenerateGL.CodeGen
{
    internal sealed class Generator
    {

        private static readonly string[] TargetVersions = {
            "3.3", "4.0", "4.1", "4.2", "4.3", "4.4", "4.5", "4.6"
        };

        private static readonly FrozenDictionary<string, string> ReservedNames =
            new Dictionary<string, string>(StringComparer.Ordinal) {
            { "string", "@string" }, { "class", "@class" }, { "namespace", "@namespace" }, { "checked", "@checked" }, { "unchecked", "@unchecked" }, { "ref", "@ref" }, { "out", "@out" }, { "in", "@in" }, { "object", "@object" }, { "event", "@event" }, { "params", "@params" }, { "base", "@base" }, { "this", "@this" }, { "void", "@void" }, { "int", "@int" }, { "uint", "@uint" }, { "long", "@long" }, { "ulong", "@ulong" }, { "short", "@short" }, { "ushort", "@ushort" }, { "float", "@float" }, { "double", "@double" }, { "decimal", "@decimal" }, { "bool", "@bool" }, { "byte", "@byte" }, { "sbyte", "@sbyte" }, { "char", "@char" }, { "struct", "@struct" }, { "public", "@public" }, { "private", "@private" }, { "protected", "@protected" }, { "internal", "@internal" }, { "fixed", "@fixed" }
        }.ToFrozenDictionary(StringComparer.Ordinal);

        private readonly IReadOnlyDictionary<string, GlCommand> _commands;
        private readonly IReadOnlyList<GlFeature> _features;
        private readonly string _outputRoot;

        public Generator(
               IReadOnlyDictionary<string, GlCommand> commands,
               IReadOnlyList<GlFeature> features,
               string outputRoot
        )
        {
            _commands = commands;
            _features = features;
            _outputRoot = outputRoot;
        }

        public void Run()
        {
            if (Directory.Exists(_outputRoot)) {
                Directory.Delete(_outputRoot, recursive:true);
            }
            Directory.CreateDirectory(_outputRoot);
            List<GlCommand> all = new List<GlCommand>();
            HashSet<string> previous = new HashSet<string>(StringComparer.Ordinal);
            foreach (string v in TargetVersions)
            {
                IReadOnlyList<GlCommand> cmdsUpTo = CollectCommandsUpToVersion(v);
                List<GlCommand> versionCmds = cmdsUpTo.Where(c => !previous.Contains(c.Name))
                                                      .ToList();
                foreach (GlCommand cmd in versionCmds) {
                    previous.Add(cmd.Name);
                }
                all.AddRange(versionCmds);
                WriteOpenGLFunctionFile(v, versionCmds);
                WriteGlContextFile(v, versionCmds);
            }
            WriteMergedGlContextFile(all);
            WriteCommandHistoryFile(BuildCommandHistory());
        }

        private IReadOnlyDictionary<string, List<CommandHistoryEntry>> BuildCommandHistory()
        {
            Dictionary<string, List<CommandHistoryEntry>> history = new(StringComparer.Ordinal);

            IEnumerable<GlFeature> ordered = _features.Where(f => Version.TryParse(f.Version, out _))
                                                      .OrderBy(f => Version.Parse(f.Version));

            foreach (GlFeature feature in ordered)
            {
                string version = feature.Version;

                foreach (string name in feature.RequiresCommands.Distinct(StringComparer.Ordinal))
                {
                    if (!_commands.ContainsKey(name) || ShouldSkip(name))
                    {
                        continue;
                    }

                    if (!history.TryGetValue(name, out List<CommandHistoryEntry>? list))
                    {
                        list = new List<CommandHistoryEntry>();
                        history[name] = list;
                    }

                    if (list.Count == 0 ||
                        list[^1].Action != CommandHistoryAction.Added ||
                        list[^1].Version != version) {
                        list.Add(new CommandHistoryEntry(version, CommandHistoryAction.Added));
                    }
                }

                foreach (string name in feature.RemovedCommands.Distinct(StringComparer.Ordinal))
                {
                    if (!_commands.ContainsKey(name) || ShouldSkip(name))
                    {
                        continue;
                    }

                    if (!history.TryGetValue(name, out List<CommandHistoryEntry>? list))
                    {
                        list = new List<CommandHistoryEntry>();
                        history[name] = list;
                    }

                    if (list.Count == 0 ||
                        list[^1].Action != CommandHistoryAction.Removed ||
                        list[^1].Version != version) {
                        list.Add(new CommandHistoryEntry(version, CommandHistoryAction.Removed));
                    }
                }
            }

            return history;
        }

        private IReadOnlyList<GlCommand> CollectCommandsUpToVersion(
                                         string version
        )
        {
            Version target = Version.Parse(version);
            IEnumerable<GlFeature> ordered = _features.Where(f => Version.TryParse(f.Version, out Version? fv) &&
                                                                  fv <= target)
                                                      .OrderBy(f => Version.Parse(f.Version));

            HashSet<string> active = new HashSet<string>(StringComparer.Ordinal);

            foreach (GlFeature feature in ordered)
            {
                foreach (string name in feature.RequiresCommands.Distinct(StringComparer.Ordinal))
                {
                    if (_commands.ContainsKey(name)) {
                        active.Add(name);
                    }
                }

                foreach (string name in feature.RemovedCommands.Distinct(StringComparer.Ordinal)) {
                    active.Remove(name);
                }
            }

            return active.Select(name => _commands.TryGetValue(name, out GlCommand? c) ? c : null)
                         .Where(c => c != null)!
                         .Cast<GlCommand>()
                         .ToList();
        }

        private static string DelegateSignature(
                              string ret,
                              IReadOnlyList<string> paramTypes
        )
        {
            if (paramTypes.Count == 0) {
                return $"delegate*<{ret}>";
            }
            return $"delegate*<{string.Join(", ", paramTypes)}, {ret}>";
        }

        private static string SafeName(
                              string name
        )
        {
            if (string.IsNullOrWhiteSpace(name)) {
                return "arg";
            }

            if (ReservedNames.TryGetValue(name, out string? mapped)) {
                return mapped;
            }

            return name;
        }

        private static bool ShouldSkip(
                            string name
        )
        {
            // No skipping; include legacy/fixed-function APIs as well.
            return false;
        }

        private static string TrimGlPrefix(
                              string name
        )
        {
            if (name.StartsWith("gl", StringComparison.Ordinal) && name.Length > 2) {
                return name.Substring(2);
            }
            return name;
        }

        private void WriteCommandHistoryFile(
                     IReadOnlyDictionary<string, List<CommandHistoryEntry>> history
        )
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// <auto-generated>");
            sb.AppendLine($"// Generated by StgSharp.GenerateGL on {DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture)}");
            sb.AppendLine("// Command availability history");
            sb.AppendLine("// </auto-generated>");
            sb.AppendLine("using System;\nusing System.Collections.Frozen;\nusing System.Collections.Generic;\n");
            sb.AppendLine("namespace StgSharp.Graphics.OpenGL");
            sb.AppendLine("{");
            sb.AppendLine("    internal enum CommandHistoryAction { Added, Removed }");
            sb.AppendLine();
            sb.AppendLine("    internal readonly record struct CommandHistoryEntry(string Version, CommandHistoryAction Action);");
            sb.AppendLine();
            sb.AppendLine("    internal static class GlCommandHistory");
            sb.AppendLine("    {");
            sb.AppendLine("        internal static readonly FrozenDictionary<string, CommandHistoryEntry[]> History = new Dictionary<string, CommandHistoryEntry[]>(StringComparer.Ordinal)");
            sb.AppendLine("        {");

            foreach (KeyValuePair<string, List<CommandHistoryEntry>> kvp in history.OrderBy(k => k.Key, StringComparer.Ordinal))
            {
                string events = string.Join(", ", kvp.Value
                                                     .OrderBy(v => Version.Parse(v.Version))
                                                     .Select(v => $"new CommandHistoryEntry(\"{v.Version}\", CommandHistoryAction.{v.Action})"));
                sb.AppendLine($"            {{ \"{kvp.Key}\", new CommandHistoryEntry[] {{ {events} }} }},");
            }

            sb.AppendLine("        }.ToFrozenDictionary(StringComparer.Ordinal);");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            string fileName = Path.Combine(_outputRoot, "glCommandHistory.cs");
            File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
        }

        private void WriteGlContextFile(
                     string version,
                     IReadOnlyList<GlCommand> commands
        )
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// <auto-generated>");
            sb.AppendLine($"// Generated by StgSharp.GenerateGL on {DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture)}");
            sb.AppendLine($"// Version: {version}");
            sb.AppendLine("// </auto-generated>");
            sb.AppendLine("using System;\nusing System.Runtime.CompilerServices;\n");
            sb.AppendLine("namespace StgSharp.Graphics.OpenGL");
            sb.AppendLine("{");
            sb.AppendLine("    public unsafe partial struct OpenglContext");
            sb.AppendLine("    {");
            foreach (GlCommand cmd in commands.OrderBy(c => c!.Name, StringComparer.Ordinal))
            {
                string ret = CsTypeMapper.MapDelegateReturn(cmd.ReturnType, cmd.ReturnPointerLevel);
                List<string> paramTypes = new List<string>();
                foreach (GlParam p in cmd.Parameters)
                {
                    string mapped = CsTypeMapper.MapDelegateParameter(p);
                    paramTypes.Add(mapped);
                }
                string decl = DelegateSignature(ret, paramTypes);
                sb.AppendLine($"        internal unsafe {decl} {cmd.Name};");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            string fileName = Path.Combine(_outputRoot, $"glContext.v{version.Replace('.', '_')}.cs");
            File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
        }

        private void WriteMergedGlContextFile(
                     IReadOnlyList<GlCommand> commands
        )
        {
            List<GlCommand> distinct = commands.Where(c => !ShouldSkip(c.Name))
                                               .GroupBy(c => c.Name, StringComparer.Ordinal)
                                               .Select(g => g.First())
                                               .OrderBy(c => c.Name, StringComparer.Ordinal)
                                               .ToList();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// <auto-generated>");
            sb.AppendLine($"// Generated by StgSharp.GenerateGL on {DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture)}");
            sb.AppendLine("// Aggregated GL3+ commands");
            sb.AppendLine("// </auto-generated>");
            sb.AppendLine("using System;\nusing System.Runtime.CompilerServices;\n");
            sb.AppendLine("namespace StgSharp.Graphics.OpenGL");
            sb.AppendLine("{");
            sb.AppendLine("    public unsafe partial struct OpenglContext");
            sb.AppendLine("    {");
            foreach (GlCommand cmd in distinct)
            {
                string ret = CsTypeMapper.MapDelegateReturn(cmd.ReturnType, cmd.ReturnPointerLevel);
                List<string> paramTypes = new List<string>();
                foreach (GlParam p in cmd.Parameters)
                {
                    string mapped = CsTypeMapper.MapDelegateParameter(p);
                    paramTypes.Add(mapped);
                }
                string decl = DelegateSignature(ret, paramTypes);
                sb.AppendLine($"        internal unsafe {decl} {cmd.Name};");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            string fileName = Path.Combine(_outputRoot, "glContext.cs");
            File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
        }

        private void WriteOpenGLFunctionFile(
                     string version,
                     IReadOnlyList<GlCommand> commands
        )
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// <auto-generated>");
            sb.AppendLine($"// Generated by StgSharp.GenerateGL on {DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture)}");
            sb.AppendLine($"// Version: {version}");
            sb.AppendLine("// </auto-generated>");
            sb.AppendLine("using System;\nusing System.Runtime.CompilerServices;\nusing System.Runtime.InteropServices;\nusing StgSharp.Graphics.OpenGL;\n");
            sb.AppendLine("namespace StgSharp.Graphics.OpenGL");
            sb.AppendLine("{");
            sb.AppendLine("    public unsafe partial class OpenGLFunction");
            sb.AppendLine("    {");
            foreach (GlCommand cmd in commands.OrderBy(c => c!.Name, StringComparer.Ordinal))
            {
                string ret = CsTypeMapper.MapReturn(cmd.ReturnType, cmd.ReturnPointerLevel);
                string rawName = cmd.Name; // original glXxx
                string methodName = TrimGlPrefix(rawName);
                List<string> paramList = new List<string>();
                List<string> callArgs = new List<string>();
                List<(string Type,string Name)> pinned = new();
                foreach (GlParam p in cmd.Parameters)
                {
                    string pname = SafeName(p.Name);
                    if (CsTypeMapper.TryMapConstPointerToSpan(p, out string spanElementType))
                    {
                        paramList.Add($"ReadOnlySpan<{spanElementType}> {pname}");
                        pinned.Add((spanElementType, pname));
                        callArgs.Add($"{pname}Ptr");
                    } else
                    {
                        string mapped = CsTypeMapper.MapParameter(p);
                        paramList.Add($"{mapped} {pname}");
                        if (mapped == "GlHandle")
                        {
                            callArgs.Add($"{pname}.Value");
                        } else
                        {
                            callArgs.Add(pname);
                        }
                    }
                }
                string signature = string.Join(", ", paramList);
                string call = string.Join(", ", callArgs);
                sb.AppendLine($"        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
                sb.AppendLine($"        public {ret} {methodName}({signature})");
                sb.AppendLine("        {");
                string prefix = ret == "void" ? string.Empty : "return ";
                if (pinned.Count == 0)
                {
                    sb.AppendLine($"            {prefix}Context.{rawName}({call});");
                } else
                {
                    string indent = "            ";
                    foreach ((string Type, string Name) pin in pinned)
                    {
                        sb.AppendLine($"{indent}fixed ({pin.Type}* {pin.Name}Ptr = {pin.Name})");
                        sb.AppendLine($"{indent}{{");
                        indent += "    ";
                    }
                    sb.AppendLine($"{indent}{prefix}Context.{rawName}({call});");
                    for (int i = 0; i < pinned.Count; i++)
                    {
                        indent = indent.Substring(0, indent.Length - 4);
                        sb.AppendLine($"{indent}}}");
                    }
                }
                sb.AppendLine("        }");
                sb.AppendLine();
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            string fileName = Path.Combine(_outputRoot, $"glFunction.v{version.Replace('.', '_')}.cs");
            File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
        }

        private enum CommandHistoryAction
        {

            Added,
            Removed

        }

        private readonly record struct CommandHistoryEntry(
                                       string Version,
                                       CommandHistoryAction Action
        );

    }
}
