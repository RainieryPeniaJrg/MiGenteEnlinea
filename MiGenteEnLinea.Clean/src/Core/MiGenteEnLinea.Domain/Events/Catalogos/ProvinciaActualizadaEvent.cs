using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Catalogos;

/// <summary>
/// Evento que se dispara cuando se actualiza el nombre de una provincia
/// </summary>
public sealed class ProvinciaActualizadaEvent : DomainEvent
{
    public int ProvinciaId { get; }
    public string NombreAnterior { get; }
    public string NombreNuevo { get; }

    public ProvinciaActualizadaEvent(int provinciaId, string nombreAnterior, string nombreNuevo)
    {
        ProvinciaId = provinciaId;
        NombreAnterior = nombreAnterior;
        NombreNuevo = nombreNuevo;
    }
}
