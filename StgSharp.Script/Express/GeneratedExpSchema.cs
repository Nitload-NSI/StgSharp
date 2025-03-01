//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="GeneratedExpSchema.cs"
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
using CommunityToolkit.HighPerformance.Helpers;

using StgSharp.Collections;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Script.Express
{
    internal class GeneratedExpSchema : ExpSchema
    {

        public GeneratedExpSchema() { }

        public static ExpSchema DefaultSchema
        {
            get => ExpSchema_Nitload.Only;
        }

        public ExpTypeSource GetExpType( string typeName )
        {
            if( BuiltinSchema.TryGetType( typeName, out ExpTypeSource? ret ) ) {
                return ret;
            }
            if( TypeDict.TryGetValue( typeName, out ret ) ) {
                return ret;
            }
            foreach( GeneratedExpSchema item in SchemaInclude.Values ) {
                if( item.TypeDict.TryGetValue( typeName, out ret ) ) {
                    return ret;
                }
            }
            throw new ExpTypeNotFoundException( typeName, this );
        }

        public bool HasSchemaInclude( GeneratedExpSchema schema )
        {
            return SchemaInclude.ContainsKey( schema.Name );
        }

        public override void LoadFromSource( ExpSchemaSource source )
        {
            throw new NotImplementedException();
        }

        public override bool TryGetConst(
                                     string name,
                                     out ExpElementInstanceBase c )
        {
            return ConstDict.TryGetValue( name, out c );
        }

        public override bool TryGetEntity( string name, out ExpEntitySource e )
        {
            return EntityDict.TryGetValue( name, out e );
        }

        public override bool TryGetFunction(
                                     string name,
                                     out ExpFunctionSource f )
        {
            return FunctionDict.TryGetValue( name, out f );
        }

        public override bool TryGetRule( string name, out ExpRuleSource r )
        {
            throw new NotImplementedException();
        }

        public override bool TryGetSchemaInclude(
                                     string name,
                                     out ExpSchema include )
        {
            throw new NotImplementedException();
        }

        public override bool TryGetType(
                                     string name,
                                     out ExpTypeSource element )
        {
            throw new NotImplementedException();
        }

    }
}
