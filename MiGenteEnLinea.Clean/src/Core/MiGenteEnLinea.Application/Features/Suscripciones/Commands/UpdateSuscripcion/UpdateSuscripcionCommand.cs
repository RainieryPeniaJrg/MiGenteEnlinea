using MediatR;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.UpdateSuscripcion;

/// <summary>
/// Comando para actualizar el plan y vencimiento de una suscripción existente.
/// </summary>
/// <remarks>
/// Migrado desde: SuscripcionesService.actualizarSuscripcion()
/// </remarks>
public record UpdateSuscripcionCommand : IRequest<Unit>
{
    /// <summary>
    /// ID del usuario cuya suscripción se actualizará.
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// ID del nuevo plan.
    /// </summary>
    public int NuevoPlanId { get; init; }

    /// <summary>
    /// Nueva fecha de vencimiento.
    /// </summary>
    public DateTime NuevoVencimiento { get; init; }
}
