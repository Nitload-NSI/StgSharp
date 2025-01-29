//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="BlueprintNode.cs"
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

namespace StgSharp.Blueprint
{
    /// <summary>
    ///
    /// </summary>
    /// <risk>
    /// Reset pipeline has risk, this operation is a direct operation to port in current node, the
    /// port on the other node cannot get the change.
    /// </risk>
#pragma warning disable CA1036
    public class BlueprintNode : IComparable<BlueprintNode>
    #pragma warning restore CA1036
    {

        private static BlueprintNode _emptyNode = new BlueprintNode(
            string.Empty, null!, false );

        private static Dictionary<string, BlueprintPipeline> _emptyLayer = new();
        private protected bool _levelAvailable, _nativeRequested;
        private protected Dictionary<string, BlueprintPipeline> _inputInterfaces;
        private protected Dictionary<string, BlueprintPipeline> _outputInterfaces;
        private protected int _level;
        private IConvertableToBlueprintNode _mainOperation;
        private string _name;

        internal BlueprintNode(
                         string name,
                         IConvertableToBlueprintNode node,
                         bool isNative )
        {
            _levelAvailable = false;
            _name = name;
            _nativeRequested = isNative;
            _mainOperation = node;
            _inputInterfaces = new Dictionary<string, BlueprintPipeline>();
            _outputInterfaces = new Dictionary<string, BlueprintPipeline>();
            foreach( string portName in node.InputInterfacesName ) {
                _inputInterfaces.Add( portName, null! );
            }

            foreach( string portName in node.OutputInterfacesName ) {
                _outputInterfaces.Add( portName, null! );
            }
        }

        #pragma warning disable CS8618
        internal BlueprintNode(
                         BlueprintNodeOperation operation,
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
            operation = operation ?? DefaultOperation;
            _mainOperation = new DefaultConvertableToBlueprintNode(
                operation,
                inputPorts,
                outputPorts);
            _inputInterfaces = new Dictionary<string, BlueprintPipeline>();
            _outputInterfaces = new Dictionary<string, BlueprintPipeline>();
            foreach( string portName in inputPorts ) {
                _inputInterfaces.Add(
                    portName,
                    SealedBlueprintPiipeline.SealOutput( this, name ) );
            }
            foreach( string portName in outputPorts ) {
                _outputInterfaces.Add( portName, null! );
            }
        }

        protected static void DefaultOperation(
            in Dictionary<string, BlueprintPipeline> input,
            in Dictionary<string, BlueprintPipeline> output)
        {
            BlueprintPipeline.SkipAll(output);
        }

        public static BlueprintNode Empty
        {
            get => _emptyNode;
        }

        public bool IsNative
        {
            get => _nativeRequested;
            internal set => _nativeRequested = value;
        }

        public static Dictionary<string, BlueprintPipeline> EmptyPortLayer
        {
            get => _emptyLayer;
        }

        public int Level
        {
            get
            {
                if( _levelAvailable ) {
                    return _level;
                }
                _level = 0;
                foreach( KeyValuePair<string, BlueprintPipeline> item in _inputInterfaces ) {
                    if( item.Value == null ) {
                        continue;
                    }
                    if( _level < item.Value.Level ) {
                        _level = item.Value.Level;
                    }
                }
                _level += 1;
                _levelAvailable = true;
                return _level;
            }
        }

        public string Name => _name;

        internal Dictionary<string, BlueprintPipeline> InputInterfaces => _inputInterfaces;

        internal Dictionary<string, BlueprintPipeline> OutputInterfaces => _outputInterfaces;

        internal IConvertableToBlueprintNode Value
        {
            get => _mainOperation;
        }

        public void Append(
                            [NotNull] BlueprintNode next,
                            string thisOutputPortName,
                            string afterInputPortName )
        {
            this._levelAvailable = false;
            BlueprintPipeline pipeline = new BlueprintPipeline( this, next );
            if( next is EndingNode enode ) {
                enode.SetCertainInputPort( afterInputPortName, pipeline );
            } else {
                next.SetCertainInputPort( afterInputPortName, pipeline );
            }
            SetCertainOutputPort( thisOutputPortName, pipeline );
        }

        public int CompareTo( BlueprintNode other )
        {
            if( null == other ) {
                throw new ArgumentNullException( nameof( other ) );
            }

            return this.Level.CompareTo( other.Level );
        }

        public void Prepend(
                            [NotNull] BlueprintNode former,
                            string formerOutputPortName,
                            string thisInputPortName )
        {
            this._levelAvailable = false;
            BlueprintPipeline pipeline = new BlueprintPipeline( former, this );
            if( former is BeginningNode bnode ) {
                bnode.SetCertainOutputPort( formerOutputPortName, pipeline );
            } else {
                former.SetCertainOutputPort( formerOutputPortName, pipeline );
            }
            SetCertainInputPort( thisInputPortName, pipeline );
        }

        public virtual void Run()
        {
            try {
                //Console.WriteLine(_name);
                BlueprintPipeline.WaitAll( _inputInterfaces );
                if( _mainOperation != null ) {
                    _mainOperation.Operation(
                        in _inputInterfaces, in _outputInterfaces );
                }
            }
            catch( Exception ) {
                throw;
            }
        }

        #region port operation

        public BlueprintPipeline GetInputPort( string name )
        {
            BlueprintPipeline ret;
            if( !_inputInterfaces.TryGetValue( name, out ret ) ) {
                throw new ArgumentOutOfRangeException(
                    $"Cannot find the interface named {name} in node {Name}." );
            }
            return ret;
        }

        public BlueprintPipeline GetOutputPort( string name )
        {
            BlueprintPipeline ret;
            if( !_outputInterfaces.TryGetValue( name, out ret ) ) {
                throw new ArgumentOutOfRangeException(
                    $"Cannot find the interface named {name} in node {Name}." );
            }
            return ret;
        }

        public void SetCertainInputPort(
                            string name,
                            BlueprintPipeline pipeline )
        {
            if( !_inputInterfaces.ContainsKey( name ) ) {
                throw new ArgumentException(
                    $"Cannot find the interface named {name} in node {Name}." );
            }
            _inputInterfaces[ name ] = pipeline;
        }

        public void SetCertainOutputPort(
                            string name,
                            BlueprintPipeline pipeline )
        {
            if( !_outputInterfaces.ContainsKey( name ) ) {
                throw new ArgumentOutOfRangeException(
                    $"Cannot find the interface named {name} in node {Name}." );
            }
            _outputInterfaces[ name ] = pipeline;
        }

        #endregion
    }//--------------------------------- end of class -----------------------------------

    public class BlueprintNodeRunException : Exception
    {

        protected BlueprintNodeRunException(
                          SerializationInfo info,
                          StreamingContext context )
            : base( info, context ) { }

        public BlueprintNodeRunException() { }

        public BlueprintNodeRunException( Exception ex )
            : base( string.Empty, ex ) { }

        public BlueprintNodeRunException( string message ) : base( message ) { }

        public BlueprintNodeRunException(
                       string message,
                       Exception innerException )
            : base( message, innerException ) { }

    }
}
