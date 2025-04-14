//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepDataReader.cs"
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
using StgSharp.Modeling.Step;
using StgSharp.Modeling.Step;
using StgSharp.Script;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Modeling.Step
{
    public partial class StepReader
    {

        public StepDataSectionTransmitter DataTransmitter { get; } = new StepDataSectionTransmitter(
            );

        public void ReadDataSection()
        {
            using( MemoryMappedViewStream ms = _memoryFile.CreateViewStream(
                0, _size, MemoryMappedFileAccess.Read ) )
            {
                StreamReader sr = new StreamReader( ms );
                long pos = _headStart;
                string line;
                while( string.IsNullOrEmpty( line = sr.ReadLine()! ) ) {
                    DataTransmitter.WriteLine( line );
                }
                sr.Dispose();
            }
        }

        [GeneratedRegex(
                @"^#(?<id>[0-9]+)\s*?=\s*?(?<expression>\S.*?)\s*;$",
                RegexOptions.Compiled | RegexOptions.Singleline )]
        private static partial Regex GetExpressionSplitter();

    }
}
