// -----------------------------------------------------------------------------
// file="GlRegistryReader"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using StgSharp.GenerateGL.Tree;

namespace StgSharp.GenerateGL.Registry
{
    /// <summary>
    ///   Projects the generic XML tree into the structural OpenGL registry model.
    /// </summary>
    internal static class GlRegistryReader
    {

        private static readonly IReadOnlyDictionary<string, string> _emptyAttributes =
            new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>(StringComparer.Ordinal));

        public static GlRegistryModel Read(
                                           XmlTreeDocument document
        )
        {
            ArgumentNullException.ThrowIfNull(document);

            XElement root = document.Tree.Root ?? throw new InvalidDataException(
                $"XML document '{document.SourcePath}' does not contain a root element.");
            if (root.Name != "registry")
            {
                throw CreateError(
                    document,
                    root,
                    $"Expected <registry>, but found <{root.Name}>.");
            }

            ValidateChildren(
                document,
                root,
                "comment",
                "types",
                "kinds",
                "groups",
                "enums",
                "commands",
                "feature",
                "extensions");

            return new GlRegistryModel(
                document.SourcePath,
                ReadOptionalElementText(root, "comment"),
                ReadSection(document, root, "types", "type").Select(
                    value => ReadType(document, value)),
                ReadSection(document, root, "kinds", "kind").Select(
                    value => ReadKind(document, value)),
                ReadSection(document, root, "groups", "group").Select(
                    value => ReadEnumGroup(document, value)),
                root.Elements("enums").Select(value => ReadEnumBlock(document, value)),
                root.Elements("commands").Select(
                    value => ReadCommandBlock(document, value)),
                root.Elements("feature").Select(value => ReadFeature(document, value)),
                ReadSection(document, root, "extensions", "extension").Select(
                    value => ReadExtension(document, value)),
                ReadElement(root));
        }

        private static GlTypeDefinition ReadType(
                                                 XmlTreeDocument document,
                                                 XElement element
        )
        {
            string name = ReadOptionalAttribute(element, "name") ??
                          ReadOptionalElementText(element, "name") ??
                          throw CreateError(
                              document,
                              element,
                              "A registry <type> requires a name attribute or child element.");

            return new GlTypeDefinition(
                name,
                ReadDeclarationText(element),
                ReadOptionalAttribute(element, "api"),
                ReadOptionalAttribute(element, "requires"),
                ReadOptionalAttribute(element, "type"),
                ReadOptionalAttribute(element, "comment"),
                ReadElement(element));
        }

        private static GlKindDefinition ReadKind(
                                                 XmlTreeDocument document,
                                                 XElement element
        )
        {
            return new GlKindDefinition(
                ReadRequiredAttribute(document, element, "name"),
                ReadRequiredAttribute(document, element, "desc"),
                ReadElement(element));
        }

        private static GlEnumGroup ReadEnumGroup(
                                                  XmlTreeDocument document,
                                                  XElement element
        )
        {
            ValidateChildren(document, element, "enum");
            return new GlEnumGroup(
                ReadRequiredAttribute(document, element, "name"),
                ReadOptionalAttribute(element, "comment"),
                Freeze(
                    element.Elements("enum").Select(
                        value => new GlNamedReference(
                            ReadRequiredAttribute(document, value, "name"),
                            ReadElement(value)))),
                ReadElement(element));
        }

        private static GlEnumBlock ReadEnumBlock(
                                                  XmlTreeDocument document,
                                                  XElement element
        )
        {
            ValidateChildren(document, element, "enum", "unused");
            return new GlEnumBlock(
                ReadOptionalAttribute(element, "namespace"),
                ReadOptionalAttribute(element, "group"),
                ReadOptionalAttribute(element, "type"),
                ReadOptionalAttribute(element, "start"),
                ReadOptionalAttribute(element, "end"),
                ReadOptionalAttribute(element, "vendor"),
                ReadOptionalAttribute(element, "comment"),
                Freeze(
                    element.Elements("enum").Select(
                        value => ReadEnum(document, value))),
                Freeze(
                    element.Elements("unused").Select(
                        value => ReadUnusedEnumRange(document, value))),
                ReadElement(element));
        }

        private static GlEnumDefinition ReadEnum(
                                                  XmlTreeDocument document,
                                                  XElement element
        )
        {
            return new GlEnumDefinition(
                ReadRequiredAttribute(document, element, "name"),
                ReadOptionalAttribute(element, "value"),
                ReadOptionalAttribute(element, "api"),
                ReadOptionalAttribute(element, "type"),
                ReadOptionalAttribute(element, "group"),
                ReadOptionalAttribute(element, "alias"),
                ReadOptionalAttribute(element, "comment"),
                ReadElement(element));
        }

