using Microsoft.EntityFrameworkCore.Storage;
using MiGenteEnLinea.Domain.Interfaces.Repositories;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Authentication;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Calificaciones;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Contratistas;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Empleadores;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Empleados;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Pagos;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Suscripciones;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;
using MiGenteEnLinea.Infrastructure.Persistence.Repositories.Authentication;
using MiGenteEnLinea.Infrastructure.Persistence.Repositories.Calificaciones;
using MiGenteEnLinea.Infrastructure.Persistence.Repositories.Contratistas;
using MiGenteEnLinea.Infrastructure.Persistence.Repositories.Empleadores;
using MiGenteEnLinea.Infrastructure.Persistence.Repositories.Empleados;
using MiGenteEnLinea.Infrastructure.Persistence.Repositories.Pagos;
using MiGenteEnLinea.Infrastructure.Persistence.Repositories.Suscripciones;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementación del patrón Unit of Work para coordinar transacciones
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly MiGenteDbContext _context;
    private IDbContextTransaction? _transaction;

    // LOTE 0: Lazy-loaded repository fields (only core 6 repositories)
    private ICredencialRepository? _credenciales;
    private IEmpleadorRepository? _empleadores;
    private IContratistaRepository? _contratistas;
    private IEmpleadoRepository? _empleados;
    private ISuscripcionRepository? _suscripciones;
    private IPlanEmpleadorRepository? _planesEmpleadores;
    private IPlanContratistaRepository? _planesContratistas;
    private IVentaRepository? _ventas;
    private ICalificacionRepository? _calificaciones;

    // TODO LOTES 1-8: Uncomment as interfaces are added to IUnitOfWork
    // private IReciboHeaderRepository? _recibosHeader;
    // private IReciboDetalleRepository? _recibosDetalle;
    // private IConceptoNominaRepository? _conceptosNomina;
    // private IDeduccionTssRepository? _deduccionesTss;
    // private IPlanEmpleadorRepository? _planesEmpleadores;
    // private IPlanContratistaRepository? _planesContratistas;
    // private IVentaRepository? _ventas;
    // private ITransaccionRepository? _transacciones;
    // private IContratacionRepository? _contrataciones;
    // private IContratoServicioRepository? _contratosServicio;
    // private IServicioOfertadoRepository? _serviciosOfertados;
    // private IRolRepository? _roles;
    // private IPermisoRepository? _permisos;
    // private INacionalidadRepository? _nacionalidades;
    // private IProvinciaRepository? _provincias;
    // private IMunicipioRepository? _municipios;
    // private ISectorRepository? _sectores;
    // private ITipoServicioRepository? _tiposServicio;
    // private IEstadoCivilRepository? _estadosCiviles;
    // private INivelAcademicoRepository? _nivelesAcademicos;
    // private ITipoCuentaRepository? _tiposCuenta;
    // private IBancoRepository? _bancos;
    // private ITipoIdentificacionRepository? _tiposIdentificacion;
    // private IConfiguracionSistemaRepository? _configuracionesSistema;
    // private IParametroSistemaRepository? _parametrosSistema;

    public UnitOfWork(MiGenteDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // ===========================================
    // LOTE 0: Repository Properties (Core 6 only)
    // ===========================================

    // Authentication
    public ICredencialRepository Credenciales =>
        _credenciales ??= new CredencialRepository(_context);

    // Empleadores
    public IEmpleadorRepository Empleadores =>
        _empleadores ??= new EmpleadorRepository(_context);

    // Contratistas
    public IContratistaRepository Contratistas =>
        _contratistas ??= new ContratistaRepository(_context);

    // Empleados
    public IEmpleadoRepository Empleados =>
        _empleados ??= new EmpleadoRepository(_context);

    // Suscripciones
    public ISuscripcionRepository Suscripciones =>
        _suscripciones ??= new SuscripcionRepository(_context);
    
    public IPlanEmpleadorRepository PlanesEmpleadores =>
        _planesEmpleadores ??= new PlanEmpleadorRepository(_context);
    
    public IPlanContratistaRepository PlanesContratistas =>
        _planesContratistas ??= new PlanContratistaRepository(_context);

    // Pagos
    public IVentaRepository Ventas =>
        _ventas ??= new VentaRepository(_context);

    // Calificaciones
    public ICalificacionRepository Calificaciones =>
        _calificaciones ??= new CalificacionRepository(_context);

    // ===========================================
    // TODO LOTES 1-8: Uncomment as added to IUnitOfWork
    // ===========================================
    // public IReciboHeaderRepository RecibosHeader =>
    //     _recibosHeader ??= new ReciboHeaderRepository(_context);
    // public IReciboDetalleRepository RecibosDetalle =>
    //     _recibosDetalle ??= new ReciboDetalleRepository(_context);
    // public IConceptoNominaRepository ConceptosNomina =>
    //     _conceptosNomina ??= new ConceptoNominaRepository(_context);
    // public IDeduccionTssRepository DeduccionesTss =>
    //     _deduccionesTss ??= new DeduccionTssRepository(_context);
    // public IPlanEmpleadorRepository PlanesEmpleadores =>
    //     _planesEmpleadores ??= new PlanEmpleadorRepository(_context);
    // public IPlanContratistaRepository PlanesContratistas =>
    //     _planesContratistas ??= new PlanContratistaRepository(_context);
    // public IVentaRepository Ventas =>
    //     _ventas ??= new VentaRepository(_context);
    // public ITransaccionRepository Transacciones =>
    //     _transacciones ??= new TransaccionRepository(_context);
    // public IContratacionRepository Contrataciones =>
    //     _contrataciones ??= new ContratacionRepository(_context);
    // public IContratoServicioRepository ContratosServicio =>
    //     _contratosServicio ??= new ContratoServicioRepository(_context);
    // public IServicioOfertadoRepository ServiciosOfertados =>
    //     _serviciosOfertados ??= new ServicioOfertadoRepository(_context);
    // public IRolRepository Roles =>
    //     _roles ??= new RolRepository(_context);
    // public IPermisoRepository Permisos =>
    //     _permisos ??= new PermisoRepository(_context);
    // public INacionalidadRepository Nacionalidades =>
    //     _nacionalidades ??= new NacionalidadRepository(_context);
    // public IProvinciaRepository Provincias =>
    //     _provincias ??= new ProvinciaRepository(_context);
    // public IMunicipioRepository Municipios =>
    //     _municipios ??= new MunicipioRepository(_context);
    // public ISectorRepository Sectores =>
    //     _sectores ??= new SectorRepository(_context);
    // public ITipoServicioRepository TiposServicio =>
    //     _tiposServicio ??= new TipoServicioRepository(_context);
    // public IEstadoCivilRepository EstadosCiviles =>
    //     _estadosCiviles ??= new EstadoCivilRepository(_context);
    // public INivelAcademicoRepository NivelesAcademicos =>
    //     _nivelesAcademicos ??= new NivelAcademicoRepository(_context);
    // public ITipoCuentaRepository TiposCuenta =>
    //     _tiposCuenta ??= new TipoCuentaRepository(_context);
    // public IBancoRepository Bancos =>
    //     _bancos ??= new BancoRepository(_context);
    // public ITipoIdentificacionRepository TiposIdentificacion =>
    //     _tiposIdentificacion ??= new TipoIdentificacionRepository(_context);
    // public IConfiguracionSistemaRepository ConfiguracionesSistema =>
    //     _configuracionesSistema ??= new ConfiguracionSistemaRepository(_context);
    // public IParametroSistemaRepository ParametrosSistema =>
    //     _parametrosSistema ??= new ParametroSistemaRepository(_context);

    // ========================================
    // TRANSACTION MANAGEMENT
    // ========================================

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("There is already an active transaction.");
        }

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        // LOTE 0: No devuelve IDbContextTransaction (evita dependencia EF Core en Domain Layer)
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("There is no active transaction to commit.");
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("There is no active transaction to rollback.");
        }

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
