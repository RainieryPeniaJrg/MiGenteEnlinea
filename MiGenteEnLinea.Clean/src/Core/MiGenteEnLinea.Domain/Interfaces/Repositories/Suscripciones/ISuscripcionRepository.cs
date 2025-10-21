using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Suscripciones;

/// <summary>
/// Repositorio para Suscripcion
/// </summary>
/// <remarks>
/// Gestión de suscripciones de usuarios (empleadores y contratistas).
/// Criterios de suscripción activa:
/// - No cancelada (Cancelada = false)
/// - No vencida (Vencimiento >= DateTime.UtcNow)
/// </remarks>
public interface ISuscripcionRepository : IRepository<Suscripcion>
{
    // ============================================
    // BÚSQUEDAS ESPECÍFICAS
    // ============================================

    /// <summary>
    /// Obtiene la suscripción activa de un usuario (no cancelada, más reciente)
    /// </summary>
    /// <remarks>
    /// Retorna la suscripción no cancelada más reciente.
    /// El llamador debe verificar EstaActiva() para validar si está vencida.
    /// </remarks>
    Task<Suscripcion?> GetActivaByUserIdAsync(string userId, CancellationToken ct = default);

    /// <summary>
    /// Obtiene la suscripción no cancelada de un usuario (puede estar vencida)
    /// </summary>
    Task<Suscripcion?> GetNoCanceladaByUserIdAsync(string userId, CancellationToken ct = default);

    /// <summary>
    /// Verifica si un usuario tiene suscripción activa (no cancelada y no vencida)
    /// </summary>
    Task<bool> TieneSuscripcionActivaAsync(string userId, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todas las suscripciones de un usuario ordenadas por fecha de inicio descendente
    /// </summary>
    Task<IEnumerable<Suscripcion>> GetByUserIdAsync(string userId, CancellationToken ct = default);
}
