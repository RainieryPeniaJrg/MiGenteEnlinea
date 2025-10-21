using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Nominas;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Nominas;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Nominas;

/// <summary>
/// Implementación del repositorio para ReciboDetalle.
/// </summary>
public class ReciboDetalleRepository : Repository<ReciboDetalle>, IReciboDetalleRepository
{
    public ReciboDetalleRepository(MiGenteDbContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ReciboDetalle>> GetByPagoIdAsync(
        int pagoId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.PagoId == pagoId)
            .OrderBy(d => d.Orden)
            .ThenBy(d => d.DetalleId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ReciboDetalle>> GetByTipoConceptoAsync(
        int pagoId, 
        int tipoConcepto, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.PagoId == pagoId && d.TipoConcepto == tipoConcepto)
            .OrderBy(d => d.Orden)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ReciboDetalle>> GetIngresosAsync(
        int pagoId, 
        CancellationToken cancellationToken = default)
    {
        return await GetByTipoConceptoAsync(pagoId, 1, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ReciboDetalle>> GetDeduccionesAsync(
        int pagoId, 
        CancellationToken cancellationToken = default)
    {
        return await GetByTipoConceptoAsync(pagoId, 2, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<decimal> GetTotalIngresosAsync(
        int pagoId, 
        CancellationToken cancellationToken = default)
    {
        var total = await _dbSet
            .Where(d => d.PagoId == pagoId && d.TipoConcepto == 1)
            .SumAsync(d => d.Monto, cancellationToken);
        
        return total;
    }

    /// <inheritdoc/>
    public async Task<decimal> GetTotalDeduccionesAsync(
        int pagoId, 
        CancellationToken cancellationToken = default)
    {
        var total = await _dbSet
            .Where(d => d.PagoId == pagoId && d.TipoConcepto == 2)
            .SumAsync(d => d.Monto, cancellationToken);
        
        // Retornar valor absoluto (las deducciones se almacenan como negativas)
        return Math.Abs(total);
    }
}
