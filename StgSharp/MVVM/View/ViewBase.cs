//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ViewBase.cs"
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
using StgSharp.Graphics;
using StgSharp.Math;
using StgSharp.MVVM.ViewModel;
using StgSharp.Timing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace StgSharp.MVVM.View
{
    /// <summary>
    /// ViewBase is the base type for all view types, providing context binding and control element
    /// binding. A complete view should contain one complete <see cref="ViewDesigner{T}" />, <see
    /// cref="ViewRender{TView}" />, and <see cref="ViewResponder" />, which provide layout
    /// descriptions of control elements, user responses, and rendering implementations in the view.
    /// Data between instances of <see cref="ViewDesigner{T}" />, <see cref="ViewRender{TView}" />,
    /// and <see cref="ViewResponder{T}" /> is shared through <see cref="ViewBase" />.
    /// </summary>

    public abstract unsafe partial class ViewBase
    {

        private (int x, int y, int z) unitCubeSize;

        private GetDataBindingEntryDelegate getDataBindingEntry;

        private int usedFrame;
        private IViewDesigner<ViewBase> _design;
        private IViewRender<ViewBase> _render;
        private IViewResponder<ViewBase> _responder;
        private TimeSpanProvider time;

        private ViewPort context;

        private delegate bool GetDataBindingEntryDelegate(
            string name,
            out DataBindingEntry entry );

        public DataBindingEntry this[ string name ]
        {
            get
            {
                if( getDataBindingEntry( name, out DataBindingEntry entry ) ) {
                    return entry;
                }
                if( TryGetObjectReference( name, out entry ) ) {
                    return entry;
                }
                throw new KeyNotFoundException();
            }
        }

        public (int x, int y, int z) UnitCubeSize
        {
            get => unitCubeSize;
        }

        public bool Activated
        {
            get;
            internal set;
        }

        public int Height
        {
            get => context.Height;
        }

        public int Width
        {
            get => context.Width;
        }

        public IntPtr Monitor => context.Monitor;

        public IntPtr ViewHandle => context.ViewPortID;

        public IViewDesigner<ViewBase> Designer
        {
            get => _design;
            protected set => _design = value;
        }

        public string ContextName => context.Name;

        public string SelfName
        {
            get;
            set;
        }

        public ViewPort Port
        {
            get => context;
        }

        internal TimeSpanProvider TimeProvider
        {
            get => time;
        }

        protected internal IViewRender<ViewBase> Render
        {
            get => _render;
            set => _render = value;
        }

        protected IViewResponder<ViewBase> Responder
        {
            get => _responder;
            set => _responder = value;
        }

        public abstract void CustomizedInitialize(
            [NotNull]ViewModelBase viewModelBinding );

        /// <summary>
        /// Get the status of a certain key on the keyboard.
        /// </summary>
        /// <param name="key"></param>
        /// <returns><see cref="KeyStatus" /> representing if a key is pressed or released. </returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public KeyStatus GetKeyStatus( KeyboardKey key )
        {
            return InternalIO.glfwGetKey( context.ViewPortID, ( int )key );
        }

        public void InternalInitialize(
            string name,
            (int, int, int) cubeSize,
            [NotNull]ViewModelBase viewModelBinding )
        {
            SelfName = name;
            unitCubeSize = cubeSize;
            getDataBindingEntry = viewModelBinding.TryGetObjectReference;
            context = viewModelBinding.ViewPortBinding;
            time = viewModelBinding.frameTimeProvider;

            CustomizedInitialize( viewModelBinding );
        }

        public abstract bool TryGetObjectReference(
            string name,
            out DataBindingEntry entry );

        public void Use()
        {
            if( usedFrame == 0 ) {
                if( !ValidateInitialization() ) {
                    throw new InvalidOperationException(
                        "This view is not correctly initialized before rendering." );
                }
            }
            Show();
            usedFrame++;
        }

        public bool ValidateInitialization()
        {
            return ( _design != null ) && ( _render != null ) && ( getDataBindingEntry != null ) && ( _responder != null ) && _design.ValidateInitializing(
                ) && _responder.ValidateInitializing() && _render.ValidateInitializing(
                );
        }

        protected internal abstract void Show();

    }
}