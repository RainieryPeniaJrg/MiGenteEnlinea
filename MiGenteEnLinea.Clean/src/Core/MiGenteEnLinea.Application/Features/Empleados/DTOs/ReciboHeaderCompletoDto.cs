namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// DTO completo para Recibo con Header, Detalle y Empleado
/// Migrado de: Empleador_Recibos_Header con Include(Detalle).Include(Empleado)
/// </summary>
public class ReciboHeaderCompletoDto
{
    // Header fields (from Empleador_Recibos_Header)
    public int PagoId { get; set; }
    public string? UserId { get; set; }
    public int? EmpleadoId { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaPago { get; set; }
    public string? ConceptoPago { get; set; }
    public int? Tipo { get; set; }
    
    // Nested relationships
    public List<EmpleadorReciboDetalleDto> Detalles { get; set; } = new();
    public EmpleadoBasicoDto? Empleado { get; set; }
}

/// <summary>
/// DTO para detalles de recibo (Empleador_Recibos_Detalle)
/// Renombrado de ReciboDetalleDto para evitar conflicto con existente
/// </summary>
public class EmpleadorReciboDetalleDto
{
    public int DetalleId { get; set; }
    public int? PagoId { get; set; }
    public string? Concepto { get; set; }
    public decimal? Monto { get; set; }
}

/// <summary>
/// DTO básico de empleado (solo campos esenciales)
/// </summary>
public class EmpleadoBasicoDto
{
    public int EmpleadoId { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Identificacion { get; set; }
}
