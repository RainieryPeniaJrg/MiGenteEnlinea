using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Contratistas;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Contratistas;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Contratistas;

/// <summary>
/// Implementación del repositorio para ContratistaServicio.
/// </summary>
public class ContratistaServicioRepository : Repository<ContratistaServicio>, IContratistaServicioRepository
{
    public ContratistaServicioRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ContratistaServicio>> GetByContratistaIdAsync(
        int contratistaId, 
        CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.ContratistaId == contratistaId)
            .OrderBy(s => s.Orden)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ContratistaServicio>> GetActivosByContratistaIdAsync(
        int contratistaId, 
        CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.ContratistaId == contratistaId && s.Activo)
            .OrderBy(s => s.Orden)
            .ToListAsync(ct);
    }

    public async Task<bool> ExisteServicioAsync(
        int contratistaId, 
        string detalleServicio, 
        CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(s => s.ContratistaId == contratistaId && 
                          s.DetalleServicio == detalleServicio, ct);
    }
}
