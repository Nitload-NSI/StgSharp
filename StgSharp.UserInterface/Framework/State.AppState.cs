using System;
using System.Collections.Generic;

namespace StgSharp.UserInterface
{
    public abstract class AppState
    {

        protected AppState(
                  string name
        )
        {
            Name = string.IsNullOrWhiteSpace(name) ?
                   throw new ArgumentException("Value cannot be null or whitespace.", nameof(name)) :
                   name;
        }

        public IDictionary<string, PanelState> Panels { get; } =
            new Dictionary<string, PanelState>(StringComparer.Ordinal);

        public IDictionary<string, object?> SharedContext { get; } =
            new Dictionary<string, object?>(StringComparer.Ordinal);

        public string Name { get; }

    }
}
