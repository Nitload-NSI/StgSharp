using System;

namespace StgSharp.UserInterface
{
    public abstract record StateChangeCommand
    {

        protected StateChangeCommand(
                  string statePath
        )
        {
            StatePath = string.IsNullOrWhiteSpace(statePath) ?
                        throw new ArgumentException("Value cannot be null or whitespace.", nameof(statePath)) :
                        statePath;
        }

        public object? Value { get; init; }

        public Guid CommandId { get; init; } = Guid.NewGuid();

        public string StatePath { get; init; }

    }
}
