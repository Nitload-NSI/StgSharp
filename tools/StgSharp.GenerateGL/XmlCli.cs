// -----------------------------------------------------------------------------
// file="XmlCli"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using StgSharp.GenerateGL.Generation;
using StgSharp.GenerateGL.Registry;
using StgSharp.GenerateGL.Tree;

namespace StgSharp.GenerateGL
{
    internal static class XmlCli
    {

        internal const int LoadErrorExitCode = 3;

        private const int DefaultDepth = 2;
        private const int DefaultLimit = 20;
        private const int DefaultSearchLimit = 5;
        private const int ReadableDepth = 6;
        private const int SearchReadableChildLimit = 8;
        private const int SearchReadableDepth = 4;
        private const int QueryErrorExitCode = 1;
        private const int SuccessExitCode = 0;
        private const int UsageErrorExitCode = 2;

        public static int Run(
                          string[] args
        )
        {
            ArgumentNullException.ThrowIfNull(args);

            if (!TryReadInvocation(args, out CliInvocation invocation, out string? error))
            {
                return WriteUsageError(error);
            }

            if (invocation.ShowHelp)
            {
                WriteHelp();
                return SuccessExitCode;
            }

            CliSession session = LoadSession(invocation.XmlPath);
            if (invocation.Interactive)
            {
                return RunInteractive(session);
            }

            return ExecuteQuery(session, invocation.Command, invocation.Arguments);
        }

        private static int ExecuteQuery(
                                        CliSession session,
                                        string command,
                                        IReadOnlyList<string> arguments
        )
        {
            try
            {
                return command switch
                {
                    "LIST" or "LS" when arguments.Count != 0 && string.Equals(
                        arguments[0],
                        "files",
                        StringComparison.OrdinalIgnoreCase) => GenerateCli.ListFiles(
                        session.GeneratedFiles,
                        arguments.Skip(1).ToArray()),
                    "SUMMARY" or "REGISTRY" or "MODEL" or
                    "LIST" or "LS" or
                    "FIND" or "SEARCH" or
                    "SHOW" or "INFO" => RegistryCli.Execute(
                        session.Registry,
                        command,
                        arguments),
                    "GENERATE" or "GEN" => QueryGenerate(session, arguments),
                    "PREVIEW" => GenerateCli.Preview(
                        session.GeneratedFiles,
                        arguments),
                    "XML" => ExecuteXmlQuery(session.Document, arguments),
                    "CLEAR" or "CLS" => WriteUsageError(
                        "clear is only available in the interactive shell."),
                    "RAW" or "TREE" or "SELECT" or "TEXT" or
                    "ATTRIBUTES" or "ATTRS" => WriteMovedXmlQuery(command),
                    _ => WriteUnknownQuery(command),
                };
            }
            catch (XPathException exception)
            {
                return WriteUsageError($"Invalid XPath: {exception.Message}");
            }
            catch (InvalidDataException exception)
            {
                Console.Error.WriteLine($"error: {exception.Message}");
                return QueryErrorExitCode;
            }
        }

        private static int QueryGenerate(
                                         CliSession session,
                                         IReadOnlyList<string> arguments
        )
        {
            int result = GenerateCli.Generate(
                session.Registry,
                arguments,
                out GeneratedFileSet? generatedFiles);
            if (generatedFiles is not null)
            {
                session.GeneratedFiles = generatedFiles;
            }

            return result;
        }

        private static int ExecuteXmlQuery(
                                           XmlTreeDocument document,
                                           IReadOnlyList<string> arguments
        )
        {
            if (arguments.Count == 0)
            {
                WriteXmlHelp();
                return SuccessExitCode;
            }

            string command = arguments[0].ToUpperInvariant();
            string[] queryArguments = arguments.Skip(1).ToArray();
            return command switch
            {
                "SUMMARY" => QueryXmlSummary(document, queryArguments),
                "RAW" => QueryRaw(document, queryArguments),
                "TREE" => QueryTree(document, queryArguments),
                "FIND" or "SEARCH" => QueryFind(document, queryArguments),
                "SHOW" => QueryShow(document, queryArguments),
                "SELECT" => QuerySelect(document, queryArguments),
                "TEXT" => QueryText(document, queryArguments),
                "ATTRIBUTES" or "ATTRS" => QueryAttributes(
                    document,
                    queryArguments),
                _ => WriteUsageError($"Unknown XML command '{command}'."),
            };
        }

