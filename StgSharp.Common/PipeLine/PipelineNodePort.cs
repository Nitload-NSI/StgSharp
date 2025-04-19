//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PipelineNodePort.cs"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.PipeLine
{
    public class PipelineNodeExport
    {

        private HashSet<PipelineNodeImport> _linkerSet = new HashSet<PipelineNodeImport>();
        private SemaphoreSlim formerCompleteSemaphore;

        public PipelineNodeExport( PipelineNode container, string name )
        {
            Container = container;
            Name = name;
            formerCompleteSemaphore = new SemaphoreSlim( initialCount: 0, maxCount: 1 );
        }

        public PipelineNode Container { get; init; }

        public SemaphoreSlim Waiter { get; init; }

        public string Name { get; init; }

        public void Connect( PipelineNodeImport port )
        {
            ArgumentNullException.ThrowIfNull( port );
            if( _linkerSet.Add( port ) ) {
                Container.Next.Add( port.Container );
            }
        }

        public static void SkipAll( Dictionary<string, PipelineNodeExport> ports )
        {
            if( ports is null ) {
                return;
            }
            foreach( PipelineNodeExport port in ports.Values ) {
                port.Waiter.Release();
            }
        }

        public void TransmitValue( IPipeLineConnectionPayload value )
        {
            foreach( PipelineNodeImport item in _linkerSet ) { }
        }

        public void WaitAll( IEnumerable<PipelineNodeExport> ports )
        {
            if( ports is null ) {
                return;
            }
            foreach( PipelineNodeExport port in ports ) {
                port.Waiter.Wait();
            }
        }

    }

    public class PipelineNodeImport
    {

        private HashSet<PipelineNodeExport> _linkerSet = new HashSet<PipelineNodeExport>();

        public PipelineNodeImport( PipelineNode container, string name )
        {
            Container = container;
            Name = name;
        }

        public IEnumerable<PipelineNodeExport> LinkedPorts => _linkerSet;

        public PipelineNode Container { get; init; }

        public SemaphoreSlim Waiter { get; init; }

        public string Name { get; init; }

        public void Connect( PipelineNodeExport port )
        {
            ArgumentNullException.ThrowIfNull( port );
            if( _linkerSet.Add( port ) ) {
                Container.Previous.Add( port.Container );
            }
        }

        public IEnumerator<PipelineNode> GetFormerNodes()
        {
            foreach( PipelineNodeExport port in _linkerSet ) {
                yield return port.Container;
            }
        }

        public void Wait()
        {
            foreach( PipelineNodeExport connector in _linkerSet ) {
                connector.Waiter.Wait();
            }
        }

        public static void WaitAll( IDictionary<string, PipelineNodeImport> ports )
        {
            if( ports is null ) {
                return;
            }
            foreach( PipelineNodeImport port in ports.Values ) {
                port.Wait();
            }
        }

        public static void WaitAll( IEnumerable<PipelineNodeImport> ports )
        {
            if( ports is null ) {
                return;
            }
            foreach( PipelineNodeImport port in ports ) {
                port.Wait();
            }
        }

    }
}
