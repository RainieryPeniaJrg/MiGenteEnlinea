using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Empleados;

/// <summary>
/// Evento de dominio que se dispara cuando se firma el contrato de un empleado.
/// </summary>
public sealed class ContratoFirmadoEvent : DomainEvent
{
    public int EmpleadoId { get; }
    public string UserId { get; }
    public string NombreCompleto { get; }
    public DateTime FechaFirma { get; }

    public ContratoFirmadoEvent(
        int empleadoId,
        string userId,
        string nombreCompleto,
        DateTime fechaFirma)
    {
        EmpleadoId = empleadoId;
        UserId = userId;
        NombreCompleto = nombreCompleto;
        FechaFirma = fechaFirma;
    }
}
