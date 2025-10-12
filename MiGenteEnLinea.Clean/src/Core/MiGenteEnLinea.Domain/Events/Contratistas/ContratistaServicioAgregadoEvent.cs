using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contratistas;

/// <summary>
/// Evento de dominio que se dispara cuando un contratista agrega un nuevo servicio a su perfil.
/// </summary>
public sealed class ContratistaServicioAgregadoEvent : DomainEvent
{
    public int ServicioId { get; }
    public int ContratistaId { get; }
    public string DetalleServicio { get; }

    public ContratistaServicioAgregadoEvent(int servicioId, int contratistaId, string detalleServicio)
    {
        ServicioId = servicioId;
        ContratistaId = contratistaId;
        DetalleServicio = detalleServicio;
    }
}
