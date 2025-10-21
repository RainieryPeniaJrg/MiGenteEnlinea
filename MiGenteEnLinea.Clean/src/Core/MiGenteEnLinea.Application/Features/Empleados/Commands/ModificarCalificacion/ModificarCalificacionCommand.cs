using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.ModificarCalificacion;

/// <summary>
/// Command para modificar una calificación existente
/// Migrado de: EmpleadosService.modificarCalificacionDeContratacion (line 491)
/// </summary>
public record ModificarCalificacionCommand : IRequest<bool>
{
    public int CalificacionId { get; init; }
    public string? Identificacion { get; init; }
    public int? Conocimientos { get; init; }
    public int? Cumplimiento { get; init; }
    public DateTime? Fecha { get; init; }
    public string? Nombre { get; init; }
    public int? Puntualidad { get; init; }
    public int? Recomendacion { get; init; }
    public string? Tipo { get; init; }
    public string? UserId { get; init; }
}
