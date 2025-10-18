namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Catalogos;

/// <summary>
/// Repositorio para la entidad Provincia.
/// Proporciona operaciones específicas para consultar provincias de República Dominicana.
/// </summary>
public interface IProvinciaRepository : IRepository<Entities.Catalogos.Provincia>
{
    /// <summary>
    /// Busca una provincia por su nombre exacto (ignorando mayúsculas/minúsculas)
    /// </summary>
    Task<Entities.Catalogos.Provincia?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todas las provincias ordenadas alfabéticamente
    /// </summary>
    Task<IEnumerable<Entities.Catalogos.Provincia>> GetAllOrderedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca provincias cuyo nombre contenga el texto especificado
    /// </summary>
    Task<IEnumerable<Entities.Catalogos.Provincia>> SearchByNombreAsync(string searchTerm, CancellationToken cancellationToken = default);
}
