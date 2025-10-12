using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Nominas;

/// <summary>
/// Evento de dominio que se dispara cuando se genera un nuevo recibo de pago.
/// </summary>
public sealed class ReciboGeneradoEvent : DomainEvent
{
    public int PagoId { get; }
    public string UserId { get; }
    public int EmpleadoId { get; }
    public string ConceptoPago { get; }
    public DateTime FechaRegistro { get; }

    public ReciboGeneradoEvent(
        int pagoId,
        string userId,
        int empleadoId,
        string conceptoPago,
        DateTime fechaRegistro)
    {
        PagoId = pagoId;
        UserId = userId;
        EmpleadoId = empleadoId;
        ConceptoPago = conceptoPago;
        FechaRegistro = fechaRegistro;
    }
}
