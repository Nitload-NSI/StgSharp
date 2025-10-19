//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="StepReader.ToModel"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using StgSharp.Script;
using StgSharp.Script.Express;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Model.Step
{
    public partial class StepReader
    {

        private volatile int _version = 0;
        private StepDataTokenReader tokenReader;

        private StepExpSyntaxAnalyzer analyzer;

        private StepModel Model { get; set; }

        private Task<StepModel> ReadToModelTask { get; set; }

        public StepModel ReadToModel()
        {
            if (Model is not null) {
                return Model;
            }
            if (_version != 0) {
                throw new InvalidOperationException(
                    "A ReadToModel method has been called and is running now, use Async version instead");
            }
            Interlocked.Increment(ref _version);
            GetInfo();
            tokenReader = new StepDataTokenReader(DataTransmitter);
            analyzer = new StepExpSyntaxAnalyzer();
            ConcurrentQueue<StepEntityDefineSequence> initStatements = analyzer.StepEntityInitStatements;
            Task secTask = Task.Run(ReadDataSection);
            Task anTask = Task.Run(ReadStepToken);
            Model = new StepModel(_info);
            while (initStatements.TryDequeue(out StepEntityDefineSequence? statement) || !anTask.IsCompleted)
            {
                if (statement is null)
                {
                    Thread.Sleep(0);
                    continue;
                }
                StepUninitializedEntity entity = Model.FromInitSequence(statement);
            }
            Task.WaitAll(secTask, anTask);
            return Model;
        }

        public async Task<StepModel> ReadToModelAsync()
        {
            if (ReadToModelTask is null) {
                ReadToModelTask = Task.Run(ReadToModel);
            }
            return await ReadToModelTask;
        }

        public StepModel ReadToModelSingleThread()
        {
            if (Model is not null) {
                return Model;
            }
            if (_version != 0) {
                throw new InvalidOperationException(
                    "A ReadToModel method has been called and is running now, use Async version instead");
            }
            Interlocked.Increment(ref _version);
            GetInfo();
            tokenReader = new StepDataTokenReader(DataTransmitter);
            analyzer = new StepExpSyntaxAnalyzer();
            ConcurrentQueue<StepEntityDefineSequence> initStatements = analyzer.StepEntityInitStatements;
            ReadDataSection();
            ReadStepToken();
            Model = new StepModel(_info);
            while (initStatements.TryDequeue(out StepEntityDefineSequence? statement))
            {
                if (statement is null)
                {
                    Thread.Sleep(0);
                    continue;
                }
                StepUninitializedEntity entity = Model.FromInitSequence(statement);
            }
            return Model;
        }

        private void ReadStepToken()
        {
            while (!tokenReader.IsEmpty)
            {
                Token t = tokenReader.ReadToken();
                analyzer.AppendToken(t);
            }
        }

    }
}
