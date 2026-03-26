namespace VssDev.Core;

/// <summary>
/// Adapts a specific output channel (screen, LED strip, HMI panel, headless sink, etc.)
/// to receive read-only <see cref="StateSnapshot"/> values from the state machine.
/// </summary>
/// <remarks>
/// <para>
/// An output adapter is the boundary between VSS application state and the physical world.
/// Each adapter drives its own clock (frame loop, timer, etc.); the state machine merely
/// delivers a new snapshot whenever state changes.
/// </para>
/// <para>
/// <see cref="AcceptSnapshot"/> may be called from any thread — implementations are responsible
/// for their own thread-safety (e.g. atomic swap of a <c>_pending</c> field for a WPF presenter).
/// </para>
/// </remarks>
public interface IOutputAdapter
{
    /// <summary>Starts the output adapter and its internal driving clock.</summary>
    void Start();

    /// <summary>Stops the adapter and releases all resources.</summary>
    void Stop();

    /// <summary>
    /// Receives the latest application state snapshot.
    /// </summary>
    /// <param name="snapshot">The new read-only state snapshot. Must not be <see langword="null"/>.</param>
    /// <remarks>
    /// This method may be called on any thread. The adapter must not block the caller.
    /// </remarks>
    void AcceptSnapshot(StateSnapshot snapshot);
}