        private static GlUnusedEnumRange ReadUnusedEnumRange(
                                                               XmlTreeDocument document,
                                                               XElement element
        )
        {
            return new GlUnusedEnumRange(
                ReadRequiredAttribute(document, element, "start"),
                ReadOptionalAttribute(element, "end"),
                ReadOptionalAttribute(element, "vendor"),
                ReadOptionalAttribute(element, "comment"),
                ReadElement(element));
        }

        private static GlCommandBlock ReadCommandBlock(
                                                        XmlTreeDocument document,
                                                        XElement element
        )
        {
            ValidateChildren(document, element, "command");
            return new GlCommandBlock(
                ReadOptionalAttribute(element, "namespace"),
                Freeze(
                    element.Elements("command").Select(
                        value => ReadCommand(document, value))),
                ReadElement(element));
        }

        private static GlCommandDefinition ReadCommand(
                                                        XmlTreeDocument document,
                                                        XElement element
        )
        {
            ValidateChildren(
                document,
                element,
                "proto",
                "param",
                "alias",
                "vecequiv",
                "glx");

            XElement prototype = element.Element("proto") ?? throw CreateError(
                document,
                element,
                "A registry <command> requires a <proto> child element.");
            string name = ReadRequiredElementText(document, prototype, "name");

            return new GlCommandDefinition(
                name,
                ReadReturn(document, prototype),
                Freeze(
                    element.Elements("param").Select(
                        value => ReadParameter(document, value))),
                Freeze(
                    element.Elements("alias").Select(
                        value => ReadNamedReference(document, value))),
                Freeze(
                    element.Elements("vecequiv").Select(
                        value => ReadNamedReference(document, value))),
                Freeze(
                    element.Elements("glx").Select(
                        value => ReadCommandProtocol(document, value))),
                ReadOptionalAttribute(element, "comment"),
                ReadElement(element));
        }

        private static GlReturnDefinition ReadReturn(
                                                       XmlTreeDocument document,
                                                       XElement element
        )
        {
            ValidateChildren(document, element, "ptype", "name");
            _ = ReadRequiredElementText(document, element, "name");
            return new GlReturnDefinition(
                ReadDeclarationText(element),
                ReadOptionalElementText(element, "ptype"),
                ReadOptionalAttribute(element, "group"),
                ReadOptionalAttribute(element, "kind"),
                ReadOptionalAttribute(element, "class"),
                ReadElement(element));
        }

        private static GlParameterDefinition ReadParameter(
                                                            XmlTreeDocument document,
                                                            XElement element
        )
        {
            ValidateChildren(document, element, "ptype", "name");
            return new GlParameterDefinition(
                ReadRequiredElementText(document, element, "name"),
                ReadDeclarationText(element),
                ReadOptionalElementText(element, "ptype"),
                ReadOptionalAttribute(element, "group"),
                ReadOptionalAttribute(element, "kind"),
                ReadOptionalAttribute(element, "class"),
                ReadOptionalAttribute(element, "len"),
                ReadElement(element));
        }

        private static GlNamedReference ReadNamedReference(
                                                            XmlTreeDocument document,
                                                            XElement element
        )
        {
            return new GlNamedReference(
                ReadRequiredAttribute(document, element, "name"),
                ReadElement(element));
        }

        private static GlCommandProtocol ReadCommandProtocol(
                                                              XmlTreeDocument document,
                                                              XElement element
        )
        {
            return new GlCommandProtocol(
                ReadRequiredAttribute(document, element, "type"),
                ReadRequiredAttribute(document, element, "opcode"),
                ReadOptionalAttribute(element, "name"),
                ReadOptionalAttribute(element, "comment"),
                ReadElement(element));
        }

        private static GlFeatureDefinition ReadFeature(
                                                        XmlTreeDocument document,
                                                        XElement element
        )
        {
            ValidateChildren(document, element, "require", "remove");
            return new GlFeatureDefinition(
                ReadRequiredAttribute(document, element, "name"),
                ReadRequiredAttribute(document, element, "api"),
                ReadRequiredAttribute(document, element, "number"),
                ReadOptionalAttribute(element, "protect"),
                ReadOptionalAttribute(element, "comment"),
                Freeze(
                    element.Elements().Select(
                        value => ReadRequirement(document, value))),
                ReadElement(element));
        }

        private static GlExtensionDefinition ReadExtension(
                                                            XmlTreeDocument document,
                                                            XElement element
        )
        {
            ValidateChildren(document, element, "require", "remove");
            return new GlExtensionDefinition(
                ReadRequiredAttribute(document, element, "name"),
                ReadOptionalAttribute(element, "supported"),
                ReadOptionalAttribute(element, "protect"),
                ReadOptionalAttribute(element, "comment"),
                Freeze(
                    element.Elements().Select(
                        value => ReadRequirement(document, value))),
                ReadElement(element));
        }

