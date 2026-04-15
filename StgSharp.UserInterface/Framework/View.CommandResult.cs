using System;

namespace StgSharp.UserInterface
{
    public sealed record CommandResult
    {

        public string? Message { get; init; }

        public object? Data { get; init; }

        public bool IsSuccess { get; init; }

        public bool IsRejected { get; init; }

        public bool IsAborted { get; init; }

        public Guid CommandId { get; init; }

    }
}
