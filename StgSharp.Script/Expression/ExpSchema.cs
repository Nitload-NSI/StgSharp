//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpSchema.cs"
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
using StgSharp.Collections;
using StgSharp.Graphics.OpenGL;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace StgSharp.Script.Expression
{
    public class ExpSchemaSource
    {

        private static Regex 
            _constDefineBegin = new Regex(
            @"\s*CONSTANT", RegexOptions.Compiled ),
            _constDefineEnd = new Regex(
            @"\s*END_CONSTANT;", RegexOptions.Compiled ),
            _schemaBegin = new Regex(
            @"\s*SCHEMA\s*(?<namespace>[A-Za-z_]+)\s?;",
            RegexOptions.Compiled ),
            _schemaEnd = new Regex(
            @"END_SCHEMA;\s*--\s*(?<namespace>[A-Za-z_]+)",
            RegexOptions.Compiled ),
            _entityDefineBegin = new Regex(
            @"\s*ENTITY\s*(?<TypeName>[A-Za-z_]+)\s*", RegexOptions.Compiled ),
            _entityDefineEnd = new Regex(
            @"\s*END_ENTITY;\s*--\s*(?<TypeName>[A-Za-z_]+)\s*",
            RegexOptions.Compiled ),
            _typeDefineBegin = new Regex(
            @"\s*TYPE\s+(?<TypeName>[A-Za-z_]+)\s*=\s*(?<Remaining>[A-Z]+)",
            RegexOptions.Compiled ),
            _typeDefineEnd = new Regex(
            @"\s*END_TYPE\s*;\s*--\s*(?<TypeName>[a-zA-Z_]+)",
            RegexOptions.Compiled ),
            _functionDefineBegin = new Regex(
            @"\s*FUNCTION\s*\s*(?<FunctionName>[a-zA-Z_]+)\s*(?<Remaining>.*)",
            RegexOptions.Compiled ),
            _functionDefineEnd = new Regex(
            @"\s*END_FUNCTION\s*;\s*--\s*(?<FunctionName>[a-zA-Z_]+)",
            RegexOptions.Compiled ),
            _ruleDefineBegin = new Regex(
            @"\s*RULE\s*(?<RuleName>[a-zA-Z_]+)\s*FOR\s*\((?<Target>[a-zA-Z_]*(\s*\)\s*;)?)",
            RegexOptions.Compiled ),
            _ruleDefineEnd = new Regex(
            @"\s*END_RULE\s*;\s*--\s*(?<RuleName>[A-Za-z_]+)",
            RegexOptions.Compiled );

        private Dictionary<string, IExpElementSource> _sourceCache = new();

        private string name;

        public ExpSchemaSource() { }

        public void ReadSchema( ExpFileReader reader )
        {
            string line = reader.ReadLine();
            while( !_schemaBegin.IsMatch( line ) ) {
                line = reader.ReadLine();
            }
            name = _schemaBegin.Match( line ).Groups[ "namespace" ].Value;

            while( !_schemaEnd.IsMatch( line ) ) {
                if( TryReadConst( line, reader,
                                  out IExpElementSource source ) ) {
                    _sourceCache.Add( source.Name, source );
                } else if( TryReadType( line, reader, out source ) ) {
                    _sourceCache.Add( source.Name, source );
                } else if( TryReadEntity( line, reader, out source ) ) {
                    _sourceCache.Add( source.Name, source );
                } else if( TryReadRule( line, reader, out source ) ) {
                    _sourceCache.Add( source.Name, source );
                } else if( TryReadFunction( line, reader, out source ) ) {
                    _sourceCache.Add( source.Name, source );
                }
                line = reader.ReadLine();
            }
        }

        public static bool TryReadSchema(
                                   string firstLine,
                                   ExpFileReader reader,
                                   out ExpSchemaSource source )
        {
            if( !_schemaBegin.IsMatch( firstLine ) ) {
                source = null!;
                return false;
            }
            ExpSchemaSource _source = new ExpSchemaSource();
            _source.name = _schemaBegin.Match( firstLine ).Groups[ "namespace" ].Value;

            while( !_schemaEnd.IsMatch( firstLine ) ) {
                if( _source.TryReadConst(
                    firstLine, reader,
                    out IExpElementSource _elementSource ) ) {
                    _source._sourceCache
                            .Add( _elementSource.Name, _elementSource );
                } else if( _source.TryReadType(
                           firstLine, reader, out _elementSource ) ) {
                    _source._sourceCache
                            .Add( _elementSource.Name, _elementSource );
                } else if( _source.TryReadEntity(
                           firstLine, reader, out _elementSource ) ) {
                    _source._sourceCache
                            .Add( _elementSource.Name, _elementSource );
                } else if( _source.TryReadRule(
                           firstLine, reader, out _elementSource ) ) {
                    _source._sourceCache
                            .Add( _elementSource.Name, _elementSource );
                } else if( _source.TryReadFunction(
                           firstLine, reader, out _elementSource ) ) {
                    _source._sourceCache
                            .Add( _elementSource.Name, _elementSource );
                }
                firstLine = reader.ReadLine();
            }
            source = _source;
            return true;
        }

        private bool TryReadConst(
                             string firstLine,
                             ExpFileReader reader,
                             out IExpElementSource source )
        {
            Match match = _constDefineBegin.Match( firstLine );
            if( match.Success ) {
                ScriptSourceTransmitter transmitter = new ScriptSourceTransmitter(
                    "END_CONSTANT" );
                ExpConstSource _source = new ExpConstSource(
                    "Constant", transmitter );
                string line = reader.ReadLine();
                while( !_constDefineEnd.IsMatch( line ) ) {
                    transmitter.WriteLine( line );
                    line = reader.ReadLine();
                }
                transmitter.WriteLine( "END_CONSTANT" );
                source = _source;
                return true;
            }
            source = null!;
            return false;
        }

        private bool TryReadEntity(
                             string firstLine,
                             ExpFileReader reader,
                             out IExpElementSource source )
        {
            Match match = _entityDefineBegin.Match( firstLine );
            if( match.Success ) {
                ScriptSourceTransmitter transmitter = new ScriptSourceTransmitter(
                    "END_ENTITY" );
                ExpEntitySource _source = new ExpEntitySource(
                    match.Groups[ "TypeName" ].ToString(), transmitter );
                string line = reader.ReadLine();
                match = _entityDefineEnd.Match( line );
                while( !match.Success ) {
                    transmitter.WriteLine( line );
                    line = reader.ReadLine();
                    match = _entityDefineEnd.Match( line );
                }
                ;
                string endedName = match.Groups[ "TypeName" ].ToString();
                if( !string.Equals( endedName, _source.Name ) ) {
                    throw new ExpInvalidTypeDeclareEndingExceptions(
                        _source.Name, endedName );
                }
                transmitter.WriteLine( "END_ENTITY" );
                source = _source;
                return true;
            }
            source = null!;
            return false;
        }

        private bool TryReadFunction(
                             string firstLine,
                             ExpFileReader reader,
                             out IExpElementSource source )
        {
            Match match = _functionDefineBegin.Match( firstLine );
            if( match.Success ) {
                ScriptSourceTransmitter transmitter = new ScriptSourceTransmitter(
                    "END_FUNCTION" );
                ExpFunctionSource _source = new ExpFunctionSource(
                    match.Groups[ "FunctionName" ].ToString(), transmitter );
                transmitter.WriteLine( match.Groups[ "Remaining" ].ToString() );
                string line = reader.ReadLine();
                match = _functionDefineEnd.Match( line );
                while( !match.Success ) {
                    transmitter.WriteLine( line );
                    line = reader.ReadLine();
                    match = _functionDefineEnd.Match( line );
                }
                string endedName = match.Groups[ "FunctionName" ].ToString();
                if( !string.Equals( endedName, _source.Name ) ) {
                    throw new ExpInvalidTypeDeclareEndingExceptions(
                        _source.Name, endedName );
                }
                transmitter.WriteLine( "END_FUNCTION" );
                source = _source;
                return true;
            }
            source = null!;
            return false;
        }

        private bool TryReadRule(
                             string firstLine,
                             ExpFileReader reader,
                             out IExpElementSource source )
        {
            Match match = _ruleDefineBegin.Match( firstLine );
            if( match.Success ) {
                ScriptSourceTransmitter transmitter = new ScriptSourceTransmitter(
                    "END_RULE" );
                ExpRuleSource _source = new ExpRuleSource(
                    match.Groups[ "RuleName" ].ToString(), transmitter );
                string target = match.Groups[ "Target" ].ToString();
                if( !string.IsNullOrEmpty( target ) ) {
                    transmitter.WriteLine( target );
                }
                string line = reader.ReadLine();
                match = _ruleDefineEnd.Match( line );
                while( !match.Success ) {
                    transmitter.WriteLine( line );
                    line = reader.ReadLine();
                    match = _ruleDefineEnd.Match( line );
                }
                string endedName = match.Groups[ "RuleName" ].ToString();
                if( !string.Equals( endedName, _source.Name ) ) {
                    throw new ExpInvalidTypeDeclareEndingExceptions(
                        _source.Name, endedName );
                }
                transmitter.WriteLine( "END_RULE" );
                source = _source;
                return true;
            }
            source = null!;
            return false;
        }

        private bool TryReadType(
                             string firstLine,
                             ExpFileReader reader,
                             out IExpElementSource source )
        {
            Match match = _typeDefineBegin.Match( firstLine );
            if( match.Success ) {
                ScriptSourceTransmitter transmitter = new ScriptSourceTransmitter(
                    "END_ENTITY" );
                ExpTypeSource _source = new ExpTypeSource(
                    match.Groups[ "TypeName" ].ToString(), transmitter );
                transmitter.WriteLine( match.Groups[ "Remaining" ].ToString() );
                string line = reader.ReadLine();
                match = _typeDefineEnd.Match( line );
                while( !match.Success ) {
                    transmitter.WriteLine( line );
                    line = reader.ReadLine();
                    match = _typeDefineEnd.Match( line );
                }
                string endedName = match.Groups[ "TypeName" ].ToString();
                if( !string.Equals( endedName, _source.Name ) ) {
                    throw new ExpInvalidTypeDeclareEndingExceptions(
                        _source.Name, endedName );
                }
                transmitter.WriteLine( "END_ENTITY" );
                source = _source;
                return true;
            }
            source = null!;
            return false;
        }

    }

    public class ExpSchema
    {

        private Dictionary<string, ExpConstSource> _constDict = new();
        private Dictionary<string, ExpEntitySource> _entityDict = new Dictionary<string, ExpEntitySource>(
            );
        private Dictionary<string, ExpFunctionSource> _functionDict = new Dictionary<string, ExpFunctionSource>(
            );
        private Dictionary<string, ExpRuleSource> _ruleDict = new Dictionary<string, ExpRuleSource>(
            );
        private Dictionary<string, ExpTypeSource> _typeDict = new Dictionary<string, ExpTypeSource>(
            );

        public string Name
        {
            get;
            internal set;
        }

        public ExpConstSource SearchConstDeclare( string typeName )
        {
            if( _constDict.TryGetValue( typeName,
                                        out ExpConstSource? source ) ) {
                return source;
            }
            if( _constDict.TryGetValue( typeName,
                                        out ExpConstSource? type ) ) { }
            throw new KeyNotFoundException();
        }

    }
}
