using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contratistas;

/// <summary>
/// Evento de dominio que se dispara cuando un contratista activa un servicio en su perfil.
/// </summary>
public sealed class ContratistaServicioActivadoEvent : DomainEvent
{
    public int ServicioId { get; }
    public int ContratistaId { get; }

    public ContratistaServicioActivadoEvent(int servicioId, int contratistaId)
    {
        ServicioId = servicioId;
        ContratistaId = contratistaId;
    }
}
