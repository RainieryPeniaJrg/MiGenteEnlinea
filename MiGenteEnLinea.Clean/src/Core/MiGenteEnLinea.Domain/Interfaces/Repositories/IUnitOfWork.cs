// using Microsoft.EntityFrameworkCore.Storage; // REMOVED: Domain layer should NOT reference EF Core

namespace MiGenteEnLinea.Domain.Interfaces.Repositories;

/// <summary>
/// Patrón Unit of Work para manejo de transacciones y coordinación de repositorios.
/// Agrupa múltiples operaciones de repositorios en una sola transacción de base de datos.
/// </summary>
/// <remarks>
/// El patrón Unit of Work proporciona:
/// 1. **Transacciones explícitas**: Control total sobre BEGIN, COMMIT, ROLLBACK
/// 2. **Consistencia de datos**: Todas las operaciones se confirman o revierten juntas
/// 3. **Coordinación de repositorios**: Acceso centralizado a todos los repositorios
/// 4. **Performance**: Una sola llamada a SaveChanges() para múltiples operaciones
/// 
/// Ejemplo de uso:
/// <code>
/// public class ProcesarVentaCommandHandler
/// {
///     private readonly IUnitOfWork _unitOfWork;
///     
///     public async Task Handle(ProcesarVentaCommand request, CancellationToken ct)
///     {
///         await _unitOfWork.BeginTransactionAsync(ct);
///         
///         try
///         {
///             // Operación 1: Crear venta
///             var venta = Venta.Create(...);
///             await _unitOfWork.Ventas.AddAsync(venta, ct);
///             
///             // Operación 2: Actualizar suscripción
///             var suscripcion = await _unitOfWork.Suscripciones.GetByIdAsync(request.SuscripcionId, ct);
///             suscripcion.Renovar(...);
///             _unitOfWork.Suscripciones.Update(suscripcion);
///             
///             // Operación 3: Registrar pago
///             var pago = PaymentGateway.Create(...);
///             await _unitOfWork.Pagos.AddAsync(pago, ct);
///             
///             // Commit: Si todas las operaciones fueron exitosas
///             await _unitOfWork.SaveChangesAsync(ct);
///             await _unitOfWork.CommitTransactionAsync(ct);
///             
///             return Result.Success();
///         }
///         catch (Exception ex)
///         {
///             // Rollback: Si alguna operación falló
///             await _unitOfWork.RollbackTransactionAsync(ct);
///             throw;
///         }
///     }
/// }
/// </code>
/// </remarks>
public interface IUnitOfWork : IDisposable
{
    // ========================================
    // REPOSITORIOS (Lazy Loading) - LOTE 0 SIMPLIFIED
    // ========================================
    // LOTE 0: Only core Rich Domain Model repositories
    // TODO LOTES 1-8: Add remaining repositories as they're implemented

    // Authentication
    Authentication.ICredencialRepository Credenciales { get; }

    // Empleadores
    Empleadores.IEmpleadorRepository Empleadores { get; }

    // Contratistas
    Contratistas.IContratistaRepository Contratistas { get; }

    // Empleados
    Empleados.IEmpleadoRepository Empleados { get; }

    // Suscripciones
    Suscripciones.ISuscripcionRepository Suscripciones { get; }
    Suscripciones.IPlanEmpleadorRepository PlanesEmpleadores { get; }
    Suscripciones.IPlanContratistaRepository PlanesContratistas { get; }

    // Pagos
    Pagos.IVentaRepository Ventas { get; }

    // Calificaciones
    Calificaciones.ICalificacionRepository Calificaciones { get; }

    // ========================================
    // TODO LOTE 1-8: Uncomment as repositories are created
    // ========================================
    // Contratistas.IContratistaServicioRepository ContratistasServicios { get; }
    // Contratistas.IContratistaFotoRepository ContratistasFotos { get; }
    // Empleados.IEmpleadoNotaRepository EmpleadosNotas { get; }
    // Empleados.IReciboHeaderRepository RecibosHeader { get; }
    // Empleados.IReciboDetalleRepository RecibosDetalle { get; }
    // Nominas.IDeduccionTssRepository DeduccionesTss { get; }
    // Suscripciones.IPlanEmpleadorRepository PlanesEmpleadores { get; }
    // Suscripciones.IPlanContratistaRepository PlanesContratistas { get; }
    // Pagos.IVentaRepository Ventas { get; }
    // Pagos.ITransaccionRepository Transacciones { get; }
    // Pagos.IPaymentGatewayRepository Pagos { get; }
    // Pagos.IEmpleadorRecibosHeaderContratacioneRepository RecibosHeaderContrataciones { get; }
    // Pagos.IEmpleadorRecibosDetalleContratacioneRepository RecibosDetalleContrataciones { get; }
    // Seguridad.IPerfileRepository Perfiles { get; }
    // Seguridad.IPermisoRepository Permisos { get; }
    // Contrataciones.IContratacionRepository Contrataciones { get; }
    // Contrataciones.IContratoServicioRepository ContratosServicio { get; }
    // Contrataciones.IServicioOfertadoRepository ServiciosOfertados { get; }
    // Contrataciones.IEmpleadoTemporalRepository EmpleadosTemporales { get; }
    // Contrataciones.IDetalleContratacionRepository DetallesContrataciones { get; }
    // Catalogos.IProvinciaRepository Provincias { get; }
    // Catalogos.ISectorRepository Sectores { get; }
    // Catalogos.IServicioRepository Servicios { get; }
    // Configuracion.IConfigCorreoRepository ConfiguracionCorreo { get; }

    // ========================================
    // TRANSACTION MANAGEMENT
    // ========================================

    /// <summary>
    /// Inicia una transacción de base de datos explícita
    /// LOTE 0: Simplified - no devuelve IDbContextTransaction (dependency en EF Core)
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <remarks>
    /// Use esto cuando necesite control explícito sobre transacciones.
    /// No olvide llamar CommitTransactionAsync() o RollbackTransactionAsync().
    /// </remarks>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirma la transacción actual (COMMIT)
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <remarks>
    /// Aplica todos los cambios realizados dentro de la transacción.
    /// Si ocurre un error durante el commit, la transacción se revierte automáticamente.
    /// </remarks>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Revierte la transacción actual (ROLLBACK)
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <remarks>
    /// Descarta todos los cambios realizados dentro de la transacción.
    /// Use esto en bloques catch cuando una operación falla.
    /// </remarks>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    // ========================================
    // PERSISTENCE
    // ========================================

    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Número de entidades afectadas</returns>
    /// <remarks>
    /// Esta es la operación que realmente ejecuta INSERT, UPDATE, DELETE en la DB.
    /// Todos los cambios realizados en repositorios se aplican aquí.
    /// </remarks>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
