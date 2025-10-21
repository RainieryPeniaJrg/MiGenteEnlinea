using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Catalogos;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Catalogos;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Catalogos;

/// <summary>
/// Implementación del repositorio para Sector
/// </summary>
public class SectorRepository : Repository<Sector>, ISectorRepository
{
    public SectorRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Sector>> GetActivosAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.Activo)
            .OrderBy(s => s.Orden)
            .ThenBy(s => s.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Sector>> GetByGrupoAsync(string grupo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.Grupo == grupo)
            .OrderBy(s => s.Orden)
            .ThenBy(s => s.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<Sector?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Codigo == codigo.ToUpperInvariant(), cancellationToken);
    }

    public async Task<IEnumerable<Sector>> SearchByNombreAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.Nombre.Contains(searchTerm))
            .OrderBy(s => s.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<string>> GetAllGruposAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.Grupo != null)
            .Select(s => s.Grupo!)
            .Distinct()
            .OrderBy(g => g)
            .ToListAsync(cancellationToken);
    }
}
