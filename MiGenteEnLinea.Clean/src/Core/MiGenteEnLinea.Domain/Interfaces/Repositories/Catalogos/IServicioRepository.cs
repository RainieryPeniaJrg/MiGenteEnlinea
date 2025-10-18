namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Catalogos;

/// <summary>
/// Repositorio para la entidad Servicio.
/// Proporciona operaciones específicas para consultar servicios ofrecidos por contratistas.
/// </summary>
public interface IServicioRepository : IRepository<Entities.Catalogos.Servicio>
{
    /// <summary>
    /// Obtiene todos los servicios activos ordenados por Orden ASC
    /// </summary>
    Task<IEnumerable<Entities.Catalogos.Servicio>> GetActivosAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene servicios filtrados por categoría
    /// </summary>
    Task<IEnumerable<Entities.Catalogos.Servicio>> GetByCategoriaAsync(string categoria, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca servicios cuya descripción contenga el texto especificado
    /// </summary>
    Task<IEnumerable<Entities.Catalogos.Servicio>> SearchByDescripcionAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene servicios creados por un usuario específico
    /// </summary>
    Task<IEnumerable<Entities.Catalogos.Servicio>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todas las categorías únicas existentes
    /// </summary>
    Task<IEnumerable<string>> GetAllCategoriasAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existe un servicio con la misma descripción
    /// </summary>
    Task<bool> ExisteServicioAsync(string descripcion, CancellationToken cancellationToken = default);
}
