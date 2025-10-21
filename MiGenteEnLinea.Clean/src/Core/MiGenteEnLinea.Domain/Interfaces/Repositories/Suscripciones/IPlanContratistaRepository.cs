using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Suscripciones;

/// <summary>
/// Repositorio para PlanContratista
/// </summary>
/// <remarks>
/// Planes de suscripción para contratistas.
/// Operaciones: Consulta de planes activos, búsqueda por ID
/// </remarks>
public interface IPlanContratistaRepository : IRepository<PlanContratista>
{
    // ============================================
    // BÚSQUEDAS ESPECÍFICAS
    // ============================================

    /// <summary>
    /// Obtiene todos los planes activos ordenados por precio
    /// </summary>
    Task<IEnumerable<PlanContratista>> GetActivosAsync(CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los planes (activos e inactivos) ordenados por precio
    /// </summary>
    Task<IEnumerable<PlanContratista>> GetAllOrderedByPrecioAsync(CancellationToken ct = default);
}
