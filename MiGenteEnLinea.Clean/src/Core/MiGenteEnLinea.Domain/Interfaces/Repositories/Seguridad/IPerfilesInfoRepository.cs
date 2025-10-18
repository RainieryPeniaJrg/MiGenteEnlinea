using MiGenteEnLinea.Domain.Entities.Seguridad;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Seguridad;

/// <summary>
/// Repositorio para gestionar información extendida de perfiles de usuario.
/// Contiene datos adicionales como identificación legal, foto de perfil,
/// presentación y datos del gerente/representante legal.
/// </summary>
public interface IPerfilesInfoRepository : IRepository<PerfilesInfo>
{
    /// <summary>
    /// Obtiene la información extendida de un perfil por su ID de usuario
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Información extendida del perfil o null si no existe</returns>
    Task<PerfilesInfo?> GetByUserIdAsync(string userId, CancellationToken ct = default);

    /// <summary>
    /// Obtiene la información extendida de un perfil por su ID de perfil
    /// </summary>
    /// <param name="perfilId">ID del perfil</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Información extendida del perfil o null si no existe</returns>
    Task<PerfilesInfo?> GetByPerfilIdAsync(int perfilId, CancellationToken ct = default);

    /// <summary>
    /// Obtiene la información extendida de un perfil por número de identificación
    /// </summary>
    /// <param name="identificacion">Número de identificación (Cédula, Pasaporte o RNC)</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Información extendida del perfil o null si no existe</returns>
    Task<PerfilesInfo?> GetByIdentificacionAsync(string identificacion, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los perfiles con un tipo de identificación específico
    /// </summary>
    /// <param name="tipoIdentificacion">Tipo de identificación (1=Cédula, 2=Pasaporte, 3=RNC)</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de perfiles con el tipo de identificación especificado</returns>
    Task<IEnumerable<PerfilesInfo>> GetByTipoIdentificacionAsync(int tipoIdentificacion, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los perfiles de empresas (que tienen nombre comercial)
    /// </summary>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de perfiles de empresas</returns>
    Task<IEnumerable<PerfilesInfo>> GetEmpresasAsync(CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los perfiles de personas físicas (sin nombre comercial)
    /// </summary>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de perfiles de personas físicas</returns>
    Task<IEnumerable<PerfilesInfo>> GetPersonasFisicasAsync(CancellationToken ct = default);

    /// <summary>
    /// Busca perfiles por nombre comercial
    /// </summary>
    /// <param name="termino">Término de búsqueda</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de perfiles que coincidan con el término en nombre comercial</returns>
    Task<IEnumerable<PerfilesInfo>> BuscarPorNombreComercialAsync(string termino, CancellationToken ct = default);

    /// <summary>
    /// Obtiene perfiles que tienen foto de perfil
    /// </summary>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de perfiles con foto</returns>
    Task<IEnumerable<PerfilesInfo>> GetConFotoPerfilAsync(CancellationToken ct = default);

    /// <summary>
    /// Verifica si existe un perfil con la identificación especificada
    /// </summary>
    /// <param name="identificacion">Número de identificación a verificar</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>True si existe, False en caso contrario</returns>
    Task<bool> ExisteIdentificacionAsync(string identificacion, CancellationToken ct = default);

    /// <summary>
    /// Verifica si un usuario tiene información extendida de perfil
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>True si tiene información extendida, False en caso contrario</returns>
    Task<bool> TieneInformacionExtendidaAsync(string userId, CancellationToken ct = default);
}
