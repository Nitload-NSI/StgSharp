using System;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.UserInterface
{
    public abstract class FrontCommandHost
    {

        public StateHost StateHost { get; private set; } = null!;

        public abstract Task<CommandResult> AcceptAsync(
                                            ViewHost source,
                                            StateChangeCommand command,
                                            CancellationToken cancellationToken = default
        );

        public abstract Task<CommandResult> AcceptAsync(
                                            ViewHost source,
                                            ExecuteCommand command,
                                            CancellationToken cancellationToken = default
        );

        internal void Attach(
                      StateHost stateHost
        )
        {
            StateHost = stateHost ?? throw new ArgumentNullException(nameof(stateHost));
        }

    }
}
