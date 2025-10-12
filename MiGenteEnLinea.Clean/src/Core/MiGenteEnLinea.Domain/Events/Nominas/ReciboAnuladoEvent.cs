using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Nominas;

/// <summary>
/// Evento de dominio que se dispara cuando se anula un recibo de pago.
/// </summary>
public sealed class ReciboAnuladoEvent : DomainEvent
{
    public int PagoId { get; }
    public string UserId { get; }
    public int EmpleadoId { get; }
    public string MotivoAnulacion { get; }
    public DateTime FechaAnulacion { get; }

    public ReciboAnuladoEvent(
        int pagoId,
        string userId,
        int empleadoId,
        string motivoAnulacion,
        DateTime fechaAnulacion)
    {
        PagoId = pagoId;
        UserId = userId;
        EmpleadoId = empleadoId;
        MotivoAnulacion = motivoAnulacion;
        FechaAnulacion = fechaAnulacion;
    }
}
