using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Catalogos;

/// <summary>
/// Evento de dominio que se dispara cuando se desactiva un servicio en el catálogo.
/// </summary>
public sealed class ServicioDesactivadoEvent : DomainEvent
{
    public int ServicioId { get; }
    public string Descripcion { get; }

    public ServicioDesactivadoEvent(int servicioId, string descripcion)
    {
        ServicioId = servicioId;
        Descripcion = descripcion;
    }
}