        private static int QueryFind(
                                     XmlTreeDocument document,
                                     IReadOnlyList<string> arguments
        )
        {
            if (!TryReadSearchOptions(arguments, out SearchOptions options, out string? error))
            {
                return WriteUsageError(error);
            }

            SearchMatch[] matches = FindMatches(document, options).ToArray();
            if (matches.Length == 0)
            {
                Console.Error.WriteLine($"No XML keys or values contain '{options.Text}'.");
                return QueryErrorExitCode;
            }

            int displayedCount = Math.Min(matches.Length, options.Limit);
            for (int index = 0; index < displayedCount; index++)
            {
                SearchMatch match = matches[index];
                XElement context = FindSearchContext(match, options.ParentLevels);
                if (index != 0)
                {
                    Console.WriteLine();
                }

                Console.WriteLine(
                    $"[{index + 1}] {match.Field}: {Preview(match.Value, 120)}");
                Console.WriteLine($"    match: {GetElementPath(match.Element)}");
                if (context != match.Element)
                {
                    Console.WriteLine($"    context: {GetElementPath(context)}");
                }

                WriteReadableElement(
                    context,
                    0,
                    SearchReadableDepth,
                    SearchReadableChildLimit);
            }

            WriteTruncation(matches.Length, displayedCount);
            return SuccessExitCode;
        }

        private static int QueryShow(
                                     XmlTreeDocument document,
                                     IReadOnlyList<string> arguments
        )
        {
            if (!TryReadSelectionOptions(
                    "show",
                    arguments,
                    out SelectionOptions options,
                    out string? error))
            {
                return WriteUsageError(error);
            }

            XElement[] matches = SelectElements(document, options.XPath);
            if (matches.Length == 0)
            {
                return WriteNoMatches(options.XPath);
            }

            int displayedCount = Math.Min(matches.Length, options.Limit);
            for (int index = 0; index < displayedCount; index++)
            {
                XElement element = matches[index];
                if (index != 0)
                {
                    Console.WriteLine();
                }

                Console.WriteLine($"[{index + 1}] {GetElementPath(element)}");
                WriteReadableElement(element, 0, ReadableDepth, DefaultLimit);
            }

            WriteTruncation(matches.Length, displayedCount);
            return SuccessExitCode;
        }

        private static int RunInteractive(
                                          CliSession initialSession
        )
        {
            CliSession session = initialSession;
            Console.WriteLine($"Loaded {session.Document.SourcePath}");
            Console.WriteLine("Type 'help' for commands, or 'exit' to leave.");

            while (true)
            {
                if (!Console.IsInputRedirected)
                {
                    Console.Write($"{Path.GetFileName(session.Document.SourcePath)}> ");
                }

                string? line = Console.ReadLine();
                if (line is null)
                {
                    return SuccessExitCode;
                }

                if (!TrySplitCommandLine(line, out string[] values, out string? error))
                {
                    Console.Error.WriteLine($"error: {error}");
                    continue;
                }

                if (values.Length == 0)
                {
                    continue;
                }

                string command = values[0].ToUpperInvariant();
                string[] arguments = values.Skip(1).ToArray();
                switch (command)
                {
                    case "EXIT" or "QUIT":
                        return SuccessExitCode;

                    case "HELP" or "?":
                        WriteHelp(arguments);
                        break;

                    case "CLEAR" or "CLS":
                        if (arguments.Length != 0)
                        {
                            Console.Error.WriteLine(
                                "error: clear does not accept arguments.");
                            break;
                        }

                        ClearConsole();
                        break;

                    case "LOAD" or "OPEN":
                        if (arguments.Length != 1)
                        {
                            Console.Error.WriteLine("error: load requires one XML path.");
                            break;
                        }

                        if (TryLoadInteractive(arguments[0], out CliSession loaded))
                        {
                            session = loaded;
                            Console.WriteLine($"Loaded {session.Document.SourcePath}");
                        }

                        break;

                    case "RELOAD":
                        if (arguments.Length != 0)
                        {
                            Console.Error.WriteLine("error: reload does not accept arguments.");
                            break;
                        }

                        if (TryLoadInteractive(
                                session.Document.SourcePath,
                                out CliSession reloaded))
                        {
                            session = reloaded;
                            Console.WriteLine($"Reloaded {session.Document.SourcePath}");
                        }

                        break;

                    default:
                        ExecuteQuery(session, command, arguments);
                        break;
                }
            }
        }

        private static int QueryXmlSummary(
                                           XmlTreeDocument document,
                                           IReadOnlyList<string> arguments
        )
        {
            if (!ExpectNoArguments("xml summary", arguments))
            {
                return UsageErrorExitCode;
            }

            XElement root = document.Tree.Root!;
            XElement[] elements = root.DescendantsAndSelf().ToArray();
            XText[] textNodes = document.Tree.DescendantNodes()
                                             .OfType<XText>()
                                             .Where(value => value is not XCData)
                                             .ToArray();
            int maxDepth = elements.Max(value => value.Ancestors().Count());

            Console.WriteLine("XML tree");
            WriteProperty("source", document.SourcePath);
            WriteProperty("source characters", document.SourceText.Length);
            WriteProperty("root", root.Name.ToString());
            WriteOptionalProperty("declaration version", document.Tree.Declaration?.Version);
            WriteOptionalProperty("declaration encoding", document.Tree.Declaration?.Encoding);
            WriteOptionalProperty("declaration standalone", document.Tree.Declaration?.Standalone);
            WriteProperty("elements", elements.Length);
            WriteProperty("attributes", elements.Sum(value => value.Attributes().Count()));
            WriteProperty("text nodes", textNodes.Length);
            WriteProperty(
                "non-whitespace text nodes",
                textNodes.Count(value => !string.IsNullOrWhiteSpace(value.Value)));
            WriteProperty(
                "CDATA nodes",
                document.Tree.DescendantNodes().OfType<XCData>().Count());
            WriteProperty(
                "comments",
                document.Tree.DescendantNodes().OfType<XComment>().Count());
            WriteProperty("maximum element depth", maxDepth);

            Console.WriteLine("  root element children");
            foreach (IGrouping<string, XElement> group in root.Elements()
                                                              .GroupBy(value => value.Name.ToString())
                                                              .OrderBy(
                                                                  value => value.Key,
                                                                  StringComparer.Ordinal))
            {
                Console.WriteLine($"    {group.Key}: {group.Count()}");
            }

            return SuccessExitCode;
        }

