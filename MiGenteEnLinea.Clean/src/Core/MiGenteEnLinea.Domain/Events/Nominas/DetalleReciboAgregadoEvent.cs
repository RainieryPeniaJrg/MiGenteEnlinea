using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Nominas;

/// <summary>
/// Evento de dominio que se dispara cuando se agrega un detalle a un recibo.
/// </summary>
public sealed class DetalleReciboAgregadoEvent : DomainEvent
{
    public int DetalleId { get; }
    public int PagoId { get; }
    public string Concepto { get; }
    public decimal Monto { get; }
    public int TipoConcepto { get; }

    public DetalleReciboAgregadoEvent(
        int detalleId,
        int pagoId,
        string concepto,
        decimal monto,
        int tipoConcepto)
    {
        DetalleId = detalleId;
        PagoId = pagoId;
        Concepto = concepto;
        Monto = monto;
        TipoConcepto = tipoConcepto;
    }
}
