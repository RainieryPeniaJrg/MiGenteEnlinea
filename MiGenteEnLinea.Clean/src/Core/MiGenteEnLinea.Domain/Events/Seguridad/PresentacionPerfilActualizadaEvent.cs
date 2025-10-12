using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se actualiza la presentación de un perfil
/// </summary>
public sealed class PresentacionPerfilActualizadaEvent : DomainEvent
{
    public int PerfilesInfoId { get; }
    public string UserId { get; }
    public string? Presentacion { get; }

    public PresentacionPerfilActualizadaEvent(
        int perfilesInfoId,
        string userId,
        string? presentacion)
    {
        PerfilesInfoId = perfilesInfoId;
        UserId = userId;
        Presentacion = presentacion;
    }
}