        private static int QueryRaw(
                                    XmlTreeDocument document,
                                    IReadOnlyList<string> arguments
        )
        {
            if (!ExpectNoArguments("raw", arguments))
            {
                return UsageErrorExitCode;
            }

            Console.Write(document.SourceText);
            return SuccessExitCode;
        }

        private static int QueryTree(
                                     XmlTreeDocument document,
                                     IReadOnlyList<string> arguments
        )
        {
            if (!TryReadTreeOptions(arguments, out TreeOptions options, out string? error))
            {
                return WriteUsageError(error);
            }

            XElement[] matches = SelectElements(document, options.XPath);
            if (matches.Length == 0)
            {
                return WriteNoMatches(options.XPath);
            }

            int displayedCount = Math.Min(matches.Length, options.Limit);
            for (int index = 0; index < displayedCount; index++)
            {
                XElement element = matches[index];
                if (index != 0)
                {
                    Console.WriteLine();
                }

                Console.WriteLine($"[{index + 1}] {GetElementPath(element)}");
                WriteTree(
                    element,
                    0,
                    options.Depth,
                    options.Limit,
                    options.IncludeWhitespace);
            }

            WriteTruncation(matches.Length, displayedCount);
            return SuccessExitCode;
        }

        private static int QuerySelect(
                                       XmlTreeDocument document,
                                       IReadOnlyList<string> arguments
        )
        {
            if (!TryReadSelectionOptions(
                    "select",
                    arguments,
                    out SelectionOptions options,
                    out string? error))
            {
                return WriteUsageError(error);
            }

            XElement[] matches = SelectElements(document, options.XPath);
            if (matches.Length == 0)
            {
                return WriteNoMatches(options.XPath);
            }

            int displayedCount = Math.Min(matches.Length, options.Limit);
            for (int index = 0; index < displayedCount; index++)
            {
                XElement element = matches[index];
                if (index != 0)
                {
                    Console.WriteLine();
                }

                Console.WriteLine(
                    $"[{index + 1}] {GetElementPath(element)} [line {ReadLine(element)}]");
                Console.WriteLine(element.ToString(SaveOptions.DisableFormatting));
            }

            WriteTruncation(matches.Length, displayedCount);
            return SuccessExitCode;
        }

        private static int QueryText(
                                     XmlTreeDocument document,
                                     IReadOnlyList<string> arguments
        )
        {
            if (!TryReadSelectionOptions(
                    "text",
                    arguments,
                    out SelectionOptions options,
                    out string? error))
            {
                return WriteUsageError(error);
            }

            XElement[] matches = SelectElements(document, options.XPath);
            if (matches.Length == 0)
            {
                return WriteNoMatches(options.XPath);
            }

            int displayedCount = Math.Min(matches.Length, options.Limit);
            foreach (XElement element in matches.Take(displayedCount))
            {
                Console.WriteLine(element.Value);
            }

            WriteTruncation(matches.Length, displayedCount);
            return SuccessExitCode;
        }

        private static int QueryAttributes(
                                           XmlTreeDocument document,
                                           IReadOnlyList<string> arguments
        )
        {
            if (!TryReadSelectionOptions(
                    "attributes",
                    arguments,
                    out SelectionOptions options,
                    out string? error))
            {
                return WriteUsageError(error);
            }

            XElement[] matches = SelectElements(document, options.XPath);
            if (matches.Length == 0)
            {
                return WriteNoMatches(options.XPath);
            }

            int displayedCount = Math.Min(matches.Length, options.Limit);
            for (int index = 0; index < displayedCount; index++)
            {
                XElement element = matches[index];
                Console.WriteLine(
                    $"[{index + 1}] {GetElementPath(element)} [line {ReadLine(element)}]");
                XAttribute[] attributes = element.Attributes().ToArray();
                if (attributes.Length == 0)
                {
                    Console.WriteLine("  (no attributes)");
                    continue;
                }

                foreach (XAttribute attribute in attributes)
                {
                    Console.WriteLine($"  @{attribute.Name} = {attribute.Value}");
                }
            }

            WriteTruncation(matches.Length, displayedCount);
            return SuccessExitCode;
        }

