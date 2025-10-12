using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento que se dispara cuando se actualiza el tipo de pago de un recibo
/// </summary>
public sealed class TipoPagoActualizadoEvent : DomainEvent
{
    public int PagoId { get; }
    public int? TipoAnterior { get; }
    public int TipoNuevo { get; }

    public TipoPagoActualizadoEvent(int pagoId, int? tipoAnterior, int tipoNuevo)
    {
        PagoId = pagoId;
        TipoAnterior = tipoAnterior;
        TipoNuevo = tipoNuevo;
    }
}
