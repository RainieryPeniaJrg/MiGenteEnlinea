using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento lanzado cuando se actualizan las credenciales de la pasarela de pagos.
/// </summary>
public sealed class PaymentGatewayCredencialesActualizadasEvent : DomainEvent
{
    public int GatewayId { get; }
    public string NuevoMerchantId { get; }
    public string NuevoTerminalId { get; }

    public PaymentGatewayCredencialesActualizadasEvent(int gatewayId, string nuevoMerchantId, string nuevoTerminalId)
    {
        GatewayId = gatewayId;
        NuevoMerchantId = nuevoMerchantId;
        NuevoTerminalId = nuevoTerminalId;
    }
}
