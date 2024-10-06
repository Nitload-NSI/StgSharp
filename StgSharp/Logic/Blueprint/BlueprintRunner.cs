//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="BlueprintRunner.cs"
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

namespace StgSharp.BluePrint
{
    public static class BlueprintRunner
    {

        private static NodeRunner[] allAvailableTask = new NodeRunner[1];
        private static CancellationTokenSource cts = new CancellationTokenSource(
            );
        private static ConcurrentStack<Blueprint> blueprintInWaiting = new ConcurrentStack<Blueprint>(
            );
        private static int _callingThread;
        private static SemaphoreSlim bpLock = new SemaphoreSlim( 0, 1 );

        internal static bool IsValidCalling
        {
            get
            {
                if( ( NodeRunner.CurrentBlueprint == null ) || //no bp running at all
                    ( ( blueprintInWaiting.Count == 0 ) && ( NodeRunner.CurrentBlueprint.IsRunning == false ) ) )  // a bp is quiting
                {
                    //in case no bp is running
                    return true;
                }
                int? id = Task.CurrentId;
                if( id is null ) {
                    // in case calling from main thread and current running bp
                    //Console.WriteLine($"\t{_callingThread}");
                    //Console.WriteLine($"\t{Thread.CurrentThread.ManagedThreadId}");
                    return _callingThread == Thread.CurrentThread.ManagedThreadId;
                }
                foreach( NodeRunner item in allAvailableTask ) {
                    if( item.Id == id ) {
                        //if calling from any bp running task
                        return true;
                    }
                }
                return false;
            }
        }

        public static void Run(
            Blueprint blueprint,
            Func<bool> terminateInstructor )
        {
            //Console.WriteLine();
            if( blueprint == null ) {
                return;
            }

            if( terminateInstructor == null ) {
                throw new ArgumentNullException( nameof( terminateInstructor ) );
            }
            if( blueprint.IsRunning ) {
                throw new InvalidOperationException(
                    "Attempt to run a blueprint instance twice." );
            }
            blueprint.ShouldClose = terminateInstructor;
            if( IsValidCalling ) {
                //Console.WriteLine("valid and run");
                if( InterruptAndRunNewBp( blueprint ) ) {
                    do { } while (InterruptAndRunNewBp(
                        NodeRunner.CurrentBlueprint ));
                }
            } else {
                bpLock.Wait();
                if( InterruptAndRunNewBp( blueprint ) ) {
                    do { } while (InterruptAndRunNewBp(
                        NodeRunner.CurrentBlueprint ));
                }

                bpLock.Release();
            }
            Interlocked.Exchange( ref _callingThread, -1 );
        }

        public static void SetMaxTaskCount( int size )
        {
            if( size < 1 ) {
                size = 1;
            }
            if( allAvailableTask.Length >= size ) {
                for( int i = size; i < allAvailableTask.Length; i++ ) {
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
            NodeRunner.CurrentBlueprint.Terminate();
            foreach( NodeRunner task in allAvailableTask ) {
                task.Cts.Cancel();
            }
            foreach( Blueprint bp in blueprintInWaiting ) {
                bp.Terminate();
            }
            blueprintInWaiting.Clear();
        }

        internal static bool InterruptAndRunNewBp( Blueprint newBp )
        {
            Debug.Assert( newBp != null );
            if( ( newBp != NodeRunner.CurrentBlueprint ) && ( NodeRunner.CurrentBlueprint != null ) ) {
                //if a running bp exists
                blueprintInWaiting.Push( NodeRunner.CurrentBlueprint );
            }

            //load new bp to current bp
            Interlocked.Exchange( ref NodeRunner.CurrentBlueprint!, newBp );
            Interlocked.Exchange(
                ref _callingThread, Thread.CurrentThread.ManagedThreadId );

            //run beginning node to init it first.
            if( !NodeRunner.CurrentBlueprint.IsRunning ) {
                ( NodeRunner.CurrentBlueprint.BeginLayer as BeginningNode )!.Run(
                    );
            }

            //refresh all tasks
            for( int i = 0; i < allAvailableTask.Length; i++ ) {
                NodeRunner item = allAvailableTask[ i ];
                if( ( item is null ) || item.IsCompleted ) {
                    allAvailableTask[ i ] = NodeRunner.Run();
                }
            }
            BlueprintNode node;
            while( !cts.Token.IsCancellationRequested && NodeRunner.CurrentBlueprint
                .RequestNextNativeNode( out node ) ) {
                node.Run();
                if( !NodeRunner.CurrentBlueprint.IsRunning ) {
                    break;
                }
            }
            ( NodeRunner.CurrentBlueprint.EndLayer as EndingNode )!.Run();
            return blueprintInWaiting.TryPeek( out NodeRunner.CurrentBlueprint );
        }

    }

    internal class NodeRunner : CancellableTask
    {

        private static Blueprint _current;

        internal NodeRunner()
            : base()
        {
            _startup = InternalRun;
        }

        internal static ref Blueprint CurrentBlueprint
        {
            get => ref _current;
        }

        internal static NodeRunner Run()
        {
            NodeRunner ret = new NodeRunner { _startup = InternalRun };
            ret._internalTask = Task.Run( ret.InternalAction );
            return ret;
        }

        private static void InternalRun( CancellationToken token )
        {
            //Console.WriteLine("Task Begin");
            BlueprintNode item;
            while( ( CurrentBlueprint != null ) && CurrentBlueprint.IsRunning && !token.IsCancellationRequested && CurrentBlueprint.RequestNexGlobalNode(
                out item ) ) {
                //Console.WriteLine($"Get node of {item.Name}, going to run.");
                item.Run();
            }

            //Console.WriteLine("Task Completed");
        }

    }//------------------------------------- End of Class ------------------------------------------
}
