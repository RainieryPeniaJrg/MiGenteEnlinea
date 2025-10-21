namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Contratistas;

/// <summary>
/// Repositorio para gestionar los servicios que ofrece un contratista.
/// </summary>
public interface IContratistaServicioRepository : IRepository<Domain.Entities.Contratistas.ContratistaServicio>
{
    /// <summary>
    /// Obtiene todos los servicios (activos e inactivos) de un contratista.
    /// </summary>
    /// <param name="contratistaId">ID del contratista</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de servicios ordenados por Orden ASC</returns>
    Task<IEnumerable<Domain.Entities.Contratistas.ContratistaServicio>> GetByContratistaIdAsync(
        int contratistaId, 
        CancellationToken ct = default);

    /// <summary>
    /// Obtiene solo los servicios activos de un contratista.
    /// </summary>
    /// <param name="contratistaId">ID del contratista</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de servicios activos ordenados por Orden ASC</returns>
    Task<IEnumerable<Domain.Entities.Contratistas.ContratistaServicio>> GetActivosByContratistaIdAsync(
        int contratistaId, 
        CancellationToken ct = default);

    /// <summary>
    /// Verifica si un contratista ya tiene un servicio con el mismo detalle.
    /// </summary>
    /// <param name="contratistaId">ID del contratista</param>
    /// <param name="detalleServicio">Detalle del servicio a verificar</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>True si ya existe, False en caso contrario</returns>
    Task<bool> ExisteServicioAsync(
        int contratistaId, 
        string detalleServicio, 
        CancellationToken ct = default);
}
