namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// DTO para EmpleadosTemporales con DetalleContrataciones incluido
/// </summary>
public class EmpleadoTemporalDto
{
    public int ContratacionId { get; set; }
    public string? UserId { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public int? Tipo { get; set; }
    public string? NombreComercial { get; set; }
    public string? Rnc { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Identificacion { get; set; }
    public string? Telefono1 { get; set; }
    public string? Direccion { get; set; }

    // Nested DetalleContrataciones
    public DetalleContratacionDto? Detalle { get; set; }
}

public class DetalleContratacionDto
{
    public int DetalleId { get; set; }
    public int? ContratacionId { get; set; }
    public string? DescripcionCorta { get; set; }
    public string? DescripcionAmpliada { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFinal { get; set; }
    public decimal? MontoAcordado { get; set; }
    public string? EsquemaPagos { get; set; }
    public int? Estatus { get; set; }
    public bool? Calificado { get; set; }
    public int? CalificacionId { get; set; }
}
