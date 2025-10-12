namespace MiGenteEnLinea.Domain.ReadModels;

/// <summary>
/// Vista de solo lectura para consulta rápida de calificaciones promedio por contratista
/// </summary>
/// <remarks>
/// Esta vista calcula y almacena el promedio de calificaciones y el total de
/// calificaciones recibidas por identificación. Optimizada para búsquedas y
/// ordenamiento por calificación sin necesidad de cálculos en tiempo real.
/// </remarks>
public sealed class VistaPromedioCalificacion
{
    /// <summary>
    /// Identificación del contratista (cédula o RNC)
    /// </summary>
    public string? Identificacion { get; init; }

    /// <summary>
    /// Calificación promedio calculada (promedio de puntualidad, cumplimiento, conocimientos y recomendación)
    /// </summary>
    public decimal? CalificacionPromedio { get; init; }

    /// <summary>
    /// Total de calificaciones recibidas
    /// </summary>
    public int? TotalRegistros { get; init; }
}
