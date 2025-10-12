using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contratistas;

/// <summary>
/// Evento de dominio que se dispara cuando un contratista desactiva una foto en su portafolio.
/// </summary>
public sealed class ContratistaFotoDesactivadaEvent : DomainEvent
{
    public int ImagenId { get; }
    public int ContratistaId { get; }

    public ContratistaFotoDesactivadaEvent(int imagenId, int contratistaId)
    {
        ImagenId = imagenId;
        ContratistaId = contratistaId;
    }
}
