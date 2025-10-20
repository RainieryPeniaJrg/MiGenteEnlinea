using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CancelarTrabajo;

/// <summary>
/// Command para cancelar un trabajo temporal (establece estatus = 3).
/// Migrado desde: EmpleadosService.cancelarTrabajo(int contratacionID, int detalleID)
/// </summary>
public record CancelarTrabajoCommand(
    int ContratacionId,
    int DetalleId
) : IRequest<bool>;
