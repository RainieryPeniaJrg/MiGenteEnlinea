using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento lanzado cuando se crea una nueva configuración de pasarela de pagos.
/// </summary>
public sealed class PaymentGatewayCreadoEvent : DomainEvent
{
    public int GatewayId { get; }
    public string MerchantId { get; }
    public bool ModoTest { get; }

    public PaymentGatewayCreadoEvent(int gatewayId, string merchantId, bool modoTest)
    {
        GatewayId = gatewayId;
        MerchantId = merchantId;
        ModoTest = modoTest;
    }
}
