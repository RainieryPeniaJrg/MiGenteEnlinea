using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se elimina la foto de perfil
/// </summary>
public sealed class FotoPerfilEliminadaEvent : DomainEvent
{
    public int PerfilesInfoId { get; }
    public string UserId { get; }

    public FotoPerfilEliminadaEvent(
        int perfilesInfoId,
        string userId)
    {
        PerfilesInfoId = perfilesInfoId;
        UserId = userId;
    }
}
