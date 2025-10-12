using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento que se dispara cuando se actualiza el concepto de un recibo de pago
/// </summary>
public sealed class ConceptoPagoActualizadoEvent : DomainEvent
{
    public int PagoId { get; }
    public string? ConceptoAnterior { get; }
    public string? ConceptoNuevo { get; }

    public ConceptoPagoActualizadoEvent(int pagoId, string? conceptoAnterior, string? conceptoNuevo)
    {
        PagoId = pagoId;
        ConceptoAnterior = conceptoAnterior;
        ConceptoNuevo = conceptoNuevo;
    }
}
