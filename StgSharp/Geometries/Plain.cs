using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Geometries
{
    public unsafe class Plain
    {
        internal vec3d origin;
        internal vec3d vec1;
        internal vec3d vec2;

        internal vec3d plainParameter;
        internal readonly float d = -1;

        public Plain(vec3d o, vec3d v1, vec3d v2)
        {
            this.origin = o;
            this.vec1 = v1 - v2 * (v1.Y / v2.Y);   //计算平面在xz平面上的方向投影
            this.vec2 = v2 - vec1 * (v2.X / v1.X); //计算平面在yz平面上的方向投影

            //|origin|-k1*|vec1|-k2*|vec2|计算plainParameter.Z；
            plainParameter.Z = origin.Z - vec1.Z * (origin.X / vec1.X) - vec2.Z * (origin.Y / vec2.Y);
            //根据plainParameter.Z和对应的方向矢量计算plainParameter.X和plainParameter.Y
            plainParameter.X = vec1.X * (vec1.Z / plainParameter.Z);
            plainParameter.Y = vec2.Y * (vec2.Z / plainParameter.Z);

            plainParameter.X = 1 / plainParameter.X;
            plainParameter.Y = 1 / plainParameter.Y;
            plainParameter.Z = 1 / plainParameter.Z;
        }

        public Plain(Point p1, Point p2, Point p3)
        {
            vec3d origin = p1._position;
            vec3d v1 = p2._position - origin;
            vec3d v2 = p3._position - origin;


            vec1 = v1 - v2 * (v1.Y / v2.Y);   //计算平面在xz平面上的方向投影
            vec2 = v2 - vec1 * (v2.X / v1.X); //计算平面在yz平面上的方向投影

            //|origin|-k1*|vec1|-k2*|vec2|计算plainParameter.Z；
            plainParameter.Z = origin.Z - vec1.Z * (origin.X / vec1.X) - vec2.Z * (origin.Y / vec2.Y);
            //根据plainParameter.Z和对应的方向矢量计算plainParameter.X和plainParameter.Y
            plainParameter.X = vec1.X * (vec1.Z / plainParameter.Z);
            plainParameter.Y = vec2.Y * (vec2.Z / plainParameter.Z);

            plainParameter.X = 1 / plainParameter.X;
            plainParameter.Y = 1 / plainParameter.Y;
            plainParameter.Z = 1 / plainParameter.Z;
        }

    }}
