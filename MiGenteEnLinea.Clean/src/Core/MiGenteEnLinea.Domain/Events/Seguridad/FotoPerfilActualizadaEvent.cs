using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se actualiza la foto de perfil
/// </summary>
public sealed class FotoPerfilActualizadaEvent : DomainEvent
{
    public int PerfilesInfoId { get; }
    public string UserId { get; }
    public bool TieneFoto { get; }

    public FotoPerfilActualizadaEvent(
        int perfilesInfoId,
        string userId,
        bool tieneFoto)
    {
        PerfilesInfoId = perfilesInfoId;
        UserId = userId;
        TieneFoto = tieneFoto;
    }
}
