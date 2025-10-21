using MediatR;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.CancelarSuscripcion;

/// <summary>
/// Comando para cancelar una suscripción activa.
/// </summary>
/// <remarks>
/// Funcionalidad nueva (no existe en Legacy).
/// Marca la suscripción como inactiva sin eliminarla (soft delete).
/// </remarks>
public record CancelarSuscripcionCommand : IRequest<Unit>
{
    /// <summary>
    /// ID del usuario cuya suscripción se cancelará.
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Motivo de cancelación (opcional).
    /// Ejemplo: "Usuario solicitó cancelación", "Falta de pago", etc.
    /// </summary>
    public string? MotivoCancelacion { get; init; }
}
