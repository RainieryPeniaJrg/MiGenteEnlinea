using MiGenteEnLinea.Domain.Interfaces.Repositories.Views;
using MiGenteEnLinea.Domain.ReadModels;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Views;

/// <summary>
/// Implementación del repositorio para VistaPerfil
/// </summary>
public class VistaPerfilRepository : ReadOnlyRepository<VistaPerfil>, IVistaPerfilRepository
{
    public VistaPerfilRepository(MiGenteDbContext context) : base(context) { }

    public async Task<VistaPerfil?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.UserId == userId, cancellationToken);
    }

    public async Task<IEnumerable<VistaPerfil>> GetByTipoAsync(int tipo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(v => v.Tipo == tipo)
            .ToListAsync(cancellationToken);
    }

    public async Task<VistaPerfil?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<VistaPerfil>> SearchByNombreAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(v => (v.Nombre != null && v.Nombre.Contains(searchTerm)) ||
                       (v.Apellido != null && v.Apellido.Contains(searchTerm)))
            .ToListAsync(cancellationToken);
    }
}

/// <summary>
/// Implementación del repositorio para VistaEmpleado
/// </summary>
public class VistaEmpleadoRepository : ReadOnlyRepository<VistaEmpleado>, IVistaEmpleadoRepository
{
    public VistaEmpleadoRepository(MiGenteDbContext context) : base(context) { }

    public async Task<IEnumerable<VistaEmpleado>> GetByEmpleadorIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.FechaRegistro)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VistaEmpleado>> GetActivosByEmpleadorIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(e => e.UserId == userId && e.Activo == true)
            .OrderByDescending(e => e.FechaRegistro)
            .ToListAsync(cancellationToken);
    }

    public async Task<VistaEmpleado?> GetByIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Identificacion == identificacion, cancellationToken);
    }

    public async Task<IEnumerable<VistaEmpleado>> SearchByNombreAsync(string userId, string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(e => e.UserId == userId &&
                       e.Nombre != null && e.Nombre.Contains(searchTerm))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VistaEmpleado>> GetByPeriodoPagoAsync(string userId, int periodoPago, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(e => e.UserId == userId && e.PeriodoPago == periodoPago && e.Activo == true)
            .ToListAsync(cancellationToken);
    }
}

/// <summary>
/// Implementación del repositorio para VistaContratista
/// </summary>
public class VistaContratistaRepository : ReadOnlyRepository<VistaContratista>, IVistaContratistaRepository
{
    public VistaContratistaRepository(MiGenteDbContext context) : base(context) { }

    public async Task<IEnumerable<VistaContratista>> GetActivosByProvinciaAsync(string provincia, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.Activo == true && c.Provincia == provincia)
            .OrderByDescending(c => c.Calificacion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VistaContratista>> GetNivelNacionalAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.Activo == true && c.NivelNacional == true)
            .OrderByDescending(c => c.Calificacion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VistaContratista>> GetBySectorAsync(string sector, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.Activo == true && c.Sector == sector)
            .OrderByDescending(c => c.Calificacion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VistaContratista>> SearchByNombreAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.Activo == true &&
                       ((c.Nombre != null && c.Nombre.Contains(searchTerm)) ||
                        (c.Apellido != null && c.Apellido.Contains(searchTerm)) ||
                        (c.Titulo != null && c.Titulo.Contains(searchTerm))))
            .OrderByDescending(c => c.Calificacion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VistaContratista>> GetTopCalificadosAsync(int top = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.Activo == true)
            .OrderByDescending(c => c.Calificacion)
            .ThenByDescending(c => c.TotalRegistros)
            .Take(top)
            .ToListAsync(cancellationToken);
    }

    public async Task<VistaContratista?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
    }
}

/// <summary>
/// Implementación del repositorio para VistaCalificacion
/// </summary>
public class VistaCalificacionRepository : ReadOnlyRepository<VistaCalificacion>, IVistaCalificacionRepository
{
    public VistaCalificacionRepository(MiGenteDbContext context) : base(context) { }

