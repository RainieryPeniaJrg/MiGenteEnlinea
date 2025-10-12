using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Catalogos;

/// <summary>
/// Evento de dominio que se dispara cuando se activa un servicio en el catálogo.
/// </summary>
public sealed class ServicioActivadoEvent : DomainEvent
{
    public int ServicioId { get; }
    public string Descripcion { get; }

    public ServicioActivadoEvent(int servicioId, string descripcion)
    {
        ServicioId = servicioId;
        Descripcion = descripcion;
    }
}
