using System;

namespace StgSharp.UserInterface.Sample
{
    public sealed class PanelStateSample : PanelState
    {
        public const string SampleStatePath = nameof(PanelStateSample) + "." + nameof(SampleState);

        private int sampleState;

        public PanelStateSample()
            : base(nameof(PanelStateSample))
        {
        }

        public int SampleState => sampleState;

        // 生成器拆分后，这里相当于某个节点 handler 的 guard，
        // 对应 ICommand 语义中的 CanExecute。
        private static bool CanApplySampleStateChange(
                    int origin,
                    int current,
                    StateValueCommand<int> command
        )
        {
            return string.Equals(command.StatePath, SampleStatePath, StringComparison.Ordinal)
                && current >= 0
                && origin != current;
        }

        // 实际项目里这整个 StateChangeHandler 都由源生成器产出；
        // 这里手写展开，只是为了展示“没有生成器时 VSS 代码长什么样”。
        public void ApplySampleStateChangeCommand(
                    StateValueCommand<int> command
        )
        {
            ArgumentNullException.ThrowIfNull(command);

            int origin = sampleState;
            int current = command.Value;
            if (!CanApplySampleStateChange(origin, current, command))
            {
                return;
            }

            sampleState = current;
            BroadcastSampleStateChanged(origin, current);
        }

        // 生成器拆分后，这里相当于某个节点写入后的 post-change hook，
        // 对应手写体系里常见的 OnChange 位置。
        private static void BroadcastSampleStateChanged(
                    int origin,
                    int current
        )
        {
            _ = origin;
            _ = current;

            // event 已禁用。手写版本若还需要状态后置联动，
            // 这里只能继续手写内部同步/广播逻辑；正式版本由生成器织入。
        }

    }
}
