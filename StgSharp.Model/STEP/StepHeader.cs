//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="STEPHeader.cs"
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
using System.Text;

namespace StgSharp.Modeling.Step
{
    /// <summary>
    /// C# implementation of HEADER part in STEP file.
    /// </summary>
    public struct StepInfo
    {

        private STEPFileDescriptor _header;
        private STEPFileName _name;
        private STEPSchema _schema;

        public (int mainLevel, int? subLevel) Level
        {
            get => _header.Level;
            internal set => _header.Level = value;
        }

        public DateTime TimeStamp
        {
            get => _name.TimeStamp;
            internal set => _name.TimeStamp = value;
        }

        public STEPFileDescriptor Descriptor
        {
            get => _header;
            internal set => _header = value;
        }

        public STEPFileName FileName
        {
            get => _name;
            internal set => _name = value;
        }

        public STEPSchema Schema
        {
            get => _schema;
            internal set => _schema = value;
        }

        public string Description
        {
            get => _header.Description;
            internal set => _header.Description = value;
        }

        public string Name
        {
            get => _name.Name;
            internal set => _name.Name = value;
        }

        public string Author
        {
            get => _name.Author;
            internal set => _name.Author = value;
        }

        public string Organization
        {
            get => _name.Organization;
            internal set => _name.Organization = value;
        }

        public string OriginatingSystem
        {
            get => _name.OriginatingSystem;
            internal set => _name.OriginatingSystem = value;
        }

        public string Authorization
        {
            get => _name.Authorization;
            internal set => _name.Authorization = value;
        }

        public string PreprocessorVersion
        {
            get => _name.PreprocessorVersion;
            internal set => _name.PreprocessorVersion = value;
        }

        public string Version
        {
            get => _name.Version;
            internal set => _name.Version = value;
        }

        public string SchemaString
        {
            get => _schema.Data;
            internal set => _schema.Data = value;
        }

    }

    /// <summary>
    /// C# implementation of FILE_NAME part in header of STEP file.
    /// </summary>
    public struct STEPFileName
    {

        public DateTime TimeStamp
        {
            get;
            internal set;
        }

        public string Name
        {
            get;
            internal set;
        }

        public string Author
        {
            get;
            internal set;
        }

        public string Description
        {
            get;
            internal set;
        }

        public string PreprocessorVersion
        {
            get;
            internal set;
        }

        public string Version
        {
            get;
            internal set;
        }

        public string Organization
        {
            get;
            internal set;
        }

        public string OriginatingSystem
        {
            get;
            internal set;
        }

        public string Authorization
        {
            get;
            internal set;
        }

    }

    public struct STEPSchema
    {

        private List<string> _schema;

        public string Data
        {
            get;
            internal set;
        }

    }

    /// <summary>
    /// C# implementation of FILE_NAME part in header of STEP file.
    /// </summary>
    public struct STEPFileDescriptor
    {

        public (int mainLevel,int? subLevel) Level
        {
            get;
            internal set;
        }

        public string Description
        {
            get;
            internal set;
        }

    }
}
