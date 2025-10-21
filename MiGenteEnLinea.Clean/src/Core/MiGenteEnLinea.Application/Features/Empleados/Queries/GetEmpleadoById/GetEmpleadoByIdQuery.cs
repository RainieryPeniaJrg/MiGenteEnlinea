using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetEmpleadoById;

/// <summary>
/// Query para obtener un empleado por ID con toda su información detallada.
/// Legacy: EmpleadosService.getEmpleadosByID(userID, id)
/// </summary>
public record GetEmpleadoByIdQuery(string UserId, int EmpleadoId) : IRequest<EmpleadoDetalleDto?>;
