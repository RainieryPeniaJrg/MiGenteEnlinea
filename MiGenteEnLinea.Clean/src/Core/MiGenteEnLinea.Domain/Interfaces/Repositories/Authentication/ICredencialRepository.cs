using MiGenteEnLinea.Domain.Entities.Authentication;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Authentication;

/// <summary>
/// Repositorio específico para entidad Credencial con queries de autenticación
/// </summary>
public interface ICredencialRepository : IRepository<Credencial>
{
    /// <summary>
    /// Obtiene una credencial por email
    /// </summary>
    Task<Credencial?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una credencial por userId
    /// </summary>
    Task<Credencial?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existe una credencial con el email especificado
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si la credencial está activa
    /// </summary>
    Task<bool> IsActivoAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene credenciales inactivas (para reportes/admin)
    /// </summary>
    Task<IEnumerable<Credencial>> GetCredencialesInactivasAsync(CancellationToken cancellationToken = default);

    // TODO: Descomentar cuando Credencial tenga propiedad Bloqueado
    // /// <summary>
    // /// Obtiene credenciales bloqueadas (para seguridad/admin)
    // /// </summary>
    // Task<IEnumerable<Credencial>> GetCredencialesBloqueadasAsync(CancellationToken cancellationToken = default);
}
