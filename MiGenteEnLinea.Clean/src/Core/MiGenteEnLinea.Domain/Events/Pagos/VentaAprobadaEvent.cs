using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento lanzado cuando una venta es aprobada por la pasarela de pagos.
/// </summary>
public sealed class VentaAprobadaEvent : DomainEvent
{
    public int VentaId { get; }
    public string UserId { get; }
    public int PlanId { get; }
    public decimal Precio { get; }
    public string IdTransaccion { get; }

    public VentaAprobadaEvent(int ventaId, string userId, int planId, decimal precio, string idTransaccion)
    {
        VentaId = ventaId;
        UserId = userId;
        PlanId = planId;
        Precio = precio;
        IdTransaccion = idTransaccion;
    }
}
