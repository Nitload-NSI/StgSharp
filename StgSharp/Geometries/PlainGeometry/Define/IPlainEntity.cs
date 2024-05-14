using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Geometries
{

    public interface IPlainEntity
    {
        public vec2d CenterPosition { get; set; }

        public bool CollideWith(IPlainEntity entity);

    }

    public class PlainEntity:IPlainEntity
    {
        private PlainGeometry _shape;

        vec2d IPlainEntity.CenterPosition 
        {
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        public PlainEntity(PlainGeometry shape)
        {
            _shape = shape;
        }

        bool IPlainEntity.CollideWith(IPlainEntity entity)
        {
            throw new NotImplementedException();
        }
    }

}
