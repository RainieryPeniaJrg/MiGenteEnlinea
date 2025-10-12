using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se actualiza el nombre de usuario de un perfil
/// </summary>
public sealed class UsuarioPerfilActualizadoEvent : DomainEvent
{
    public int PerfilId { get; }
    public string UserId { get; }
    public string? UsuarioAnterior { get; }
    public string? UsuarioNuevo { get; }

    public UsuarioPerfilActualizadoEvent(
        int perfilId,
        string userId,
        string? usuarioAnterior,
        string? usuarioNuevo)
    {
        PerfilId = perfilId;
        UserId = userId;
        UsuarioAnterior = usuarioAnterior;
        UsuarioNuevo = usuarioNuevo;
    }
}
