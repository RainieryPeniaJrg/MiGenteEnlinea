using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Nominas;

/// <summary>
/// Evento de dominio que se dispara cuando se modifica el monto de un detalle de recibo.
/// </summary>
public sealed class MontoDetalleModificadoEvent : DomainEvent
{
    public int DetalleId { get; }
    public int PagoId { get; }
    public string Concepto { get; }
    public decimal MontoAnterior { get; }
    public decimal MontoNuevo { get; }

    public MontoDetalleModificadoEvent(
        int detalleId,
        int pagoId,
        string concepto,
        decimal montoAnterior,
        decimal montoNuevo)
    {
        DetalleId = detalleId;
        PagoId = pagoId;
        Concepto = concepto;
        MontoAnterior = montoAnterior;
        MontoNuevo = montoNuevo;
    }
}
