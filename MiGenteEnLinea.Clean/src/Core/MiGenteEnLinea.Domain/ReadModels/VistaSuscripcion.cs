namespace MiGenteEnLinea.Domain.ReadModels;

/// <summary>
/// Vista de solo lectura para consulta de suscripciones activas con información del plan
/// </summary>
/// <remarks>
/// Esta vista combina información de suscripciones con el nombre del plan asociado,
/// facilitando reportes y consultas de suscripciones sin necesidad de JOINs adicionales.
/// Usada para verificar estado de suscripciones y fechas de vencimiento.
/// </remarks>
public sealed class VistaSuscripcion
{
    /// <summary>
    /// ID de la suscripción
    /// </summary>
    public int SuscripcionId { get; init; }

    /// <summary>
    /// ID del usuario suscrito
    /// </summary>
    public string? UserId { get; init; }

    /// <summary>
    /// ID del plan contratado
    /// </summary>
    public int? PlanId { get; init; }

    /// <summary>
    /// Fecha de vencimiento de la suscripción
    /// </summary>
    public DateOnly? Vencimiento { get; init; }

    /// <summary>
    /// Nombre del plan (ej: "Plan Básico", "Plan Premium")
    /// </summary>
    public string? Nombre { get; init; }

    /// <summary>
    /// Fecha del próximo pago programado
    /// </summary>
    public DateTime? ProximoPago { get; init; }

    /// <summary>
    /// Fecha de inicio de la suscripción
    /// </summary>
    public DateOnly? FechaInicio { get; init; }
}
