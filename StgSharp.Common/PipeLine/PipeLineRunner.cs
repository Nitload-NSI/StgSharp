//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PipeLineRunner.cs"
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
using StgSharp.Threading;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

using System.Threading.Tasks;

namespace StgSharp.PipeLine
{
    public static class PipeLineRunner
    {

        private static NodeRunner[] allAvailableTask = CreateBlueprintRunnerArray();
        private static CancellationTokenSource cts = new CancellationTokenSource();
        private static ConcurrentStack<PipelineScheduler> pipelineInWaiting 
            = new ConcurrentStack<PipelineScheduler>();
        private static int _callingThread;
        private static SemaphoreSlim pipelineLock = new SemaphoreSlim( 0, 1 );

        public static void Run( PipelineScheduler pipeline )
        {
            //Console.WriteLine();
            if( pipeline == null )
            {
                return;
            }
            if( pipeline.IsRunning ) {
                throw new InvalidOperationException( "Attempt to run a blueprint instance twice." );
            }
            if( !IsValidCalling() ) {
                pipelineLock.Wait();
            }
            if( InterruptAndRunNewPipeline( pipeline ) )
            {
                while( InterruptAndRunNewPipeline( NodeRunner.CurrentPipeline! ) ) { }
            }
            pipelineLock.Release();
            Interlocked.Exchange( ref _callingThread, -1 );
        }

        public static void SetMaxTaskCount( int size )
        {
            if( size < 1 ) {
                size = 1;
            }
            if( allAvailableTask.Length >= size )
            {
                for( int i = size; i < allAvailableTask.Length; i++ )
                {
                    if( allAvailableTask[ i ] != null ) {
                        allAvailableTask[ i ].Cancel();
                    }
                }
            }
            Array.Resize( ref allAvailableTask, size );
        }

        public static void Terminate()
        {
            cts.Cancel();
            if( NodeRunner.CurrentPipeline is null ) {
                return;
            }
            NodeRunner.CurrentPipeline.Terminate();
            foreach( NodeRunner task in allAvailableTask ) {
                task.Cts.Cancel();
            }
            foreach( PipelineScheduler bp in pipelineInWaiting ) {
                bp.Terminate();
            }
            pipelineInWaiting.Clear();
        }

        internal static bool InterruptAndRunNewPipeline( PipelineScheduler newPipeline )
        {
            ArgumentNullException.ThrowIfNull( newPipeline );
            if( ( newPipeline != NodeRunner.CurrentPipeline ) &&
                ( NodeRunner.CurrentPipeline != null ) )
            {
                //if a running bp exists
                pipelineInWaiting.Push( NodeRunner.CurrentPipeline );
            }

            //load new bp to current bp
            NodeRunner.CurrentPipeline = newPipeline;
            Interlocked.Exchange( ref _callingThread, Environment.CurrentManagedThreadId );

            //run beginning node to init it first.
            if( !NodeRunner.CurrentPipeline.IsRunning )
            {
                ( NodeRunner.CurrentPipeline.BeginLayer as BeginningNode )!.Run();
            }

            //refresh all tasks
            /*
            for( int i = 0; i < allAvailableTask.Length; i++ )
            {
                NodeRunner item = allAvailableTask[ i ];
                if( ( item is null ) || item.IsCompleted ) {
                    allAvailableTask[ i ] = NodeRunner.Run();
                }
            }
            /**/
            PipelineNode node;
            while( !cts.Token.IsCancellationRequested &&
                   NodeRunner.CurrentPipeline.RequestNextMainThreadNode( out node ) )
            {
                node.Run();
                if( !NodeRunner.CurrentPipeline.IsRunning )
                {
                    break;
                }
            }

            while( ( NodeRunner.CurrentPipeline != null ) &&
                   NodeRunner.CurrentPipeline.IsRunning &&
                   !cts.Token.IsCancellationRequested &&
                   NodeRunner.CurrentPipeline!.RequestNexConcurrentNode( out node ) )
            {
                node.Run();
                if( !NodeRunner.CurrentPipeline!.IsRunning )
                {
                    break;
                }
            }
            ( NodeRunner.CurrentPipeline!.EndLayer as EndingNode )!.Run();
            bool isEmpty = pipelineInWaiting.TryPop( out PipelineScheduler? pipeline );
            NodeRunner.CurrentPipeline = pipeline!;
            return isEmpty;
        }

        /// <summary>
        ///   <para> Only the following occasions of calling are valid: </para> <para>1. no bp is
        ///   running or the only bp is quitting.</para><para>2. calling from the main thread or a
        ///   thread the current bp is running on. </para>
        /// </summary>
        internal static bool IsValidCalling()
        {
            if( ( NodeRunner.CurrentPipeline == null ) || //no bp running at all
                ( ( pipelineInWaiting.IsEmpty ) && ( NodeRunner.CurrentPipeline.IsRunning == false ) ) )
            {
                // a bp is quitting
                //in case no bp is running
                return true;
            }
            int? id = Task.CurrentId;
            if( id is null )
            {
                // in case calling from main thread and current running bp
                return _callingThread == Environment.CurrentManagedThreadId;
            }
            foreach( NodeRunner item in allAvailableTask )
            {
                if( item.Id == id )
                {
                    //if calling from any bp running task
                    return true;
                }
            }
            return false;
        }

        private static NodeRunner[] CreateBlueprintRunnerArray()
        {
            int count = Environment.ProcessorCount;
            return new NodeRunner[ count < 2 ? count : count - 2];
        }

    }

    internal class NodeRunner : CancellableTask
    {

        private static volatile PipelineScheduler? _current;

        internal NodeRunner()
            : base()
        {
            _startup = InternalRun;
        }

        internal static PipelineScheduler? CurrentPipeline
        {
            get => _current;
            set => _current = value;
        }

        internal static NodeRunner Run()
        {
            NodeRunner ret = new NodeRunner
            {
                _startup = InternalRun
            };
            ret._internalTask = Task.Run( ret.InternalAction );
            return ret;
        }

        private static void InternalRun( CancellationToken token )
        {
            PipelineNode item;
            Console.WriteLine( $"Global thread {Environment.CurrentManagedThreadId} starts." );
            while( ( CurrentPipeline != null ) &&
                   CurrentPipeline.IsRunning &&
                   !token.IsCancellationRequested &&
                   CurrentPipeline.RequestNexConcurrentNode( out item ) ) {
                item.Run();
            }
        }

    }//------------------------------------- End of Class ------------------------------------------
}
