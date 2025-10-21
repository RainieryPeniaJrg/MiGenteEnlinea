using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetReciboContratacion;

/// <summary>
/// Query para obtener un recibo de contratación con su detalle y empleado temporal.
/// Migrado desde: EmpleadosService.GetContratacion_ReciboByPagoID(int pagoID) - line 222
/// </summary>
/// <param name="PagoId">ID del pago/recibo a consultar</param>
public record GetReciboContratacionQuery(
    int PagoId
) : IRequest<ReciboContratacionDto?>;
