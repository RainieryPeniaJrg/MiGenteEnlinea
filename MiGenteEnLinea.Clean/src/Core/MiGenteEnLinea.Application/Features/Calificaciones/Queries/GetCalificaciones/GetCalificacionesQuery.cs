using MediatR;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetCalificaciones;

/// <summary>
/// Query para obtener calificaciones por identificación
/// </summary>
/// <remarks>
/// Migrado desde: CalificacionesService.getById(string id, string userID = null) (línea 18)
/// 
/// LEGACY BEHAVIOR:
/// - db.VCalificaciones.Where(x => x.identificacion == id && x.userID == userID).OrderByDescending(x => x.calificacionID).ToList()
/// - Si userID es null, solo filtra por identificacion
/// - Retorna lista ordenada descendente por calificacionID
/// </remarks>
public record GetCalificacionesQuery(
    string Identificacion,
    string? UserId = null
) : IRequest<List<CalificacionVistaDto>>;
