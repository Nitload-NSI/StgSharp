//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="ViewDesigner"
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
using StgSharp.Controlling.UsrActivity;
using StgSharp.Controls;
using StgSharp.Geometries;
using StgSharp.Graphics;

using StgSharp.Mathematics;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace StgSharp.MVVM
{
    public abstract partial class ViewBase
    {

        public interface IViewDesigner<out TView> where TView: ViewBase
        {

            abstract void CustomizedInitialize();

            abstract void Initialize();

            abstract bool ValidateInitializing();

        }

        /// <summary>
        ///   ViewDesigner provides descriptions of various controls in the view,  binds the
        ///   controls to specific user actions and behaviors,  and describes the layout and
        ///   interactions of each control.
        /// </summary>
        public abstract class ViewDesigner<TView> : IViewDesigner<TView> where TView: ViewBase
        {

            private KeyboardButtonGroup currentButtonGroup;

            private List<ControllingItem> _controlList;
            protected TView _binding;

            public ViewDesigner(TView binding)
            {
                _binding = binding;
                _controlList = new List<ControllingItem>();
            }

            protected TView Binding
            {
                get => _binding;
            }

            public CharacterLineIndexImage CreateCharacterLine(
                                           int length,
                                           char blankFilling,
                                           PlainGeometryMesh tittle,
                                           IEnumerable<(char, PlainGeometryMesh)> charList)
            {
                CharacterLineIndexImage ret = new CharacterLineIndexImage(
                    length, blankFilling, tittle, charList);
                _controlList.Add(ret);
                return ret;
            }

            public TriggerButton CreateKeyboardButton(
                                 Rectangle bonding,
                                 TextureProvider defaultAnimation,
                                 bool isEnable)
            {
                TriggerButton ret = new TriggerButton();
                ret.BoundingBox = bonding;
                ret.Texture = defaultAnimation;
                if (isEnable)
                {
                    ret.IsDisabled = false;
                    ret.Enable();
                } else
                {
                    ret.IsDisabled = true;
                    ret.Disable();
                }
                currentButtonGroup.AddButton(ret);
                return ret;
            }

            public TButtonGroup CreateKeyboardButtonGroup<TButtonGroup>(
                                IClickTrigger leftBidning,
                                IClickTrigger rightBinding,
                                IClickTrigger upBinding,
                                IClickTrigger downBinding,
                                IClickTrigger confirmBinding)
                where TButtonGroup: KeyboardButtonGroup, new()
            {
                TButtonGroup ret = new TButtonGroup();
                _controlList.Add(ret);
                currentButtonGroup = ret;
                _binding.Responder[leftBidning] = ret.MoveLeft;
                _binding.Responder[rightBinding] = ret.MoveRight;
                _binding.Responder[upBinding] = ret.MoveUp;
                _binding.Responder[downBinding] = ret.MoveDown;
                _binding.Responder[confirmBinding] = ret.Confirm;
                return ret;
            }

            public abstract void CustomizedInitialize();

            public void Initialize()
            {
                CustomizedInitialize();
            }

            public abstract bool ValidateInitializing();

        }//---------------------------------- End of Class -----------------------------------------

    }
}
