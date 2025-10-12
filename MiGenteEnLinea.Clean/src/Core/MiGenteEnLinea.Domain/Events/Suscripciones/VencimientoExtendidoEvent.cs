using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Suscripciones;

/// <summary>
/// Evento de dominio: Se extendió el vencimiento de una suscripción
/// </summary>
public sealed class VencimientoExtendidoEvent : DomainEvent
{
    public int SuscripcionId { get; }
    public string UserId { get; }
    public int DiasExtendidos { get; }
    public DateOnly NuevoVencimiento { get; }

    public VencimientoExtendidoEvent(
        int suscripcionId,
        string userId,
        int diasExtendidos,
        DateOnly nuevoVencimiento)
    {
        SuscripcionId = suscripcionId;
        UserId = userId;
        DiasExtendidos = diasExtendidos;
        NuevoVencimiento = nuevoVencimiento;
    }
}
