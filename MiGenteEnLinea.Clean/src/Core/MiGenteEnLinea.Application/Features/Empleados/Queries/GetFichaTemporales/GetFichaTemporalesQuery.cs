using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetFichaTemporales;

/// <summary>
/// Query para obtener ficha de empleado temporal con detalles de contratación
/// Migrado de: EmpleadosService.obtenerFichaTemporales (line 517)
/// </summary>
public record GetFichaTemporalesQuery : IRequest<EmpleadoTemporalDto?>
{
    public int ContratacionId { get; init; }
    public string UserId { get; init; } = string.Empty;
}
