using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Empleados;

/// <summary>
/// Evento de dominio que se dispara cuando se desactiva un empleado (baja).
/// </summary>
public sealed class EmpleadoDesactivadoEvent : DomainEvent
{
    public int EmpleadoId { get; }
    public string UserId { get; }
    public string NombreCompleto { get; }
    public DateTime FechaSalida { get; }
    public string MotivoBaja { get; }

    public EmpleadoDesactivadoEvent(
        int empleadoId,
        string userId,
        string nombreCompleto,
        DateTime fechaSalida,
        string motivoBaja)
    {
        EmpleadoId = empleadoId;
        UserId = userId;
        NombreCompleto = nombreCompleto;
        FechaSalida = fechaSalida;
        MotivoBaja = motivoBaja;
    }
}