        private static XElement[] SelectElements(
                                                 XmlTreeDocument document,
                                                 string xpath
        )
        {
            return document.Tree.XPathSelectElements(xpath).ToArray();
        }

        private static IEnumerable<SearchMatch> FindMatches(
                                                               XmlTreeDocument document,
                                                               SearchOptions options
        )
        {
            XElement root = document.Tree.Root!;
            foreach (XElement element in root.DescendantsAndSelf())
            {
                if (MatchesSearch(element.Name.LocalName, options))
                {
                    yield return new SearchMatch(element, "element", element.Name.LocalName);
                    continue;
                }

                XAttribute? attribute = element.Attributes().FirstOrDefault(value =>
                    MatchesSearch(value.Name.LocalName, options) ||
                    MatchesSearch(value.Value, options));
                if (attribute is not null)
                {
                    yield return new SearchMatch(
                        element,
                        $"@{attribute.Name.LocalName}",
                        attribute.Value);
                    continue;
                }

                string text = ReadDirectText(element);
                if (text.Length != 0 && MatchesSearch(text, options))
                {
                    yield return new SearchMatch(element, "$text", text);
                }
            }
        }

        private static bool MatchesSearch(
                                          string value,
                                          SearchOptions options
        )
        {
            return options.Exact
                ? string.Equals(value, options.Text, StringComparison.OrdinalIgnoreCase)
                : value.Contains(options.Text, StringComparison.OrdinalIgnoreCase);
        }

        private static XElement FindSearchContext(
                                                  SearchMatch match,
                                                  int? parentLevels
        )
        {
            XElement context = match.Element;
            int levels = parentLevels ?? (match.Field == "$text" ? 2 : 0);
            for (int index = 0; index < levels; index++)
            {
                if (context.Parent is not XElement parent)
                {
                    break;
                }

                if (parentLevels is null && parent.Elements().Take(DefaultLimit + 1).Count() >
                    DefaultLimit)
                {
                    break;
                }

                context = parent;
            }

            return context;
        }

        private static void WriteReadableElement(
                                                 XElement element,
                                                 int depth,
                                                 int maximumDepth,
                                                 int childLimit
        )
        {
            string padding = new string(' ', depth * 2);
            Console.WriteLine($"{padding}{element.Name.LocalName}:  # line {ReadLine(element)}");

            foreach (XAttribute attribute in element.Attributes())
            {
                Console.WriteLine(
                    $"{padding}  @{attribute.Name.LocalName}: {Preview(attribute.Value, 200)}");
            }

            string directText = ReadDirectText(element);
            if (directText.Length != 0)
            {
                Console.WriteLine($"{padding}  $text: {Preview(directText, 200)}");
            }

            foreach (XComment comment in element.Nodes().OfType<XComment>())
            {
                Console.WriteLine($"{padding}  $comment: {Preview(comment.Value.Trim(), 200)}");
            }

            XElement[] children = element.Elements().ToArray();
            if (children.Length == 0)
            {
                return;
            }

            if (depth >= maximumDepth)
            {
                Console.WriteLine($"{padding}  ...: {children.Length} child element(s)");
                return;
            }

            Dictionary<XName, int> totals = children.GroupBy(value => value.Name)
                                                   .ToDictionary(
                                                       value => value.Key,
                                                       value => value.Count());
            Dictionary<XName, int> indices = new Dictionary<XName, int>();
            int displayedCount = Math.Min(children.Length, childLimit);
            foreach (XElement child in children.Take(displayedCount))
            {
                indices.TryGetValue(child.Name, out int currentIndex);
                currentIndex++;
                indices[child.Name] = currentIndex;

                string name = totals[child.Name] == 1
                    ? child.Name.LocalName
                    : $"{child.Name.LocalName}[{currentIndex}]";
                if (!child.HasElements && !child.HasAttributes)
                {
                    Console.WriteLine(
                        $"{padding}  {name}: {Preview(child.Value.Trim(), 200)}");
                    continue;
                }

                WriteReadableChild(child, name, depth + 1, maximumDepth, childLimit);
            }

            if (displayedCount != children.Length)
            {
                Console.WriteLine(
                    $"{padding}  ...: {children.Length - displayedCount} more child element(s)");
            }
        }

