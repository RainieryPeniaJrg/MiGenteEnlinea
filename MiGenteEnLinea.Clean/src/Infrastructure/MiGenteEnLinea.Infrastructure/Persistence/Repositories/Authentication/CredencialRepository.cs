using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Authentication;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Authentication;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Authentication;

/// <summary>
/// Implementación del repositorio para la entidad Credencial
/// LOTE 0: Placeholder básico (se extenderá en LOTE 1)
/// </summary>
public class CredencialRepository : Repository<Credencial>, ICredencialRepository
{
    public CredencialRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<Credencial?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    public async Task<Credencial?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(c => c.Email == email, cancellationToken);
    }

    public async Task<bool> IsActivoAsync(string userId, CancellationToken cancellationToken = default)
    {
        var credencial = await GetByUserIdAsync(userId, cancellationToken);
        return credencial?.Activo ?? false;
    }

    public async Task<IEnumerable<Credencial>> GetCredencialesInactivasAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => !c.Activo)
            .ToListAsync(cancellationToken);
    }

    // TODO: Implementar cuando la entidad Credencial tenga propiedad Bloqueado
    // public async Task<IEnumerable<Credencial>> GetCredencialesBloqueadasAsync(CancellationToken cancellationToken = default)
    // {
    //     return await _dbSet
    //         .Where(c => c.Bloqueado)
    //         .ToListAsync(cancellationToken);
    // }
}
