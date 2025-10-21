using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Configuracion;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Configuracion;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Configuracion;

/// <summary>
/// Implementación del repositorio para ConfigCorreo
/// </summary>
public class ConfigCorreoRepository : Repository<ConfigCorreo>, IConfigCorreoRepository
{
    public ConfigCorreoRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<ConfigCorreo?> GetConfiguracionActivaAsync(CancellationToken cancellationToken = default)
    {
        // Solo debe haber una configuración en el sistema
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExisteConfiguracionAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(cancellationToken);
    }

    public async Task<ConfigCorreo?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower(), cancellationToken);
    }
}
