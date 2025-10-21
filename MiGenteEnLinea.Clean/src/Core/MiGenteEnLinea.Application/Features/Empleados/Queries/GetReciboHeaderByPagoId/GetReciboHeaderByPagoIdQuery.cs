using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetReciboHeaderByPagoId;

/// <summary>
/// Query para obtener Recibo Header con su detalle por PagoID
/// Migrado de: EmpleadosService.GetEmpleador_ReciboByPagoID (line 212)
/// </summary>
public record GetReciboHeaderByPagoIdQuery : IRequest<ReciboHeaderCompletoDto?>
{
    public int PagoId { get; init; }
}
