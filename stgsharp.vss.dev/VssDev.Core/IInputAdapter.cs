namespace VssDev.Core;

/// <summary>
/// Adapts a specific input source (mouse, keyboard, hardware button panel, serial device, etc.)
/// into a stream of <see cref="Intent"/> values delivered to the state machine.
/// </summary>
/// <remarks>
/// <para>
/// An input adapter is the boundary between the physical world and VSS application logic.
/// Its only job is to translate raw signals into <see cref="Intent"/> objects and forward them
/// via the <c>dispatch</c> delegate — no semantic interpretation should happen here.
/// </para>
/// <para>
/// Implementations may run on any thread; <c>dispatch</c> is guaranteed to be thread-safe
/// because the state machine's internal <c>Channel&lt;Intent&gt;</c> is written atomically.
/// </para>
/// </remarks>
public interface IInputAdapter
{
    /// <summary>
    /// Starts the adapter and registers the dispatch entry point.
    /// </summary>
    /// <param name="dispatch">
    /// Delegate provided by <see cref="IVssState"/> used to enqueue each arriving
    /// <see cref="Intent"/>. Must not be <see langword="null"/>.
    /// </param>
    void Start(Action<Intent> dispatch);

    /// <summary>Stops input collection and releases all resources held by the adapter.</summary>
    void Stop();
}
