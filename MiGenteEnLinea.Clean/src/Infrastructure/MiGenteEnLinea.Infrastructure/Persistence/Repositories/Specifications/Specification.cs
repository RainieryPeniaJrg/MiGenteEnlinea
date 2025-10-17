using System.Linq.Expressions;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Specifications;

/// <summary>
/// Clase base abstracta para implementar el patrón Specification.
/// Permite construir queries complejas de forma reutilizable y componible.
/// </summary>
/// <typeparam name="T">Tipo de entidad sobre la que se aplica la especificación</typeparam>
/// <remarks>
/// El patrón Specification encapsula criterios de búsqueda en objetos reutilizables.
/// Esto permite:
/// 1. Reutilizar queries complejas entre múltiples handlers
/// 2. Testear especificaciones sin ejecutar queries reales
/// 3. Combinar especificaciones con operadores lógicos (futuro: AND, OR, NOT)
/// 4. Mantener código DRY (Don't Repeat Yourself)
/// 
/// Ejemplo de uso:
/// <code>
/// public class EmpleadoresActivosSpec : Specification&lt;Empleador&gt;
/// {
///     public EmpleadoresActivosSpec(int planId) 
///         : base(e => e.Activo && e.PlanId == planId)
///     {
///         // Include para cargar empleados (eager loading)
///         AddInclude(e => e.Empleados);
///         
///         // Ordenar por nombre comercial
///         ApplyOrderBy(e => e.NombreComercial);
///         
///         // Paginación: primeros 10 resultados
///         ApplyPaging(0, 10);
///     }
/// }
/// 
/// // Uso en repositorio
/// var empleadores = await _repository.GetBySpecificationAsync(
///     new EmpleadoresActivosSpec(planId: 5)
/// );
/// </code>
/// </remarks>
public abstract class Specification<T> : ISpecification<T>
{
    /// <summary>
    /// Constructor que acepta un criterio de filtrado (WHERE clause)
    /// </summary>
    /// <param name="criteria">Expresión lambda para filtrar entidades</param>
    protected Specification(Expression<Func<T, bool>>? criteria = null)
    {
        Criteria = criteria;
    }

    // ========================================
    // PROPIEDADES PÚBLICAS (ISpecification<T>)
    // ========================================

    public Expression<Func<T, bool>>? Criteria { get; }

    public List<Expression<Func<T, object>>> Includes { get; } = new();

    public List<string> IncludeStrings { get; } = new();

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPagingEnabled { get; private set; }

    // ========================================
    // MÉTODOS PROTEGIDOS (BUILDER PATTERN)
    // ========================================

    /// <summary>
    /// Agrega una relación para cargar con Include (eager loading)
    /// </summary>
    /// <param name="includeExpression">Expresión que define la propiedad de navegación</param>
    /// <remarks>
    /// Ejemplo: AddInclude(e => e.Empleados) carga la colección de empleados
    /// </remarks>
    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    /// <summary>
    /// Agrega un Include anidado usando string (para ThenInclude)
    /// </summary>
    /// <param name="includeString">String con la ruta de la relación</param>
    /// <remarks>
    /// Ejemplo: AddInclude("Empleados.Recibos.Detalles") carga relaciones anidadas
    /// </remarks>
    protected virtual void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    /// <summary>
    /// Aplica ordenamiento ascendente (ORDER BY)
    /// </summary>
    /// <param name="orderByExpression">Expresión que define el campo de ordenamiento</param>
    /// <remarks>
    /// Ejemplo: ApplyOrderBy(e => e.Nombre) ordena alfabéticamente por nombre
    /// </remarks>
    protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    /// <summary>
    /// Aplica ordenamiento descendente (ORDER BY DESC)
    /// </summary>
    /// <param name="orderByDescendingExpression">Expresión que define el campo de ordenamiento</param>
    /// <remarks>
    /// Ejemplo: ApplyOrderByDescending(e => e.FechaCreacion) ordena por fecha más reciente primero
    /// </remarks>
    protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    /// <summary>
    /// Aplica paginación (SKIP/TAKE para LIMIT/OFFSET)
    /// </summary>
    /// <param name="skip">Número de registros a saltar (offset)</param>
    /// <param name="take">Número de registros a tomar (limit)</param>
    /// <remarks>
    /// Ejemplo: ApplyPaging(20, 10) salta 20 registros y toma los siguientes 10
    /// (equivalente a página 3 con tamaño 10)
    /// </remarks>
    protected virtual void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    /// <summary>
    /// Aplica paginación basada en número de página y tamaño
    /// </summary>
    /// <param name="pageNumber">Número de página (1-based)</param>
    /// <param name="pageSize">Tamaño de página</param>
    /// <remarks>
    /// Ejemplo: ApplyPageBasedPaging(pageNumber: 3, pageSize: 10) devuelve registros 21-30
    /// </remarks>
    protected virtual void ApplyPageBasedPaging(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentException("Page number must be >= 1", nameof(pageNumber));

        if (pageSize < 1)
            throw new ArgumentException("Page size must be >= 1", nameof(pageSize));

        Skip = (pageNumber - 1) * pageSize;
        Take = pageSize;
        IsPagingEnabled = true;
    }
}
