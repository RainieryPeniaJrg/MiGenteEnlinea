namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// DTO para recibo de contratación con su detalle y empleado temporal.
/// Representa Empleador_Recibos_Header_Contrataciones + Detalle + EmpleadosTemporales
/// </summary>
public class ReciboContratacionDto
{
    // Header fields
    public int PagoId { get; set; }
    public string? UserId { get; set; }
    public int? ContratacionId { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaPago { get; set; }
    public string? ConceptoPago { get; set; }
    public int? Tipo { get; set; }

    // Detalle
    public List<ReciboContratacionDetalleDto> Detalles { get; set; } = new();

    // EmpleadoTemporal (from Include)
    public EmpleadoTemporalSimpleDto? EmpleadoTemporal { get; set; }

    // Calculated total from detalles
    public decimal Total => Detalles.Sum(d => d.Monto ?? 0);
}

/// <summary>
/// DTO para el detalle del recibo de contratación
/// </summary>
public class ReciboContratacionDetalleDto
{
    public int DetalleId { get; set; }
    public int? PagoId { get; set; }
    public string? Concepto { get; set; }
    public decimal? Monto { get; set; }
}

/// <summary>
/// DTO simplificado de empleado temporal para incluir en el recibo
/// </summary>
public class EmpleadoTemporalSimpleDto
{
    public int ContratacionId { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Cedula { get; set; }
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
}
