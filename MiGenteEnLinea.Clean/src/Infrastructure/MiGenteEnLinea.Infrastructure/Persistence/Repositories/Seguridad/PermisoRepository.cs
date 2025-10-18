using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Seguridad;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Seguridad;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Seguridad;

/// <summary>
/// Implementación del repositorio de permisos de usuarios
/// </summary>
public class PermisoRepository : Repository<Permiso>, IPermisoRepository
{
    public PermisoRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<Permiso?> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);
    }

    public async Task<bool> UsuarioTienePermisoAsync(string userId, int permiso, CancellationToken ct = default)
    {
        var permisos = await GetByUserIdAsync(userId, ct);
        return permisos?.TienePermiso(permiso) ?? false;
    }

    public async Task<bool> EsAdministradorAsync(string userId, CancellationToken ct = default)
    {
        var permisos = await GetByUserIdAsync(userId, ct);
        return permisos?.EsAdministrador() ?? false;
    }

    public async Task<IEnumerable<Permiso>> GetUsuariosConPermisoAsync(int permiso, CancellationToken ct = default)
    {
        // Obtener todos los permisos y filtrar en memoria usando el método de dominio
        var todosLosPermisos = await _dbSet
            .AsNoTracking()
            .ToListAsync(ct);

        return todosLosPermisos.Where(p => p.TienePermiso(permiso)).ToList();
    }

    public async Task<IEnumerable<Permiso>> GetAdministradoresAsync(CancellationToken ct = default)
    {
        return await GetUsuariosConPermisoAsync(PermisosFlags.Administracion, ct);
    }

    public async Task<bool> ExistePermisoParaUsuarioAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(p => p.UserId == userId, ct);
    }
}
