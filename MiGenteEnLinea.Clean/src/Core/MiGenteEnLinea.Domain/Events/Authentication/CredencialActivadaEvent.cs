using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Authentication;

/// <summary>
/// Evento que se dispara cuando una credencial es activada
/// </summary>
public sealed class CredencialActivadaEvent : DomainEvent
{
    public int CredencialId { get; }
    public string UserId { get; }
    public string Email { get; }

    public CredencialActivadaEvent(int credencialId, string userId, string email)
    {
        CredencialId = credencialId;
        UserId = userId;
        Email = email;
    }
}
