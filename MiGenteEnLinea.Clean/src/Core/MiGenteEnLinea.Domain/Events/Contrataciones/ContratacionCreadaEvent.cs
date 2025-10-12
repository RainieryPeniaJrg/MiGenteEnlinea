using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contrataciones;

/// <summary>
/// Evento de dominio que se dispara cuando se crea una nueva propuesta de contratación.
/// </summary>
public sealed class ContratacionCreadaEvent : DomainEvent
{
    public int DetalleId { get; }
    public string DescripcionCorta { get; }
    public DateOnly FechaInicio { get; }
    public DateOnly FechaFinal { get; }
    public decimal MontoAcordado { get; }

    public ContratacionCreadaEvent(
        int detalleId,
        string descripcionCorta,
        DateOnly fechaInicio,
        DateOnly fechaFinal,
        decimal montoAcordado)
    {
        DetalleId = detalleId;
        DescripcionCorta = descripcionCorta;
        FechaInicio = fechaInicio;
        FechaFinal = fechaFinal;
        MontoAcordado = montoAcordado;
    }
}
