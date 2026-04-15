using System;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.UserInterface
{
    public abstract class ServiceManager
    {

        public StateHost StateHost { get; private set; } = null!;

        public abstract Task<object?> ExecuteAsync(
                                      ExecuteCommand command,
                                      CancellationToken cancellationToken = default
        );

        public abstract Task<ServiceLoadAccomplishmentEvent> LoadAsync(
                                                             ServiceLoadCommand command,
                                                             CancellationToken cancellationToken = default
        );

        public abstract ValueTask RegisterAsync(
                                  string serviceName,
                                  ServiceHost serviceHost,
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
