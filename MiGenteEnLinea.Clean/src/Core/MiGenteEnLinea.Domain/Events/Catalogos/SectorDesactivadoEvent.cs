using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Catalogos;

/// <summary>
/// Evento de dominio que se dispara cuando se desactiva un sector en el catálogo.
/// </summary>
public sealed class SectorDesactivadoEvent : DomainEvent
{
    public int SectorId { get; }
    public string Nombre { get; }

    public SectorDesactivadoEvent(int sectorId, string nombre)
    {
        SectorId = sectorId;
        Nombre = nombre;
    }
}
