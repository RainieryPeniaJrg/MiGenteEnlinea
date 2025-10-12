using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Empleados;

/// <summary>
/// Evento de dominio que se dispara cuando se elimina una nota de un empleado.
/// </summary>
public sealed class NotaEmpleadoEliminadaEvent : DomainEvent
{
    public int NotaId { get; }
    public int EmpleadoId { get; }
    public string UserId { get; }
    public DateTime FechaEliminacion { get; }

    public NotaEmpleadoEliminadaEvent(
        int notaId,
        int empleadoId,
        string userId,
        DateTime fechaEliminacion)
    {
        NotaId = notaId;
        EmpleadoId = empleadoId;
        UserId = userId;
        FechaEliminacion = fechaEliminacion;
    }
}
