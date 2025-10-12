using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contrataciones;

/// <summary>
/// Evento de dominio que se dispara cuando un contratista acepta una propuesta de contratación.
/// </summary>
public sealed class ContratacionAceptadaEvent : DomainEvent
{
    public int DetalleId { get; }
    public decimal MontoAcordado { get; }

    public ContratacionAceptadaEvent(int detalleId, decimal montoAcordado)
    {
        DetalleId = detalleId;
        MontoAcordado = montoAcordado;
    }
}
