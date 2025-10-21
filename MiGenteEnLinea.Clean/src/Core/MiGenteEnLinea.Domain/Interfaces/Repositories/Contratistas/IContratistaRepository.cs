using MiGenteEnLinea.Domain.Entities.Contratistas;
using System.Linq.Expressions;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Contratistas;

/// <summary>
/// Repositorio específico para la entidad Contratista
/// </summary>
public interface IContratistaRepository : IRepository<Contratista>
{
    // ============================================
    // BÚSQUEDAS BÁSICAS
    // ============================================

    /// <summary>
    /// Obtiene un Contratista por su UserId (relación 1:1 con Credenciales)
    /// </summary>
    Task<Contratista?> GetByUserIdAsync(string userId, CancellationToken ct = default);

    /// <summary>
    /// Verifica si existe un Contratista para el UserId especificado
    /// </summary>
    Task<bool> ExistsByUserIdAsync(string userId, CancellationToken ct = default);

    // ============================================
    // BÚSQUEDAS PAGINADAS CON FILTROS COMPLEJOS
    // ============================================

    /// <summary>
    /// Busca contratistas con filtros complejos y paginación
    /// </summary>
    /// <param name="searchTerm">Búsqueda en Titulo, Presentacion, Sector</param>
    /// <param name="provincia">Filtro por provincia</param>
    /// <param name="sector">Filtro por sector</param>
    /// <param name="experienciaMinima">Experiencia mínima en años</param>
    /// <param name="soloActivos">Si true, filtra solo activos</param>
    /// <param name="pageNumber">Número de página</param>
    /// <param name="pageSize">Tamaño de página</param>
    Task<(IEnumerable<Contratista> Items, int TotalCount)> SearchAsync(
        string? searchTerm,
        string? provincia,
        string? sector,
        int? experienciaMinima,
        bool soloActivos,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default);

    // ============================================
    // PROYECCIONES DTO (OPTIMIZACIÓN)
    // ============================================

    /// <summary>
    /// Obtiene proyección DTO de un Contratista por ID
    /// </summary>
    Task<TResult?> GetByIdProjectedAsync<TResult>(
        int id,
        Expression<Func<Contratista, TResult>> selector,
        CancellationToken ct = default) where TResult : class;

    /// <summary>
    /// Obtiene proyección DTO de un Contratista por UserId
    /// </summary>
    Task<TResult?> GetByUserIdProjectedAsync<TResult>(
        string userId,
        Expression<Func<Contratista, TResult>> selector,
        CancellationToken ct = default) where TResult : class;

    /// <summary>
    /// Busca contratistas con proyección DTO
    /// </summary>
    Task<(IEnumerable<TResult> Items, int TotalCount)> SearchProjectedAsync<TResult>(
        string? searchTerm,
        string? provincia,
        string? sector,
        int? experienciaMinima,
        bool soloActivos,
        int pageNumber,
        int pageSize,
        Expression<Func<Contratista, TResult>> selector,
        CancellationToken ct = default) where TResult : class;
}
