using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.Commands.CreateRemuneraciones;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.UpdateRemuneraciones;

/// <summary>
/// Command para actualizar remuneraciones de un empleado (elimina existentes y crea nuevas).
/// Migrado de: EmpleadosService.actualizarRemuneraciones(List<Remuneraciones> rem, int empleadoID) - Line 659
/// </summary>
public record UpdateRemuneracionesCommand(
    string UserId,
    int EmpleadoId,
    List<RemuneracionItemDto> Remuneraciones
) : IRequest<bool>;
