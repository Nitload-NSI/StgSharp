using StgSharp.Controls;
using StgSharp.Geometries;
using StgSharp.Graphics;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StgSharp.Controls
{
    public class MarkingLabel : ControllingItem
    {
        public ReadOnlySpan<vec4d> TextureBox 
        { 
            get => marker.TextureCoord;
        }
        public vec2d Position { get; set; }
        public Rectangle BoundingBox { get; set; }

        public bool IsEntity => true;

        private vec2d _currentPosition, _movementVec;

        private TimeFunction _movement;
        private PlainGeometryMesh marker;

        public void MoveTo(vec2d target)
        {
            Move(target - Position);
        }

        public void Move(vec2d movement)
        {
            if (!_movement.IsComplete)
            {
                Position = _currentPosition + _movement.Calculate() * _movementVec;
            }
        }

        public MarkingLabel(PlainGeometryMesh marker, TimeFunction movement)
        {
            this.marker = marker;
            _movement = movement;
        }

        public IEnumerator<PlainGeometryMesh> GetEnumerator()
        {
            yield return marker;
        }
    }

}
