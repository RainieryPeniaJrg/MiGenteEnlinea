using System.Linq.Expressions;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories;

/// <summary>
/// Interfaz genérica para operaciones CRUD básicas en repositorios.
/// Define el contrato base que todos los repositorios específicos deben implementar.
/// </summary>
/// <typeparam name="T">Tipo de entidad que maneja el repositorio</typeparam>
public interface IRepository<T> where T : class
{
    // ========================================
    // READ OPERATIONS (QUERIES)
    // ========================================

    /// <summary>
    /// Obtiene una entidad por su ID
    /// </summary>
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todas las entidades del repositorio
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Colección de todas las entidades</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca entidades que cumplan con el predicado especificado
    /// </summary>
    /// <param name="predicate">Expresión lambda para filtrar</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Colección de entidades que cumplen el predicado</returns>
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

    // ========================================
    // WRITE OPERATIONS (COMMANDS)
    // ========================================

    /// <summary>
    /// Agrega una nueva entidad al repositorio
    /// </summary>
    /// <param name="entity">Entidad a agregar</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Entidad agregada</returns>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega múltiples entidades al repositorio
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    /// <param name="entity">Entidad con los datos actualizados</param>
    void Update(T entity);

    /// <summary>
    /// Actualiza múltiples entidades
    /// </summary>
    void UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// Elimina una entidad del repositorio
    /// </summary>
    /// <param name="entity">Entidad a eliminar</param>
    void Remove(T entity);

    /// <summary>
    /// Elimina múltiples entidades del repositorio
    /// </summary>
    void RemoveRange(IEnumerable<T> entities);

    // ========================================
    // SPECIFICATION PATTERN (QUERIES COMPLEJAS)
    // ========================================

    /// <summary>
    /// Obtiene entidades que cumplen una especificación compleja
    /// </summary>
    /// <param name="specification">Especificación con criterios de búsqueda</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Colección de entidades que cumplen la especificación</returns>
    Task<IEnumerable<T>> GetBySpecificationAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene la primera entidad que cumple una especificación, o null
    /// </summary>
    Task<T?> FirstOrDefaultBySpecificationAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);

    // ========================================
    // COUNT & EXISTENCE CHECKS
    // ========================================

    /// <summary>
    /// Cuenta todas las entidades en el repositorio
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
    /// Verifica si existe alguna entidad en el repositorio
    /// </summary>
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
}
