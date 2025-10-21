using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetVistaContratacionTemporal;

/// <summary>
/// Query para obtener Vista de Contratación Temporal
/// Migrado de: EmpleadosService.obtenerVistaTemporal (line 554)
/// </summary>
public record GetVistaContratacionTemporalQuery : IRequest<VistaContratacionTemporalDto?>
{
    public int ContratacionId { get; init; }
    public string UserId { get; init; } = string.Empty;
}
