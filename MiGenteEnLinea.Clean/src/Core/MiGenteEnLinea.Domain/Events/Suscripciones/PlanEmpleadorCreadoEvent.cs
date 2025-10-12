using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Suscripciones;

/// <summary>
/// Evento lanzado cuando se crea un nuevo plan de empleador.
/// </summary>
public sealed class PlanEmpleadorCreadoEvent : DomainEvent
{
    public int PlanId { get; }
    public string Nombre { get; }
    public decimal Precio { get; }
    public int LimiteEmpleados { get; }

    public PlanEmpleadorCreadoEvent(int planId, string nombre, decimal precio, int limiteEmpleados)
    {
        PlanId = planId;
        Nombre = nombre;
        Precio = precio;
        LimiteEmpleados = limiteEmpleados;
    }
}
