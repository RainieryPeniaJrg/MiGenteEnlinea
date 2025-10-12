using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Suscripciones;

/// <summary>
/// Evento de dominio: Se creó una nueva suscripción
/// </summary>
public sealed class SuscripcionCreadaEvent : DomainEvent
{
    public int SuscripcionId { get; }
    public string UserId { get; }
    public int PlanId { get; }
    public DateOnly FechaVencimiento { get; }

    public SuscripcionCreadaEvent(
        int suscripcionId,
        string userId,
        int planId,
        DateOnly fechaVencimiento)
    {
        SuscripcionId = suscripcionId;
        UserId = userId;
        PlanId = planId;
        FechaVencimiento = fechaVencimiento;
    }
}
