using MediatR;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.EliminarEmpleadoTemporal;

/// <summary>
/// Command para eliminar un empleado temporal y sus datos relacionados (cascade delete).
/// Implementa eliminarEmpleadoTemporal() del Legacy (EmpleadosService.cs línea 299-357).
/// </summary>
/// <remarks>
/// LÓGICA LEGACY EXACTA:
/// <code>
/// public bool eliminarEmpleadoTemporal(int contratacionID)
/// {
///     // 1. Buscar EmpleadoTemporal
///     var tmp = db.EmpleadosTemporales.Where(a => a.contratacionID == contratacionID).FirstOrDefault();
///     
///     if (tmp != null)
///     {
///         // 2. Para cada recibo asociado:
///         foreach (var recibos in tmp.Empleador_Recibos_Header_Contrataciones)
///         {
///             // 2a. Eliminar detalles de recibos
///             var detallesAEliminar = db.Empleador_Recibos_Detalle_Contrataciones
///                 .Where(d => d.pagoID == recibos.pagoID);
///             db.Empleador_Recibos_Detalle_Contrataciones.RemoveRange(detallesAEliminar);
///             db.SaveChanges();
///             
///             // 2b. Eliminar header de recibos
///             db.Empleador_Recibos_Header_Contrataciones.Remove(recibos);
///             db.SaveChanges();
///         }
///         
///         // 3. Eliminar EmpleadoTemporal
///         db.EmpleadosTemporales.Remove(tmp);
///         db.SaveChanges();
///     }
///     
///     return true;
/// }
/// </code>
/// 
/// COMPORTAMIENTO LEGACY:
/// - Hard delete (no soft delete)
/// - Cascade manual en este orden:
///   1. Empleador_Recibos_Detalle_Contrataciones (detalles)
///   2. Empleador_Recibos_Header_Contrataciones (headers)
///   3. EmpleadosTemporales (root)
/// - Múltiples DbContexts (anti-pattern pero funcional)
/// - Siempre retorna true
/// 
/// IMPLEMENTACIÓN DDD:
/// - Usa transacción única para mantener atomicidad
/// - Delete cascade manual respetando orden de dependencias
/// - No usa navigation properties (DDD puro con shadow properties)
/// - Respeta configuración Fluent API (DeleteBehavior.Restrict)
/// 
/// GAP-007: Delete cascade completo de empleado temporal
/// </remarks>
public record EliminarEmpleadoTemporalCommand : IRequest<bool>
{
    /// <summary>
    /// ID de la contratación temporal a eliminar
    /// </summary>
    public int ContratacionId { get; init; }
}
