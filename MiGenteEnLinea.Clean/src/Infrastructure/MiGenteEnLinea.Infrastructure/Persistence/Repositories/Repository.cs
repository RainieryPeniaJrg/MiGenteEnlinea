using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Interfaces.Repositories;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;
using MiGenteEnLinea.Infrastructure.Persistence.Repositories.Specifications;
using System.Linq.Expressions;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementación genérica del patrón Repository usando Entity Framework Core.
/// Proporciona operaciones CRUD básicas para cualquier entidad.
/// </summary>
/// <typeparam name="T">Tipo de entidad que maneja el repositorio</typeparam>
/// <remarks>
/// Esta clase es la implementación base que heredan todos los repositorios específicos.
/// Utiliza EF Core para interactuar con la base de datos.
/// 
/// Ejemplo de uso (repositorio específico):
/// <code>
/// public class CredencialRepository : Repository&lt;Credencial&gt;, ICredencialRepository
/// {
///     public CredencialRepository(MiGenteDbContext context) : base(context) { }
///     
///     // Métodos específicos adicionales
///     public async Task&lt;Credencial?&gt; GetByEmailAsync(string email, CancellationToken ct)
///     {
///         return await _context.Credenciales
///             .FirstOrDefaultAsync(c => c.Email.Value == email, ct);
///     }
/// }
/// </code>
/// </remarks>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly MiGenteDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(MiGenteDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<T>();
    }

    // ========================================
    // READ OPERATIONS (QUERIES)
    // ========================================

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task<T?> SingleOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    // ========================================
    // WRITE OPERATIONS (COMMANDS)
    // ========================================

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    // ========================================
    // SPECIFICATION PATTERN (QUERIES COMPLEJAS)
    // ========================================

    public virtual async Task<IEnumerable<T>> GetBySpecificationAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        // Aplica la especificación al DbSet usando SpecificationEvaluator
        var query = ApplySpecification(specification);
        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> FirstOrDefaultBySpecificationAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        // Aplica la especificación y obtiene el primer resultado
        var query = ApplySpecification(specification);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    // ========================================
    // COUNT & EXISTENCE CHECKS
    // ========================================

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }

    public virtual async Task<int> CountAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(predicate, cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(cancellationToken);
    }

    // ========================================
    // PRIVATE HELPERS
    // ========================================

    /// <summary>
    /// Aplica una especificación a un IQueryable usando SpecificationEvaluator
    /// </summary>
    /// <param name="specification">Especificación a aplicar</param>
    /// <returns>IQueryable con la especificación aplicada</returns>
    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), specification);
    }
}
