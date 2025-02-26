//-----------------------------------------------------------------------
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
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static StgSharp.Script.Express.ExpCompile;

namespace StgSharp.Script.Express
{
    public interface IExpBuiltin { }

    internal partial class ExpSchema_Nitload : ExpSchema
    {

        private static readonly ExpSchema_Nitload _onlyInstance = new ExpSchema_Nitload(
            );

        private ExpSchema_Nitload()
            : base()
        {
            Name = "Nitload";
        }

        public static ExpSchema Only
        {
            get => _onlyInstance;
        }

        public override void LoadFromSource( ExpSchemaSource source )
        {
            return;
        }

        public override bool TryGetConst(
                                     string name,
                                     out ExpElementInstanceBase c )
        {
            return ConstDict.TryGetValue( name, out c );
        }

        public override bool TryGetEntity( string name, out ExpEntitySource e )
        {
            e = null;
            return false;
        }

        public override bool TryGetFunction(
                                     string name,
                                     out ExpFunctionSource f )
        {
            throw new NotImplementedException();
        }

        public override bool TryGetRule( string name, out ExpRuleSource r )
        {
            r = null;
            return false;
        }

        public override bool TryGetSchemaInclude(
                                     string name,
                                     out ExpSchema include )
        {
            include = null;
            return false;
        }

        public override bool TryGetType( string name, out ExpTypeSource t )
        {
            return TypeDict.TryGetValue( name, out t );
        }

        private void Init()
        {
            IEnumerable<KeyValuePair<string, ExpTypeSource>> builtinTypeSource = [
                new KeyValuePair<string, ExpTypeSource>(
                PoolString( KeyWord.Int ),
                new ExpBuiltinType( KeyWord.Int, typeof( int ) ) ),
                new KeyValuePair<string, ExpTypeSource>(
                PoolString( KeyWord.Real ),
                new ExpBuiltinType( KeyWord.Real, typeof( float ) ) ),
                new KeyValuePair<string, ExpTypeSource>(
                PoolString( KeyWord.Bool ),
                new ExpBuiltinType( KeyWord.Bool, typeof( bool ) ) ),
                new KeyValuePair<string, ExpTypeSource>(
                PoolString( KeyWord.Logic ),
                new ExpBuiltinType( KeyWord.Logic, typeof( ExpLogic ) ) ),
                new KeyValuePair<string, ExpTypeSource>(
                PoolString( KeyWord.String ),
                new ExpBuiltinType( KeyWord.String, typeof( string ) ) ),
                new KeyValuePair<string, ExpTypeSource>(
                PoolString( KeyWord.Binary ),
                new ExpBuiltinType( KeyWord.Binary, typeof( BitArray ) ) ),

                ];
            TypeDict = new Dictionary<string, ExpTypeSource>(
                builtinTypeSource, Multiplexer ).ToFrozenDictionary();


            ConstDict = new Dictionary<string, ExpElementInstanceBase>(
                Multiplexer ) {
                { PoolString( "PI" ), new ExpBuiltinConstant<float>(
                    PoolString( "PI" ), MathF.PI ) },
                { PoolString( "E" ), new ExpBuiltinConstant<float>(
                    PoolString( "E" ), MathF.E ) },
            }.ToFrozenDictionary();


            /*
            Operator = new Dictionary<string, ExpBinaryOperatorNode>(
                Multiplexer )
            {
                { PoolString( KeyWord.ADD ), new ExpBinaryOperatorNode(
                    KeyWord.ADD ) },
                { PoolString( KeyWord.SUB ), new ExpBinaryOperatorNode(
                    KeyWord.SUB ) },
                { PoolString( KeyWord.MUL ), new ExpBinaryOperatorNode(
                    KeyWord.MUL ) },
                { PoolString( KeyWord.DIV ), new ExpBinaryOperatorNode(
                    KeyWord.DIV ) },
            }.ToFrozenDictionary();
            /**/
        }

    }
}
