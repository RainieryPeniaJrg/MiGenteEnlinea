namespace MiGenteEnLinea.Application.Features.Contrataciones.DTOs;

/// <summary>
/// DTO completo con todos los detalles de una contratación.
/// Usado para vistas de detalle.
/// </summary>
public class ContratacionDetalleDto
{
    public int DetalleId { get; set; }
    public int? ContratacionId { get; set; }
    public string DescripcionCorta { get; set; } = string.Empty;
    public string? DescripcionAmpliada { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFinal { get; set; }
    public decimal MontoAcordado { get; set; }
    public string? EsquemaPagos { get; set; }
    public int Estatus { get; set; }
    public string NombreEstado { get; set; } = string.Empty;
    public bool Calificado { get; set; }
    public int? CalificacionId { get; set; }
    public string? Notas { get; set; }
    public string? MotivoCancelacion { get; set; }
    public DateTime? FechaInicioReal { get; set; }
    public DateTime? FechaFinalizacionReal { get; set; }
    public int PorcentajeAvance { get; set; }

    // Propiedades calculadas
    public int DuracionEstimadaDias { get; set; }
    public int? DuracionRealDias { get; set; }
    public bool EstaRetrasada { get; set; }
    public bool PuedeSerCalificada { get; set; }
    public bool PuedeSerCancelada { get; set; }
    public bool PuedeSerModificada { get; set; }
}
