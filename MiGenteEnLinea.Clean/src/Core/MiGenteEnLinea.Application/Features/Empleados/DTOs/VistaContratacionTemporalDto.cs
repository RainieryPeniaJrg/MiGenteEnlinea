namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// DTO for VistaContratacionTemporal view
/// Migrado de: VContratacionesTemporales (Legacy)
/// </summary>
public class VistaContratacionTemporalDto
{
    // Contratación
    public int ContratacionId { get; set; }
    public string? UserId { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public int? Tipo { get; set; }
    
    // Información del Contratista
    public string? NombreComercial { get; set; }
    public string? Rnc { get; set; }
    public string? Identificacion { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Alias { get; set; }
    
    // Ubicación
    public string? Direccion { get; set; }
    public string? Provincia { get; set; }
    public string? Municipio { get; set; }
    public string? Telefono1 { get; set; }
    public string? Telefono2 { get; set; }
    
    // Detalle de Contratación
    public int DetalleId { get; set; }
    public int? Expr1 { get; set; }
    public string? DescripcionCorta { get; set; }
    public string? DescripcionAmpliada { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFinal { get; set; }
    public decimal? MontoAcordado { get; set; }
    public string? EsquemaPagos { get; set; }
    public int? Estatus { get; set; }
    
    // Composiciones
    public string? ComposicionNombre { get; set; }
    public string? ComposicionId { get; set; }
    
    // Calificaciones
    public int? Conocimientos { get; set; }
    public int? Puntualidad { get; set; }
    public int? Recomendacion { get; set; }
    public int? Cumplimiento { get; set; }
}
