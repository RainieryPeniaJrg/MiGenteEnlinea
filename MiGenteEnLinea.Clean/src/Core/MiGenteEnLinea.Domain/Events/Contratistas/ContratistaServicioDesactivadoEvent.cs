using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contratistas;

/// <summary>
/// Evento de dominio que se dispara cuando un contratista desactiva un servicio en su perfil.
/// </summary>
public sealed class ContratistaServicioDesactivadoEvent : DomainEvent
{
    public int ServicioId { get; }
    public int ContratistaId { get; }

    public ContratistaServicioDesactivadoEvent(int servicioId, int contratistaId)
    {
        ServicioId = servicioId;
        ContratistaId = contratistaId;
    }
}
