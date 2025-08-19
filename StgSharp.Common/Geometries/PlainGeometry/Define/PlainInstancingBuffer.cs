//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PlainInstancingBuffer.cs"
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
using StgSharp.Mathematics;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Geometries
{
    public class PlainInstancingBuffer<T> : IDisposable, IEnumerable<T>, IInstancingBuffer
        where T: IInstancing
    {

        private bool disposedValue;

        private CoordinatePlain globalCorrdinnate;
        private List<Vec4> coordList;
        private List<IInstancing> instanceList;
        private List<float> scalingList;
        private Mutex mutex;

        public PlainInstancingBuffer()
        {
            coordList = new List<Vec4>( capacity: 100 );
            instanceList = new List<IInstancing>( capacity: 100 );
            scalingList = new List<float>( capacity: 100 );
            coordArray = Array.Empty<Vec4>();
            scaleArray = Array.Empty<float>();
            mutex = new Mutex();
        }

        public List<IInstancing> InstanceList
        {
            get => instanceList;
        }

        public List<Vec4> CoordAndRotationList
        {
            get => coordList;
        }

        public List<float> ScalingList
        {
            get => scalingList;
        }

        public Span<Vec4> CoordAndRotationSpan
        {
            get
            {
                if( coordArray.Length != coordList.Capacity )
                {
                    FieldInfo fieldInfo = typeof( List<Vec4> ).
                        GetField( "_items", BindingFlags.NonPublic | BindingFlags.Instance );
                    if( fieldInfo == null )
                    {
                        #pragma warning disable CA1065
                        #pragma warning disable CA2201
                        throw new NullReferenceException();
                        #pragma warning restore CA2201
                        #pragma warning restore CA1065
                    }
                    coordArray = ( Vec4[] )fieldInfo.GetValue( coordList );
                }
                return new Span<Vec4>( coordArray, 0, coordList.Count );
            }
        }

        public Span<float> ScalingSpan
        {
            get
            {
                if( scaleArray.Length != coordList.Capacity )
                {
                    FieldInfo fieldInfo = typeof( List<float> ).
                        GetField( "_items", BindingFlags.NonPublic | BindingFlags.Instance );
                    if( fieldInfo == null )
                    {
                        #pragma warning disable CA1065
                        #pragma warning disable CA2201
                        throw new NullReferenceException();
                        #pragma warning restore CA2201
                        #pragma warning restore CA1065
                    }
                    scaleArray = ( float[] )fieldInfo.GetValue( scalingList );
                }
                return new Span<float>( scaleArray, 0, coordList.Count );
            }
        }

        public int CreateInstanceID()
        {
            mutex.WaitOne();
            int ret = instanceList.Count;
            coordList.Add( new Vec4() );
            scalingList.Add( 0 );
            mutex.ReleaseMutex();
            return ret;
        }

        // ~PlainInstancingBuffer()
        // {
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose( disposing: true );
            GC.SuppressFinalize( this );
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach( IInstancing item in instanceList ) {
                yield return ( T )item;
            }
        }

        public void Remove( T instance )
        {
            mutex.WaitOne();
            if( instance.GlobalBuffer != this ) {
                throw new ArgumentException(
                    message: "instance does not belong to current Particle Buffer",
                    paramName: nameof( instance ) );
            }
            int index = instance.BufferId;
            IInstancing temp = instanceList[ instanceList.Count - 1 ];
            instanceList[ instance.BufferId ] = temp;
            temp.BufferId = instance.BufferId;

            coordList[ instance.BufferId ] = coordList[ coordList.Count - 1 ];
            scalingList[ instance.BufferId ] = scalingList[ scalingList.Count - 1 ];

            instanceList.RemoveAt( instanceList.Count - 1 );
            coordList.RemoveAt( coordList.Count - 1 );
            scalingList.RemoveAt( scalingList.Count - 1 );
            mutex.ReleaseMutex();
        }

        public void UpdateAllParticles()
        {
            foreach( IInstancing particle in instanceList ) {
                particle.Move();
            }
        }

        protected virtual void Dispose( bool disposing )
        {
            if( !disposedValue )
            {
                if( disposing ) {
                    mutex.Dispose();
                }

                coordList.Clear();
                instanceList.Clear();
                scalingList.Clear();
                disposedValue = true;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach( IInstancing item in instanceList ) {
                yield return ( T )item;
            }
        }

        IGeometry IInstancingBuffer.TypicalShape => throw new NotImplementedException();

        #region field of mem opt

        private Vec4[] coordArray;
        private float[] scaleArray;

    #endregion
    }
}
