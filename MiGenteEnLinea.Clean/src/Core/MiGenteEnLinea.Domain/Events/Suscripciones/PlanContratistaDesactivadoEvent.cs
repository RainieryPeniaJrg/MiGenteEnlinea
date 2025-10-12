using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Suscripciones;

/// <summary>
/// Evento lanzado cuando se desactiva un plan de contratista.
/// </summary>
public sealed class PlanContratistaDesactivadoEvent : DomainEvent
{
    public int PlanId { get; }
    public string NombrePlan { get; }

    public PlanContratistaDesactivadoEvent(int planId, string nombrePlan)
    {
        PlanId = planId;
        NombrePlan = nombrePlan;
    }
}
