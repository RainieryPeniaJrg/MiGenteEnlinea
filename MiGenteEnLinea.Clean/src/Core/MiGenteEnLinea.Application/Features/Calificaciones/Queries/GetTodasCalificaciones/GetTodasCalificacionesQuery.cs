using MediatR;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetTodasCalificaciones;

/// <summary>
/// Query para obtener todas las calificaciones
/// Migrado desde: CalificacionesService.getTodas() (línea 11)
/// 
/// LEGACY BEHAVIOR:
/// - return db.VCalificaciones.ToList();
/// - Retorna todas las calificaciones usando vista VCalificaciones
/// 
/// CLEAN ARCHITECTURE:
/// - Usa CalificacionVistaDto (placeholder hasta que vista exista)
/// - Retorna lista completa
/// </summary>
public record GetTodasCalificacionesQuery : IRequest<List<CalificacionVistaDto>>;
