using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Suscripciones;

/// <summary>
/// Evento de dominio: Se renovó una suscripción
/// </summary>
public sealed class SuscripcionRenovadaEvent : DomainEvent
{
    public int SuscripcionId { get; }
    public string UserId { get; }
    public DateOnly NuevoVencimiento { get; }

    public SuscripcionRenovadaEvent(
        int suscripcionId,
        string userId,
        DateOnly nuevoVencimiento)
    {
        SuscripcionId = suscripcionId;
        UserId = userId;
        NuevoVencimiento = nuevoVencimiento;
    }
}
