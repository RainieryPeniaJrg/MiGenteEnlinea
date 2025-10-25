using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CreateRemuneraciones;

/// <summary>
/// Command para crear múltiples remuneraciones en batch (GAP-008).
/// Implementa guardarOtrasRemuneraciones() del Legacy (EmpleadosService.cs línea 646-654).
/// </summary>
/// <remarks>
/// LÓGICA LEGACY EXACTA:
/// <code>
/// public bool guardarOtrasRemuneraciones(List<Remuneraciones> rem)
/// {
///     using (migenteEntities db = new migenteEntities())
///     {
///         db.Remuneraciones.AddRange(rem);
///         db.SaveChanges();
///         return true;
///     }
/// }
/// </code>
/// 
/// COMPORTAMIENTO LEGACY:
/// - Batch insert de N remuneraciones
/// - Sin validaciones (asume datos válidos del frontend)
/// - Siempre retorna true
/// - No verifica si empleado existe
/// 
/// IMPLEMENTACIÓN DDD:
/// - Usa Remuneracion.Crear() factory method
/// - Valida cada remuneración antes de insertar
/// - Mantiene paridad funcional (return true)
/// - Batch insert optimizado con AddRange()
/// 
/// GAP-008: Batch insert de remuneraciones adicionales
/// </remarks>
public record CreateRemuneracionesCommand : IRequest<bool>
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
    /// Lista de remuneraciones a crear
    /// </summary>
    public List<RemuneracionItemDto> Remuneraciones { get; init; } = new();
}

/// <summary>
/// DTO para item de remuneración
/// </summary>
public class RemuneracionItemDto
{
    public string Descripcion { get; set; } = string.Empty;
    public decimal Monto { get; set; }
}
