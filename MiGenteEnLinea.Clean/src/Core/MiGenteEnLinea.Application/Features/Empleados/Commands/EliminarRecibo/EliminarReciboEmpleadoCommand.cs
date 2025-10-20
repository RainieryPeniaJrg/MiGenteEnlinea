using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.EliminarRecibo;

/// <summary>
/// Command para eliminar un recibo de empleado (Header + Detalle).
/// Migrado desde: EmpleadosService.eliminarReciboEmpleado(int pagoID)
/// </summary>
public record EliminarReciboEmpleadoCommand(
    int PagoId
) : IRequest<bool>;
