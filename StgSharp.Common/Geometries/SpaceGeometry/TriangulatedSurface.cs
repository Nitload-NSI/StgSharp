//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="TriangulatedSurface"
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
using StgSharp.HighPerformance;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text;

namespace StgSharp.Geometries
{
    public class TriangulatedSurface
    {

        private HashSet<(int begin,int end)> _sides;

        private List<List<int>> _sideIndices;
        private List<int> _triangleIndices;
        private List<Vec3> _vertices;

        public TriangulatedSurface()
        {
            _triangleIndices = new List<int>();
            _vertices = new List<Vec3>();
            _sides = new HashSet<(int,int)>();
            _sideIndices = new List<List<int>>();
        }

        /**/
        public IEnumerable<List<int>> SideIndicesEnumeratble
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _sideIndices ;
        }

        /*
        public Triangle GetTriangle( int index )
        {
            Vec3HP v1,v2,v3;
            int t = index * 3;
            v1 = _vertices[ t + 0 ];
            v2 = _vertices[ t + 1 ];
            v3 = _vertices[ t + 2 ];
            return new Triangle( v1, v2, v3 );
        }
        /**/

        public Span<Vec3> VertexSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => CollectionsMarshal.AsSpan(_vertices);
        }

        public Span<int> TriangleIndicesSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => CollectionsMarshal.AsSpan(_triangleIndices);
        }

        /**/
        public void AddIndicesByOrder(int v1, int v2, int v3)
        {
            _triangleIndices.Add(v1);
            _triangleIndices.Add(v2);
            _triangleIndices.Add(v3);
            if (!(_sides.Remove((v1,v2)) || _sides.Remove((v2, v1)))) {
                _ = _sides.Add((v1, v2));
            }
            if (!(_sides.Remove((v2, v3)) || _sides.Remove((v3, v2)))) {
                _ = _sides.Add((v2, v3));
            }
            if (!(_sides.Remove((v3, v1)) || _sides.Remove((v1, v3)))) {
                _ = _sides.Add((v3, v1));
            }
        }

        public void AddVertexByOrder(Vec3 vertex)
        {
            _vertices.Add(vertex);
        }

        public int TriangleCount()
        {
            return _triangleIndices.Count / 3;
        }

        public void TrimExcess()
        {
            _triangleIndices.TrimExcess();
            _vertices.TrimExcess();
            Dictionary<int, int> side_dictionary = _sides.ToDictionary(x => x.begin, x => x.end);
            int begin = side_dictionary.First().Key;
            List<int> sideLoop = new(begin);
            _sideIndices.Add(sideLoop);
            while (side_dictionary.Count > 0)
            {
                if (side_dictionary.Remove(begin, out begin))
                {
                    sideLoop.Add(begin);
                } else
                {
                    begin = side_dictionary.First().Key;
                    sideLoop = new List<int>(begin);
                    _sideIndices.Add(sideLoop);
                }
            }
            _sideIndices.TrimExcess();
        }

        public int VertexCount()
        {
            return _vertices.Count;
        }

    }
}
