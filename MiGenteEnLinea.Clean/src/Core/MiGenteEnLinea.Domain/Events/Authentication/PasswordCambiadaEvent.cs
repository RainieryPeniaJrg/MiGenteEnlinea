using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Authentication;

/// <summary>
/// Evento que se dispara cuando se cambia la contraseña de una credencial
/// </summary>
public sealed class PasswordCambiadaEvent : DomainEvent
{
    public int CredencialId { get; }
    public string UserId { get; }
    public DateTime FechaCambio { get; }

    public PasswordCambiadaEvent(int credencialId, string userId, DateTime fechaCambio)
    {
        CredencialId = credencialId;
        UserId = userId;
        FechaCambio = fechaCambio;
    }
}
