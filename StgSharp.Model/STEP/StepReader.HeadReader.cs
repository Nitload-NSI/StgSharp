//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepReader.HeadReader"
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
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StgSharp.Model.Step
{
    public partial class StepReader
    {

        private static Regex expressionSplitter = GetStepHeadExpressionParser();
        /**/
        private StepInfo _info;

        public StepInfo GetInfo()
        {
            if (_info is not null) {
                return _info;
            }
            StepInfo info = new StepInfo();
            List<StepHeaderLineConstructor> constructorList = new List<StepHeaderLineConstructor>();
            using (MemoryMappedViewStream ms = _memoryFile.CreateViewStream(
                0, _size, MemoryMappedFileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    long pos = _headStart;
                    ms.Seek(_headStart, SeekOrigin.Begin);
                    while (pos <= _headEnd)
                    {
                        string token = ReadToken(sr, ref pos);
                        if (token == "HEADER;")
                        {
                            continue;
                        }

                        if (token == "ENDSEC;")
                        {
                            break;
                        }

                        GroupCollection match = expressionSplitter.Match(token).Groups;
                        constructorList.Add(
                            new StepHeaderLineConstructor(
                                0, match["CALLER"].ToString(), match["PARAM"].ToString()));
                    }
                    foreach (StepHeaderLineConstructor constructor in constructorList)
                    {
                        switch (constructor.Caller)
                        {
                            case "FILE_DESCRIPTION":
                                info.Description = StepDataParser.ToString(constructor[0]);
                                info.Level = StepDataParser.ToVersion(constructor[1]);
                                break;
                            case "FILE_NAME":
                                info.Name = StepDataParser.ToString(constructor[0]);
                                info.TimeStamp = StepDataParser.ToDateTime(constructor[1]);
                                info.Author = StepDataParser.ToString(constructor[2]);
                                info.Organization = StepDataParser.ToString(constructor[3]);
                                info.PreprocessorVersion = StepDataParser.ToString(
                                    constructor[4]);
                                info.OriginatingSystem = StepDataParser.ToString(constructor[5]);
                                info.Authorization = StepDataParser.ToString(constructor[6]);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return info;
        }

        [GeneratedRegex(@"^(?<CALLER>[A-Z_]+)\s?\((?<PARAM>.+)\)\;", RegexOptions.Compiled)]
        private static partial Regex GetStepHeadExpressionParser();
        /**/

    }
}