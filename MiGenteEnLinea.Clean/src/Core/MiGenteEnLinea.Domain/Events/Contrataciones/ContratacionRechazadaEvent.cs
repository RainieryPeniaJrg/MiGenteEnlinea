using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contrataciones;

/// <summary>
/// Evento de dominio que se dispara cuando un contratista rechaza una propuesta de contratación.
/// </summary>
public sealed class ContratacionRechazadaEvent : DomainEvent
{
    public int DetalleId { get; }
    public string Motivo { get; }

    public ContratacionRechazadaEvent(int detalleId, string motivo)
    {
        DetalleId = detalleId;
        Motivo = motivo;
    }
}
