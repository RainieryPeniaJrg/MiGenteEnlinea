using MiGenteEnLinea.Domain.Entities.Seguridad;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Seguridad;

/// <summary>
/// Repositorio para gestionar permisos de usuarios en el sistema.
/// Utiliza un sistema de atributos (flags binarios) para permisos granulares.
/// </summary>
public interface IPermisoRepository : IRepository<Permiso>
{
    /// <summary>
    /// Obtiene los permisos de un usuario específico
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Permisos del usuario o null si no existen</returns>
    Task<Permiso?> GetByUserIdAsync(string userId, CancellationToken ct = default);

    /// <summary>
    /// Verifica si un usuario tiene un permiso específico
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="permiso">Flag de permiso a verificar (ej: PermisosFlags.Lectura)</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>True si el usuario tiene el permiso, False en caso contrario</returns>
    Task<bool> UsuarioTienePermisoAsync(string userId, int permiso, CancellationToken ct = default);

    /// <summary>
    /// Verifica si un usuario es administrador (tiene permisos de administración)
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>True si es administrador, False en caso contrario</returns>
    Task<bool> EsAdministradorAsync(string userId, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los usuarios con un permiso específico
    /// </summary>
    /// <param name="permiso">Flag de permiso (ej: PermisosFlags.Administracion)</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de permisos que incluyen el flag especificado</returns>
    Task<IEnumerable<Permiso>> GetUsuariosConPermisoAsync(int permiso, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los usuarios administradores del sistema
    /// </summary>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de permisos con flag de administración activo</returns>
    Task<IEnumerable<Permiso>> GetAdministradoresAsync(CancellationToken ct = default);

    /// <summary>
    /// Verifica si existe un registro de permisos para un usuario
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>True si existe, False en caso contrario</returns>
    Task<bool> ExistePermisoParaUsuarioAsync(string userId, CancellationToken ct = default);
}
