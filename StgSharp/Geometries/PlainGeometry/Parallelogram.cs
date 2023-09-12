using StgSharp.Controlling;
using StgSharp.Math;

namespace StgSharp.Geometries
{
    internal class Parallelogram : Polygon4
    {
        internal Point center;

        public sealed override Point RefPoint01 => center;
        public sealed override Point RefPoint02 => vertex01;
        public sealed override Point RefPoint03 => vertex02;

        internal readonly GetLocationHandler movCenterOperation = new GetLocationHandler(GeometryOperation.DefualtMotion);

        public GetLocationHandler MovCenterOperation { get { return movCenterOperation; } }
        public GetLocationHandler MovVertex01Operation { get { return movVertex01Operation; } }
        public GetLocationHandler MovVertex02Operation { get { return movVertex02Operation; } }

        public virtual Vec3d MovCenter(uint tick)
        {
            return movCenterOperation.Invoke(tick);
        }

        /// <summary>
        /// Move the third vertix. this is a useless method for Parallelograms.
        /// </summary>
        /// <param name="tick">Current time tick</param>
        /// <returns></returns>
        /// <exception cref="UnusedVertexException">Geometry overdefined</exception>
        public sealed override Vec3d MovVertex03(uint tick)
        {
            throw new UnusedVertexException();
        }

        /// <summary>
        /// Move the third vertix. This method is useless for Parallelograms.
        /// </summary>
        /// <param name="tick">Current time tick</param>
        /// <returns></returns>
        /// <exception cref="UnusedVertexException">Geometry overdefined</exception>
        public sealed override Vec3d MovVertex04(uint tick)
        {
            throw new UnusedVertexException();
        }

        internal sealed override void OnRender(uint tick)
        {
            uint nowTick = TimeLine.tickCounter._value;
            this.vertex01.Position = RefOrigin.Position + movVertex01Operation(nowTick);
            this.vertex02.Position = RefOrigin.Position + movVertex02Operation(nowTick);
            this.vertex03.Position = center.Position * 2 - vertex01.Position;
            this.vertex04.Position = center.Position * 2 - vertex02.Position;
        }

    }
}
