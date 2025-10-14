using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetRemuneracionesByEmpleado;

/// <summary>
/// Query para obtener las remuneraciones extras de un empleado.
/// Legacy: EmpleadosService.obtenerRemuneraciones(userID, empleadoID)
/// </summary>
/// <remarks>
/// Retorna los 3 slots de remuneraciones.
/// Los slots vacíos tendrán Descripcion y Monto = null.
/// </remarks>
public record GetRemuneracionesByEmpleadoQuery : IRequest<List<RemuneracionDto>>
{
    /// <summary>
    /// Identificador del empleador (para validar propiedad del empleado).
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Identificador del empleado del cual se obtendrán las remuneraciones.
    /// </summary>
    public int EmpleadoId { get; init; }
}
