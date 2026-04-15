using System;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.UserInterface
{
    public abstract class ServiceHost
    {

        protected ServiceHost(
                  string name
        )
        {
            Name = string.IsNullOrWhiteSpace(name) ?
                   throw new ArgumentException("Value cannot be null or whitespace.", nameof(name)) :
                   name;
        }

        public string Name { get; }

        public abstract Task<object?> ExecuteAsync(
                                      ExecuteCommand command,
                                      CancellationToken cancellationToken = default
        );

        public abstract ValueTask InitializeAsync(
                                  ServiceLoadCommand command,
                                  CancellationToken cancellationToken = default
        );

        public abstract ValueTask StopAsync(
                                  CancellationToken cancellationToken = default
        );

    }
}
