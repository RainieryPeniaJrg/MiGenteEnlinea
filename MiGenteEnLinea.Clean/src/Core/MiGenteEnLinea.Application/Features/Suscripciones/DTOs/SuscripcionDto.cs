namespace MiGenteEnLinea.Application.Features.Suscripciones.DTOs;

/// <summary>
/// DTO para representar una suscripción en las respuestas de la API.
/// </summary>
/// <remarks>
/// Incluye propiedades computadas como EstaActiva y DiasRestantes
/// para facilitar el consumo por parte del frontend.
/// </remarks>
public record SuscripcionDto
{
    /// <summary>
    /// ID único de la suscripción.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// ID del usuario (Credencial.Id).
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// ID del plan asociado (PlanEmpleador o PlanContratista).
    /// </summary>
    public int PlanId { get; init; }

    /// <summary>
    /// Fecha de vencimiento de la suscripción.
    /// </summary>
    public DateOnly Vencimiento { get; init; }

    /// <summary>
    /// Fecha de inicio de la suscripción.
    /// </summary>
    public DateTime FechaInicio { get; init; }

    /// <summary>
    /// Indica si la suscripción está activa (no vencida y no cancelada).
    /// Propiedad computada desde el dominio.
    /// </summary>
    public bool EstaActiva { get; init; }

    /// <summary>
    /// Indica si la suscripción fue cancelada.
    /// </summary>
    public bool Cancelada { get; init; }

    /// <summary>
    /// Número de días restantes hasta el vencimiento.
    /// Valor negativo si ya venció. Propiedad computada.
    /// </summary>
    public int DiasRestantes { get; init; }

    /// <summary>
    /// Fecha en que se canceló la suscripción (si aplica).
    /// </summary>
    public DateTime? FechaCancelacion { get; init; }

    /// <summary>
    /// Motivo de cancelación (si aplica).
    /// </summary>
    public string? RazonCancelacion { get; init; }
}
