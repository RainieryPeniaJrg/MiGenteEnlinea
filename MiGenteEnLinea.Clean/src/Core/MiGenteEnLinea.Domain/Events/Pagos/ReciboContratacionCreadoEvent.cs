using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento que se dispara cuando se crea un nuevo recibo de pago por contratación
/// </summary>
public sealed class ReciboContratacionCreadoEvent : DomainEvent
{
    public int PagoId { get; }
    public string UserId { get; }
    public int? ContratacionId { get; }
    public string? ConceptoPago { get; }
    public int? Tipo { get; }

    public ReciboContratacionCreadoEvent(
        int pagoId,
        string userId,
        int? contratacionId,
        string? conceptoPago,
        int? tipo)
    {
        PagoId = pagoId;
        UserId = userId;
        ContratacionId = contratacionId;
        ConceptoPago = conceptoPago;
        Tipo = tipo;
    }
}
