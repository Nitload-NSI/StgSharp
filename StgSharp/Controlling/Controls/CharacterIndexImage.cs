using StgSharp.Geometries;
using StgSharp.Graphics;
using StgSharp.Math;

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
        private int length;
        private char[] displayIndex;
        private char defaultChar;

        Dictionary<char, PlainGeometryMesh> texIndex;
        PlainGeometryMesh _tittleMesh;

        public bool IsEntity
        {
            get => true;
        }

        public ReadOnlySpan<vec4d> TextureBox
        {
            get => throw new NotSupportedException();
        }
        public vec2d Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Rectangle BoundingBox { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Display(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = string.Empty;
            }

            string result = message.Length >= length ? 
                message.Substring(message.Length - displayIndex.Length) : 
                message.PadLeft(length,defaultChar);

            displayIndex = result.ToCharArray();
        }

        public IEnumerator<PlainGeometryMesh> GetEnumerator()
        {
            yield return _tittleMesh;
            foreach (char item in displayIndex)
            {
                yield return texIndex[item];
            }
        }

        public CharacterLineIndexImage(
            int length, char defaultChar, PlainGeometryMesh tittleMesh,
            IEnumerable<(char, PlainGeometryMesh)> indexEnumeration)
        {
            this.length = length;
            displayIndex = new char[length];
            _tittleMesh = tittleMesh;
            texIndex = indexEnumeration.ToDictionary(item => item.Item1, item => item.Item2);
        }

    }
}
