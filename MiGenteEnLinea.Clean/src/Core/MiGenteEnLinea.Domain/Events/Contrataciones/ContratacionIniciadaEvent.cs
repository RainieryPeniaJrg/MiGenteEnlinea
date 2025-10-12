using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contrataciones;

/// <summary>
/// Evento de dominio que se dispara cuando inicia el trabajo de una contratación.
/// </summary>
public sealed class ContratacionIniciadaEvent : DomainEvent
{
    public int DetalleId { get; }
    public DateTime FechaInicioReal { get; }

    public ContratacionIniciadaEvent(int detalleId, DateTime fechaInicioReal)
    {
        DetalleId = detalleId;
        FechaInicioReal = fechaInicioReal;
    }
}
