//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ButtonGroup.cs"
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
using StgSharp.Geometries;
using StgSharp.Graphics;
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;

namespace StgSharp.Controls
{
    public abstract class KeyboardButtonGroup : ControllingItem
    {

        private List<TriggerButton> buttonList;
        private TriggerButton current;

        public KeyboardButtonGroup()
        {
            buttonList = new List<TriggerButton>();
        }

        public bool IsEntity
        {
            get => false;
        }

        public ReadOnlySpan<Vec4> TextureBox
        {
            get => throw new NotSupportedException();
        }

        public Rectangle BoundingBox
        {
            get;
            set;
        }

        public TriggerButton CurrentButton
        {
            get => current;
        }

        public Vec2 Position
        {
            get;
            set;
        }

        protected List<TriggerButton> AllButton
        {
            get => buttonList;
        }

        public void AddButton( TriggerButton button )
        {
            buttonList.Add( button );
        }

        public abstract void Confirm();

        public IEnumerator<PlainGeometryMesh> GetEnumerator()
        {
            foreach( TriggerButton item in buttonList ) {
                yield return item.Single();
            }
        }

        public void MoveDown()
        {
            TriggerButton button = current;
            while( ( button.Down != null ) && button.Down.IsDisabled ) {
                button = button.Right;
            }
            if( current == button ) {
                return;
            }
            current.DeHighlight();
            button.Enable();
            current = button;
        }

        public void MoveLeft()
        {
            TriggerButton button = current;
            while( ( button.Left != null ) && button.Left.IsDisabled ) {
                button = button.Left;
            }
            if( current == button ) {
                return;
            }
            current.DeHighlight();
            button.Enable();
            current = button;
        }

        public void MoveRight()
        {
            TriggerButton button = current;
            while( ( button.Right != null ) && button.Right.IsDisabled ) {
                button = button.Right;
            }
            if( current == button ) {
                return;
            }
            current.DeHighlight();
            button.Enable();
            current = button;
        }

        public void MoveUp()
        {
            TriggerButton button = current;
            while( ( button.Up != null ) && button.Up.IsDisabled ) {
                button = button.Right;
            }
            if( current == button ) {
                return;
            }
            current.DeHighlight();
            button.Enable();
            current = button;
        }

    }
}
