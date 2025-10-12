using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Suscripciones;

/// <summary>
/// Evento de dominio: Se reactivó una suscripción cancelada
/// </summary>
public sealed class SuscripcionReactivadaEvent : DomainEvent
{
    public int SuscripcionId { get; }
    public string UserId { get; }

    public SuscripcionReactivadaEvent(
        int suscripcionId,
        string userId)
    {
        SuscripcionId = suscripcionId;
        UserId = userId;
    }
}
