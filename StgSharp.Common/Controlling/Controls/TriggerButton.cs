//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="TriggerButton"
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
using StgSharp.Controls;
using StgSharp.Geometries;
using StgSharp.Graphics;
using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphic;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Controls
{
    public delegate void RenderButtonTextureCallback(ButtonStatus status, float beginTime, in TextureProvider texture);

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

    public class TriggerButton : ControllingItem
    {

        private Action? confirmCallback;
        private Action? disableCallback;

        #nullable enable
        private Action? enableCallback;
        private float _statusBeginTime;

        private RenderButtonTextureCallback _renderButton;
        private TextureProvider _texture;

        #nullable restore

        public TriggerButton()
        {
            CurrentStatus = ButtonStatus.Disable;
        }

        public event Action ConfirmEvent
        {
            add { confirmCallback += value; }
            remove { confirmCallback -= value; }
        }

        public event Action DisableEvent
        {
            add { disableCallback += value; }
            remove { disableCallback -= value; }
        }

        public event Action EnableEvent
        {
            add { enableCallback += value; }
            remove { enableCallback -= value; }
        }

        public event RenderButtonTextureCallback RenderButtonTexture
        {
            add
            {
                if (_renderButton == null)
                {
                    _renderButton = value;
                } else
                {
                    _renderButton += value;
                }
            }
            remove
            {
                if (_renderButton != null) {
                    _renderButton -= value;
                }
            }
        }

        public bool IsHighLighted { get; internal set; }

        public bool IsDisabled { get; internal set; }

        public bool IsEntity => true;

        public ReadOnlySpan<Vec4> TextureBox
        {
            get => MemoryMarshal.Cast<Vec2, Vec4>(_texture.TextureCoordinate);
        }

        public Rectangle BoundingBox { get; set; }

        public TriggerButton Left { get; internal set; }

        public TriggerButton Right { get; internal set; }

        public TriggerButton Up { get; internal set; }

        public TriggerButton Down { get; internal set; }

        public Vec2 Position { get; set; }

        internal TextureProvider Texture
        {
            get => _texture;
            set => _texture = value;
        }

        protected ButtonStatus CurrentStatus { get; set; }

        public void DeHighlight()
        {
            if (CurrentStatus == ButtonStatus.Highlight) {
                CurrentStatus = ButtonStatus.HighlightToEnable;
            }
        }

        public void Disable()
        {
            CurrentStatus = CurrentStatus switch
            {
                ButtonStatus.Enable => ButtonStatus.EnableToDisable,
                ButtonStatus.Highlight => ButtonStatus.HighlightToDisable,
                _ => CurrentStatus,
            };
            if (disableCallback != null) {
                disableCallback();
            }
        }

        public void Enable()
        {
            if (CurrentStatus == ButtonStatus.Disable)
            {
                CurrentStatus = ButtonStatus.DisableToEnable;
            } else
            {
                return;
            }
            if (enableCallback != null) {
                enableCallback();
            }
        }

        public IEnumerator<PlainGeometryMesh> GetEnumerator()
        {
            _renderButton(CurrentStatus, _statusBeginTime, in _texture);
            yield return new PlainGeometryMesh(this.BoundingBox, _texture);
        }

        public void Highlight()
        {
            if (CurrentStatus == ButtonStatus.Enable)
            {
                CurrentStatus |= ButtonStatus.EnableToHighlight;
            } else
            {
                return;
            }
            if (confirmCallback != null) {
                confirmCallback();
            }
        }

        internal void Init(ButtonStatus status)
        {
            if (CurrentStatus == ButtonStatus.Error) {
                CurrentStatus = status;
            }
        }

    }//----------------------------------- End of class --------------------------------------------
}
