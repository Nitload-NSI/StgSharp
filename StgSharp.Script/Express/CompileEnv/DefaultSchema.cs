﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="DefaultSchema.cs"
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
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

using static StgSharp.Script.Express.ExpressCompile;

namespace StgSharp.Script.Express
{
    public sealed partial class ExpSchema_Builtin : ExpSchema
    {

        private static readonly ExpSchema_Builtin _onlyInstance = new ExpSchema_Builtin();

        private ExpSchema_Builtin()
            : base()
        {
            Name = "Nitload";
        }

        internal static ExpSchema_Builtin Only
        {
            get => _onlyInstance;
        }

        public override void LoadFromSource( ExpSchemaSource source )
        {
            return;
        }

        public override bool TryGetConst( string name, out ExpElementInstance c )
        {
            return ConstDict.TryGetValue( name, out c );
        }

        public override bool TryGetEntity( string name, out ExpEntitySource e )
        {
            e = null;
            return false;
        }

        public override bool TryGetFunction( string name, out ExpFunctionSource f )
        {
            return FunctionDict.TryGetValue( name, out f! );
        }

        public override bool TryGetProcedure( string name, out ExpProcedureSource p )
        {
            return ProcedureDict.TryGetValue( name, out p! );
        }

        public override bool TryGetRule( string name, out ExpRuleSource r )
        {
            r = null!;
            return false;
        }

        public override bool TryGetSchemaInclude( string name, out ExpSchema include )
        {
            include = null!;
            return false;
        }

        public override bool TryGetType( string name, out ExpTypeSource t )
        {
            return TypeDict.TryGetValue( name, out t );
        }

        internal void Init()
        {
            IEnumerable<KeyValuePair<string, ExpTypeSource>> builtinTypeSource = [
                new KeyValuePair<string, ExpTypeSource>(
                PoolString( Keyword.Integer ),
                new ExpBuiltinType( Keyword.Integer, typeof( int ) ) ),
                new KeyValuePair<string, ExpTypeSource>(
                PoolString( Keyword.Real ), new ExpBuiltinType( Keyword.Real, typeof( float ) ) ),
                new KeyValuePair<string, ExpTypeSource>(
                PoolString( Keyword.Boolean ),
                new ExpBuiltinType( Keyword.Boolean, typeof( bool ) ) ),
                new KeyValuePair<string, ExpTypeSource>(
                PoolString( Keyword.Logical ),
                new ExpBuiltinType( Keyword.Logical, typeof( ExpLogic ) ) ),
                new KeyValuePair<string, ExpTypeSource>(
                PoolString( Keyword.String ),
                new ExpBuiltinType( Keyword.String, typeof( string ) ) ),
                new KeyValuePair<string, ExpTypeSource>(
                PoolString( Keyword.Binary ),
                new ExpBuiltinType( Keyword.Binary, typeof( BitArray ) ) ),

                ];
            TypeDict = new Dictionary<string, ExpTypeSource>( builtinTypeSource,
                                                              Multiplexer ).ToFrozenDictionary();


            ConstDict = new Dictionary<string, ExpElementInstance>( Multiplexer ) {
                { PoolString( "PI" ), new ExpRealNumberNode(
                    new Token( PoolString( "PI" ), -1, -1, TokenFlag.Number ), MathF.PI, true ) },
                { PoolString( "E" ), new ExpRealNumberNode(
                    new Token( PoolString( "E" ), -1, -1, TokenFlag.Number ), MathF.E, true ) },
            }.ToFrozenDictionary();
        }

    }
}
