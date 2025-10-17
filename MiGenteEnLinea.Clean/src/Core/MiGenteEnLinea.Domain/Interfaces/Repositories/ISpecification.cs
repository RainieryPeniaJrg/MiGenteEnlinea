using System.Linq.Expressions;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories;

/// <summary>
/// Patrón Specification para construir queries complejas y reutilizables.
/// Encapsula criterios de búsqueda, ordenamiento, paginación y carga de relaciones.
/// </summary>
/// <typeparam name="T">Tipo de entidad sobre la que se aplica la especificación</typeparam>
/// <remarks>
/// El patrón Specification permite:
/// 1. Reutilizar queries complejas entre múltiples handlers
/// 2. Combinar especificaciones con operadores lógicos (AND, OR, NOT)
/// 3. Testear queries sin ejecutarlas contra la base de datos
/// 4. Mantener queries DRY (Don't Repeat Yourself)
/// 
/// Ejemplo de uso:
/// <code>
/// public class EmpleadoresActivosConPlanPremiumSpec : Specification&lt;Empleador&gt;
/// {
///     public EmpleadoresActivosConPlanPremiumSpec()
///         : base(e => e.Activo && e.PlanId == 5)
///     {
///         AddInclude(e => e.Empleados);
///         ApplyOrderBy(e => e.NombreComercial);
///         ApplyPaging(0, 10);
///     }
/// }
/// 
/// // Uso en handler
/// var empleadores = await _repository.GetBySpecificationAsync(
///     new EmpleadoresActivosConPlanPremiumSpec()
/// );
/// </code>
/// </remarks>
public interface ISpecification<T>
{
    /// <summary>
    /// Criterio de filtrado (WHERE clause)
    /// </summary>
    /// <remarks>
    /// Expresión lambda que define qué entidades deben incluirse en el resultado.
    /// Ejemplo: e => e.Activo && e.PlanId > 0
    /// </remarks>
    Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// Lista de expresiones para cargar relaciones (Include)
    /// </summary>
    /// <remarks>
    /// Equivalente a EF Core's .Include() para carga eager de navegación.
    /// Ejemplo: e => e.Empleados, e => e.Suscripcion
    /// </remarks>
    List<Expression<Func<T, object>>> Includes { get; }

    /// <summary>
    /// Lista de strings para Include anidados (ThenInclude)
    /// </summary>
    /// <remarks>
    /// Para relaciones anidadas como "Empleados.Recibos.Detalles"
    /// </remarks>
    List<string> IncludeStrings { get; }

    /// <summary>
    /// Expresión para ordenamiento ascendente (ORDER BY)
    /// </summary>
    Expression<Func<T, object>>? OrderBy { get; }

    /// <summary>
    /// Expresión para ordenamiento descendente (ORDER BY DESC)
    /// </summary>
    Expression<Func<T, object>>? OrderByDescending { get; }

    /// <summary>
    /// Número de registros a tomar (LIMIT)
    /// </summary>
    /// <remarks>
    /// Usado para paginación. Ejemplo: Take(10) para obtener 10 registros.
    /// </remarks>
    int Take { get; }

    /// <summary>
    /// Número de registros a saltar (OFFSET)
    /// </summary>
    /// <remarks>
    /// Usado para paginación. Ejemplo: Skip(20) para saltar los primeros 20 registros.
    /// </remarks>
    int Skip { get; }

    /// <summary>
    /// Indica si la paginación está habilitada
    /// </summary>
    /// <remarks>
    /// Se activa automáticamente cuando Skip > 0 o Take > 0
    /// </remarks>
    bool IsPagingEnabled { get; }
}
