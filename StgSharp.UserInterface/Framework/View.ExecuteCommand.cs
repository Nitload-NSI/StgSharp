using System;
using System.Collections.Generic;

namespace StgSharp.UserInterface
{
    public abstract record ExecuteCommand
    {

        protected ExecuteCommand(
                  string operationName
        )
        {
            OperationName = string.IsNullOrWhiteSpace(operationName) ?
                            throw new ArgumentException("Value cannot be null or whitespace.", nameof(operationName)) :
                            operationName;
        }

        public Guid CommandId { get; init; } = Guid.NewGuid();

        public IReadOnlyList<object?> Parameters { get; init; } = Array.Empty<object?>();

        public string OperationName { get; init; }

    }
}
