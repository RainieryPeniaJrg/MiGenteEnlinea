using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Catalogos;

/// <summary>
/// Evento de dominio que se dispara cuando se crea un nuevo sector económico en el catálogo.
/// </summary>
public sealed class SectorCreadoEvent : DomainEvent
{
    public int SectorId { get; }
    public string Nombre { get; }
    public string? Codigo { get; }
    public string? Grupo { get; }

    public SectorCreadoEvent(int sectorId, string nombre, string? codigo, string? grupo)
    {
        SectorId = sectorId;
        Nombre = nombre;
        Codigo = codigo;
        Grupo = grupo;
    }
}
