//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ExpExceptions.cs"
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
using System.Text;

namespace StgSharp.Script.Express
{
    public class ExpCompileNotInitializedException : InvalidOperationException
    {

        public ExpCompileNotInitializedException()
            : base(
            "Attempt to compile an EXPRESS script before the init of compiler." ) { }

    }

    public class ExpInvalidCharException : Exception
    {

        public ExpInvalidCharException( char c )
            : base( $"An unknown char {c} occurred." ) { }

    }

    public class ExpInvalidTokenException : Exception
    {

        public ExpInvalidTokenException( int index )
            : base( $"Cannot read a token from line at {index}" ) { }

    }

    public class ExpInvalidTypeDeclareEndingExceptions : Exception
    {

        public ExpInvalidTypeDeclareEndingExceptions(
                       string declaredName,
                       string endedName )
            : base(
            $"The datatype is declared with name {declaredName} but ended with {endedName}" ) { }

    }

    public class ExpInvalidElementNameException : Exception
    {

        public ExpInvalidElementNameException(
                       string elementName,
                       ExpSchema schema )
            : base(
            $"Cannot find any element named {elementName} in {schema.Name}" ) { }

    }

    public class ExpInvalidTypeException : InvalidCastException
    {

        public ExpInvalidTypeException( string required, string provided )
            : base(
            $"Type {required} is required at position, but a {provided} instance is provided." ) { }

    }

    public class ExpInvalidCollectionMemberTypeException : InvalidCastException
    {

        public ExpInvalidCollectionMemberTypeException(
                       ExpElementInstanceBase elementInstance,
                       ExpCollectionBase collection )
            : base(
            $"Attempt to add or remove an element of type: {elementInstance.ElementType} from a collection collects {collection.MemberType}" ) { }

    }

    public class ExpInvalidSchemaIncludeException : InvalidOperationException
    {

        public ExpInvalidSchemaIncludeException(
                       ExpSchema context,
                       ExpSchema source )
            : base(
            $"Attempted to reference an EXPRESS language expression in the current schema context '{context.Name}', " + $"but this expression comes from another schema '{source.Name}' that has not been included." ) { }

    }

    public class ExpCaseNotFoundException : ArgumentOutOfRangeException
    {

        public ExpCaseNotFoundException(
                       ExpSwitchNode token,
                       ExpBaseNode label )
            : base(
            $"Attempt to read or write the label {label.Name} not exist in SWITCH expression {token.Name}" ) { }

    }

    public class ExpTypeNotFoundException : Exception
    {

        public ExpTypeNotFoundException( string typeName, ExpSchema context )
            : base(
            $"Cannot find the type {typeName} in schema {context.Name} and its included schemas" ) { }

    }
}
