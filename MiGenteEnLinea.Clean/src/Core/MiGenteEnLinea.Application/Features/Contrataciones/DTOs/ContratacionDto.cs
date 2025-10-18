namespace MiGenteEnLinea.Application.Features.Contrataciones.DTOs;

/// <summary>
/// DTO simplificado para listados de contrataciones.
/// </summary>
public class ContratacionDto
{
    /// <summary>
    /// ID del detalle de contratación
    /// </summary>
    public int DetalleId { get; set; }

    /// <summary>
    /// ID de la contratación padre (EmpleadoTemporal)
    /// </summary>
    public int? ContratacionId { get; set; }

    /// <summary>
    /// Descripción breve del trabajo
    /// </summary>
    public string DescripcionCorta { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de inicio acordada
    /// </summary>
    public DateOnly FechaInicio { get; set; }

    /// <summary>
    /// Fecha de finalización acordada
    /// </summary>
    public DateOnly FechaFinal { get; set; }

    /// <summary>
    /// Monto total acordado
    /// </summary>
    public decimal MontoAcordado { get; set; }

    /// <summary>
    /// Estado actual (1=Pendiente, 2=Aceptada, 3=En Progreso, 4=Completada, 5=Cancelada, 6=Rechazada)
    /// </summary>
    public int Estatus { get; set; }

    /// <summary>
    /// Nombre del estado en texto
    /// </summary>
    public string NombreEstado { get; set; } = string.Empty;

    /// <summary>
    /// Indica si ya fue calificada
    /// </summary>
    public bool Calificado { get; set; }

    /// <summary>
    /// Porcentaje de avance (0-100)
    /// </summary>
    public int PorcentajeAvance { get; set; }

    /// <summary>
    /// Fecha real de inicio (si ya comenzó)
    /// </summary>
    public DateTime? FechaInicioReal { get; set; }

    /// <summary>
    /// Fecha real de finalización (si ya terminó)
    /// </summary>
    public DateTime? FechaFinalizacionReal { get; set; }
}
