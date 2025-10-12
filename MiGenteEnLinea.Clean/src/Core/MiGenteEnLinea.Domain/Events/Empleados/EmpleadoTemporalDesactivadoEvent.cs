using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Empleados;

/// <summary>
/// Evento de dominio que se dispara cuando se desactiva un empleado temporal.
/// </summary>
public sealed class EmpleadoTemporalDesactivadoEvent : DomainEvent
{
    public int ContratacionId { get; }
    public string UserId { get; }
    public string NombreCompleto { get; }
    public DateTime FechaDesactivacion { get; }

    public EmpleadoTemporalDesactivadoEvent(
        int contratacionId,
        string userId,
        string nombreCompleto,
        DateTime fechaDesactivacion)
    {
        ContratacionId = contratacionId;
        UserId = userId;
        NombreCompleto = nombreCompleto;
        FechaDesactivacion = fechaDesactivacion;
    }
}
