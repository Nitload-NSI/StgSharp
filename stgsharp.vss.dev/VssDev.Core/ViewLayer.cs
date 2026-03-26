namespace VssDev.Core;

/// <summary>
/// Container that aggregates all input and output adapters for a single application instance.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="ViewLayer"/> is the only VSS component that knows about concrete adapter
/// implementations. Everything beneath it (state machine, services) remains oblivious to
/// the existence of the view layer.
/// </para>
/// <example>
/// Typical assembly at application startup:
/// <code>
/// var view = new ViewLayer()
///     .Add(new WpfInputAdapter(mainWindow))
///     .Add(new WpfPresenter(mainWindow));
///
/// var state = new AppState(view.AcceptSnapshot, myService);
/// view.Start(state.Dispatch);
/// </code>
/// </example>
/// </remarks>
public sealed class ViewLayer
{
    private readonly List<IInputAdapter>  _inputs  = [];
    private readonly List<IOutputAdapter> _outputs = [];

    /// <summary>Registers an input adapter.</summary>
    public ViewLayer Add(IInputAdapter adapter)
    {
        ArgumentNullException.ThrowIfNull(adapter);
        _inputs.Add(adapter);
        return this;
    }

    /// <summary>Registers an output adapter.</summary>
    public ViewLayer Add(IOutputAdapter adapter)
    {
        ArgumentNullException.ThrowIfNull(adapter);
        _outputs.Add(adapter);
        return this;
    }

    /// <summary>
    /// Starts all registered adapters.
    /// </summary>
    /// <param name="dispatch">
    /// The state machine's dispatch entry point, forwarded to every input adapter.
    /// </param>
    public void Start(Action<Intent> dispatch)
    {
        ArgumentNullException.ThrowIfNull(dispatch);
        foreach (var input in _inputs)
            input.Start(dispatch);
        foreach (var output in _outputs)
            output.Start();
    }

    /// <summary>Stops all registered adapters.</summary>
    public void Stop()
    {
        foreach (var input in _inputs)
            input.Stop();
        foreach (var output in _outputs)
            output.Stop();
    }

    /// <summary>
    /// Broadcasts a new state snapshot to all registered output adapters.
    /// </summary>
    /// <param name="snapshot">The latest read-only application state.</param>
    public void AcceptSnapshot(StateSnapshot snapshot)
    {
        foreach (var output in _outputs)
            output.AcceptSnapshot(snapshot);
    }
}
