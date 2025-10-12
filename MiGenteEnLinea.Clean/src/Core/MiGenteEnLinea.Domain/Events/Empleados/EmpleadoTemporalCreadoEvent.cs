using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Empleados;

/// <summary>
/// Evento de dominio que se dispara cuando se crea un empleado temporal.
/// </summary>
public sealed class EmpleadoTemporalCreadoEvent : DomainEvent
{
    public int ContratacionId { get; }
    public string UserId { get; }
    public string NombreCompleto { get; }
    public int Tipo { get; }

    public EmpleadoTemporalCreadoEvent(
        int contratacionId,
        string userId,
        string nombreCompleto,
        int tipo)
    {
        ContratacionId = contratacionId;
        UserId = userId;
        NombreCompleto = nombreCompleto;
        Tipo = tipo;
    }
}
