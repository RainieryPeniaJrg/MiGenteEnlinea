using MiGenteEnLinea.Domain.Entities.Seguridad;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Seguridad;

/// <summary>
/// Repositorio para gestionar perfiles de usuario (Empleadores y Contratistas).
/// Un perfil contiene la información básica del usuario y su tipo.
/// </summary>
public interface IPerfileRepository : IRepository<Perfile>
{
    /// <summary>
    /// Obtiene un perfil por su ID de usuario
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Perfil del usuario o null si no existe</returns>
    Task<Perfile?> GetByUserIdAsync(string userId, CancellationToken ct = default);

    /// <summary>
    /// Obtiene un perfil por correo electrónico
    /// </summary>
    /// <param name="email">Correo electrónico del usuario</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Perfil del usuario o null si no existe</returns>
    Task<Perfile?> GetByEmailAsync(string email, CancellationToken ct = default);

    /// <summary>
    /// Obtiene un perfil por nombre de usuario (login)
    /// </summary>
    /// <param name="usuario">Nombre de usuario</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Perfil del usuario o null si no existe</returns>
    Task<Perfile?> GetByUsuarioAsync(string usuario, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los perfiles de un tipo específico
    /// </summary>
    /// <param name="tipo">Tipo de perfil (1=Empleador, 2=Contratista)</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de perfiles del tipo especificado</returns>
    Task<IEnumerable<Perfile>> GetByTipoAsync(int tipo, CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los perfiles de empleadores
    /// </summary>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de perfiles de empleadores</returns>
    Task<IEnumerable<Perfile>> GetEmpleadoresAsync(CancellationToken ct = default);

    /// <summary>
    /// Obtiene todos los perfiles de contratistas
    /// </summary>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de perfiles de contratistas</returns>
    Task<IEnumerable<Perfile>> GetContratistasAsync(CancellationToken ct = default);

    /// <summary>
    /// Busca perfiles por nombre o apellido
    /// </summary>
    /// <param name="termino">Término de búsqueda (se busca en nombre y apellido)</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de perfiles que coincidan con el término</returns>
    Task<IEnumerable<Perfile>> BuscarPorNombreAsync(string termino, CancellationToken ct = default);

    /// <summary>
    /// Verifica si existe un perfil con el email especificado
    /// </summary>
    /// <param name="email">Correo electrónico a verificar</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>True si existe, False en caso contrario</returns>
    Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default);

    /// <summary>
    /// Verifica si existe un perfil con el nombre de usuario especificado
    /// </summary>
    /// <param name="usuario">Nombre de usuario a verificar</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>True si existe, False en caso contrario</returns>
    Task<bool> ExisteUsuarioAsync(string usuario, CancellationToken ct = default);

    /// <summary>
    /// Obtiene perfiles creados en un rango de fechas
    /// </summary>
    /// <param name="fechaInicio">Fecha de inicio del rango</param>
    /// <param name="fechaFin">Fecha de fin del rango</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de perfiles creados en el rango especificado</returns>
    Task<IEnumerable<Perfile>> GetByFechaCreacionAsync(DateTime fechaInicio, DateTime fechaFin, CancellationToken ct = default);
}
