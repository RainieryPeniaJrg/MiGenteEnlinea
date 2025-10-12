using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contratistas;

/// <summary>
/// Evento de dominio que se dispara cuando un contratista activa una foto en su portafolio.
/// </summary>
public sealed class ContratistaFotoActivadaEvent : DomainEvent
{
    public int ImagenId { get; }
    public int ContratistaId { get; }

    public ContratistaFotoActivadaEvent(int imagenId, int contratistaId)
    {
        ImagenId = imagenId;
        ContratistaId = contratistaId;
    }
}
