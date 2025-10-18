using System.Linq.Expressions;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories;

/// <summary>
/// Interfaz genérica para repositorios de solo lectura (views)
/// </summary>
/// <typeparam name="T">Tipo de entidad read-only que maneja el repositorio</typeparam>
/// <remarks>
/// Repositorios read-only son para vistas de base de datos.
/// NO incluyen operaciones de escritura (Add, Update, Remove).
/// </remarks>
public interface IReadOnlyRepository<T> where T : class
{
    // ========================================
    // READ OPERATIONS ONLY
    // ========================================

    /// <summary>
    /// Obtiene una entidad por su ID
    /// </summary>
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todas las entidades
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca entidades que cumplan con el predicado especificado
    /// </summary>
    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene la primera entidad que cumple el predicado, o null si no existe
    /// </summary>
    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una única entidad que cumple el predicado, o null si no existe.
    /// Lanza excepción si hay más de una coincidencia.
    /// </summary>
    Task<T?> SingleOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cuenta todas las entidades
    /// </summary>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Cuenta entidades que cumplen un predicado
    /// </summary>
    Task<int> CountAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existe alguna entidad que cumpla el predicado
    /// </summary>
    Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existe alguna entidad
    /// </summary>
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
}
