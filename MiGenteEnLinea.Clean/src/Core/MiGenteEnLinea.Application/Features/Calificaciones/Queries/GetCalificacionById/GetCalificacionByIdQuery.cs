using MediatR;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetCalificacionById;

/// <summary>
/// Query: Obtener una calificación específica por su ID
/// Mapea a: CalificacionesService.getCalificacionByID(calificacionID)
/// </summary>
/// <remarks>
/// Legacy behavior: 
/// - db.Calificaciones.Where(x => x.calificacionID == calificacionID)
///   .OrderByDescending(x => x.calificacionID).FirstOrDefault()
/// 
/// Returns: CalificacionDto si existe, null si no encuentra
/// </remarks>
public record GetCalificacionByIdQuery : IRequest<CalificacionDto?>
{
    /// <summary>
    /// ID de la calificación a buscar
    /// </summary>
    public int CalificacionId { get; init; }

    public GetCalificacionByIdQuery(int calificacionId)
    {
        CalificacionId = calificacionId;
    }
}
