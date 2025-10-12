using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se actualiza el nombre comercial de un perfil
/// </summary>
public sealed class NombreComercialActualizadoEvent : DomainEvent
{
    public int PerfilesInfoId { get; }
    public string UserId { get; }
    public string? NombreComercialAnterior { get; }
    public string? NombreComercialNuevo { get; }

    public NombreComercialActualizadoEvent(
        int perfilesInfoId,
        string userId,
        string? nombreComercialAnterior,
        string? nombreComercialNuevo)
    {
        PerfilesInfoId = perfilesInfoId;
        UserId = userId;
        NombreComercialAnterior = nombreComercialAnterior;
        NombreComercialNuevo = nombreComercialNuevo;
    }
}
