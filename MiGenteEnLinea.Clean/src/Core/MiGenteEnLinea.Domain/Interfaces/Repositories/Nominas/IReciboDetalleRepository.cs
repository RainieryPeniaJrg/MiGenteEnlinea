using MiGenteEnLinea.Domain.Entities.Nominas;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Nominas;

/// <summary>
/// Repositorio para entidad ReciboDetalle (líneas de detalle de recibos de nómina).
/// </summary>
public interface IReciboDetalleRepository : IRepository<ReciboDetalle>
{
    /// <summary>
    /// Obtiene todos los detalles de un recibo específico.
    /// </summary>
    /// <param name="pagoId">ID del recibo header</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Colección de detalles del recibo</returns>
    Task<IEnumerable<ReciboDetalle>> GetByPagoIdAsync(
        int pagoId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene detalles por tipo de concepto.
    /// </summary>
    /// <param name="pagoId">ID del recibo header</param>
    /// <param name="tipoConcepto">Tipo (1=Ingreso, 2=Deducción)</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Colección de detalles del tipo especificado</returns>
    Task<IEnumerable<ReciboDetalle>> GetByTipoConceptoAsync(
        int pagoId, 
        int tipoConcepto, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene solo los ingresos de un recibo.
    /// </summary>
    /// <param name="pagoId">ID del recibo header</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Colección de detalles de tipo ingreso</returns>
    Task<IEnumerable<ReciboDetalle>> GetIngresosAsync(
        int pagoId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene solo las deducciones de un recibo.
    /// </summary>
    /// <param name="pagoId">ID del recibo header</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Colección de detalles de tipo deducción</returns>
    Task<IEnumerable<ReciboDetalle>> GetDeduccionesAsync(
        int pagoId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Calcula el total de ingresos para un recibo.
    /// </summary>
    /// <param name="pagoId">ID del recibo header</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Suma total de ingresos</returns>
    Task<decimal> GetTotalIngresosAsync(
        int pagoId, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Calcula el total de deducciones para un recibo.
    /// </summary>
    /// <param name="pagoId">ID del recibo header</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Suma total de deducciones (valor absoluto)</returns>
    Task<decimal> GetTotalDeduccionesAsync(
        int pagoId, 
        CancellationToken cancellationToken = default);
}
