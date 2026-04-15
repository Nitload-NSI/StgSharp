using System;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.UserInterface
{
    public abstract class StateHost
    {

        protected StateHost(
                  FrontCommandHost frontCommandHost,
                  AppState appState,
                  ServiceManager serviceManager
        )
        {
            FrontCommandHost = frontCommandHost ??
                throw new ArgumentNullException(nameof(frontCommandHost));
            AppState = appState ?? throw new ArgumentNullException(nameof(appState));
            ServiceManager = serviceManager ??
                throw new ArgumentNullException(nameof(serviceManager));

            FrontCommandHost.Attach(this);
            ServiceManager.Attach(this);
        }

        public AppState AppState { get; }

        public FrontCommandHost FrontCommandHost { get; }

        public ServiceManager ServiceManager { get; }

        public virtual async Task<CommandResult> PostAsync(
                                                 ViewHost source,
                                                 StateChangeCommand command,
                                                 CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(command);

            CommandResult result = await FrontCommandHost.AcceptAsync(source, command, cancellationToken)
                                                         .ConfigureAwait(false);
            await source.ReceiveCommandResultAsync(result, cancellationToken).ConfigureAwait(false);
            return result;
        }

        public virtual async Task<CommandResult> PostAsync(
                                                 ViewHost source,
                                                 ExecuteCommand command,
                                                 CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(command);

            CommandResult result = await FrontCommandHost.AcceptAsync(source, command, cancellationToken)
                                                         .ConfigureAwait(false);
            await source.ReceiveCommandResultAsync(result, cancellationToken).ConfigureAwait(false);
            return result;
        }

    }
}
