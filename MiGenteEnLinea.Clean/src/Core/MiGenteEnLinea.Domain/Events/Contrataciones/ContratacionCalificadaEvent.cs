using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contrataciones;

/// <summary>
/// Evento de dominio que se dispara cuando un empleador califica una contratación completada.
/// </summary>
public sealed class ContratacionCalificadaEvent : DomainEvent
{
    public int DetalleId { get; }
    public int CalificacionId { get; }

    public ContratacionCalificadaEvent(int detalleId, int calificacionId)
    {
        DetalleId = detalleId;
        CalificacionId = calificacionId;
    }
}
