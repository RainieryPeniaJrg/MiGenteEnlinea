using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Suscripciones;

/// <summary>
/// Evento de dominio: Se cambió el plan de una suscripción
/// </summary>
public sealed class PlanCambiadoEvent : DomainEvent
{
    public int SuscripcionId { get; }
    public string UserId { get; }
    public int PlanAnterior { get; }
    public int PlanNuevo { get; }

    public PlanCambiadoEvent(
        int suscripcionId,
        string userId,
        int planAnterior,
        int planNuevo)
    {
        SuscripcionId = suscripcionId;
        UserId = userId;
        PlanAnterior = planAnterior;
        PlanNuevo = planNuevo;
    }
}
