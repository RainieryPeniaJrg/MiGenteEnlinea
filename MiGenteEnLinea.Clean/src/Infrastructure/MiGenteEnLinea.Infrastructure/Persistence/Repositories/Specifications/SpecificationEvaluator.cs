using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Specifications;

/// <summary>
/// Evalúa especificaciones y las aplica a un IQueryable de Entity Framework Core.
/// Este es el componente que traduce las especificaciones a queries LINQ ejecutables.
/// </summary>
/// <typeparam name="T">Tipo de entidad sobre la que se aplica la especificación</typeparam>
/// <remarks>
/// El SpecificationEvaluator es el "motor" que convierte especificaciones en queries EF Core.
/// Toma una especificación (criteria, includes, orderby, paging) y la aplica al DbSet.
/// 
/// Proceso de evaluación:
/// 1. Aplica criterio de filtrado (WHERE)
/// 2. Aplica Includes para eager loading
/// 3. Aplica ordenamiento (ORDER BY / ORDER BY DESC)
/// 4. Aplica paginación (SKIP/TAKE)
/// 
/// Ejemplo de query generado:
/// <code>
/// // Specification:
/// new EmpleadoresActivosSpec(planId: 5) con:
/// - Criteria: e => e.Activo && e.PlanId == 5
/// - Include: e => e.Empleados
/// - OrderBy: e => e.NombreComercial
/// - Paging: Skip(0).Take(10)
/// 
/// // SQL generado:
/// SELECT TOP 10 *
/// FROM Empleadores e
/// LEFT JOIN Empleados emp ON emp.EmpleadorId = e.EmpleadorId
/// WHERE e.Activo = 1 AND e.PlanId = 5
/// ORDER BY e.NombreComercial
/// </code>
/// </remarks>
public static class SpecificationEvaluator<T> where T : class
{
    /// <summary>
    /// Aplica una especificación a un IQueryable y retorna el query modificado
    /// </summary>
    /// <param name="inputQuery">Query base (típicamente DbSet&lt;T&gt;.AsQueryable())</param>
    /// <param name="specification">Especificación a aplicar</param>
    /// <returns>IQueryable con la especificación aplicada</returns>
    /// <remarks>
    /// Este método construye el query en el orden correcto:
    /// WHERE → INCLUDE → ORDER BY → SKIP/TAKE
    /// 
    /// Importante: El query NO se ejecuta aquí, solo se construye.
    /// La ejecución ocurre cuando se llama ToListAsync(), FirstOrDefaultAsync(), etc.
    /// </remarks>
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
    {
        var query = inputQuery;

        // ========================================
        // 1. APLICAR CRITERIO DE FILTRADO (WHERE)
        // ========================================
        // Ejemplo: WHERE e.Activo = 1 AND e.PlanId = 5
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        // ========================================
        // 2. APLICAR INCLUDES PARA EAGER LOADING
        // ========================================
        // Ejemplo: .Include(e => e.Empleados)
        
        // 2a. Includes con expresiones lambda
        if (specification.Includes.Any())
        {
            query = specification.Includes
                .Aggregate(query, (current, include) => current.Include(include));
        }

        // 2b. Includes con strings (para ThenInclude anidados)
        // Ejemplo: .Include("Empleados.Recibos.Detalles")
        if (specification.IncludeStrings.Any())
        {
            query = specification.IncludeStrings
                .Aggregate(query, (current, include) => current.Include(include));
        }

        // ========================================
        // 3. APLICAR ORDENAMIENTO (ORDER BY)
        // ========================================
        // Ejemplo: ORDER BY e.NombreComercial ASC
        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        // Ejemplo: ORDER BY e.FechaCreacion DESC
        else if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        // ========================================
        // 4. APLICAR PAGINACIÓN (SKIP/TAKE)
        // ========================================
        // Ejemplo: OFFSET 20 ROWS FETCH NEXT 10 ROWS ONLY
        if (specification.IsPagingEnabled)
        {
            // Skip debe aplicarse antes de Take
            if (specification.Skip > 0)
            {
                query = query.Skip(specification.Skip);
            }

            if (specification.Take > 0)
            {
                query = query.Take(specification.Take);
            }
        }

        return query;
    }
}
