using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Suscripciones;

/// <summary>
/// Repositorio para PlanEmpleador
/// </summary>
/// <remarks>
/// Planes de suscripción para empleadores.
/// Operaciones: Consulta de planes activos, búsqueda por ID
/// </remarks>
public interface IPlanEmpleadorRepository : IRepository<PlanEmpleador>
{
    // ============================================
    // BÚSQUEDAS ESPECÍFICAS
    // ============================================

    /// <summary>
    /// Obtiene todos los planes activos ordenados por precio
    /// </summary>
    Task<IEnumerable<PlanEmpleador>> GetActivosAsync(CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los planes (activos e inactivos) ordenados por precio
    /// </summary>
    Task<IEnumerable<PlanEmpleador>> GetAllOrderedByPrecioAsync(CancellationToken ct = default);
}
