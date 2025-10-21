using MiGenteEnLinea.Domain.ReadModels;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Views;

/// <summary>
/// Repositorio de solo lectura para la vista VistaContratista
/// </summary>
/// <remarks>
/// Vista optimizada para búsqueda de contratistas con calificaciones y servicios.
/// Read-only: No se permiten operaciones de escritura.
/// </remarks>
public interface IVistaContratistaRepository : IReadOnlyRepository<VistaContratista>
{
    /// <summary>
    /// Obtiene contratistas activos por provincia
    /// </summary>
    Task<IEnumerable<VistaContratista>> GetActivosByProvinciaAsync(string provincia, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene contratistas que trabajan a nivel nacional
    /// </summary>
    Task<IEnumerable<VistaContratista>> GetNivelNacionalAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca contratistas por sector
    /// </summary>
    Task<IEnumerable<VistaContratista>> GetBySectorAsync(string sector, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca contratistas por nombre o apellido (búsqueda parcial)
    /// </summary>
    Task<IEnumerable<VistaContratista>> SearchByNombreAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene contratistas ordenados por calificación (mejores primero)
    /// </summary>
    Task<IEnumerable<VistaContratista>> GetTopCalificadosAsync(int top = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un contratista por UserId
    /// </summary>
    Task<VistaContratista?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}
