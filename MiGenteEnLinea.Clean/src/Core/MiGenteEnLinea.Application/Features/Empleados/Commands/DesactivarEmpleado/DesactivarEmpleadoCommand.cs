using MediatR;
using System;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.DesactivarEmpleado;

/// <summary>
/// Command para dar de baja (desactivar) un empleado permanente.
/// Legacy: EmpleadosService.darDeBaja(empleadoID, userID, fechaBaja, prestaciones, motivo)
/// Implementa soft delete: Activo=false, guarda metadata de baja.
/// </summary>
public record DesactivarEmpleadoCommand : IRequest<bool>
{
    public int EmpleadoId { get; init; }
    public string UserId { get; init; } = string.Empty;
    public DateTime FechaBaja { get; init; }
    public decimal Prestaciones { get; init; }
    public string MotivoBaja { get; init; } = string.Empty;
}
