using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Catalogos;

/// <summary>
/// Evento de dominio que se dispara cuando se crea un nuevo servicio en el catálogo.
/// </summary>
public sealed class ServicioCreadoEvent : DomainEvent
{
    public int ServicioId { get; }
    public string Descripcion { get; }
    public string? Categoria { get; }

    public ServicioCreadoEvent(int servicioId, string descripcion, string? categoria)
    {
        ServicioId = servicioId;
        Descripcion = descripcion;
        Categoria = categoria;
    }
}
