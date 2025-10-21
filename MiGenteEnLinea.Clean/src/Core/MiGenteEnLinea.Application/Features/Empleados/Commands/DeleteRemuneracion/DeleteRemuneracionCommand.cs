using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.DeleteRemuneracion;

/// <summary>
/// Command para eliminar una remuneración adicional de un empleado
/// Migrado desde: EmpleadosService.quitarRemuneracion(string userID, int id)
/// Legacy line: 64-74 en EmpleadosService.cs
/// </summary>
public record DeleteRemuneracionCommand(
    string UserId,
    int RemuneracionId
) : IRequest<Unit>;
