using StgSharp.Controlling;
using StgSharp.Math;

namespace StgSharp.Geometries
{
    public class Parallelogram : Polygon4
    {
        internal Point center;

        public sealed override Point RefPoint0 => center;
        public sealed override Point RefPoint1 => new Point(vertexMat.mat.colum0);
        public sealed override Point RefPoint2 => new Point(vertexMat.mat.colum1);

        internal readonly GetLocationHandler movCenterOperation =
            new GetLocationHandler(GeometryOperation.DefualtMotion);

        public GetLocationHandler MovCenterOperation { get { return movCenterOperation; } }
        public GetLocationHandler MovVertex01Operation { get { return moveVertex0Operation; } }
        public GetLocationHandler MovVertex02Operation { get { return moveVertex1Operation; } }

        public Parallelogram(
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z,
            float v3x, float v3y, float v3z
            ) : base(
                v0x, v0y, v0z,
                v1x, v1y, v1z,
                v2x, v2y, v2z,
                v3x, v2y, v3z
                )
        {
            if (
                vertexMat.Colum0 + vertexMat.Colum2 !=
                vertexMat.Colum1 + vertexMat.Colum3
                )
            {
                InternalIO.InternalWriteLog("Init of geometry item failed, because four vertices cannot form a rectangle.", LogType.Warning);
            }
        }

        public virtual vec3d MovCenter(uint tick)
        {
            return movCenterOperation.Invoke(tick);
        }

        /// <summary>
        /// Move the third vertix. this is a useless method for Parallelograms.
        /// </summary>
        /// <param name="tick">Current time tick</param>
        /// <returns></returns>
        /// <exception cref="UnusedVertexException">Geometry overdefined</exception>
        public sealed override vec3d MoveVertex2(uint tick)
        {
            throw new UnusedVertexException();
        }

        /// <summary>
        /// Move the third vertix. This method is useless for Parallelograms.
        /// </summary>
        /// <param name="tick">Current time tick</param>
        /// <returns></returns>
        /// <exception cref="UnusedVertexException">Geometry overdefined</exception>
        public sealed override vec3d MoveVertex3(uint tick)
        {
            throw new UnusedVertexException();
        }

        internal sealed override void OnRender(uint tick)
        {
            uint nowTick = TimeLine.tickCounter._value;
            this.vertexMat.Colum0 = RefOrigin.Position + moveVertex0Operation(nowTick);
            this.vertexMat.Colum1 = RefOrigin.Position + moveVertex1Operation(nowTick);
            this.vertexMat.Colum2 = center.Position * 2 - vertexMat.Colum0;
            this.vertexMat.Colum3 = center.Position * 2 - vertexMat.Colum1;
        }

    }
}
