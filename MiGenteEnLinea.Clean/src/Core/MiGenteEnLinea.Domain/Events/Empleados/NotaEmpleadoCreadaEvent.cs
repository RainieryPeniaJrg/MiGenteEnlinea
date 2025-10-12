using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Empleados;

/// <summary>
/// Evento de dominio que se dispara cuando se crea una nueva nota para un empleado.
/// </summary>
public sealed class NotaEmpleadoCreadaEvent : DomainEvent
{
    public int NotaId { get; }
    public int EmpleadoId { get; }
    public string UserId { get; }
    public string NotaResumen { get; }
    public DateTime FechaCreacion { get; }

    public NotaEmpleadoCreadaEvent(
        int notaId,
        int empleadoId,
        string userId,
        string notaResumen,
        DateTime fechaCreacion)
    {
        NotaId = notaId;
        EmpleadoId = empleadoId;
        UserId = userId;
        NotaResumen = notaResumen;
        FechaCreacion = fechaCreacion;
    }
}
