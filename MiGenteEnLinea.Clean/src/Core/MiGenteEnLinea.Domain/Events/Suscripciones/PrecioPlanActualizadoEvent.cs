using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Suscripciones;

/// <summary>
/// Evento lanzado cuando se actualiza el precio de un plan de empleador.
/// </summary>
public sealed class PrecioPlanActualizadoEvent : DomainEvent
{
    public int PlanId { get; }
    public string NombrePlan { get; }
    public decimal PrecioAnterior { get; }
    public decimal PrecioNuevo { get; }

    public PrecioPlanActualizadoEvent(int planId, string nombrePlan, decimal precioAnterior, decimal precioNuevo)
    {
        PlanId = planId;
        NombrePlan = nombrePlan;
        PrecioAnterior = precioAnterior;
        PrecioNuevo = precioNuevo;
    }
}
