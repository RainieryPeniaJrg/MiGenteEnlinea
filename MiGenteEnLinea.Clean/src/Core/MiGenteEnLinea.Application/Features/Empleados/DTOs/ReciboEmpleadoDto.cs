namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// DTO para recibos de empleados desde la vista VRecibosEmpleados.
/// </summary>
public class ReciboEmpleadoDto
{
    public decimal? Total { get; set; }
    public int PagoId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int? EmpleadoId { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaPago { get; set; }
    public string ConceptoPago { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
}
