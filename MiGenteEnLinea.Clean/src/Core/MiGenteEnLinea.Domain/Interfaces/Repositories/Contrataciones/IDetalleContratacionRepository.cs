namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Contrataciones;

/// <summary>
/// Repositorio para gestionar las contrataciones entre empleadores y contratistas.
/// </summary>
public interface IDetalleContratacionRepository : IRepository<Domain.Entities.Contrataciones.DetalleContratacion>
{
    /// <summary>
    /// Obtiene todas las contrataciones asociadas a una contratación padre (EmpleadoTemporal).
    /// </summary>
    /// <param name="contratacionId">ID de la contratación padre</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de detalles de contratación ordenados por FechaInicio DESC</returns>
    Task<IEnumerable<Domain.Entities.Contrataciones.DetalleContratacion>> GetByContratacionIdAsync(
        int contratacionId, 
        CancellationToken ct = default);

    /// <summary>
    /// Obtiene las contrataciones de un empleador filtradas por estado.
    /// </summary>
    /// <param name="empleadorId">ID del empleador (se obtiene de la relación con Contratacion)</param>
    /// <param name="estatus">Estado de la contratación (1=Pendiente, 2=Aceptada, 3=En Progreso, 4=Completada, 5=Cancelada, 6=Rechazada)</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de contrataciones ordenadas por FechaInicio DESC</returns>
    Task<IEnumerable<Domain.Entities.Contrataciones.DetalleContratacion>> GetByEstatusAsync(
        int estatus, 
        CancellationToken ct = default);

    /// <summary>
    /// Obtiene las contrataciones pendientes de calificación (Completadas y no calificadas).
    /// </summary>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de contrataciones completadas sin calificar</returns>
    Task<IEnumerable<Domain.Entities.Contrataciones.DetalleContratacion>> GetPendientesCalificacionAsync(
        CancellationToken ct = default);

    /// <summary>
    /// Obtiene las contrataciones activas (En Progreso).
    /// </summary>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de contrataciones en progreso ordenadas por FechaInicio DESC</returns>
    Task<IEnumerable<Domain.Entities.Contrataciones.DetalleContratacion>> GetActivasAsync(
        CancellationToken ct = default);

    /// <summary>
    /// Obtiene las contrataciones retrasadas (En Progreso y FechaFinal < hoy).
    /// </summary>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de contrataciones retrasadas</returns>
    Task<IEnumerable<Domain.Entities.Contrataciones.DetalleContratacion>> GetRetrasadasAsync(
        CancellationToken ct = default);
}
