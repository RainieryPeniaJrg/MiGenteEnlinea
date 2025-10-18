using MiGenteEnLinea.Domain.ReadModels;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Views;

/// <summary>
/// Repositorio de solo lectura para la vista VistaPerfil
/// </summary>
/// <remarks>
/// Esta vista combina información de perfiles con perfilesInfo.
/// Read-only: No se permiten operaciones de escritura (Add, Update, Remove).
/// </remarks>
public interface IVistaPerfilRepository : IReadOnlyRepository<VistaPerfil>
{
    /// <summary>
    /// Obtiene un perfil por UserId
    /// </summary>
    Task<VistaPerfil?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene perfiles por tipo (1=Empleador, 2=Contratista)
    /// </summary>
    Task<IEnumerable<VistaPerfil>> GetByTipoAsync(int tipo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un perfil por email
    /// </summary>
    Task<VistaPerfil?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca perfiles por nombre o apellido (búsqueda parcial)
    /// </summary>
    Task<IEnumerable<VistaPerfil>> SearchByNombreAsync(string searchTerm, CancellationToken cancellationToken = default);
}
