namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// DTO for VPagosContrataciones view
/// Represents payment records for contractor services
/// </summary>
public class PagoContratacionDto
{
    public int PagoId { get; set; }
    public string? UserId { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaPago { get; set; }
    public string? Expr1 { get; set; } // Expression from view
    public decimal? Monto { get; set; }
    public int? ContratacionId { get; set; }
    public int? DetalleId { get; set; }
}
