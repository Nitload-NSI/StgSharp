//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="GlXmlParser"
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
using System.Linq;
using System.Xml.Linq;

namespace GenerateGL.CodeGen
{
    internal static class GlXmlParser
    {

        public static (IReadOnlyDictionary<string, GlCommand> Commands, IReadOnlyList<GlFeature> Features) Parse(
                                                                                                           string xmlPath
        )
        {
            XDocument doc = XDocument.Load(xmlPath, LoadOptions.PreserveWhitespace);
            Dictionary<string, GlCommand> commands = ParseCommands(doc);
            List<GlFeature> features = ParseFeatures(doc);
            return (commands, features);
        }

        private static string BuildCType(
                              XElement element
        )
        {
            return BuildCType(element, out _, out _, out _);
        }

        private static string BuildCType(
                              XElement element,
                              out int pointerLevel,
                              out bool isPointer,
                              out bool isConst
        )
        {
            // CType is usually text parts + <ptype>
            IEnumerable<string> parts = element.Nodes()
                                               .Select(n => n switch
                                               {
                                                   XText t => t.Value,
                                                   XElement el when el.Name == "ptype" => el.Value,
                                                   _ => string.Empty
                                               });
            string raw = string.Concat(parts).Trim();
            pointerLevel = raw.Count(ch => ch == '*');
            isPointer = pointerLevel > 0;
            isConst = raw.Contains("const", StringComparison.OrdinalIgnoreCase);
            string cleaned = raw.Replace("const", string.Empty, StringComparison.OrdinalIgnoreCase)
                                .Replace("*", string.Empty)
                                .Trim();
            return cleaned;
        }

        private static Dictionary<string, GlCommand> ParseCommands(
                                                     XDocument doc
        )
        {
            Dictionary<string, GlCommand> dict = new Dictionary<string, GlCommand>(StringComparer.Ordinal);
            IEnumerable<XElement> commandElements = doc.Root?.Element("commands")?.Elements("command") ??
                Enumerable.Empty<XElement>();
            foreach (XElement cmd in commandElements)
            {
                XElement? proto = cmd.Element("proto");
                if (proto is null)
                {
                    continue;
                }

                string name = proto.Element("name")?.Value ?? string.Empty;
                string returnType = BuildCType(proto, out int retPtrLevel, out _,
                                               out bool retIsConst);

                List<GlParam> parameters = new List<GlParam>();
                foreach (XElement p in cmd.Elements("param"))
                {
                    string pName = p.Element("name")?.Value ?? string.Empty;
                    string cType = BuildCType(p, out int pointerLevel, out _, out bool isConst);
                    parameters.Add(new GlParam(pName, cType, pointerLevel, isConst));
                }

                if (!string.IsNullOrEmpty(name)) {
                    dict[name] = new GlCommand(name, returnType, retPtrLevel, retIsConst,
                                               parameters);
                }
            }
            return dict;
        }

        private static List<GlFeature> ParseFeatures(
                                       XDocument doc
        )
        {
            List<GlFeature> list = new List<GlFeature>();
            IEnumerable<XElement> featureElements = doc.Root?.Elements("feature") ??
                Enumerable.Empty<XElement>();
            foreach (XElement feature in featureElements)
            {
                string? api = feature.Attribute("api")?.Value;
                if (!string.Equals(api, "gl", StringComparison.Ordinal))
                {
                    continue;
                }

                string? profile = feature.Attribute("profile")?.Value;

                // Only core (or no profile for legacy) to stay aligned with core profile
                if (!string.IsNullOrEmpty(profile) &&
                    !string.Equals(profile, "core", StringComparison.Ordinal))
                {
                    continue;
                }

                string version = feature.Attribute("number")?.Value ?? string.Empty;
                List<string> requires = new List<string>();
                foreach (XElement req in feature.Elements("require"))
                {
                    string? reqProfile = req.Attribute("profile")?.Value;
                    if (!string.IsNullOrEmpty(reqProfile) &&
                        !string.Equals(reqProfile, "core", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    foreach (XElement cmd in req.Elements("command"))
                    {
                        string? name = cmd.Attribute("name")?.Value;
                        if (!string.IsNullOrEmpty(name)) {
                            requires.Add(name);
                        }
                    }
                }

                List<string> removes = new List<string>();
                foreach (XElement rem in feature.Elements("remove"))
                {
                    string? remProfile = rem.Attribute("profile")?.Value;
                    if (!string.IsNullOrEmpty(remProfile) &&
                        !string.Equals(remProfile, "core", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    foreach (XElement cmd in rem.Elements("command"))
                    {
                        string? name = cmd.Attribute("name")?.Value;
                        if (!string.IsNullOrEmpty(name)) {
                            removes.Add(name);
                        }
                    }
                }

                list.Add(new GlFeature(version, requires, removes));
            }
            return list;
        }

    }
}
