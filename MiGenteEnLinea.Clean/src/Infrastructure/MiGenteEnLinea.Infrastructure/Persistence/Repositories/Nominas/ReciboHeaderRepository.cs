using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Nominas;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Nominas;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Nominas;

/// <summary>
/// Implementación del repositorio para ReciboHeader.
/// </summary>
public class ReciboHeaderRepository : Repository<ReciboHeader>, IReciboHeaderRepository
{
    public ReciboHeaderRepository(MiGenteDbContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ReciboHeader>> GetByEmpleadoIdAsync(
        int empleadoId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.EmpleadoId == empleadoId)
            .OrderByDescending(r => r.FechaRegistro)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ReciboHeader>> GetByEmpleadorIdAsync(
        string userId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.FechaRegistro)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ReciboHeader>> GetByPeriodoAsync(
        DateOnly periodoInicio, 
        DateOnly periodoFin, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.PeriodoInicio >= periodoInicio && r.PeriodoFin <= periodoFin)
            .OrderBy(r => r.PeriodoInicio)
            .ThenBy(r => r.EmpleadoId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ReciboHeader>> GetByEstadoAsync(
        int estado, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.Estado == estado)
            .OrderByDescending(r => r.FechaRegistro)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ReciboHeader?> GetWithDetallesAsync(
        int pagoId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Detalles)
            .FirstOrDefaultAsync(r => r.PagoId == pagoId, cancellationToken);
    }
}
