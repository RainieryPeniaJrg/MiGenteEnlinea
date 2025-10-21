using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Catalogos;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Catalogos;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Catalogos;

/// <summary>
/// Implementación del repositorio para Servicio
/// </summary>
public class ServicioRepository : Repository<Servicio>, IServicioRepository
{
    public ServicioRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Servicio>> GetActivosAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.Activo)
            .OrderBy(s => s.Orden)
            .ThenBy(s => s.Descripcion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Servicio>> GetByCategoriaAsync(string categoria, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.Categoria == categoria)
            .OrderBy(s => s.Orden)
            .ThenBy(s => s.Descripcion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Servicio>> SearchByDescripcionAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.Descripcion.Contains(searchTerm))
            .OrderBy(s => s.Descripcion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Servicio>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderBy(s => s.Orden)
            .ThenBy(s => s.Descripcion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<string>> GetAllCategoriasAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.Categoria != null)
            .Select(s => s.Categoria!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExisteServicioAsync(string descripcion, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(s => s.Descripcion.ToLower() == descripcion.ToLower(), cancellationToken);
    }
}
