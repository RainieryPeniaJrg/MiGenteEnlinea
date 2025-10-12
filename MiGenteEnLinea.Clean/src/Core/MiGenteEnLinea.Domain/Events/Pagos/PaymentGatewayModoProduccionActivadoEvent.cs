using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento lanzado cuando se activa el modo producción en la pasarela de pagos.
/// </summary>
public sealed class PaymentGatewayModoProduccionActivadoEvent : DomainEvent
{
    public int GatewayId { get; }
    public string MerchantId { get; }

    public PaymentGatewayModoProduccionActivadoEvent(int gatewayId, string merchantId)
    {
        GatewayId = gatewayId;
        MerchantId = merchantId;
    }
}
