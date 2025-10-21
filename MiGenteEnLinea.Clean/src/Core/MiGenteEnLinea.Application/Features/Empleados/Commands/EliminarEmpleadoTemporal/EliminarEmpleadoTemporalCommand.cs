using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.EliminarEmpleadoTemporal;

/// <summary>
/// Command para eliminar un empleado temporal y sus recibos asociados (cascade delete).
/// Migrado desde: EmpleadosService.eliminarEmpleadoTemporal(int contratacionID) - line 298
/// </summary>
/// <param name="ContratacionId">ID del empleado temporal a eliminar</param>
public record EliminarEmpleadoTemporalCommand(
    int ContratacionId
) : IRequest<bool>;
