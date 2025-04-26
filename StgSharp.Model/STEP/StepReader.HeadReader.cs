//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepHeadReader.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the ¡°Software¡±), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED ¡°AS IS¡±, 
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
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StgSharp.Modeling.Step
{
    public partial class StepReader
    {

        private static Regex expressionSplitter = new Regex(
            @"^(?<CALLER>[A-Z_]+)\s?\((?<PARAM>.+)\)\;", RegexOptions.Compiled );
        /**/
        private StepInfo _info;

        public StepInfo GetInfo()
        {
            if( _info is not null ) {
                return _info;
            }
            StepInfo info = new StepInfo();
            List<StepObjectConstructor> constructorList = new List<StepObjectConstructor>();
            using( MemoryMappedViewStream ms = _memoryFile.CreateViewStream(
                0, _size, MemoryMappedFileAccess.Read ) )
            {
                using( StreamReader sr = new StreamReader( ms ) )
                {
                    long pos = _headStart;
                    ms.Seek( _headStart, SeekOrigin.Begin );
                    while( pos <= _headEnd )
                    {
                        string token = ReadToken( sr, ref pos );
                        if( token == "HEADER;" )
                        {
                            continue;
                        }

                        if( token == "ENDSEC;" )
                        {
                            break;
                        }

                        GroupCollection match = expressionSplitter.Match( token ).Groups;
                        constructorList.Add(
                            new StepObjectConstructor(
                                0, match[ "CALLER" ].ToString(), match[ "PARAM" ].ToString() ) );
                    }
                    foreach( StepObjectConstructor constructor in constructorList )
                    {
                        switch( constructor.Caller )
                        {
                            case "FILE_DESCRIPTION":
                                info.Description = StepDataParser.ToString( constructor[ 0 ] );
                                info.Level = StepDataParser.ToVersion( constructor[ 1 ] );
                                break;
                            case "FILE_NAME":
                                info.Name = StepDataParser.ToString( constructor[ 0 ] );
                                info.TimeStamp = StepDataParser.ToDateTime( constructor[ 1 ] );
                                info.Author = StepDataParser.ToString( constructor[ 2 ] );
                                info.Organization = StepDataParser.ToString( constructor[ 3 ] );
                                info.PreprocessorVersion = StepDataParser.ToString(
                                    constructor[ 4 ] );
                                info.OriginatingSystem = StepDataParser.ToString( constructor[ 5 ] );
                                info.Authorization = StepDataParser.ToString( constructor[ 6 ] );
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return info;
        }
        /**/

    }
}