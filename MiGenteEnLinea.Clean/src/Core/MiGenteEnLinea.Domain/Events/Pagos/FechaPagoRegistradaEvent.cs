using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento que se dispara cuando se registra la fecha de pago de un recibo
/// </summary>
public sealed class FechaPagoRegistradaEvent : DomainEvent
{
    public int PagoId { get; }
    public DateTime? FechaAnterior { get; }
    public DateTime FechaNueva { get; }

    public FechaPagoRegistradaEvent(int pagoId, DateTime? fechaAnterior, DateTime fechaNueva)
    {
        PagoId = pagoId;
        FechaAnterior = fechaAnterior;
        FechaNueva = fechaNueva;
    }
}