        private static void WriteReadableChild(
                                               XElement element,
                                               string name,
                                               int depth,
                                               int maximumDepth,
                                               int childLimit
        )
        {
            string padding = new string(' ', depth * 2);
            Console.WriteLine($"{padding}{name}:  # line {ReadLine(element)}");

            foreach (XAttribute attribute in element.Attributes())
            {
                Console.WriteLine(
                    $"{padding}  @{attribute.Name.LocalName}: {Preview(attribute.Value, 200)}");
            }

            string directText = ReadDirectText(element);
            if (directText.Length != 0)
            {
                Console.WriteLine($"{padding}  $text: {Preview(directText, 200)}");
            }

            XElement[] children = element.Elements().ToArray();
            if (children.Length == 0)
            {
                return;
            }

            if (depth >= maximumDepth)
            {
                Console.WriteLine($"{padding}  ...: {children.Length} child element(s)");
                return;
            }

            Dictionary<XName, int> totals = children.GroupBy(value => value.Name)
                                                   .ToDictionary(
                                                       value => value.Key,
                                                       value => value.Count());
            Dictionary<XName, int> indices = new Dictionary<XName, int>();
            int displayedCount = Math.Min(children.Length, childLimit);
            foreach (XElement child in children.Take(displayedCount))
            {
                indices.TryGetValue(child.Name, out int currentIndex);
                currentIndex++;
                indices[child.Name] = currentIndex;

                string childName = totals[child.Name] == 1
                    ? child.Name.LocalName
                    : $"{child.Name.LocalName}[{currentIndex}]";
                if (!child.HasElements && !child.HasAttributes)
                {
                    Console.WriteLine(
                        $"{padding}  {childName}: {Preview(child.Value.Trim(), 200)}");
                    continue;
                }

                WriteReadableChild(
                    child,
                    childName,
                    depth + 1,
                    maximumDepth,
                    childLimit);
            }

            if (displayedCount != children.Length)
            {
                Console.WriteLine(
                    $"{padding}  ...: {children.Length - displayedCount} more child element(s)");
            }
        }

        private static string ReadDirectText(
                                             XElement element
        )
        {
            return string.Join(
                ' ',
                element.Nodes()
                       .OfType<XText>()
                       .Where(value => value is not XCData &&
                                       !string.IsNullOrWhiteSpace(value.Value))
                       .Select(value => value.Value.Trim()));
        }

        private static void WriteTree(
                                      XElement element,
                                      int depth,
                                      int maximumDepth,
                                      int childLimit,
                                      bool includeWhitespace
        )
        {
            string padding = new string(' ', depth * 2);
            string attributes = string.Join(
                string.Empty,
                element.Attributes().Select(value =>
                    $" {value.Name}=\"{Preview(value.Value, 80)}\""));
            Console.WriteLine(
                $"{padding}<{element.Name}{attributes}> [line {ReadLine(element)}]");

            XNode[] children = element.Nodes()
                                      .Where(value => IsVisible(value, includeWhitespace))
                                      .ToArray();
            if (children.Length == 0)
            {
                return;
            }

            if (depth >= maximumDepth)
            {
                Console.WriteLine($"{padding}  ... {children.Length} child node(s)");
                return;
            }

            int displayedCount = Math.Min(children.Length, childLimit);
            foreach (XNode child in children.Take(displayedCount))
            {
                switch (child)
                {
                    case XElement childElement:
                        WriteTree(
                            childElement,
                            depth + 1,
                            maximumDepth,
                            childLimit,
                            includeWhitespace);
                        break;

                    case XCData cdata:
                        WriteNodeValue("#cdata", cdata.Value, depth + 1);
                        break;

                    case XText text:
                        WriteNodeValue("#text", text.Value, depth + 1);
                        break;

                    case XComment comment:
                        WriteNodeValue("#comment", comment.Value, depth + 1);
                        break;

                    case XProcessingInstruction instruction:
                        WriteNodeValue(
                            $"?{instruction.Target}",
                            instruction.Data,
                            depth + 1);
                        break;
                }
            }

            if (displayedCount != children.Length)
            {
                Console.WriteLine(
                    $"{padding}  ... {children.Length - displayedCount} more child node(s)");
            }
        }

        private static bool IsVisible(
                                      XNode node,
                                      bool includeWhitespace
        )
        {
            return includeWhitespace || node is not XText text ||
                   !string.IsNullOrWhiteSpace(text.Value);
        }

        private static void WriteNodeValue(
                                           string kind,
                                           string value,
                                           int depth
        )
        {
            string padding = new string(' ', depth * 2);
            Console.WriteLine($"{padding}{kind} \"{Preview(value, 120)}\"");
        }

        private static string Preview(
                                      string value,
                                      int maximumLength
        )
        {
            string escaped = value.Replace("\r", "\\r", StringComparison.Ordinal)
                                  .Replace("\n", "\\n", StringComparison.Ordinal)
                                  .Replace("\t", "\\t", StringComparison.Ordinal)
                                  .Replace("\"", "\\\"", StringComparison.Ordinal);
            return escaped.Length <= maximumLength
                ? escaped
                : $"{escaped[..maximumLength]}...";
        }

        private static string GetElementPath(
                                             XElement element
        )
        {
            List<string> segments = new List<string>();
            foreach (XElement current in element.AncestorsAndSelf().Reverse())
            {
                string segment = current.Name.LocalName;
                if (current.Parent is XElement parent)
                {
                    XElement[] siblings = parent.Elements(current.Name).ToArray();
                    if (siblings.Length > 1)
                    {
                        int index = Array.IndexOf(siblings, current) + 1;
                        segment = $"{segment}[{index}]";
                    }
                }

                segments.Add(segment);
            }

            return $"/{string.Join('/', segments)}";
        }

