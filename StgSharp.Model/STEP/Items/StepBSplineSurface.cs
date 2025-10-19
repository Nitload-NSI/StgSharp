//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepBSplineSurface"
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
using CommunityToolkit.HighPerformance;

using StgSharp.Script.Express;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Model.Step
{
    public enum StepBSplineSurfaceForm
    {

        PLANE_SURF,
        CYLINDRICAL_SURF,
        CONICAL_SURF,
        SPHERICAL_SURF,
        TOROIDAL_SURF,
        SURF_OF_REVOLUTION,
        RULED_SURF,
        GENERALISED_CONE,
        QUADRIC_SURF,
        SURF_OF_LINEAR_EXTRUSION,
        UNSPECIFIED,

    }

    public class StepBSplineSurface : StepBoundedSurface, IExpConvertableFrom<StepBSplineSurface>
    {

        public const string CONICAL_SURF_TEXT = "CONICAL_SURF";
        public const string CYLINDRICAL_SURF_TEXT = "CYLINDRICAL_SURF";
        public const string GENERALISED_CONE_TEXT = "GENERALISED_CONE";
        public const string PLANE_SURF_TEXT = "PLANE_SURF";
        public const string QUADRIC_SURF_TEXT = "QUADRIC_SURF";
        public const string RULED_SURF_TEXT = "RULED_SURF";
        public const string SPHERICAL_SURF_TEXT = "SPHERICAL_SURF";
        public const string SURF_OF_LINEAR_EXTRUSION_TEXT = "SURF_OF_LINEAR_EXTRUSION";
        public const string SURF_OF_REVOLUTION_TEXT = "SURF_OF_REVOLUTION";
        public const string TOROIDAL_SURF_TEXT = "TOROIDAL_SURF";
        public const string UNSPECIFIED_TEXT = "UNSPECIFIED";

        public StepBSplineSurface(StepModel model) : base(model) { }

        public bool UClosed { get; set; }

        public bool VClosed { get; set; }

        public bool SelfIntersections { get; set; }

        public int UDegree { get; set; }

        public int VDegree { get; set; }

        public int UUpper => ControlPointsList.Count - 1;

        public int VUpper => ControlPointsList[0].Count - 1;

        public List<List<StepCartesianPoint>> ControlPointsList { get; set; }
            = new List<List<StepCartesianPoint>>();

        public StepBSplineSurfaceForm SurfaceForm { get; set; }

        public static StepBSplineSurface CreateFromSyntax(StepModel binder, ExpSyntaxNode syntax)
        {
            ExpNodePresidentEnumerator enumerator = syntax.ToPresidentEnumerator();
            enumerator.AssertEnumeratorCount(7);
            StepBSplineSurface ret = new StepBSplineSurface(binder);

            ret.UDegree = (enumerator[0] as ExpIntNode).Value;
            ret.VDegree = (enumerator[1] as ExpIntNode).Value;

            ExpNodePresidentEnumerator pointList = enumerator[2].TupleToPresidentEnumerator();
            pointList.AssertEnumeratorCount(2, int.MaxValue);
            for (int i = 0; i < pointList.Count; i++)
            {
                ExpNodePresidentEnumerator points = pointList[i].TupleToPresidentEnumerator();
                points.AssertEnumeratorCount(2, int.MaxValue);
                List<StepCartesianPoint> pList = new List<StepCartesianPoint>(points.Count);
                CollectionsMarshal.SetCount(pList, points.Count);
                Span<StepCartesianPoint> ls = CollectionsMarshal.AsSpan(pList);
                Span<ExpSyntaxNode> aspan = points.AsSpan();
                for (int j = 0; j < points.Count; j++)
                {
                    StepCartesianPoint p = binder[aspan[i] as StepEntityInstanceNode] as StepCartesianPoint;
                    ls[j] = p;
                }
                ret.ControlPointsList.Add(pList);
            }

            ret.SurfaceForm = GetSurfaceForm(enumerator[3].CodeConvertTemplate);
            ret.UClosed = (enumerator[4] as ExpBoolNode).Value;
            ret.VClosed = (enumerator[5] as ExpBoolNode).Value;
            ret.SelfIntersections = (enumerator[6] as ExpBoolNode).Value;
            return ret;
        }

        public void FromInstance(StepBSplineSurface entity)
        {
            base.FromInstance(entity);
            UClosed = entity.UClosed;
            VClosed = entity.VClosed;
            UDegree = entity.UDegree;
            VDegree = entity.VDegree;
            ControlPointsList.Clear();
            foreach (List<StepCartesianPoint> list in entity.ControlPointsList) {
                ControlPointsList.Add(new List<StepCartesianPoint>(list));
            }
            SurfaceForm = entity.SurfaceForm;
        }

        protected static StepBSplineSurfaceForm GetSurfaceForm(string literal)
        {
            return literal switch
            {
                CONICAL_SURF_TEXT => StepBSplineSurfaceForm.PLANE_SURF,
                CYLINDRICAL_SURF_TEXT => StepBSplineSurfaceForm.CYLINDRICAL_SURF,
                GENERALISED_CONE_TEXT => StepBSplineSurfaceForm.CONICAL_SURF,
                PLANE_SURF_TEXT => StepBSplineSurfaceForm.SPHERICAL_SURF,
                QUADRIC_SURF_TEXT => StepBSplineSurfaceForm.TOROIDAL_SURF,
                RULED_SURF_TEXT => StepBSplineSurfaceForm.SURF_OF_REVOLUTION,
                SPHERICAL_SURF_TEXT => StepBSplineSurfaceForm.RULED_SURF,
                SURF_OF_LINEAR_EXTRUSION_TEXT => StepBSplineSurfaceForm.GENERALISED_CONE,
                SURF_OF_REVOLUTION_TEXT => StepBSplineSurfaceForm.QUADRIC_SURF,
                TOROIDAL_SURF_TEXT => StepBSplineSurfaceForm.SURF_OF_LINEAR_EXTRUSION,
                UNSPECIFIED_TEXT => StepBSplineSurfaceForm.UNSPECIFIED,
                _ => StepBSplineSurfaceForm.UNSPECIFIED,
            };
        }

        protected static string GetSurfaceFormString(StepBSplineSurfaceForm form)
        {
            return form switch
            {
                StepBSplineSurfaceForm.PLANE_SURF => CONICAL_SURF_TEXT,
                StepBSplineSurfaceForm.CYLINDRICAL_SURF => CYLINDRICAL_SURF_TEXT,
                StepBSplineSurfaceForm.CONICAL_SURF => GENERALISED_CONE_TEXT,
                StepBSplineSurfaceForm.SPHERICAL_SURF => PLANE_SURF_TEXT,
                StepBSplineSurfaceForm.TOROIDAL_SURF => QUADRIC_SURF_TEXT,
                StepBSplineSurfaceForm.SURF_OF_REVOLUTION => RULED_SURF_TEXT,
                StepBSplineSurfaceForm.RULED_SURF => SPHERICAL_SURF_TEXT,
                StepBSplineSurfaceForm.GENERALISED_CONE => SURF_OF_LINEAR_EXTRUSION_TEXT,
                StepBSplineSurfaceForm.QUADRIC_SURF => SURF_OF_REVOLUTION_TEXT,
                StepBSplineSurfaceForm.SURF_OF_LINEAR_EXTRUSION => TOROIDAL_SURF_TEXT,
                StepBSplineSurfaceForm.UNSPECIFIED => UNSPECIFIED_TEXT,
                _ => UNSPECIFIED_TEXT
            };
        }

    }
}