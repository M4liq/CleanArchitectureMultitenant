namespace Application.Common;

/// <summary>
/// For internal application logic that's meant to be used only within the application layer.
/// Use mediator for cross-boundary operations (public API).
/// Use bounded services for separating internal application logic.
/// Bounded services are registered automatically in DI through assembly scanning.
/// Validation should be done on mediator level.
/// </summary>
public interface IBoundedService {}