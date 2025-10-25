using MediatR;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.CancelarTrabajo;

/// <summary>
/// Command para cancelar un trabajo/contratación.
/// Implementa cancelarTrabajo() del Legacy (EmpleadosService.cs línea 233-245).
/// </summary>
/// <remarks>
/// LÓGICA LEGACY EXACTA:
/// <code>
/// public bool cancelarTrabajo(int contratacionID, int detalleID)
/// {
///     using (var db = new migenteEntities())
///     {
///         DetalleContrataciones detalle = db.DetalleContrataciones
///             .Where(x => x.contratacionID == contratacionID && x.detalleID == detalleID)
///             .FirstOrDefault();
///         
///         if (detalle != null)
///         {
///             detalle.estatus = 3; // ⚠️ Legacy usa 3 para "Cancelada"
///             db.SaveChanges();
///         }
///         
///         return true;
///     }
/// }
/// </code>
/// 
/// IMPORTANTE - DISCREPANCIA ESTATUS:
/// - Legacy: estatus = 3 significa "Cancelada"
/// - Domain DDD: estatus = 3 es ESTADO_EN_PROGRESO, estatus = 5 es ESTADO_CANCELADA
/// 
/// Para mantener paridad con Legacy, este Command usa estatus = 3.
/// En una futura refactorización, debería usarse DetalleContratacion.Cancelar() con estatus = 5.
/// 
/// GAP-006: Simple update de estatus, no requiere motivo (a diferencia del DDD).
/// </remarks>
public record CancelarTrabajoCommand : IRequest<bool>
{
    /// <summary>
    /// ID de la contratación (FK a EmpleadosTemporales)
    /// </summary>
    public int ContratacionId { get; init; }

    /// <summary>
    /// ID del detalle de contratación
    /// </summary>
    public int DetalleId { get; init; }
}
