using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetReciboById;

/// <summary>
/// Query para obtener un recibo de pago por su ID con todos los detalles.
/// Mapea: EmpleadosService.GetEmpleador_ReciboByPagoID()
/// </summary>
public record GetReciboByIdQuery : IRequest<ReciboDetalleDto>
{
    /// <summary>
    /// GUID del empleador (para validar propiedad)
    /// </summary>
    public string UserId { get; init; } = null!;

    /// <summary>
    /// ID del recibo (PagoId)
    /// </summary>
    public int PagoId { get; init; }
}
