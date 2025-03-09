//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpElementInstanceBase.cs"
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
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace StgSharp.Script.Express
{
    [Flags]
    public enum ExpElementType : int
    {

        Void = 0,
        Const = 1,
        Entity = 2,
        Type = 4,
        Function = 8,
        Section = 16,
        Expression = 32,
        Rule = 64,
        Procedure = 128,
        Collection = 256,

    }

    public abstract class ExpElementInstanceBase : ExpNode
    {

        protected ExpElementInstanceBase( Token source ) : base( source ) { }

        public ExpElementType ElementType
        {
            get;
        }

        public override ExpNode Left => Empty;

        public override ExpNode Right => Empty;

        public abstract string TypeName
        {
            get;
        }

        public static ExpElementInstanceBase CreateLiteral( Token t )
        {
            if( int.TryParse( t.Value, out int _int ) ) {
                return new ExpIntNode( t, _int );
            } else if( float.TryParse( t.Value, out float _float ) ) {
                return new ExpRealNumberNode( t, _float );
            } else if( bool.TryParse( t.Value, out bool _bool ) ) {
                return new ExpBoolNode( t, _bool );
            } else if( ExpLogic.TryParse( t.Value, out ExpLogic _logic ) ) {
                return new ExpLogicValueNode( t, _logic );
            } else {
                return new ExpStringNode( t, t.Value );
            }
        }

    }
}
