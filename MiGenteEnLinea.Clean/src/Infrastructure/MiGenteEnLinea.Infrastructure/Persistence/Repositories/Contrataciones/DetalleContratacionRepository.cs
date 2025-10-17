using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Contrataciones;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Contrataciones;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Contrataciones;

/// <summary>
/// Implementación del repositorio para DetalleContratacion.
/// </summary>
public class DetalleContratacionRepository : Repository<DetalleContratacion>, IDetalleContratacionRepository
{
    public DetalleContratacionRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DetalleContratacion>> GetByContratacionIdAsync(
        int contratacionId, 
        CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(d => d.ContratacionId == contratacionId)
            .OrderByDescending(d => d.FechaInicio)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<DetalleContratacion>> GetByEstatusAsync(
        int estatus, 
        CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(d => d.Estatus == estatus)
            .OrderByDescending(d => d.FechaInicio)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<DetalleContratacion>> GetPendientesCalificacionAsync(
        CancellationToken ct = default)
    {
        // Estado 4 = Completada, Calificado = false
        return await _dbSet
            .AsNoTracking()
            .Where(d => d.Estatus == 4 && !d.Calificado)
            .OrderByDescending(d => d.FechaFinalizacionReal)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<DetalleContratacion>> GetActivasAsync(
        CancellationToken ct = default)
    {
        // Estado 3 = En Progreso
        return await _dbSet
            .AsNoTracking()
            .Where(d => d.Estatus == 3)
            .OrderByDescending(d => d.FechaInicio)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<DetalleContratacion>> GetRetrasadasAsync(
        CancellationToken ct = default)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Now);
        
        // Estado 3 = En Progreso y FechaFinal < hoy
        return await _dbSet
            .AsNoTracking()
            .Where(d => d.Estatus == 3 && d.FechaFinal < hoy)
            .OrderByDescending(d => d.FechaInicio)
            .ToListAsync(ct);
    }
}
