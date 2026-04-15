using System;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.UserInterface
{
    public abstract class ViewHost
    {

        protected ViewHost(
                  string name,
                  StateHost stateHost
        )
        {
            Name = string.IsNullOrWhiteSpace(name) ?
                   throw new ArgumentException("Value cannot be null or whitespace.", nameof(name)) :
                   name;
            StateHost = stateHost ?? throw new ArgumentNullException(nameof(stateHost));
        }

        public string Name { get; }

        protected StateHost StateHost { get; }

        public virtual Task<CommandResult> PostAsync(
                                           StateChangeCommand command,
                                           CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(command);
            return StateHost.PostAsync(this, command, cancellationToken);
        }

        public virtual Task<CommandResult> PostAsync(
                                           ExecuteCommand command,
                                           CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(command);
            return StateHost.PostAsync(this, command, cancellationToken);
        }

        public virtual ValueTask ReceiveCommandResultAsync(
                                 CommandResult result,
                                 CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(result);
            return ValueTask.CompletedTask;
        }

    }
}
