using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Suscripciones;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Suscripciones;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Suscripciones;

/// <summary>
/// Implementación del repositorio para PlanEmpleador
/// </summary>
public class PlanEmpleadorRepository : Repository<PlanEmpleador>, IPlanEmpleadorRepository
{
    public PlanEmpleadorRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PlanEmpleador>> GetActivosAsync(CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.Activo)
            .OrderBy(p => p.Precio)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<PlanEmpleador>> GetAllOrderedByPrecioAsync(CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .OrderBy(p => p.Precio)
            .ToListAsync(ct);
    }
}
