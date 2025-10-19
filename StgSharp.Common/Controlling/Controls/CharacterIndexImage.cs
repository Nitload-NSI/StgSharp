//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="CharacterIndexImage"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Geometries;
using StgSharp.Graphics;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphic;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace StgSharp.Controls
{
    public class CharacterLineIndexImage : ControllingItem
    {

        private char[] displayIndex;
        private char defaultChar;

        private Dictionary<char, PlainGeometryMesh> texIndex;
        private int length;
        private PlainGeometryMesh _tittleMesh;

        public CharacterLineIndexImage(
               int length,
               char defaultChar,
               PlainGeometryMesh tittleMesh,
               IEnumerable<(char, PlainGeometryMesh)> indexEnumeration)
        {
            this.length = length;
            displayIndex = new char[length];
            _tittleMesh = tittleMesh;
            texIndex = indexEnumeration.ToDictionary(item => item.Item1, item => item.Item2);
        }

        public bool IsEntity
        {
            get => true;
        }

        public ReadOnlySpan<Vec4> TextureBox
        {
            get => throw new NotSupportedException();
        }

        public Rectangle BoundingBox
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public Vec2 Position
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public void Display(string message)
        {
            if (string.IsNullOrEmpty(message)) {
                message = string.Empty;
            }

            string result = (message.Length >= length) ?
                            message.Substring(message.Length - displayIndex.Length) :
                            message.PadLeft(
                        length, defaultChar);

            displayIndex = result.ToCharArray();
        }

        public IEnumerator<PlainGeometryMesh> GetEnumerator()
        {
            yield return _tittleMesh;
            foreach (char item in displayIndex) {
                yield return texIndex[item];
            }
        }

    }
}
