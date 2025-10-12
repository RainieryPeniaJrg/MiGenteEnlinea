using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Suscripciones;

/// <summary>
/// Evento de dominio: Se canceló una suscripción
/// </summary>
public sealed class SuscripcionCanceladaEvent : DomainEvent
{
    public int SuscripcionId { get; }
    public string UserId { get; }
    public string? Razon { get; }

    public SuscripcionCanceladaEvent(
        int suscripcionId,
        string userId,
        string? razon)
    {
        SuscripcionId = suscripcionId;
        UserId = userId;
        Razon = razon;
    }
}
