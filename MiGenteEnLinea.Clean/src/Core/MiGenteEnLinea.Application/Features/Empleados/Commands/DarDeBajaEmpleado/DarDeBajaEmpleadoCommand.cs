using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.DarDeBajaEmpleado;

/// <summary>
/// Command para dar de baja a un empleado.
/// Migrado desde: EmpleadosService.darDeBaja(int empleadoID, string userID, DateTime fechaBaja, decimal prestaciones, string motivo)
/// </summary>
public record DarDeBajaEmpleadoCommand(
    int EmpleadoId,
    string UserId,
    DateTime FechaBaja,
    decimal Prestaciones,
    string Motivo
) : IRequest<bool>;
