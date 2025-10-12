using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento lanzado cuando se crea una nueva venta (transacción iniciada).
/// </summary>
public sealed class VentaCreadaEvent : DomainEvent
{
    public int VentaId { get; }
    public string UserId { get; }
    public int PlanId { get; }
    public decimal Precio { get; }
    public string IdempotencyKey { get; }

    public VentaCreadaEvent(int ventaId, string userId, int planId, decimal precio, string idempotencyKey)
    {
        VentaId = ventaId;
        UserId = userId;
        PlanId = planId;
        Precio = precio;
        IdempotencyKey = idempotencyKey;
    }
}
