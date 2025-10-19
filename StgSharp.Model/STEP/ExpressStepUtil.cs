//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ExpressStepUtil"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
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
using CommunityToolkit.HighPerformance.Buffers;

using StgSharp.Model.Step;
using StgSharp.Script;
using StgSharp.Script.Express;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StgSharp.Model.Step
{
    public class ExpressStepUtil : ExpSchema
    {

        public const string AdvancedFaceText = "ADVANCED_FACE";
        public const string Axis2Placement2DText = "AXIS2_PLACEMENT_2D";
        public const string Axis2Placement3DText = "AXIS2_PLACEMENT_3D";
        public const string BSplineCurveWithKnotsText = "B_SPLINE_CURVE_WITH_KNOTS";
        public const string CartesianPointText = "CARTESIAN_POINT";
        public const string CircleText = "CIRCLE";
        public const string CylindricalSurfaceText = "CYLINDRICAL_SURFACE";
        public const string DirectionText = "DIRECTION";
        public const string EdgeCurveText = "EDGE_CURVE";
        public const string EdgeLoopText = "EDGE_LOOP";
        public const string EllipseText = "ELLIPSE";
        public const string FaceBoundText = "FACE_BOUND";
        public const string FaceOuterBoundText = "FACE_OUTER_BOUND";
        public const string LineText = "LINE";
        public const string OrientedEdgeText = "ORIENTED_EDGE";
        public const string PlaneText = "PLANE";
        public const string RepresentationItemText = "REPRESENTATION_ITEM";
        public const string VectorText = "VECTOR";
        public const string VertexPointText = "VERTEX_POINT";

        private static
            HashSet<string> names = [ AdvancedFaceText, Axis2Placement2DText, Axis2Placement3DText, BSplineCurveWithKnotsText, CartesianPointText, CircleText, CylindricalSurfaceText, DirectionText, EdgeCurveText, EdgeLoopText, EllipseText, FaceBoundText, FaceOuterBoundText, LineText, OrientedEdgeText, PlaneText, RepresentationItemText, VectorText, VertexPointText, ];

        private FrozenDictionary<string, ExpEntitySource> entitiesDefine;
        private StringPool _entityNamePool = new StringPool();

        public ExpressStepUtil()
        {
            IEnumerable<KeyValuePair<string, ExpEntitySource>> defineSource = [];
            entitiesDefine = new Dictionary<string, ExpEntitySource>(defineSource)
                .ToFrozenDictionary();
            foreach (string item in names) {
                _entityNamePool.Add(item);
            }
        }

        internal StepModel Current { get; set; }

        public override void LoadFromSource(ExpSchemaSource source)
        {
            return;
        }

        public override bool TryGetConst(string name, out ExpElementInstance c)
        {
            c = null!;
            return false;
        }

        public override bool TryGetEntity(string name, out ExpEntitySource e)
        {
            return entitiesDefine.TryGetValue(name, out e!);
        }

        public override bool TryGetFunction(string name, out ExpFunctionSource f)
        {
            f = null!;
            return false;
        }

        public override bool TryGetProcedure(string name, out ExpProcedureSource p)
        {
            p = null!;
            return false;
        }

        public override bool TryGetRule(string name, out ExpRuleSource r)
        {
            r = null!;
            return false;
        }

        public override bool TryGetSchemaInclude(string name, out ExpSchema include)
        {
            include = null!;
            return false;
        }

        public override bool TryGetType(string name, out ExpTypeSource t)
        {
            t = null!;
            return false;
        }

    }
}