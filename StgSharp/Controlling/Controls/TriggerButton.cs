//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="TriggerButton.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using StgSharp.Controls;
using StgSharp.Geometries;
using StgSharp.Graphics;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Controls
{
    public enum ButtonStatus
    {
        Error = 0b0,
        Disable = 0b1,
        Enable = 0b10,
        Highlight = 0b100,
        Confirm = 0b1000,

        DisableToEnable = Disable | Enable,
        EnableToDisable = -(Enable | DisableToEnable),

        EnableToHighlight = Enable | Highlight,
        HighlightToEnable = -(Enable | Highlight),

        HighlightToDisable = -(Highlight | Disable),
    }

    public delegate void RenderButtonTextureCallback(ButtonStatus status, float beginTime, in TextureProvider texture);

    public class TriggerButton : ControllingItem
    {
        private TextureProvider _texture;
        private float _statusBeginTime;

        public TriggerButton Left { get; internal set; }
        public TriggerButton Right { get; internal set; }
        public TriggerButton Up { get; internal set; }
        public TriggerButton Down { get; internal set; }

#nullable enable
        private Action? enableCallback;
        private Action? disableCallback;
        private Action? confirmCallback;
#nullable restore

        public TriggerButton()
        {
            CurrentStatus = ButtonStatus.Disable;
        }

        protected ButtonStatus CurrentStatus { get; set; }

        private RenderButtonTextureCallback _renderButton;

        public event RenderButtonTextureCallback RenderButtonTexture 
        {
            add
            {
                if (_renderButton == null)
                {
                    _renderButton = value;
                }
                else
                {
                    _renderButton += value;
                }
            }
            remove 
            {
                if (_renderButton != null)
                {
                    _renderButton -= value;
                }
            }
        }

        internal TextureProvider Texture
        {
            get => _texture;
            set => _texture = value;
        }

        public ReadOnlySpan<vec4d> TextureBox
        {
            get => MemoryMarshal.Cast<vec2d, vec4d>(_texture.TextureCoordinate); 
        }

        public bool IsHighLighted { get; internal set; }

        public bool IsDisabled { get; internal set; }
        public vec2d Position { get; set; }
        public Rectangle BoundingBox { get; set; }

        public bool IsEntity => true;

        public event Action EnableEvent
        {
            add { enableCallback += value; } 
            remove { enableCallback -= value; }
        }

        public event Action DisableEvent
        {
            add { disableCallback += value; }
            remove { disableCallback -= value; }
        }

        public event Action ConfirmEvent
        {
            add { confirmCallback += value; }
            remove { confirmCallback -= value; }
        }

        internal void Init( ButtonStatus status )
        {
            if (CurrentStatus == ButtonStatus.Error)
            {
                CurrentStatus = status;
            }
        }

        public void Enable()
        {
            if (CurrentStatus == ButtonStatus.Disable)
            {
                CurrentStatus = ButtonStatus.DisableToEnable;
            }
            else
            {
                return;
            }
            if ( enableCallback != null)enableCallback();
        }

        public void Disable()
        {
            CurrentStatus = CurrentStatus switch
            {
                ButtonStatus.Enable => ButtonStatus.EnableToDisable,
                ButtonStatus.Highlight => ButtonStatus.HighlightToDisable,
                _ => CurrentStatus,
            };
            if (disableCallback != null) disableCallback();
        }

        public void Highlight()
        {
            if (CurrentStatus == ButtonStatus.Enable)
            {
                CurrentStatus |= ButtonStatus.EnableToHighlight;
            }
            else
            {
                return;
            }
            if (confirmCallback != null) confirmCallback();
        }

        public void DeHighlight()
        {
            if (CurrentStatus == ButtonStatus.Highlight)
            {
                CurrentStatus = ButtonStatus.HighlightToEnable;
            }
        }

        public IEnumerator<PlainGeometryMesh> GetEnumerator()
        {
            _renderButton(CurrentStatus,_statusBeginTime, in _texture);
            yield return new PlainGeometryMesh(this.BoundingBox, _texture);
        }

    }//----------------------------------- End of class --------------------------------------------
}
