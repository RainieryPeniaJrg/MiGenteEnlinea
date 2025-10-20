using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CalificarContratacion;

/// <summary>
/// Command para marcar una contratación como calificada
/// Migrado de: EmpleadosService.calificarContratacion (line 471)
/// </summary>
public record CalificarContratacionCommand : IRequest<bool>
{
    /// <summary>
    /// ID de la contratación a calificar
    /// </summary>
    public int ContratacionId { get; init; }

    /// <summary>
    /// ID de la calificación asociada
    /// </summary>
    public int CalificacionId { get; init; }
}
