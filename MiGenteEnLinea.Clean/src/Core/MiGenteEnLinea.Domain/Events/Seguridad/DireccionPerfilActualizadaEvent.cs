using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se actualiza la dirección de un perfil
/// </summary>
public sealed class DireccionPerfilActualizadaEvent : DomainEvent
{
    public int PerfilesInfoId { get; }
    public string UserId { get; }
    public string? Direccion { get; }

    public DireccionPerfilActualizadaEvent(
        int perfilesInfoId,
        string userId,
        string? direccion)
    {
        PerfilesInfoId = perfilesInfoId;
        UserId = userId;
        Direccion = direccion;
    }
}
