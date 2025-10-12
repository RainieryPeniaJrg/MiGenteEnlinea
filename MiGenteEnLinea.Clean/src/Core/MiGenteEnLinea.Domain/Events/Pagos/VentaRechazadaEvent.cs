using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento lanzado cuando una venta es rechazada por la pasarela de pagos.
/// </summary>
public sealed class VentaRechazadaEvent : DomainEvent
{
    public int VentaId { get; }
    public string UserId { get; }
    public string MotivoRechazo { get; }

    public VentaRechazadaEvent(int ventaId, string userId, string motivoRechazo)
    {
        VentaId = ventaId;
        UserId = userId;
        MotivoRechazo = motivoRechazo;
    }
}
