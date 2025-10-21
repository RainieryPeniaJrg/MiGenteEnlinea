using MiGenteEnLinea.Domain.Entities.Nominas;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Nominas;

/// <summary>
/// Repositorio para entidad ReciboHeader (encabezado de recibos de nómina).
/// </summary>
public interface IReciboHeaderRepository : IRepository<ReciboHeader>
{
    /// <summary>
    /// Obtiene todos los recibos de un empleado específico.
    /// </summary>
    /// <param name="empleadoId">ID del empleado</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Colección de recibos del empleado</returns>
    Task<IEnumerable<ReciboHeader>> GetByEmpleadoIdAsync(
        int empleadoId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los recibos de un empleador específico.
    /// </summary>
    /// <param name="userId">ID del empleador</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Colección de recibos del empleador</returns>
    Task<IEnumerable<ReciboHeader>> GetByEmpleadorIdAsync(
        string userId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene recibos por período específico.
    /// </summary>
    /// <param name="periodoInicio">Fecha de inicio del período</param>
    /// <param name="periodoFin">Fecha de fin del período</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Colección de recibos en el período</returns>
    Task<IEnumerable<ReciboHeader>> GetByPeriodoAsync(
        DateOnly periodoInicio, 
        DateOnly periodoFin, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene recibos por estado.
    /// </summary>
    /// <param name="estado">Estado del recibo (1=Pendiente, 2=Pagado, 3=Anulado)</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Colección de recibos con el estado especificado</returns>
    Task<IEnumerable<ReciboHeader>> GetByEstadoAsync(
        int estado, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un recibo con sus detalles incluidos.
    /// </summary>
    /// <param name="pagoId">ID del recibo</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Recibo con detalles cargados, o null si no existe</returns>
    Task<ReciboHeader?> GetWithDetallesAsync(
        int pagoId, 
        CancellationToken cancellationToken = default);
}
