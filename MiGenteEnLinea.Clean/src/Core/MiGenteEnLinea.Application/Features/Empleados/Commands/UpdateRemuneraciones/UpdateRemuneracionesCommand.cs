using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.Commands.CreateRemuneraciones;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.UpdateRemuneraciones;

/// <summary>
/// Command para actualizar remuneraciones de un empleado (GAP-009).
/// Implementa actualizarRemuneraciones() del Legacy (EmpleadosService.cs línea 657-676).
/// </summary>
/// <remarks>
/// LÓGICA LEGACY EXACTA:
/// <code>
/// public bool actualizarRemuneraciones(List<Remuneraciones> rem, int empleadoID)
/// {
///     // DbContext 1: Delete existing
///     using (migenteEntities db = new migenteEntities())
///     {
///         var result = db.Remuneraciones.Where(x => x.empleadoID == empleadoID).FirstOrDefault();
///         if (result != null)
///         {
///             db.Remuneraciones.Remove(result);
///             db.SaveChanges();
///         }
///     }
///     
///     // DbContext 2: Insert new
///     using (migenteEntities db1 = new migenteEntities())
///     {
///         db1.Remuneraciones.AddRange(rem);
///         db1.SaveChanges();
///         return true;
///     }
/// }
/// </code>
/// 
/// ⚠️ BUG EN LEGACY:
/// - Usa Where().FirstOrDefault() que solo elimina la PRIMERA remuneración
/// - Debería usar Where().ToList() para eliminar TODAS las remuneraciones
/// - Por paridad Legacy, replicamos el bug (solo elimina primera)
/// - TODO: En producción, corregir a RemoveRange(Where().ToList())
/// 
/// COMPORTAMIENTO LEGACY:
/// - Elimina solo la primera remuneración del empleado (BUG)
/// - Luego inserta todas las nuevas remuneraciones
/// - Usa 2 DbContext separados (anti-pattern)
/// - Siempre retorna true
/// 
/// IMPLEMENTACIÓN DDD:
/// - Usa Remuneracion.Crear() factory method
/// - Transacción única (vs 2 DbContext Legacy)
/// - Mantiene bug Legacy por paridad (solo elimina primera)
/// - Logging estructurado
/// 
/// GAP-009: Replace strategy (delete + insert)
/// </remarks>
public record UpdateRemuneracionesCommand : IRequest<bool>
{
    /// <summary>
    /// ID del usuario empleador
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// ID del empleado
    /// </summary>
    public int EmpleadoId { get; init; }

    /// <summary>
    /// Lista de nuevas remuneraciones (reemplazan las existentes)
    /// </summary>
    public List<RemuneracionItemDto> Remuneraciones { get; init; } = new();
}
