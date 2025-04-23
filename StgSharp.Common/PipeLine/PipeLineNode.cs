//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PipelineNode.cs"
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
using StgSharp.Entities;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.PipeLine
{
    /// <summary>
    ///
    /// </summary>
    /// <risk>
    ///   Reset pipeline has risk, this operation is a direct operation to port in current node, the
    ///   port on the other node cannot get the change.
    /// </risk>
#pragma warning disable CA1036
    public class PipelineNode
    #pragma warning restore CA1036
    {

        private static Dictionary<string, PipelineConnector> _emptyLayer = new();
        private static PipelineNode _emptyNode = new PipelineNode( string.Empty, null!, false );
        private IConvertableToPipelineNode _mainOperation;
        private string _name;
        private protected bool _levelAvailable, _nativeRequested;
        private protected Dictionary<string, PipelineNodeImport> _input;
        private protected Dictionary<string, PipelineNodeExport> _output;
        private protected int _level;

        internal PipelineNode( string name, IConvertableToPipelineNode node, bool isNative )
        {
            _levelAvailable = false;
            _name = name;
            _nativeRequested = isNative;
            _mainOperation = node;
            InputPorts = new Dictionary<string, PipelineNodeImport>();
            OutputPorts = new Dictionary<string, PipelineNodeExport>();
            foreach( string portName in node.InputInterfacesName ) {
                InputPorts.Add( portName, new PipelineNodeImport( this, portName ) );
            }

            foreach( string portName in node.OutputInterfacesName ) {
                OutputPorts.Add( portName, new PipelineNodeExport( this, portName ) );
            }
        }

        #pragma warning disable CS8618
        internal PipelineNode(
                 PipelineNodeOperation operation,
                 string name,
                 bool isNative,
                 string[] inputPorts,
                 string[] outputPorts )
        #pragma warning restore CS8618
        {
            if( ( inputPorts == null ) || ( inputPorts.Length == 0 ) ) {
                throw new ArgumentNullException( nameof( inputPorts ) );
            }
            if( ( outputPorts == null ) || ( outputPorts.Length == 0 ) ) {
                throw new ArgumentNullException( nameof( outputPorts ) );
            }
            _levelAvailable = false;
            _name = name;
            _nativeRequested = isNative;
            if( operation == null ) {
                operation = DefaultOperation;
            }
            _mainOperation = new DefaultConvertableToBlueprintNode(
                operation, inputPorts, outputPorts );
            InputPorts = new Dictionary<string, PipelineNodeImport>();
            OutputPorts = new Dictionary<string, PipelineNodeExport>();
        }

        public bool IsNative
        {
            get => _nativeRequested;
            internal set => _nativeRequested = value;
        }

        public static Dictionary<string, PipelineConnector> EmptyPortLayer
        {
            get => _emptyLayer;
        }

        public static PipelineNode Empty
        {
            get => _emptyNode;
        }

        public string Name => _name;

        internal Dictionary<string, PipelineNodeImport> InputPorts
        {
            get => _input;
            private set => _input = value;
        }

        internal Dictionary<string, PipelineNodeExport> OutputPorts
        {
            get => _output;
            private set => _output = value;
        }

        internal HashSet<PipelineNode> Previous { get; } = new HashSet<PipelineNode>();

        internal HashSet<PipelineNode> Next { get; } = new HashSet<PipelineNode>();

        internal IConvertableToPipelineNode Value
        {
            get => _mainOperation;
        }

        public static void Connect(
                           PipelineNode front,
                           string frontNodePort,
                           string backNodePort,
                           PipelineNode back )
        {
            ArgumentNullException.ThrowIfNull( front );
            ArgumentNullException.ThrowIfNull( back );
            if( !front.OutputPorts.TryGetValue( frontNodePort,
                                                out PipelineNodeExport? frontPort ) ) {
                throw new ArgumentOutOfRangeException( $"Cannot find port named {frontNodePort}" );
            }
            if( !back.InputPorts.TryGetValue( backNodePort, out PipelineNodeImport? backPort ) ) {
                throw new ArgumentOutOfRangeException( $"Cannot find port named {backNodePort}" );
            }
            backPort.Connect( frontPort );
            frontPort.Connect( backPort );
        }

        public IEnumerator<PipelineNode> GetFormerNodes()
        {
            foreach( PipelineNodeImport port in InputPorts.Values )
            {
                IEnumerator<PipelineNode> enumerator = port.GetFormerNodes();
                while( enumerator.MoveNext() ) {
                    yield return enumerator.Current;
                }
            }
        }

        public virtual void Run()
        {
            try
            {
                //Console.WriteLine(_name);
                PipelineNodeImport.WaitAll( InputPorts );
                if( _mainOperation != null ) {
                    _mainOperation.NodeMain( in _input, in _output );
                }
            }
            catch( Exception )
            {
                throw;
            }
        }

        protected static void DefaultOperation(
                              in Dictionary<string, PipelineNodeImport> input,
                              in Dictionary<string, PipelineNodeExport> output )
        {
            PipelineNodeExport.SkipAll( output );
        }

        #region port operation

        public PipelineNodeImport GetInputPort( string name )
        {
            if( !InputPorts.TryGetValue( name, out PipelineNodeImport? ret ) ) {
                throw new ArgumentOutOfRangeException(
                    $"Cannot find the interface named {name} in node {Name}." );
            }
            return ret;
        }

        public PipelineNodeExport GetOutputPort( string name )
        {
            if( !OutputPorts.TryGetValue( name, out PipelineNodeExport? ret ) ) {
                throw new ArgumentOutOfRangeException(
                    $"Cannot find the interface named {name} in node {Name}." );
            }
            return ret;
        }

        public void DefineCertainInputPort( string name )
        {
            if( InputPorts.ContainsKey( name ) ) {
                throw new ArgumentException( $"Port named {name} in node {Name} has been defined." );
            }
            InputPorts[ name ] = new PipelineNodeImport( this, name );
        }

        public void DefineCertainOutputPort( string name )
        {
            if( OutputPorts.ContainsKey( name ) ) {
                throw new ArgumentOutOfRangeException(
                    $"Port named {name} in node {Name} has been defined." );
            }
            OutputPorts[ name ] = new PipelineNodeExport( this, name );
        }

        #endregion
    }//--------------------------------- end of class -----------------------------------

    public class PipelineNodeRunException : Exception
    {

        public PipelineNodeRunException() { }

        public PipelineNodeRunException( Exception ex ) : base( string.Empty, ex ) { }

        public PipelineNodeRunException( string message ) : base( message ) { }

        public PipelineNodeRunException( string message, Exception innerException )
            : base( message, innerException ) { }

    }
}
