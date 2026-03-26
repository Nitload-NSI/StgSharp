namespace VssDev.Core;

/// <summary>
/// Represents the VSS state machine contract: receives <see cref="Intent"/> values, performs
/// state transitions, and notifies the view layer via <see cref="StateSnapshot"/>.
/// </summary>
/// <remarks>
/// <para>
/// The state machine is the sole owner of application state. It consumes all input sources
/// (view-layer intents and service events) through a single serialised queue to guarantee
/// atomic state transitions.
/// </para>
/// <para>
/// Implementations must never perform I/O, long-running computation, or direct UI manipulation.
/// All side-effects are delegated to service layer collaborators.
/// </para>
/// </remarks>
public interface IVssState
{
    /// <summary>
    /// Enqueues an <see cref="Intent"/> for processing on the state machine's serial consumer loop.
    /// </summary>
    /// <param name="intent">The intent to enqueue. Must not be <see langword="null"/>.</param>
    /// <remarks>
    /// This method is thread-safe and non-blocking; it may be called from any thread.
    /// </remarks>
    void Dispatch(Intent intent);

    /// <summary>Starts the state machine's internal consumer loop.</summary>
    Task RunAsync(CancellationToken cancellationToken = default);
}
