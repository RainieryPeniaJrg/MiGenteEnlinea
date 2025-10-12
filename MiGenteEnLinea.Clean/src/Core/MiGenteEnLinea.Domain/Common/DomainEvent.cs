namespace MiGenteEnLinea.Domain.Common;

/// <summary>
/// Clase base para eventos de dominio.
/// Representa algo significativo que ocurrió en el dominio del negocio.
/// </summary>
public abstract class DomainEvent
{
    /// <summary>
    /// Identificador único del evento
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Momento en que ocurrió el evento (UTC)
    /// </summary>
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
