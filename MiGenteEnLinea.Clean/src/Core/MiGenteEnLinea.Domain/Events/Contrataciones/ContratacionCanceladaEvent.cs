using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contrataciones;

/// <summary>
/// Evento de dominio que se dispara cuando una contratación es cancelada.
/// </summary>
public sealed class ContratacionCanceladaEvent : DomainEvent
{
    public int DetalleId { get; }
    public string Motivo { get; }

    public ContratacionCanceladaEvent(int detalleId, string motivo)
    {
        DetalleId = detalleId;
        Motivo = motivo;
    }
}
