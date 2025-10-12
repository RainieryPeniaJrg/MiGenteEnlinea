using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento lanzado cuando se activa el modo test en la pasarela de pagos.
/// </summary>
public sealed class PaymentGatewayModoTestActivadoEvent : DomainEvent
{
    public int GatewayId { get; }

    public PaymentGatewayModoTestActivadoEvent(int gatewayId)
    {
        GatewayId = gatewayId;
    }
}
