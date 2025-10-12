using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento que se dispara cuando se actualiza un campo de una línea de detalle de recibo
/// </summary>
public sealed class DetalleReciboActualizadoEvent : DomainEvent
{
    public int DetalleId { get; }
    public int? PagoId { get; }
    public string Campo { get; }
    public string? ValorAnterior { get; }
    public string? ValorNuevo { get; }

    public DetalleReciboActualizadoEvent(
        int detalleId,
        int? pagoId,
        string campo,
        string? valorAnterior,
        string? valorNuevo)
    {
        DetalleId = detalleId;
        PagoId = pagoId;
        Campo = campo;
        ValorAnterior = valorAnterior;
        ValorNuevo = valorNuevo;
    }
}
