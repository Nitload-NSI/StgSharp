using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Logic
{
    public class CrossFrameOperation : IConvertableToBlueprintNode
    {
        public Action MainExecution { get; private protected set; }

        private readonly int cycleCount;
        private int currentCount;

        public static string InputInterfacesName => "TaskIn";

        public static string OutputInterfacesName => "TaskOut";

        string[] IConvertableToBlueprintNode.InputInterfacesName => [InputInterfacesName];

        string[] IConvertableToBlueprintNode.OutputInterfacesName => [OutputInterfacesName];

        public void ExecuteMain(in Dictionary<string, BlueprintPipeline> input, in Dictionary<string, BlueprintPipeline> output)
        {
            if (currentCount < cycleCount)
            {
                Interlocked.Increment(ref currentCount);
            }
            else
            {
                Interlocked.Exchange(ref currentCount, 0);
                Task.Run(MainExecution);
            }
            //Console.WriteLine(currentCount);
            BlueprintPipeline.SkipAll(output);
        }

        public CrossFrameOperation(Action startup, int cycleCount)
        {
            MainExecution = startup;
            this.cycleCount = cycleCount > 1 ? cycleCount : throw new ArgumentException(
                $"The {typeof(CrossFrameOperation).Name} crosses only one cycle, " +
                $"please use a {typeof(BlueprintNode).Name} instead.");
            this.cycleCount = cycleCount;
            currentCount = cycleCount;
        }

    }
}
