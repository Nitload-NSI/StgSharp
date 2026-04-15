using System;
using System.Collections.Generic;

namespace StgSharp.UserInterface
{
    public abstract record ServiceLoadCommand
    {

        protected ServiceLoadCommand(
                  string serviceName
        )
        {
            ServiceName = string.IsNullOrWhiteSpace(serviceName) ?
                          throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceName)) :
                          serviceName;
        }

        public IReadOnlyDictionary<string, object?> Parameters { get; init; } =
            new Dictionary<string, object?>(StringComparer.Ordinal);

        public string ServiceName { get; init; }

    }
}
