using MiGenteEnLinea.Domain.ReadModels;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Views;

/// <summary>
/// Repositorio de solo lectura para la vista VistaCalificacion
/// </summary>
public interface IVistaCalificacionRepository : IReadOnlyRepository<VistaCalificacion>
{
    /// <summary>
    /// Obtiene calificaciones de un contratista
    /// </summary>
    Task<IEnumerable<VistaCalificacion>> GetByContratistaIdAsync(int contratistaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene calificaciones hechas por un usuario
    /// </summary>
    Task<IEnumerable<VistaCalificacion>> GetByUsuarioIdAsync(string userId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repositorio de solo lectura para la vista VistaPromedioCalificacion
/// </summary>
public interface IVistaPromedioCalificacionRepository : IReadOnlyRepository<VistaPromedioCalificacion>
{
    /// <summary>
    /// Obtiene el promedio de calificación de un contratista
    /// </summary>
    Task<VistaPromedioCalificacion?> GetByContratistaIdAsync(int contratistaId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repositorio de solo lectura para la vista VistaSuscripcion
/// </summary>
public interface IVistaSuscripcionRepository : IReadOnlyRepository<VistaSuscripcion>
{
    /// <summary>
    /// Obtiene suscripciones por UserId
    /// </summary>
    Task<IEnumerable<VistaSuscripcion>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene suscripción activa de un usuario
    /// </summary>
    Task<VistaSuscripcion?> GetActivaByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repositorio de solo lectura para la vista VistaPago
/// </summary>
public interface IVistaPagoRepository : IReadOnlyRepository<VistaPago>
{
    /// <summary>
    /// Obtiene pagos por UserId
    /// </summary>
    Task<IEnumerable<VistaPago>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene pagos por rango de fechas
    /// </summary>
    Task<IEnumerable<VistaPago>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repositorio de solo lectura para la vista VistaPagoContratacion
/// </summary>
public interface IVistaPagoContratacionRepository : IReadOnlyRepository<VistaPagoContratacion>
{
    /// <summary>
    /// Obtiene pagos de contrataciones por empleador
    /// </summary>
    Task<IEnumerable<VistaPagoContratacion>> GetByEmpleadorIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene pagos de contrataciones por contratista
    /// </summary>
    Task<IEnumerable<VistaPagoContratacion>> GetByContratistaIdAsync(int contratistaId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repositorio de solo lectura para la vista VistaContratacionTemporal
/// </summary>
public interface IVistaContratacionTemporalRepository : IReadOnlyRepository<VistaContratacionTemporal>
{
    /// <summary>
    /// Obtiene contrataciones por empleador
    /// </summary>
    Task<IEnumerable<VistaContratacionTemporal>> GetByEmpleadorIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene contrataciones por contratista
    /// </summary>
    Task<IEnumerable<VistaContratacionTemporal>> GetByContratistaIdAsync(int contratistaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene contrataciones activas (en progreso)
    /// </summary>
    Task<IEnumerable<VistaContratacionTemporal>> GetActivasAsync(CancellationToken cancellationToken = default);
}
