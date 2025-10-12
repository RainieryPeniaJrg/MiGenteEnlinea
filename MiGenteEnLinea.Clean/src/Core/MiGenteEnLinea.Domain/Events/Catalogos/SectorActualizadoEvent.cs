using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Catalogos;

/// <summary>
/// Evento de dominio que se dispara cuando se actualiza el nombre de un sector.
/// </summary>
public sealed class SectorActualizadoEvent : DomainEvent
{
    public int SectorId { get; }
    public string NombreAnterior { get; }
    public string NombreNuevo { get; }

    public SectorActualizadoEvent(int sectorId, string nombreAnterior, string nombreNuevo)
    {
        SectorId = sectorId;
        NombreAnterior = nombreAnterior;
        NombreNuevo = nombreNuevo;
    }
}
