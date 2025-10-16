using MediatR;
using MiGenteEnLinea.Domain.Entities.Pagos;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetVentasByUserId;

/// <summary>
/// Query para obtener el historial de ventas/pagos de un usuario.
/// </summary>
/// <remarks>
/// NUEVA FUNCIONALIDAD (no existe en Legacy).
/// Uso: Dashboard de usuario, historial de pagos, auditoría.
/// Retorna ventas paginadas ordenadas por fecha descendente.
/// </remarks>
public record GetVentasByUserIdQuery : IRequest<List<Venta>>
{
    /// <summary>
    /// ID del usuario (Credencial.Id).
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Número de página (1-based). Default: 1.
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Tamaño de página. Default: 10.
    /// </summary>
    public int PageSize { get; init; } = 10;

    /// <summary>
    /// Si es true, solo retorna ventas aprobadas. Si es false, retorna todas.
    /// </summary>
    public bool SoloAprobadas { get; init; } = false;
}
