using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetTodosLosTemporales;

/// <summary>
/// Query para obtener todos los empleados temporales de un usuario
/// Migrado de: EmpleadosService.obtenerTodosLosTemporales (line 526)
/// </summary>
public record GetTodosLosTemporalesQuery : IRequest<List<EmpleadoTemporalDto>>
{
    public string UserId { get; init; } = string.Empty;
}
