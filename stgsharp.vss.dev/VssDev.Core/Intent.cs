namespace VssDev.Core;

/// <summary>
/// Represents a raw, semantic-free signal dispatched from an input adapter to the state machine.
/// </summary>
/// <remarks>
/// An <see cref="Intent"/> describes <em>what happened</em> (e.g. a button was pressed) without
/// making any judgement about <em>what it means</em>. All interpretation is the exclusive
/// responsibility of <see cref="IVssState"/>.
/// </remarks>
public abstract record Intent;
