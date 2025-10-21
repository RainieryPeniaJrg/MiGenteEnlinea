using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Pagos;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Pagos;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Pagos;

/// <summary>
/// Implementación del repositorio para Venta
/// </summary>
public class VentaRepository : Repository<Venta>, IVentaRepository
{
    public VentaRepository(MiGenteDbContext context) : base(context) { }

    public async Task<IEnumerable<Venta>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.FechaTransaccion)
            .ToListAsync(ct);
    }

    public async Task<Venta?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.IdempotencyKey == idempotencyKey, ct);
    }

    public async Task<IEnumerable<Venta>> GetAprobadasByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(v => v.UserId == userId && v.Estado == 2) // 2 = Aprobada
            .OrderByDescending(v => v.FechaTransaccion)
            .ToListAsync(ct);
    }
}
