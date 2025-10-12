using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Catalogos;

/// <summary>
/// Evento que se dispara cuando se crea una nueva provincia
/// </summary>
public sealed class ProvinciaCreadaEvent : DomainEvent
{
    public int ProvinciaId { get; }
    public string Nombre { get; }

    public ProvinciaCreadaEvent(int provinciaId, string nombre)
    {
        ProvinciaId = provinciaId;
        Nombre = nombre;
    }
}
