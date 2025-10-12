using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento que se dispara cuando se actualiza el monto de una línea de detalle
/// </summary>
public sealed class MontoDetalleActualizadoEvent : DomainEvent
{
    public int DetalleId { get; }
    public int? PagoId { get; }
    public decimal? MontoAnterior { get; }
    public decimal MontoNuevo { get; }

    public MontoDetalleActualizadoEvent(
        int detalleId,
        int? pagoId,
        decimal? montoAnterior,
        decimal montoNuevo)
    {
        DetalleId = detalleId;
        PagoId = pagoId;
        MontoAnterior = montoAnterior;
        MontoNuevo = montoNuevo;
    }
}
