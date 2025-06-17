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
using Microsoft.CodeAnalysis.CSharp.Syntax;

using StgSharp.Math;
using StgSharp.Script.Express;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace StgSharp.Model.Step
{
    internal static partial class StepDataParser
    {

        private static Regex _stringParser = GetStringParser();
        private static Regex _vectorParser = GetVectorParser();
        private static Type _implementorInterface = typeof( IExpConvertableFrom<> );

        public static StepRepresentationItem Implement(
                                             StepRepresentationItem left,
                                             StepRepresentationItem right )
        {
            Type tLeft = left.GetType(),
                tRight = right.GetType(),
                iLeft = _implementorInterface.MakeGenericType( tLeft ),
                iRight = _implementorInterface.MakeGenericType( tRight );
            if( tLeft.IsAssignableTo( iRight ) )
            {
                ( ( dynamic )left ).FromInstance( ( dynamic )right );
                return left;
            }
            if( tRight.IsAssignableTo( iLeft ) )
            {
                ( ( dynamic )right ).FromInstance( ( dynamic )left );
                return right;
            }
            throw new InvalidCastException(
                $"{tLeft.Name } and { tRight.Name } has no implementation or inheritance relationship" );
        }

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

        public static Vec3 ToVector3D( ExpNodePresidentEnumerator enumerator )
        {
            enumerator.AssertEnumeratorCount( 3 );
            Vec3 result = new Vec3();
            ExpSyntaxNode node = enumerator.Current;
            if( node is ExpRealNumberNode x )
            {
                result.X = x.Value;
                enumerator.MoveNext();
            } else
            {
                throw new InvalidCastException();
            }
            if( node is ExpRealNumberNode y )
            {
                result.Y = y.Value;
                enumerator.MoveNext();
            } else
            {
                throw new InvalidCastException();
            }
            if( node is ExpRealNumberNode z )
            {
                result.Z = z.Value;
                enumerator.MoveNext();
            } else
            {
                throw new InvalidCastException();
            }
            return result;
        }

        public static Vec3 ToVector3D( ExpSyntaxNode node )
        {
            Vec3 result = new Vec3();
            if( node is ExpTupleNode )
            {
                ToVector3DPrivate( node.Right, ref result );
            } else
            {
                ToVector3DPrivate( node, ref result );
            }
            return result;
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

        private static void ToVector3DPrivate( ExpSyntaxNode node, ref Vec3 result )
        {
            ExpSyntaxNode begin = node;
            if( node is ExpRealNumberNode x )
            {
                result.X = x.Value;
                node = node.Next;
            } else
            {
                throw new InvalidCastException();
            }
            if( node is ExpRealNumberNode y )
            {
                result.Y = y.Value;
                node = node.Next;
            } else
            {
                throw new InvalidCastException();
            }
            if( node is ExpRealNumberNode z )
            {
                result.Z = z.Value;
                node = node.Next;
            } else
            {
                throw new InvalidCastException();
            }
            if( !ReferenceEquals( node, begin ) ) {
                throw new InvalidCastException();
            }
        }

    }
}
