// -----------------------------------------------------------------------------
// file="Uniform"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

namespace StgSharp.Graphics.OpenGL
{
    public abstract class Uniform
    {

        internal readonly GlHandle id;

        protected Uniform(
                  GlHandle handle
        )
        {
            id = handle;
        }

        public GlHandle Handle => id;

    }

    public sealed class Uniform<T> : Uniform where T : unmanaged
    {

        public Uniform(
               GlHandle handle
        )
            : base(handle) { }

    }

    public sealed class Uniform<T, U> : Uniform where T : unmanaged where U : struct
    {

        public Uniform(
               GlHandle handle
        )
            : base(handle) { }

    }

    public sealed class Uniform<T, U, V> : Uniform where T : unmanaged where U : struct
        where V : struct
    {

        public Uniform(
               GlHandle handle
        )
            : base(handle) { }

    }

    public sealed class Uniform<T, U, V, W> : Uniform where T : unmanaged where U : struct
        where V : struct
        where W : struct
    {

        public Uniform(
               GlHandle handle
        )
            : base(handle) { }

    }
}
