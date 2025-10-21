using MediatR;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.ProcesarVentaSinPago;

/// <summary>
/// Comando para procesar una venta sin pago (para planes gratuitos o promocionales).
/// </summary>
/// <remarks>
/// Usado cuando:
/// - Plan gratuito (Precio = 0)
/// - Plan promocional sin costo
/// - Suscripción de cortesía
/// </remarks>
public record ProcesarVentaSinPagoCommand : IRequest<int>
{
    /// <summary>
    /// ID del usuario que adquiere el plan.
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// ID del plan (debe ser gratuito).
    /// </summary>
    public int PlanId { get; init; }

    /// <summary>
    /// Motivo de la asignación gratuita (opcional).
    /// Ejemplo: "Plan promocional", "Cortesía por cliente VIP", etc.
    /// </summary>
    public string? Motivo { get; init; }
}