        private static GlRequirementBlock ReadRequirement(
                                                           XmlTreeDocument document,
                                                           XElement element
        )
        {
            ValidateChildren(document, element, "type", "enum", "command");
            GlRequirementOperation operation = element.Name.LocalName switch
            {
                "require" => GlRequirementOperation.Require,
                "remove" => GlRequirementOperation.Remove,
                _ => throw CreateError(
                    document,
                    element,
                    $"Expected <require> or <remove>, but found <{element.Name}>."),
            };

            return new GlRequirementBlock(
                operation,
                ReadOptionalAttribute(element, "api"),
                ReadOptionalAttribute(element, "profile"),
                ReadOptionalAttribute(element, "comment"),
                Freeze(
                    element.Elements().Select(
                        value => ReadRegistryReference(document, value))),
                ReadElement(element));
        }

        private static GlRegistryReference ReadRegistryReference(
                                                                  XmlTreeDocument document,
                                                                  XElement element
        )
        {
            GlRegistryReferenceKind kind = element.Name.LocalName switch
            {
                "type" => GlRegistryReferenceKind.Type,
                "enum" => GlRegistryReferenceKind.Enum,
                "command" => GlRegistryReferenceKind.Command,
                _ => throw CreateError(
                    document,
                    element,
                    $"Unsupported registry reference <{element.Name}>."),
            };
            return new GlRegistryReference(
                kind,
                ReadRequiredAttribute(document, element, "name"),
                ReadOptionalAttribute(element, "comment"),
                ReadElement(element));
        }

        private static IEnumerable<XElement> ReadSection(
                                                          XmlTreeDocument document,
                                                          XElement root,
                                                          XName sectionName,
                                                          XName itemName
        )
        {
            foreach (XElement section in root.Elements(sectionName))
            {
                ValidateChildren(document, section, itemName.LocalName);
                foreach (XElement item in section.Elements(itemName))
                {
                    yield return item;
                }
            }
        }

        private static string ReadRequiredAttribute(
                                                     XmlTreeDocument document,
                                                     XElement element,
                                                     XName name
        )
        {
            string? value = ReadOptionalAttribute(element, name);
            if (value is null)
            {
                throw CreateError(
                    document,
                    element,
                    $"<{element.Name}> requires the '{name}' attribute.");
            }

            return value;
        }

        private static string? ReadOptionalAttribute(
                                                     XElement element,
                                                     XName name
        )
        {
            string? value = (string?)element.Attribute(name);
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private static string ReadRequiredElementText(
                                                      XmlTreeDocument document,
                                                      XElement element,
                                                      XName name
        )
        {
            string? value = ReadOptionalElementText(element, name);
            if (value is null)
            {
                throw CreateError(
                    document,
                    element,
                    $"<{element.Name}> requires a non-empty <{name}> child element.");
            }

            return value;
        }

        private static string? ReadOptionalElementText(
                                                       XElement element,
                                                       XName name
        )
        {
            string? value = element.Element(name)?.Value.Trim();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private static string ReadDeclarationText(
                                                  XElement element
        )
        {
            return element.Value.Trim();
        }

        private static GlRegistryElement ReadElement(
                                                       XElement element
        )
        {
            IXmlLineInfo lineInfo = element;
            GlSourceLocation location = lineInfo.HasLineInfo()
                ? new GlSourceLocation(lineInfo.LineNumber, lineInfo.LinePosition)
                : default;
            IReadOnlyDictionary<string, string> attributes = element.HasAttributes
                ? new ReadOnlyDictionary<string, string>(
                    element.Attributes().ToDictionary(
                        value => value.Name.ToString(),
                        value => value.Value,
                        StringComparer.Ordinal))
                : _emptyAttributes;
            return new GlRegistryElement(
                location,
                element.ToString(SaveOptions.DisableFormatting),
                attributes);
        }

        private static ReadOnlyCollection<T> Freeze<T>(
                                                        IEnumerable<T> values
        )
        {
            return Array.AsReadOnly(values.ToArray());
        }

        private static void ValidateChildren(
                                             XmlTreeDocument document,
                                             XElement element,
                                             params string[] allowedNames
        )
        {
            HashSet<string> allowed = new HashSet<string>(
                allowedNames,
                StringComparer.Ordinal);
            XElement? unsupported = element.Elements().FirstOrDefault(
                value => !allowed.Contains(value.Name.LocalName));
            if (unsupported is not null)
            {
                throw CreateError(
                    document,
                    unsupported,
                    $"Unsupported <{unsupported.Name}> child in <{element.Name}>.");
            }
        }

        private static InvalidDataException CreateError(
                                                        XmlTreeDocument document,
                                                        XElement element,
                                                        string message
        )
        {
            IXmlLineInfo lineInfo = element;
            string location = lineInfo.HasLineInfo()
                ? $"({lineInfo.LineNumber},{lineInfo.LinePosition})"
                : string.Empty;
            return new InvalidDataException(
                $"{document.SourcePath}{location}: {message}");
        }

    }
}
