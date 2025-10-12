using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Suscripciones;

/// <summary>
/// Evento lanzado cuando se crea un nuevo plan de contratista.
/// </summary>
public sealed class PlanContratistaCreadoEvent : DomainEvent
{
    public int PlanId { get; }
    public string NombrePlan { get; }
    public decimal Precio { get; }

    public PlanContratistaCreadoEvent(int planId, string nombrePlan, decimal precio)
    {
        PlanId = planId;
        NombrePlan = nombrePlan;
        Precio = precio;
    }
}
