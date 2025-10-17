using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Suscripciones;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Suscripciones;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Suscripciones;

/// <summary>
/// Implementación del repositorio para Suscripcion
/// </summary>
public class SuscripcionRepository : Repository<Suscripcion>, ISuscripcionRepository
{
    public SuscripcionRepository(MiGenteDbContext context) : base(context) { }

    public async Task<Suscripcion?> GetActivaByUserIdAsync(string userId, CancellationToken ct = default)
    {
        // Retorna la suscripción no cancelada más reciente
        // El llamador debe verificar EstaActiva() para validar si está vencida
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.UserId == userId && !s.Cancelada)
            .OrderByDescending(s => s.FechaInicio)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<Suscripcion?> GetNoCanceladaByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .Where(s => s.UserId == userId && !s.Cancelada)
            .OrderByDescending(s => s.FechaInicio)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<bool> TieneSuscripcionActivaAsync(string userId, CancellationToken ct = default)
    {
        var suscripcion = await GetActivaByUserIdAsync(userId, ct);
        return suscripcion?.EstaActiva() ?? false;
    }

    public async Task<IEnumerable<Suscripcion>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.FechaInicio)
            .ToListAsync(ct);
    }
}
