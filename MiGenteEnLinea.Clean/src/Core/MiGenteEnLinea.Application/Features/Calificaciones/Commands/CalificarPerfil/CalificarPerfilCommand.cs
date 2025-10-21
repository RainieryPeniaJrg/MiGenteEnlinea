using MediatR;
using MiGenteEnLinea.Domain.Entities.Calificaciones;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Commands.CalificarPerfil;

/// <summary>
/// Command para crear una nueva calificación de perfil
/// </summary>
/// <remarks>
/// Migrado desde: CalificacionesService.calificarPerfil(Calificaciones cal) (línea 50)
/// 
/// LEGACY BEHAVIOR:
/// - db.Calificaciones.Add(cal); db.SaveChanges(); return cal;
/// - Simple insert
/// - Retorna entity completa con ID generado
/// 
/// CLEAN ARCHITECTURE:
/// - Usa factory method del Domain: Calificacion.Create()
/// - Retorna ID generado
/// - Validación con FluentValidation
/// 
/// ESTRUCTURA LEGACY:
/// - fecha (DateTime)
/// - userID (string) → EmpleadorUserId
/// - tipo (string) → fijo "Contratista" (legacy)
/// - identificacion (string) → ContratistaIdentificacion
/// - nombre (string) → ContratistaNombre
/// - puntualidad (int) → 1-5
/// - cumplimiento (int) → 1-5
/// - conocimientos (int) → 1-5
/// - recomendacion (int) → 1-5
/// </remarks>
public record CalificarPerfilCommand(
    string EmpleadorUserId,
    string ContratistaIdentificacion,
    string ContratistaNombre,
    int Puntualidad,
    int Cumplimiento,
    int Conocimientos,
    int Recomendacion
) : IRequest<int>;