    public async Task<IEnumerable<VistaCalificacion>> GetByContratistaIdAsync(int contratistaId, CancellationToken cancellationToken = default)
    {
        // VistaCalificacion usa Identificacion, no ContratistaId
        // Este método necesitaría la identificación del contratista
        return await _dbSet
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VistaCalificacion>> GetByUsuarioIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}

/// <summary>
/// Implementación del repositorio para VistaPromedioCalificacion
/// </summary>
public class VistaPromedioCalificacionRepository : ReadOnlyRepository<VistaPromedioCalificacion>, IVistaPromedioCalificacionRepository
{
    public VistaPromedioCalificacionRepository(MiGenteDbContext context) : base(context) { }

    public async Task<VistaPromedioCalificacion?> GetByContratistaIdAsync(int contratistaId, CancellationToken cancellationToken = default)
    {
        // VistaPromedioCalificacion usa Identificacion como clave, no ContratistaId
        // Este método necesitaría la identificación del contratista
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
}

/// <summary>
/// Implementación del repositorio para VistaSuscripcion
/// </summary>
public class VistaSuscripcionRepository : ReadOnlyRepository<VistaSuscripcion>, IVistaSuscripcionRepository
{
    public VistaSuscripcionRepository(MiGenteDbContext context) : base(context) { }

    public async Task<IEnumerable<VistaSuscripcion>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.FechaInicio)
            .ToListAsync(cancellationToken);
    }

    public async Task<VistaSuscripcion?> GetActivaByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Now);
        
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.UserId == userId && s.Vencimiento >= hoy)
            .OrderByDescending(s => s.FechaInicio)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

/// <summary>
/// Implementación del repositorio para VistaPago
/// </summary>
public class VistaPagoRepository : ReadOnlyRepository<VistaPago>, IVistaPagoRepository
{
    public VistaPagoRepository(MiGenteDbContext context) : base(context) { }

    public async Task<IEnumerable<VistaPago>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VistaPago>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.FechaPago >= fechaInicio && p.FechaPago <= fechaFin)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync(cancellationToken);
    }
}

/// <summary>
/// Implementación del repositorio para VistaPagoContratacion
/// </summary>
public class VistaPagoContratacionRepository : ReadOnlyRepository<VistaPagoContratacion>, IVistaPagoContratacionRepository
{
    public VistaPagoContratacionRepository(MiGenteDbContext context) : base(context) { }

    public async Task<IEnumerable<VistaPagoContratacion>> GetByEmpleadorIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VistaPagoContratacion>> GetByContratistaIdAsync(int contratistaId, CancellationToken cancellationToken = default)
    {
        // VistaPagoContratacion usa ContratacionId, no ContratistaId
        // Este método necesitaría filtrar por ContratacionId
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.ContratacionId == contratistaId)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync(cancellationToken);
    }
}

/// <summary>
/// Implementación del repositorio para VistaContratacionTemporal
/// </summary>
public class VistaContratacionTemporalRepository : ReadOnlyRepository<VistaContratacionTemporal>, IVistaContratacionTemporalRepository
{
    public VistaContratacionTemporalRepository(MiGenteDbContext context) : base(context) { }

    public async Task<IEnumerable<VistaContratacionTemporal>> GetByEmpleadorIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.FechaRegistro)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VistaContratacionTemporal>> GetByContratistaIdAsync(int contratistaId, CancellationToken cancellationToken = default)
    {
        // VistaContratacionTemporal usa Identificacion para el contratista
        // Este método necesitaría la identificación del contratista
        return await _dbSet
            .AsNoTracking()
            .OrderByDescending(c => c.FechaRegistro)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VistaContratacionTemporal>> GetActivasAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.Estatus == 3) // 3 = En Progreso
            .OrderByDescending(c => c.FechaRegistro)
            .ToListAsync(cancellationToken);
    }
}
