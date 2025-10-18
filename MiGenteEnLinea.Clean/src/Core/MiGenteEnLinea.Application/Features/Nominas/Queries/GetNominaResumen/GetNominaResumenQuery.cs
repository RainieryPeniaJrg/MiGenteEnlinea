using MediatR;
using MiGenteEnLinea.Application.Features.Nominas.DTOs;

namespace MiGenteEnLinea.Application.Features.Nominas.Queries.GetNominaResumen;

/// <summary>
/// Query para obtener resumen de nómina por período.
/// Incluye totales, deducciones, estadísticas y desglose por empleado.
/// </summary>
public record GetNominaResumenQuery : IRequest<NominaResumenDto>
{
    /// <summary>
    /// ID del empleador
    /// </summary>
    public int EmpleadorId { get; init; }

    /// <summary>
    /// Período de consulta (ej: "2025-01", "2025-Q1")
    /// </summary>
    public string Periodo { get; init; } = string.Empty;

    /// <summary>
    /// Fecha inicio del período (alternativa a Periodo)
    /// </summary>
    public DateTime? FechaInicio { get; init; }

    /// <summary>
    /// Fecha fin del período (alternativa a Periodo)
    /// </summary>
    public DateTime? FechaFin { get; init; }

    /// <summary>
    /// Incluir detalle por empleado
    /// </summary>
    public bool IncluirDetalleEmpleados { get; init; } = true;
}
