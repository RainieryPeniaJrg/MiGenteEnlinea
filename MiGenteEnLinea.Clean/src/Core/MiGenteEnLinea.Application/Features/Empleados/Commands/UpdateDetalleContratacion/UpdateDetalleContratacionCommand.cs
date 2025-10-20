using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.UpdateDetalleContratacion;

/// <summary>
/// Command to update an existing DetalleContrataciones entry
/// Migrated from: EmpleadosService.actualizarContratacion(DetalleContrataciones det) - line 448
/// </summary>
public record UpdateDetalleContratacionCommand : IRequest<bool>
{
    public int ContratacionId { get; init; }
    public string? DescripcionCorta { get; init; }
    public string? DescripcionAmpliada { get; init; }
    public DateTime? FechaInicio { get; init; }
    public DateTime? FechaFin { get; init; }
    public decimal? MontoAcordado { get; init; }
    public string? EsquemaPagos { get; init; }
    public int? Estatus { get; init; }
}
