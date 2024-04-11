﻿//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="LogicValue.cs"
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Logic
{
    /// <summary>
    /// A pair of <see cref="bool"/> value
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct Bool2
    {

        /// <summary>
        /// The first <see cref="bool"/> value.
        /// </summary>
        [FieldOffset(0)] public bool bool0;

        /// <summary>
        /// The second <see cref="bool"/> value.
        /// </summary>
        [FieldOffset(1)] public bool bool1;

        [FieldOffset(0)] public byte byte0;

        [FieldOffset(1)] public bool byte1;

        /// <summary>
        /// A <see cref="ushort"/> value.
        /// The lower byte is <see cref="bool0"/>,
        /// and higher byte is <see cref="bool1"/>.
        /// </summary>
        [FieldOffset(0)] public ushort Value;

    }
}