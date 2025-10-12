using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Empleados;

/// <summary>
/// Evento de dominio que se dispara cuando se actualiza el salario de un empleado.
/// </summary>
public sealed class SalarioActualizadoEvent : DomainEvent
{
    public int EmpleadoId { get; }
    public string UserId { get; }
    public string NombreCompleto { get; }
    public decimal SalarioAnterior { get; }
    public decimal SalarioNuevo { get; }
    public DateTime FechaActualizacion { get; }

    public SalarioActualizadoEvent(
        int empleadoId,
        string userId,
        string nombreCompleto,
        decimal salarioAnterior,
        decimal salarioNuevo,
        DateTime fechaActualizacion)
    {
        EmpleadoId = empleadoId;
        UserId = userId;
        NombreCompleto = nombreCompleto;
        SalarioAnterior = salarioAnterior;
        SalarioNuevo = salarioNuevo;
        FechaActualizacion = fechaActualizacion;
    }
}
