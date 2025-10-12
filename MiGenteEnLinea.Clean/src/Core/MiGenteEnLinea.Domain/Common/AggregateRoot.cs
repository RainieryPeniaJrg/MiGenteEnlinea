namespace MiGenteEnLinea.Domain.Common;

/// <summary>
/// Raíz de agregado según DDD.
/// Maneja una colección de eventos de dominio para comunicación entre agregados.
/// </summary>
public abstract class AggregateRoot : AuditableEntity
{
    private readonly List<DomainEvent> _events = new();

    /// <summary>
    /// Eventos de dominio pendientes de procesamiento
    /// </summary>
    public IReadOnlyList<DomainEvent> Events => _events.AsReadOnly();

    /// <summary>
    /// Registra un nuevo evento de dominio
    /// </summary>
    protected void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }

    /// <summary>
    /// Limpia la lista de eventos después de procesarlos
    /// </summary>
    public void ClearEvents()
    {
        _events.Clear();
    }
}
