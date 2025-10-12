using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Catalogos;

/// <summary>
/// Evento de dominio que se dispara cuando se activa un sector en el catálogo.
/// </summary>
public sealed class SectorActivadoEvent : DomainEvent
{
    public int SectorId { get; }
    public string Nombre { get; }

    public SectorActivadoEvent(int sectorId, string nombre)
    {
        SectorId = sectorId;
        Nombre = nombre;
    }
}
