using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.EliminarRecibo;

/// <summary>
/// Command para eliminar un recibo de contratación (Header + Detalle).
/// Migrado desde: EmpleadosService.eliminarReciboContratacion(int pagoID)
/// </summary>
public record EliminarReciboContratacionCommand(
    int PagoId
) : IRequest<bool>;
