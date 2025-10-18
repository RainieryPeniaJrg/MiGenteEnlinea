namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Catalogos;

/// <summary>
/// Repositorio para la entidad Sector.
/// Proporciona operaciones específicas para consultar sectores económicos.
/// </summary>
public interface ISectorRepository : IRepository<Entities.Catalogos.Sector>
{
    /// <summary>
    /// Obtiene todos los sectores activos ordenados por Orden ASC
    /// </summary>
    Task<IEnumerable<Entities.Catalogos.Sector>> GetActivosAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene sectores filtrados por grupo
    /// </summary>
    Task<IEnumerable<Entities.Catalogos.Sector>> GetByGrupoAsync(string grupo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca un sector por su código único
    /// </summary>
    Task<Entities.Catalogos.Sector?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca sectores cuyo nombre contenga el texto especificado
    /// </summary>
    Task<IEnumerable<Entities.Catalogos.Sector>> SearchByNombreAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los grupos únicos existentes
    /// </summary>
    Task<IEnumerable<string>> GetAllGruposAsync(CancellationToken cancellationToken = default);
}
