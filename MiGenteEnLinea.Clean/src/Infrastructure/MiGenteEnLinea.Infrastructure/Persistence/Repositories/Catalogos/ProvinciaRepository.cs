using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Catalogos;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Catalogos;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Catalogos;

/// <summary>
/// Implementación del repositorio para Provincia
/// </summary>
public class ProvinciaRepository : Repository<Provincia>, IProvinciaRepository
{
    public ProvinciaRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<Provincia?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Nombre.ToLower() == nombre.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<Provincia>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Provincia>> SearchByNombreAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.Nombre.Contains(searchTerm))
            .OrderBy(p => p.Nombre)
            .ToListAsync(cancellationToken);
    }
}
