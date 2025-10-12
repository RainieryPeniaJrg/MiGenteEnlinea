using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Empleados;

/// <summary>
/// Evento de dominio que se dispara cuando se crea un nuevo empleado.
/// </summary>
public sealed class EmpleadoCreadoEvent : DomainEvent
{
    public int EmpleadoId { get; }
    public string UserId { get; }
    public string NombreCompleto { get; }
    public string Identificacion { get; }

    public EmpleadoCreadoEvent(
        int empleadoId,
        string userId,
        string nombreCompleto,
        string identificacion)
    {
        EmpleadoId = empleadoId;
        UserId = userId;
        NombreCompleto = nombreCompleto;
        Identificacion = identificacion;
    }
}
