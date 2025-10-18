using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Seguridad;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Seguridad;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Seguridad;

/// <summary>
/// Implementación del repositorio de información extendida de perfiles
/// </summary>
public class PerfilesInfoRepository : Repository<PerfilesInfo>, IPerfilesInfoRepository
{
    public PerfilesInfoRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<PerfilesInfo?> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);
    }

    public async Task<PerfilesInfo?> GetByPerfilIdAsync(int perfilId, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PerfilId == perfilId, ct);
    }

    public async Task<PerfilesInfo?> GetByIdentificacionAsync(string identificacion, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Identificacion == identificacion, ct);
    }

    public async Task<IEnumerable<PerfilesInfo>> GetByTipoIdentificacionAsync(
        int tipoIdentificacion,
        CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.TipoIdentificacion == tipoIdentificacion)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<PerfilesInfo>> GetEmpresasAsync(CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.NombreComercial != null && p.NombreComercial != "")
            .OrderBy(p => p.NombreComercial)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<PerfilesInfo>> GetPersonasFisicasAsync(CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.NombreComercial == null || p.NombreComercial == "")
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<PerfilesInfo>> BuscarPorNombreComercialAsync(
        string termino,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(termino))
            return Enumerable.Empty<PerfilesInfo>();

        var terminoLower = termino.ToLower();

        return await _dbSet
            .AsNoTracking()
            .Where(p => p.NombreComercial != null &&
                       p.NombreComercial.ToLower().Contains(terminoLower))
            .OrderBy(p => p.NombreComercial)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<PerfilesInfo>> GetConFotoPerfilAsync(CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.FotoPerfil != null && p.FotoPerfil.Length > 0)
            .ToListAsync(ct);
    }

    public async Task<bool> ExisteIdentificacionAsync(string identificacion, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(p => p.Identificacion == identificacion, ct);
    }

    public async Task<bool> TieneInformacionExtendidaAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(p => p.UserId == userId, ct);
    }
}
