using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Seguridad;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Seguridad;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Seguridad;

/// <summary>
/// Implementación del repositorio de perfiles de usuario
/// </summary>
public class PerfileRepository : Repository<Perfile>, IPerfileRepository
{
    public PerfileRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<Perfile?> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);
    }

    public async Task<Perfile?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email == email, ct);
    }

    public async Task<Perfile?> GetByUsuarioAsync(string usuario, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Usuario == usuario, ct);
    }

    public async Task<IEnumerable<Perfile>> GetByTipoAsync(int tipo, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.Tipo == tipo)
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Perfile>> GetEmpleadoresAsync(CancellationToken ct = default)
    {
        return await GetByTipoAsync(TipoPerfilEnum.Empleador, ct);
    }

    public async Task<IEnumerable<Perfile>> GetContratistasAsync(CancellationToken ct = default)
    {
        return await GetByTipoAsync(TipoPerfilEnum.Contratista, ct);
    }

    public async Task<IEnumerable<Perfile>> BuscarPorNombreAsync(string termino, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(termino))
            return Enumerable.Empty<Perfile>();

        var terminoLower = termino.ToLower();

        return await _dbSet
            .AsNoTracking()
            .Where(p => p.Nombre.ToLower().Contains(terminoLower) ||
                       p.Apellido.ToLower().Contains(terminoLower))
            .OrderBy(p => p.Nombre)
            .ThenBy(p => p.Apellido)
            .ToListAsync(ct);
    }

    public async Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(p => p.Email == email, ct);
    }

    public async Task<bool> ExisteUsuarioAsync(string usuario, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(usuario))
            return false;

        return await _dbSet
            .AsNoTracking()
            .AnyAsync(p => p.Usuario == usuario, ct);
    }

    public async Task<IEnumerable<Perfile>> GetByFechaCreacionAsync(
        DateTime fechaInicio,
        DateTime fechaFin,
        CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.FechaCreacion >= fechaInicio && p.FechaCreacion <= fechaFin)
            .OrderBy(p => p.FechaCreacion)
            .ToListAsync(ct);
    }
}
