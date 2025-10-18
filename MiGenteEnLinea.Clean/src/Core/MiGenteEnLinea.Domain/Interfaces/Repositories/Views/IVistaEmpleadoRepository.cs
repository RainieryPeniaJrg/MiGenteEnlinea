using MiGenteEnLinea.Domain.ReadModels;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Views;

/// <summary>
/// Repositorio de solo lectura para la vista VistaEmpleado
/// </summary>
/// <remarks>
/// Vista optimizada para consultas de empleados con toda su información.
/// Read-only: No se permiten operaciones de escritura.
/// </remarks>
public interface IVistaEmpleadoRepository : IReadOnlyRepository<VistaEmpleado>
{
    /// <summary>
    /// Obtiene empleados por UserId del empleador
    /// </summary>
    Task<IEnumerable<VistaEmpleado>> GetByEmpleadorIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene solo empleados activos de un empleador
    /// </summary>
    Task<IEnumerable<VistaEmpleado>> GetActivosByEmpleadorIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un empleado por identificación (cédula)
    /// </summary>
    Task<VistaEmpleado?> GetByIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca empleados por nombre (búsqueda parcial)
    /// </summary>
    Task<IEnumerable<VistaEmpleado>> SearchByNombreAsync(string userId, string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene empleados por periodo de pago
    /// </summary>
    Task<IEnumerable<VistaEmpleado>> GetByPeriodoPagoAsync(string userId, int periodoPago, CancellationToken cancellationToken = default);
}
