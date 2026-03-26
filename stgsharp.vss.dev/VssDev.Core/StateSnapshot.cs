namespace VssDev.Core;

/// <summary>
/// Represents an immutable snapshot of all UI-visible state at a single point in time.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="StateSnapshot"/> is the only value the state machine exposes to the view layer.
/// It must be a pure value type (C# <c>record</c>) so that output adapters can hold a reference
/// to the current snapshot without risk of mutation.
/// </para>
/// <para>
/// The state machine creates new snapshots via the <c>with</c> expression rather than mutating
/// existing ones:
/// <code>
/// Commit(_snapshot with { IsBusy = true, StatusMessage = "Processing…" });
/// </code>
/// </para>
/// </remarks>
public abstract record StateSnapshot
{
    /// <summary>Gets whether the state machine is currently performing a long-running operation.</summary>
    public bool IsBusy { get; init; }

    /// <summary>Gets a human-readable status message suitable for display in a status bar.</summary>
    public string StatusMessage { get; init; } = string.Empty;
}
