using MediatR;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.RenovarSuscripcion;

/// <summary>
/// Comando para renovar una suscripción existente extendiendo su vencimiento.
/// </summary>
/// <remarks>
/// Funcionalidad nueva (no existe en Legacy).
/// Útil para renovaciones manuales sin procesar pago.
/// </remarks>
public record RenovarSuscripcionCommand : IRequest<Unit>
{
    /// <summary>
    /// ID del usuario cuya suscripción se renovará.
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Cantidad de meses a extender la suscripción.
    /// Por defecto: 1 mes.
    /// </summary>
    public int MesesExtension { get; init; } = 1;

    /// <summary>
    /// Motivo de la renovación (opcional).
    /// Ejemplo: "Cortesía", "Compensación por inconvenientes", etc.
    /// </summary>
    public string? Motivo { get; init; }
}
