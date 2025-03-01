﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpElmentSource.cs"
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    public interface IExpElementSource : ITypeSource<IExpElementSource>
    {

        public ExpElementType ElementType
        {
            get;
        }

        IScriptSourceProvider SourceProvider
        {
            get;
        }

        string Name
        {
            get;
        }

        public void Analyse();

    }

    public abstract class ExpInstantiableElementBase : IExpElementSource
    {

        public abstract ExpElementType ElementType
        {
            get;
        }

        public abstract IScriptSourceProvider SourceProvider
        {
            get;
        }

        public abstract string Name
        {
            get;
        }

        public abstract void Analyse();

        public abstract ExpBaseNode CreateInstanceToken();

        public abstract bool IsConvertable( IExpElementSource targetType );

    }

    public abstract class ExpImmutableElementBase : IExpElementSource
    {

        public abstract ExpElementType ElementType
        {
            get;
        }

        public abstract IScriptSourceProvider SourceProvider
        {
            get;
        }

        public abstract string Name
        {
            get;
        }

        public abstract void Analyse();

        public bool IsConvertable( IExpElementSource targetType )
        {
            return false;
        }

        public abstract ExpBaseNode MakeReference( params object[] options );

    }
}
