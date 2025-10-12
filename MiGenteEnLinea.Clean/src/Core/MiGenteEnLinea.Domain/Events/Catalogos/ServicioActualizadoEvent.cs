using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Catalogos;

/// <summary>
/// Evento de dominio que se dispara cuando se actualiza la descripción de un servicio.
/// </summary>
public sealed class ServicioActualizadoEvent : DomainEvent
{
    public int ServicioId { get; }
    public string DescripcionAnterior { get; }
    public string DescripcionNueva { get; }

    public ServicioActualizadoEvent(int servicioId, string descripcionAnterior, string descripcionNueva)
    {
        ServicioId = servicioId;
        DescripcionAnterior = descripcionAnterior;
        DescripcionNueva = descripcionNueva;
    }
}
