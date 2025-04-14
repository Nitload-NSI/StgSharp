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
using StgSharp.Script.Express;

using System;
using System.Runtime.CompilerServices;

namespace StgSharp.Modeling.Step
{
    public interface IStepEntity<TSelf> : IExpMarshalType<TSelf> where TSelf: IExpMarshalType<TSelf>
    {

        int Id { get; }

        string Name { get; }

        public void Init( StepModel model, ExpNode param );

    }

    public class StepUncertainEntity : IStepEntity<StepUncertainEntity>
    {

        private ExpNode data;

        private int _id;
        private string _name;

        public StepUncertainEntity( string name, int id )
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

        public static StepUncertainEntity Create( int id )
        {
            return new StepUncertainEntity( string.Empty, id );
        }

        public static StepUncertainEntity Create( string name, ExpNode node )
        {
            return new StepUncertainEntity( name, 0 );
        }

        void IStepEntity<StepUncertainEntity>.Init( StepModel model, ExpNode param )
        {
            return;
        }

    }
}