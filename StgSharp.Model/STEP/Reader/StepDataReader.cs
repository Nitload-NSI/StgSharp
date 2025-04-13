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

namespace StgSharp.Modeling.STEP
{
    public partial class StepReader
    {

        private Regex expressionSpilitter = GetExpressionSplitter();

        private ThreadLocal<ConcurrentQueue<StepCodeLine>> _codeLineQueue 
            = new ThreadLocal<ConcurrentQueue<StepCodeLine>>();

        public ConcurrentQueue<StepCodeLine> DataSectionPool
        {
            get => _codeLineQueue.Value;
        }

        public void ReadDataSection()
        {
            if( _codeLineQueue.Value == null ) {
                _codeLineQueue.Value = new ConcurrentQueue<StepCodeLine>();
            }
            using( MemoryMappedViewStream ms = _memoryFile.CreateViewStream(
                0, _size, MemoryMappedFileAccess.Read ) )
            {
                using( StreamReader sr = new StreamReader( ms ) )
                {
                    long pos = _headStart;
                    string line;
                    while( string.IsNullOrEmpty( line = sr.ReadLine()! ) )
                    {
                        Match match = expressionSpilitter.Match( line );
                        if( match.Success )
                        {
                            int id = int.Parse( match.Groups[ "id" ].Value );
                            string expression = match.Groups[ "expression" ].Value;
                            StepCodeLine stepExpression = new StepCodeLine( id, expression );
                            _codeLineQueue.Value.Enqueue( stepExpression );
                        }
                    }
                }
            }
        }

        [GeneratedRegex(
                @"^#(?<id>[0-9]+)\s*?=\s*?(?<expression>\S.*?)\s*;$",
                RegexOptions.Compiled | RegexOptions.Singleline )]
        private static partial Regex GetExpressionSplitter();

    }
}
