using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Suscripciones;

/// <summary>
/// Evento lanzado cuando se desactiva un plan de empleador.
/// </summary>
public sealed class PlanEmpleadorDesactivadoEvent : DomainEvent
{
    public int PlanId { get; }
    public string Nombre { get; }

    public PlanEmpleadorDesactivadoEvent(int planId, string nombre)
    {
        PlanId = planId;
        Nombre = nombre;
    }
}
