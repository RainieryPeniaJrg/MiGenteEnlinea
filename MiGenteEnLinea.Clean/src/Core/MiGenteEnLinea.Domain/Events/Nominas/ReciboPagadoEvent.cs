using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Nominas;

/// <summary>
/// Evento de dominio que se dispara cuando se marca un recibo como pagado.
/// </summary>
public sealed class ReciboPagadoEvent : DomainEvent
{
    public int PagoId { get; }
    public string UserId { get; }
    public int EmpleadoId { get; }
    public decimal NetoPagar { get; }
    public DateTime FechaPago { get; }

    public ReciboPagadoEvent(
        int pagoId,
        string userId,
        int empleadoId,
        decimal netoPagar,
        DateTime fechaPago)
    {
        PagoId = pagoId;
        UserId = userId;
        EmpleadoId = empleadoId;
        NetoPagar = netoPagar;
        FechaPago = fechaPago;
    }
}
