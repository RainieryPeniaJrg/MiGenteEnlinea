using MediatR;
using MiGenteEnLinea.Application.Common.Models;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetEmpleadosByEmpleador;

/// <summary>
/// Query para obtener listado paginado de empleados de un empleador.
/// Legacy: EmpleadosService.getEmpleados(userID) + getVEmpleados(userID)
/// Soporta búsqueda y filtrado por estado activo.
/// </summary>
public record GetEmpleadosByEmpleadorQuery : IRequest<PaginatedList<EmpleadoListDto>>
{
    public string UserId { get; init; } = string.Empty;
    public int PageIndex { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; }
    public bool? SoloActivos { get; init; } = true;
}
