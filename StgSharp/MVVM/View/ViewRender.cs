//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ViewRender.cs"
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
using StgSharp.Data;
using StgSharp.Graphics;

using StgSharp.Math;
using StgSharp.MVVM.ViewModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.MVVM.View
{
    public abstract partial class ViewBase
    {

        protected internal interface IViewRender<out TView>
            where TView: ViewBase
        {

            RenderStream this[ DataLabel renderName ]
            {
                get;
            }

            void CustomizedInitialize();

            IEnumerator<ViewPort> GetEnumerator();

            void Initialize();

            bool ValidateInitializing();

        }

        protected abstract class ViewRender<TView> : IViewRender<TView>
            where TView: ViewBase
        {

            protected ViewRender( TView binding )
            {
                _binding = binding;
                _render = new Dictionary<DataLabel, RenderStream>();
            }

            public RenderStream this[ DataLabel renderName ]
            {
                get => _render.TryGetValue(
                    renderName, out RenderStream stream ) ?
                stream : ( null! );
                protected set
                {
                    if( _render.ContainsKey( renderName ) ) {
                        _render[ renderName ] = value;
                    } else {
                        _render.Add( renderName, value );
                    }
                }
            }

            public TView Binding => _binding;

            protected ViewPort ContextBinding => _binding.context;

            protected ViewDesigner<TView> Design => ( _binding.Designer as ViewDesigner<TView> )!;

            protected Dictionary<DataLabel, RenderStream> Render
            {
                get => _render;
            }

            protected ViewResponder<TView> Responder => ( _binding.Responder as ViewResponder<TView> )!;

            public T CreateRenderStream<T>( (int x, int y, int z) unitCubeSize )
                where T: RenderStream, new()
            {
                T ret = new T();
                ret.Initialize(
                    _binding.context, unitCubeSize, Binding.TimeProvider );
                return ret;
            }

            public T CreateRenderStream<T>(
                ViewPort vp,
                (int x, int y, int z) unitCubeSize )
                where T: RenderStream, new()
            {
                T ret = new T();
                ret.Initialize( vp, unitCubeSize, Binding.TimeProvider );
                return ret;
            }

            public abstract void CustomizedInitialize();

            public IEnumerator<ViewPort> GetEnumerator()
            {
                foreach( RenderStream item in Render.Values ) {
                    yield return item.BindedViewPortContext;
                }
            }

            public void Initialize()
            {
                CustomizedInitialize();
            }

            public abstract bool ValidateInitializing();

            #pragma warning disable CS8618 
            private Dictionary<DataLabel, RenderStream> _render;
            private TView _binding;
#pragma warning restore CS8618
        }

    }
}
