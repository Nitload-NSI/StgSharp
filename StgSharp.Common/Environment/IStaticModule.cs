//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="IStaticModule"
// Project: StgSharp
// AuthorGroup: Nitload Space
// Copyright (c) Nitload Space. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace StgSharp
{
    /// <summary>
    ///   Represents a statically activated module that can be registered and initialized as part of
    ///   the application startup sequence, and gracefully torn down on shutdown.
    /// </summary>
    public interface IStaticModule
    {

        /// <summary>
        ///   Gets the display name of this module, used for logging and diagnostics.
        /// </summary>
        string ModuleName { get; }

        /// <summary>
        ///   Performs all initialization work for this module using the provided profile.
        /// </summary>
        /// <param name="profile">
        ///   The configuration profile supplied at registration time. Use <see
        ///   cref="IModuleInitializeProfile.Empty" /> if no configuration is required.
        /// </param>
        void InitializeModule(IModuleInitializeProfile profile);

        /// <summary>
        ///   Releases all resources acquired during <see cref="InitializeModule" />. Implementations should
        ///   be idempotent: calling this method more than once must not throw.
        /// </summary>
        void UninitializeModule();

    }

    /// <summary>
    ///   Marker interface for module initialization configuration. Implementations are plain data
    ///   carriers; no static factory is required.
    /// </summary>
    public interface IModuleInitializeProfile
    {

        /// <summary>
        ///   A shared, allocation-free empty profile for modules that require no configuration.
        /// </summary>
        static readonly IModuleInitializeProfile Empty = EmptyModuleInitializeProfile.Instance;

        private sealed class EmptyModuleInitializeProfile : IModuleInitializeProfile
        {

            public static readonly EmptyModuleInitializeProfile Instance = new();

            private EmptyModuleInitializeProfile() { }

        }

    }

    /// <summary>
    ///   Pairs an <see cref="IStaticModule" /> with its <see cref="IModuleInitializeProfile" />,
    ///   ensuring the two are never stored or iterated independently.
    /// </summary>
    /// <remarks>
    ///   Using a <see langword="readonly struct" /> avoids the boxing overhead that would occur if
    ///   the pair were stored as two parallel lists of reference types.
    /// </remarks>
    public readonly struct ModuleDescriptor
    {

        /// <summary>
        ///   Initializes a new <see cref="ModuleDescriptor" /> using <see
        ///   cref="IModuleInitializeProfile.Empty" /> as the profile.
        /// </summary>
        public ModuleDescriptor(IStaticModule module) : this(module, IModuleInitializeProfile.Empty) { }

        /// <summary>
        ///   Initializes a new <see cref="ModuleDescriptor" /> with an explicit profile.
        /// </summary>
        public ModuleDescriptor(IStaticModule module, IModuleInitializeProfile profile)
        {
            Module = module;
            Profile = profile;
        }

        /// <summary>
        ///   Gets the configuration profile associated with the module.
        /// </summary>
        public IModuleInitializeProfile Profile { get; }

        /// <summary>
        ///   Gets the module to be initialized.
        /// </summary>
        public IStaticModule Module { get; }

        /// <summary>
        ///   Calls <see cref="IStaticModule.InitializeModule" /> with the stored profile.
        /// </summary>
        public void Initialize()
        {
            Module.InitializeModule(Profile);
        }

        /// <summary>
        ///   Calls <see cref="IStaticModule.UninitializeModule" /> on the stored module.
        /// </summary>
        public void Uninitialize()
        {
            Module.UninitializeModule();
        }

    }

    /// <summary>
    ///   An ordered collection of modules to be initialized during application startup and
    ///   uninitialized on shutdown. Supports three registration strategies so that the <c>new()</c>
    ///   generic constraint is never a hard requirement.
    /// </summary>
    public sealed class ModuleToInitializeCollection : IEnumerable<IStaticModule>, IStaticModule
    {

        private readonly List<ModuleDescriptor> _descriptors = [];

        internal ModuleToInitializeCollection(World.Initializer i)
        {
            _descriptors.Add(new ModuleDescriptor(i));
        }

        // ── IEnumerable<IStaticModule> ───────────────────────────────────────────

        /// <inheritdoc />
        public IEnumerator<IStaticModule> GetEnumerator()
        {
            foreach (ModuleDescriptor d in _descriptors) {
                yield return d.Module;
            }
        }

        // ── Initialization / Uninitialization ────────────────────────────────────

        /// <summary>
        ///   Initializes all registered modules in the order they were registered. Each module
        ///   receives the profile that was supplied at registration time.
        /// </summary>
        public void InitializeModule()
        {
            for (int i = 0; i < _descriptors.Count; i++) {
                _descriptors[i].Initialize();
            }
        }

        /// <summary>
        ///   Uninitializes all registered modules in <b> reverse </b> registration order, mirroring
        ///   the typical LIFO teardown convention (e.g., GLFW must be unloaded after all subsystems
        ///   that depend on it).
        /// </summary>
        public void UninitializeModule()
        {
            for (int i = _descriptors.Count - 1; i >= 0; i--) {
                _descriptors[i].Uninitialize();
            }
        }

        // ── Registration API ─────────────────────────────────────────────────────

        /// <summary>
        ///   Registers a module of type <typeparamref name="T" /> using its default constructor.
        ///   This is the most concise overload for simple modules that expose a public
        ///   parameterless constructor.
        /// </summary>
        /// <typeparam name="T">
        ///   A concrete module type that implements <see cref="IStaticModule" /> and exposes a
        ///   public parameterless constructor.
        /// </typeparam>
        /// <param name="profile">
        ///   Optional initialization profile. Defaults to <see
        ///   cref="IModuleInitializeProfile.Empty" /> when <see langword="null" />.
        /// </param>
        public ModuleToInitializeCollection UseModule<T>(IModuleInitializeProfile? profile = null)
            where T: IStaticModule, new()
        {
            return UseModule(static() => new T(), profile);
        }

        /// <summary>
        ///   Registers a module produced by a factory delegate. This overload imposes no generic
        ///   constraints and supports any construction strategy, including dependency injection and
        ///   parameterized constructors.
        /// </summary>
        /// <param name="factory">
        ///   A delegate that constructs and returns the module instance. The delegate is invoked
        ///   once, immediately at registration time. If <paramref name="factory" /> is <see
        ///   langword="null" /> or returns <see langword="null" />, the call is silently ignored
        ///   and the collection is returned unchanged.
        /// </param>
        /// <param name="profile">
        ///   Optional initialization profile. Defaults to <see
        ///   cref="IModuleInitializeProfile.Empty" /> when <see langword="null" />.
        /// </param>
        public ModuleToInitializeCollection UseModule(
                                            Func<IStaticModule> factory,
                                            IModuleInitializeProfile? profile = null)
        {
            if (factory is null) {
                return this;
            }
            IStaticModule? mod = factory();
            if (mod is null) {
                return this;
            }
            _descriptors.Add(new ModuleDescriptor(
                mod,
                profile ?? IModuleInitializeProfile.Empty));
            return this;
        }

        /// <summary>
        ///   Registers a pre-existing module instance. Use this overload when the module is
        ///   provided externally, such as from a DI container, an object pool, or a singleton.
        /// </summary>
        /// <param name="module">
        ///   The module instance to register.
        /// </param>
        /// <param name="profile">
        ///   Optional initialization profile. Defaults to <see
        ///   cref="IModuleInitializeProfile.Empty" /> when <see langword="null" />.
        /// </param>
        public ModuleToInitializeCollection UseModule(IStaticModule module, IModuleInitializeProfile? profile = null)
        {
            _descriptors.Add(new ModuleDescriptor(
                module,
                profile ?? IModuleInitializeProfile.Empty));
            return this;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // ── IStaticModule explicit implementation ────────────────────────────────

        /// <summary>
        ///   Explicit <see cref="IStaticModule" /> implementation. Delegates to <see
        ///   cref="InitializeModule()" />, ignoring the supplied profile because each contained module uses
        ///   its own registered profile.
        /// </summary>
        void IStaticModule.InitializeModule(IModuleInitializeProfile profile)
        {
            InitializeModule();
        }

        /// <inheritdoc />
        string IStaticModule.ModuleName => "Module Initializer";

    }
}