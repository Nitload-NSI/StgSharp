// -----------------------------------------------------------------------------
// file="L4.Exceptions"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

namespace StgSharp.HighPerformance.Memory
{
#pragma warning disable CA1032 // Implement standard exception constructors

    public sealed class L4PredictUnregisteredException : Exception
    {

        public L4PredictUnregisteredException(
               int id
        )
            : base($"L4 predictor with id {id} is null. Maybe it has been unregistered.") { }

    }

    public sealed class L4PredictConflictException : Exception
    {

        public L4PredictConflictException(
               nuint basePtr,
               ushort policy,
               int current
        )
            : base($"L4 predictor with id {current} has already registered a cache line with base pointer {basePtr} and policy {policy}.") { }

    }
#pragma warning restore CA1032 // Implement standard exception constructors
}