        private static int ReadLine(
                                    XObject value
        )
        {
            return value is IXmlLineInfo lineInfo && lineInfo.HasLineInfo()
                ? lineInfo.LineNumber
                : 0;
        }

        private static bool TryLoadInteractive(
                                               string path,
                                               out CliSession session
        )
        {
            try
            {
                session = LoadSession(path);
                return true;
            }
            catch (IOException exception)
            {
                return WriteLoadError(exception, out session);
            }
            catch (XmlException exception)
            {
                return WriteLoadError(exception, out session);
            }
            catch (InvalidDataException exception)
            {
                return WriteLoadError(exception, out session);
            }
            catch (UnauthorizedAccessException exception)
            {
                return WriteLoadError(exception, out session);
            }
            catch (ArgumentException exception)
            {
                return WriteLoadError(exception, out session);
            }
            catch (NotSupportedException exception)
            {
                return WriteLoadError(exception, out session);
            }
        }

        private static CliSession LoadSession(
                                              string path
        )
        {
            XmlTreeDocument document = XmlTreeReader.Load(path);
            GlRegistryModel registry = GlRegistryReader.Read(document);
            return new CliSession(document, registry);
        }

        private static bool WriteLoadError(
                                           Exception exception,
                                           out CliSession session
        )
        {
            session = null!;
            Console.Error.WriteLine($"error: {exception.Message}");
            return false;
        }

        private static void ClearConsole()
        {
            if (Console.IsOutputRedirected)
            {
                return;
            }

            try
            {
                Console.Clear();
            }
            catch (IOException exception)
            {
                Console.Error.WriteLine($"error: {exception.Message}");
            }
            catch (PlatformNotSupportedException exception)
            {
                Console.Error.WriteLine($"error: {exception.Message}");
            }
        }

        private static bool TrySplitCommandLine(
                                                string line,
                                                out string[] values,
                                                out string? error
        )
        {
            List<string> result = new List<string>();
            StringBuilder current = new StringBuilder();
            bool hasToken = false;
            bool isQuoted = false;

            for (int index = 0; index < line.Length; index++)
            {
                char value = line[index];
                if (value == '"')
                {
                    isQuoted = !isQuoted;
                    hasToken = true;
                    continue;
                }

                if (value == '\\' && isQuoted && index + 1 < line.Length &&
                    line[index + 1] == '"')
                {
                    current.Append('"');
                    index++;
                    hasToken = true;
                    continue;
                }

                if (char.IsWhiteSpace(value) && !isQuoted)
                {
                    if (hasToken)
                    {
                        result.Add(current.ToString());
                        current.Clear();
                        hasToken = false;
                    }

                    continue;
                }

                current.Append(value);
                hasToken = true;
            }

            if (isQuoted)
            {
                values = Array.Empty<string>();
                error = "Unterminated double quote.";
                return false;
            }

            if (hasToken)
            {
                result.Add(current.ToString());
            }

            values = result.ToArray();
            error = null;
            return true;
        }

        private static bool TryReadInvocation(
                                              string[] args,
                                              out CliInvocation invocation,
                                              out string? error
        )
        {
            string xmlPath = Path.Combine(AppContext.BaseDirectory, "xml", "gl.xml");
            List<string> remaining = new List<string>(args.Length);
            for (int index = 0; index < args.Length; index++)
            {
                string argument = args[index];
                if (argument == "--xml")
                {
                    if (++index >= args.Length || args[index].StartsWith("--", StringComparison.Ordinal))
                    {
                        invocation = null!;
                        error = "--xml requires a file path.";
                        return false;
                    }

                    xmlPath = args[index];
                    continue;
                }

                remaining.Add(argument);
            }

            bool showHelp = remaining.Count == 1 &&
                            (remaining[0] is "--help" or "-h" || string.Equals(
                                 remaining[0],
                                 "help",
                                 StringComparison.OrdinalIgnoreCase));
            bool interactive = remaining.Count == 0 || remaining.Count == 1 &&
                               (string.Equals(
                                    remaining[0],
                                    "shell",
                                    StringComparison.OrdinalIgnoreCase) || string.Equals(
                                    remaining[0],
                                    "repl",
                                    StringComparison.OrdinalIgnoreCase));
            invocation = new CliInvocation(
                xmlPath,
                showHelp || interactive ? string.Empty : remaining[0].ToUpperInvariant(),
                showHelp || interactive
                    ? Array.Empty<string>()
                    : remaining.Skip(1).ToArray(),
                showHelp,
                interactive);
            error = null;
            return true;
        }

