using MediatR;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetPromedioCalificacion;

/// <summary>
/// Query: Obtener promedio y distribución de calificaciones para un contratista/empleado
/// Feature NUEVA: No existe en Legacy (mejora sobre getTodas/getById)
/// </summary>
/// <remarks>
/// Calcula:
/// - Promedio general de ratings
/// - Total de calificaciones
/// - Distribución por estrellas (1-5)
/// - Porcentajes de positivas/negativas
/// 
/// Usado para: 
/// - Dashboard de perfil público
/// - Rating stars display
/// - Analytics de reputación
/// </remarks>
public record GetPromedioCalificacionQuery : IRequest<PromedioCalificacionDto?>
{
    /// <summary>
    /// Identificación del contratista/empleado (RNC o Cédula)
    /// </summary>
    public string Identificacion { get; init; }

    public GetPromedioCalificacionQuery(string identificacion)
    {
        Identificacion = identificacion;
    }
}
