﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpSyntaxAnalyzer.DepthEnter.cs"
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
using Microsoft.CodeAnalysis.Diagnostics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExpKeyword = StgSharp.Script.Express.ExpressCompile.Keyword;

namespace StgSharp.Script.Express
{
    public partial class ExpSyntaxAnalyzer
    {

        internal void AttemptEnterNewDepth( Token t )
        {
            switch( t.Value )
            {
                case ExpKeyword.If:
                    _cache.IncreaseDepth( ( int )ExpCompileStateCode.IfBranch );
                    return;
                case ExpKeyword.Case:
                    _cache.IncreaseDepth( ( int )ExpCompileStateCode.CaseBranch );
                    return;
                case ExpKeyword.Repeat:
                    _cache.IncreaseDepth( ( int )ExpCompileStateCode.RepeatLoop );
                    return;
                case ExpKeyword.While:
                    _cache.IncreaseDepth( ( int )ExpCompileStateCode.UntilLoop );
                    return;

                default:
                    break;
            }
        }

    }
}