        private static bool TryReadTreeOptions(
                                               IReadOnlyList<string> arguments,
                                               out TreeOptions options,
                                               out string? error
        )
        {
            string xpath = "/*";
            int depth = DefaultDepth;
            int limit = DefaultLimit;
            bool includeWhitespace = false;
            int index = 0;
            if (arguments.Count != 0 && !arguments[0].StartsWith("--", StringComparison.Ordinal))
            {
                xpath = arguments[0];
                index++;
            }

            while (index < arguments.Count)
            {
                string argument = arguments[index++];
                if (argument == "--whitespace")
                {
                    includeWhitespace = true;
                    continue;
                }

                if (argument == "--depth" && TryReadNonNegativeInt(
                        arguments,
                        ref index,
                        out depth))
                {
                    continue;
                }

                if (argument == "--limit" && TryReadPositiveInt(
                        arguments,
                        ref index,
                        out limit))
                {
                    continue;
                }

                options = null!;
                error = $"Unknown or invalid tree option '{argument}'.";
                return false;
            }

            options = new TreeOptions(xpath, depth, limit, includeWhitespace);
            error = null;
            return true;
        }

        private static bool TryReadSearchOptions(
                                                 IReadOnlyList<string> arguments,
                                                 out SearchOptions options,
                                                 out string? error
        )
        {
            if (arguments.Count == 0 || arguments[0].StartsWith("--", StringComparison.Ordinal))
            {
                options = null!;
                error = "find requires a search string.";
                return false;
            }

            string text = arguments[0];
            int limit = DefaultSearchLimit;
            int? parentLevels = null;
            bool exact = false;
            int index = 1;
            while (index < arguments.Count)
            {
                string argument = arguments[index++];
                if (argument == "--exact")
                {
                    exact = true;
                    continue;
                }

                if (argument == "--limit" && TryReadPositiveInt(
                        arguments,
                        ref index,
                        out limit))
                {
                    continue;
                }

                if (argument == "--up" && TryReadNonNegativeInt(
                        arguments,
                        ref index,
                        out int levels))
                {
                    parentLevels = levels;
                    continue;
                }

                options = null!;
                error = $"Unknown or invalid find option '{argument}'.";
                return false;
            }

            options = new SearchOptions(text, limit, parentLevels, exact);
            error = null;
            return true;
        }

        private static bool TryReadSelectionOptions(
                                                    string query,
                                                    IReadOnlyList<string> arguments,
                                                    out SelectionOptions options,
                                                    out string? error
        )
        {
            if (arguments.Count == 0 || arguments[0].StartsWith("--", StringComparison.Ordinal))
            {
                options = null!;
                error = $"{query} requires an XPath expression.";
                return false;
            }

            string xpath = arguments[0];
            int limit = DefaultLimit;
            int index = 1;
            while (index < arguments.Count)
            {
                string argument = arguments[index++];
                if (argument == "--limit" && TryReadPositiveInt(
                        arguments,
                        ref index,
                        out limit))
                {
                    continue;
                }

                options = null!;
                error = $"Unknown or invalid {query} option '{argument}'.";
                return false;
            }

            options = new SelectionOptions(xpath, limit);
            error = null;
            return true;
        }

        private static bool TryReadPositiveInt(
                                               IReadOnlyList<string> arguments,
                                               ref int index,
                                               out int value
        )
        {
            return TryReadInt(arguments, ref index, out value) && value > 0;
        }

        private static bool TryReadNonNegativeInt(
                                                  IReadOnlyList<string> arguments,
                                                  ref int index,
                                                  out int value
        )
        {
            return TryReadInt(arguments, ref index, out value) && value >= 0;
        }

        private static bool TryReadInt(
                                       IReadOnlyList<string> arguments,
                                       ref int index,
                                       out int value
        )
        {
            if (index >= arguments.Count)
            {
                value = 0;
                return false;
            }

            return int.TryParse(
                arguments[index++],
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out value);
        }

        private static bool ExpectNoArguments(
                                              string query,
                                              IReadOnlyCollection<string> arguments
        )
        {
            if (arguments.Count == 0)
            {
                return true;
            }

            Console.Error.WriteLine($"error: {query} does not accept arguments.");
            return false;
        }

        private static int WriteNoMatches(
                                          string xpath
        )
        {
            Console.Error.WriteLine($"XPath selected no elements: {xpath}");
            return QueryErrorExitCode;
        }

        private static int WriteUnknownQuery(
                                             string query
        )
        {
            Console.Error.WriteLine($"error: unknown query '{query}'.");
            Console.Error.WriteLine(
                "Run with --help, or type help in the interactive shell.");
            return UsageErrorExitCode;
        }

        private static int WriteMovedXmlQuery(
                                              string query
        )
        {
            string command = query switch
            {
                "RAW" => "raw",
                "TREE" => "tree",
                "SELECT" => "select",
                "TEXT" => "text",
                "ATTRIBUTES" or "ATTRS" => "attributes",
                _ => query,
            };
            Console.Error.WriteLine(
                $"error: '{command}' is an XML diagnostic command; use 'xml {command}'.");
            return UsageErrorExitCode;
        }

        private static int WriteUsageError(
                                           string? error
        )
        {
            Console.Error.WriteLine($"error: {error}");
            Console.Error.WriteLine(
                "Run with --help, or type help in the interactive shell.");
            return UsageErrorExitCode;
        }

