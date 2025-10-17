using MiGenteEnLinea.Domain.Entities.Pagos;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Pagos;

/// <summary>
/// Repositorio para Venta
/// </summary>
/// <remarks>
/// Gestión de transacciones de venta (suscripciones, planes, etc.).
/// Estados: Pendiente, Aprobada, Rechazada, Cancelada
/// </remarks>
public interface IVentaRepository : IRepository<Venta>
{
    // ============================================
    // BÚSQUEDAS ESPECÍFICAS
    // ============================================

    /// <summary>
    /// Obtiene todas las ventas de un usuario ordenadas por fecha descendente
    /// </summary>
    Task<IEnumerable<Venta>> GetByUserIdAsync(string userId, CancellationToken ct = default);

    /// <summary>
    /// Obtiene una venta por su idempotency key (evitar duplicados)
    /// </summary>
    Task<Venta?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken ct = default);

    /// <summary>
    /// Obtiene ventas aprobadas de un usuario
    /// </summary>
    Task<IEnumerable<Venta>> GetAprobadasByUserIdAsync(string userId, CancellationToken ct = default);
}
