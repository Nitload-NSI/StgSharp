using System;

namespace StgSharp.UserInterface
{
    public sealed record StateValueCommand<TValue> : StateChangeCommand
    {

        public StateValueCommand(
                  string statePath,
                  TValue value
        ) : base(statePath)
        {
            Value = value;
        }

        public new TValue Value { get; init; }

    }
}