        private static void WriteTruncation(
                                            int totalCount,
                                            int displayedCount
        )
        {
            if (displayedCount != totalCount)
            {
                Console.WriteLine($"... {totalCount - displayedCount} more match(es)");
            }
        }

        private static void WriteProperty(
                                          string name,
                                          object value
        )
        {
            Console.WriteLine($"  {name}: {value}");
        }

        private static void WriteOptionalProperty(
                                                  string name,
                                                  string? value
        )
        {
            if (value is not null)
            {
                WriteProperty(name, value);
            }
        }

        private static void WriteHelp(
                                      string[]? arguments = null
        )
        {
            if (arguments is { Length: 1 } && string.Equals(
                    arguments[0],
                    "generate",
                    StringComparison.OrdinalIgnoreCase))
            {
                GenerateCli.WriteHelp();
                return;
            }

            if (arguments is { Length: > 0 })
            {
                Console.Error.WriteLine("error: help accepts only the 'generate' topic.");
                return;
            }

            Console.WriteLine("Inspect the structural OpenGL registry model.");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  StgSharp.GenerateGL [--xml <path>]        # interactive shell");
            Console.WriteLine("  StgSharp.GenerateGL [--xml <path>] shell");
            Console.WriteLine("  StgSharp.GenerateGL [--xml <path>] summary");
            Console.WriteLine("  StgSharp.GenerateGL [--xml <path>] list <category> " +
                              "[filter] [--limit <n>]");
            Console.WriteLine("  StgSharp.GenerateGL [--xml <path>] find <text> " +
                              "[--exact] [--limit <n>]");
            Console.WriteLine("  StgSharp.GenerateGL [--xml <path>] show <category> <name> " +
                              "[--limit <n>]");
            Console.WriteLine("  StgSharp.GenerateGL [--xml <path>] generate");
            Console.WriteLine("  StgSharp.GenerateGL [--xml <path>] list files");
            Console.WriteLine("  StgSharp.GenerateGL [--xml <path>] preview <filename>");
            Console.WriteLine("  StgSharp.GenerateGL [--xml <path>] xml <command> [...]");
            Console.WriteLine();
            Console.WriteLine("Registry categories:");
            Console.WriteLine(
                "  types, kinds, groups, enums, commands, features, extensions");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  StgSharp.GenerateGL summary");
            Console.WriteLine("  StgSharp.GenerateGL list commands BindBuffer");
            Console.WriteLine("  StgSharp.GenerateGL find glBindBuffer");
            Console.WriteLine("  StgSharp.GenerateGL show command glBindBuffer");
            Console.WriteLine("  StgSharp.GenerateGL show extension GL_KHR_debug");
            Console.WriteLine("  StgSharp.GenerateGL generate");
            Console.WriteLine("  StgSharp.GenerateGL list files");
            Console.WriteLine("  StgSharp.GenerateGL preview glconst.cs");
            Console.WriteLine("  StgSharp.GenerateGL xml tree --depth 1");
            Console.WriteLine();
            Console.WriteLine("Interactive commands:");
            Console.WriteLine("  help, load <path>, reload, clear, exit");
            Console.WriteLine();
            Console.WriteLine("Use 'xml' to list the raw XML diagnostic commands.");
            Console.WriteLine("Registry values and XML attributes remain strings.");
            Console.WriteLine("The default XML is copied beside the executable at xml/gl.xml.");
        }

        private static void WriteXmlHelp()
        {
            Console.WriteLine("Raw XML diagnostic commands:");
            Console.WriteLine("  xml summary");
            Console.WriteLine("  xml raw");
            Console.WriteLine(
                "  xml tree [xpath] [--depth <n>] [--limit <n>] [--whitespace]");
            Console.WriteLine(
                "  xml find <text> [--exact] [--up <n>] [--limit <n>]");
            Console.WriteLine("  xml show <xpath> [--limit <n>]");
            Console.WriteLine("  xml select <xpath> [--limit <n>]");
            Console.WriteLine("  xml text <xpath> [--limit <n>]");
            Console.WriteLine("  xml attributes <xpath> [--limit <n>]");
        }

        private sealed class CliSession
        {

            public CliSession(
                              XmlTreeDocument document,
                              GlRegistryModel registry
            )
            {
                Document = document;
                Registry = registry;
            }

            public XmlTreeDocument Document { get; }

            public GlRegistryModel Registry { get; }

            public GeneratedFileSet GeneratedFiles { get; set; } = GeneratedFileSet.Empty;

        }

        private sealed record CliInvocation(
            string XmlPath,
            string Command,
            IReadOnlyList<string> Arguments,
            bool ShowHelp,
            bool Interactive
        );

        private sealed record SelectionOptions(
            string XPath,
            int Limit
        );

        private sealed record SearchMatch(
            XElement Element,
            string Field,
            string Value
        );

        private sealed record SearchOptions(
            string Text,
            int Limit,
            int? ParentLevels,
            bool Exact
        );

        private sealed record TreeOptions(
            string XPath,
            int Depth,
            int Limit,
            bool IncludeWhitespace
        );

    }
}
