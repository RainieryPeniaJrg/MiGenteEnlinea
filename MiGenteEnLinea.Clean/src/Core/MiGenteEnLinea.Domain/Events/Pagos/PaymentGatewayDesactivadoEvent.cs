using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento lanzado cuando se desactiva la pasarela de pagos.
/// </summary>
public sealed class PaymentGatewayDesactivadoEvent : DomainEvent
{
    public int GatewayId { get; }

    public PaymentGatewayDesactivadoEvent(int gatewayId)
    {
        GatewayId = gatewayId;
    }
}
