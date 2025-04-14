//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="StepDataParser.cs"
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
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace StgSharp.Modeling.Step
{
    public static partial class StepDataParser
    {

        private static Regex _stringParser = GetStringParser();
        private static Regex _vectorParser = GetVectorParser();

        public static DateTime ToDateTime( string source )
        {
            Match match = _stringParser.Match( source );
            if( match.Success ) {
                return DateTime.Parse( match.Groups[ "value" ].Value );
            }
            throw new InvalidCastException();
        }

        public static string ToString( string source )
        {
            Match match = _stringParser.Match( source );
            if( match.Success ) {
                return match.Groups[ "value" ].Value;
            }
            return string.Empty;
        }

        public static Vec3 ToVector3D( string source )
        {
            Match match = _vectorParser.Match( source );
            if( match.Success ) {
                return new Vec3(
                    float.Parse( match.Groups[ "x" ].Value ),
                    float.Parse( match.Groups[ "y" ].Value ),
                    float.Parse( match.Groups[ "z" ].Value ) );
            }
            return Vec3.Zero;
        }

        public static (int, int?) ToVersion( string source )
        {
            Match match = _stringParser.Match( source );
            if( match.Success )
            {
                string data = match.Groups[ "value" ].Value;
                if( data.Contains( ';' ) )
                {
                    string[] subData = data.Split( ';' );
                    return (int.Parse( subData[ 0 ] ), int.Parse( subData[ 1 ] ));
                }
                return (int.Parse( data ), null);
            }
            return (-1,-1);
        }

        [GeneratedRegex( @"(\(\s?'|')\s*(?<value>.+)\s*('\s*\)|')", RegexOptions.Singleline )]
        private static partial Regex GetStringParser();

        [GeneratedRegex(
                @"^\(\s+(?<x>-?[0-9]+\.[0-9]+),\s+(?<y>-?[0-9]+\.[0-9]+),\s+(?<z>-?[0-9]+\.[0-9]+)\s+\)$",
                RegexOptions.Singleline )]
        private static partial Regex GetVectorParser();

    }
}
