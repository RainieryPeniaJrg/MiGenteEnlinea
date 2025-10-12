namespace MiGenteEnLinea.Domain.ReadModels;

/// <summary>
/// Vista de solo lectura para consultas de pagos a empleados permanentes
/// </summary>
/// <remarks>
/// Esta vista agrega información de recibos de pago (headers) con sus detalles
/// para obtener el monto total de cada pago. Usada para reportes de nómina
/// y auditoría de pagos a empleados.
/// </remarks>
public sealed class VistaPago
{
    /// <summary>
    /// ID del pago
    /// </summary>
    public int PagoId { get; init; }

    /// <summary>
    /// ID del usuario empleador que realizó el pago
    /// </summary>
    public string? UserId { get; init; }

    /// <summary>
    /// ID del empleado que recibió el pago
    /// </summary>
    public int? EmpleadoId { get; init; }

    /// <summary>
    /// Fecha de registro del recibo
    /// </summary>
    public DateTime? FechaRegistro { get; init; }

    /// <summary>
    /// Fecha en que se realizó el pago
    /// </summary>
    public DateTime? FechaPago { get; init; }

    /// <summary>
    /// Campo de expresión 1 (auxiliar)
    /// </summary>
    public string? Expr1 { get; init; }

    /// <summary>
    /// Monto total del pago (suma de todos los detalles)
    /// </summary>
    public decimal? Monto { get; init; }
}
