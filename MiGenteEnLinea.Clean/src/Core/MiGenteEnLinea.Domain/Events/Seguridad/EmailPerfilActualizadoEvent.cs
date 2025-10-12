using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se actualiza el email de un perfil
/// </summary>
public sealed class EmailPerfilActualizadoEvent : DomainEvent
{
    public int PerfilId { get; }
    public string UserId { get; }
    public string EmailAnterior { get; }
    public string EmailNuevo { get; }

    public EmailPerfilActualizadoEvent(
        int perfilId,
        string userId,
        string emailAnterior,
        string emailNuevo)
    {
        PerfilId = perfilId;
        UserId = userId;
        EmailAnterior = emailAnterior;
        EmailNuevo = emailNuevo;
    }
}
