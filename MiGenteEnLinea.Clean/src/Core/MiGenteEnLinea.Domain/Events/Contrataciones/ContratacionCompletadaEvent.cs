using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contrataciones;

/// <summary>
/// Evento de dominio que se dispara cuando una contratación se marca como completada.
/// </summary>
public sealed class ContratacionCompletadaEvent : DomainEvent
{
    public int DetalleId { get; }
    public DateTime FechaFinalizacionReal { get; }
    public decimal MontoAcordado { get; }

    public ContratacionCompletadaEvent(
        int detalleId,
        DateTime fechaFinalizacionReal,
        decimal montoAcordado)
    {
        DetalleId = detalleId;
        FechaFinalizacionReal = fechaFinalizacionReal;
        MontoAcordado = montoAcordado;
    }
}
