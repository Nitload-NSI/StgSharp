//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="IStepEntity.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the ¡°Software¡±), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED ¡°AS IS¡±, 
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
using Microsoft.CodeAnalysis.CSharp.Syntax;

using StgSharp.Math;
using StgSharp.Modeling.Step;
using StgSharp.PipeLine;
using StgSharp.Script.Express;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StgSharp.Modeling.Step
{
    public interface IStepEntity : IExpMarshalType<StepUninitializedEntity>
    {

        int Id { get; }

        string Name { get; }

        public void Init( StepModel model, ExpSyntaxNode param );

    }

    public class StepUninitializedEntity : IStepEntity, IConvertableToBlueprintNode
    {

        private ExpSyntaxNode data;
        private int _id;
        private StepModel _model;
        private string _name;

        public StepUninitializedEntity( string name, int id )
        {
            _name = name;
            _id = id;
        }

        public int Id
        {
            get => _id;
        }

        public string Name
        {
            get => _name;
        }

        public static StepUninitializedEntity Create( int id )
        {
            return new StepUninitializedEntity( string.Empty, id );
        }

        public static StepUninitializedEntity Create( string name, ExpSyntaxNode node )
        {
            return new StepUninitializedEntity( name, 0 );
        }

        public void Init( StepModel model, ExpSyntaxNode param )
        {
            if( param is ExpFunctionCallingNode caller )
            {
                string entityName = caller.FunctionCaller.Name;
                ExpSyntaxNode p = caller.Right;
                ExpNodeNextEnumerator e = p.ToEnumerator();
            } else
            {
                throw new ExpInvalidSyntaxException(
                    "Assigned syntax node is not a function caller." );
            }
        }

        public void InitGeometry()
        {
            throw new NotImplementedException();
        }

        void IConvertableToBlueprintNode.NodeMain(
                                         in Dictionary<string, PipelineNodeImport> input,
                                         in Dictionary<string, PipelineNodeExport> output )
        {
            InitGeometry();
            PipelineNodeExport.SkipAll( output );
        }

        IEnumerable<string> IConvertableToBlueprintNode.InputInterfacesName => ["in"];

        PipelineNodeOperation IConvertableToBlueprintNode.Operation => ( this as IConvertableToBlueprintNode ).NodeMain;

        IEnumerable<string> IConvertableToBlueprintNode.OutputInterfacesName => ["out"];

    }
}