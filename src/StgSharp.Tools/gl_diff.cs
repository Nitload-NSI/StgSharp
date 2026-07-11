using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

partial class Program
{
    private static readonly string csharpFieldsFile = @"d:\platform-dev\StgSharp.Tools\cs.txt";
    private static readonly string cStructFieldsFile = @"d:\platform-dev\StgSharp.Tools\c.txt";

    static void Main()
    {
        var csharpFields = ParseFieldNames(File.ReadAllText(csharpFieldsFile), stripGlPrefix: true);
        var cStructFields = ParseFieldNames(File.ReadAllText(cStructFieldsFile), stripGlPrefix: false);

        var missingInCStruct = new HashSet<string>(csharpFields).Except(cStructFields).ToList();
        var missingInCSharp = new HashSet<string>(cStructFields).Except(csharpFields).ToList();


        Console.WriteLine("Fields only in C# file:");
        missingInCStruct.ForEach(f => Console.WriteLine($"  {f}"));

        Console.WriteLine("Fields only in C struct file:");
        missingInCSharp.ForEach(f => Console.WriteLine($"  {f}"));
    }

    private static HashSet<string> ParseFieldNames(string content, bool stripGlPrefix)
    {
        content = StripComments(content);

        var matches = stripGlPrefix
            ? CSharpFieldRegex().Matches(content)
            : CStructFieldRegex().Matches(content);

        var fields = new HashSet<string>();
        foreach (Match match in matches)
        {
            var nameGroup = match.Groups["name"];
            if (nameGroup.Success)
            {
                fields.Add(nameGroup.Value);
            }
            else if (match.Groups.Count > 1)
            {
                fields.Add(match.Groups[1].Value);
            }
        }
        return fields;
    }

    private static string StripComments(string text)
    {
        // Remove // comments
        text = LineCommentRegex().Replace(text, string.Empty);
        // Remove /* */ comments
        text = BlockCommentRegex().Replace(text, string.Empty);
        return text;
    }

    // C#: internal delegate*<...> glName; capture Name without the gl prefix
    [GeneratedRegex(@"^\s*internal\s+(?:unsafe\s+)?delegate\*<[^>]+?>\s+gl(?<name>[A-Za-z0-9_]+)\s*;", RegexOptions.Multiline)]
    private static partial Regex CSharpFieldRegex();

    // C: PFNGLXYZPROC Name; capture the Name
    [GeneratedRegex(@"^\s*PFNGL[A-Z0-9_]+PROC\s+(?<name>[A-Za-z0-9_]+)\s*;", RegexOptions.Multiline)]
    private static partial Regex CStructFieldRegex();

    [GeneratedRegex("//.*", RegexOptions.None)]
    private static partial Regex LineCommentRegex();

    [GeneratedRegex("/\\*.*?\\*/", RegexOptions.Singleline)]
    private static partial Regex BlockCommentRegex();
}