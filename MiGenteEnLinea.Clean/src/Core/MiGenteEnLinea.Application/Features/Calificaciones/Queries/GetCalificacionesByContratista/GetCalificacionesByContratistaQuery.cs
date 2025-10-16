using MediatR;
using MiGenteEnLinea.Application.Common.Models;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetCalificacionesByContratista;

/// <summary>
/// Query: Obtener calificaciones de un contratista/empleado
/// Mapea del Legacy: CalificacionesService.getById()
/// </summary>
public record GetCalificacionesByContratistaQuery : IRequest<PaginatedList<CalificacionDto>>
{
    /// <summary>
    /// Identificación del contratista/empleado (RNC o cédula)
    /// </summary>
    public string Identificacion { get; init; } = string.Empty;

    /// <summary>
    /// Filtro opcional: ID del usuario que calificó (para ver solo calificaciones de un usuario específico)
    /// </summary>
    public string? UserId { get; init; }

    /// <summary>
    /// Número de página (1-based)
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Tamaño de página
    /// </summary>
    public int PageSize { get; init; } = 10;

    /// <summary>
    /// Ordenar por (FechaCreacion, Rating)
    /// </summary>
    public string? OrderBy { get; init; }

    /// <summary>
    /// Dirección de orden (asc, desc)
    /// </summary>
    public string? OrderDirection { get; init; } = "desc";
}
