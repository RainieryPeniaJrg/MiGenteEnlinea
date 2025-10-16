using MediatR;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.CreateSuscripcion;

/// <summary>
/// Comando para crear una nueva suscripción de usuario a un plan.
/// </summary>
/// <remarks>
/// Migrado desde: SuscripcionesService.guardarSuscripcion()
/// </remarks>
public record CreateSuscripcionCommand : IRequest<int>
{
    /// <summary>
    /// ID del usuario que adquiere la suscripción.
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// ID del plan al que se suscribe (Empleador o Contratista).
    /// </summary>
    public int PlanId { get; init; }

    /// <summary>
    /// Fecha de inicio de la suscripción.
    /// Por defecto: DateTime.UtcNow.
    /// </summary>
    public DateTime? FechaInicio { get; init; }
}
