using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento lanzado cuando se procesa un reembolso para una venta.
/// </summary>
public sealed class VentaReembolsadaEvent : DomainEvent
{
    public int VentaId { get; }
    public string UserId { get; }
    public int PlanId { get; }
    public decimal MontoReembolsado { get; }
    public string MotivoReembolso { get; }

    public VentaReembolsadaEvent(int ventaId, string userId, int planId, decimal montoReembolsado, string motivoReembolso)
    {
        VentaId = ventaId;
        UserId = userId;
        PlanId = planId;
        MontoReembolsado = montoReembolsado;
        MotivoReembolso = motivoReembolso;
    }
}
