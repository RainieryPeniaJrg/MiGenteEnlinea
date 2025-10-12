using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento que se dispara cuando se agrega una línea de detalle a un recibo
/// </summary>
public sealed class DetalleReciboAgregadoEvent : DomainEvent
{
    public int DetalleId { get; }
    public int? PagoId { get; }
    public string? Concepto { get; }
    public decimal? Monto { get; }

    public DetalleReciboAgregadoEvent(
        int detalleId,
        int? pagoId,
        string? concepto,
        decimal? monto)
    {
        DetalleId = detalleId;
        PagoId = pagoId;
        Concepto = concepto;
        Monto = monto;
    }
}
