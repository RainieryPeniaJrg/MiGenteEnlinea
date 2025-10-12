using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se actualizan los teléfonos de un perfil
/// </summary>
public sealed class TelefonosPerfilActualizadosEvent : DomainEvent
{
    public int PerfilId { get; }
    public string UserId { get; }
    public string? Telefono1 { get; }
    public string? Telefono2 { get; }

    public TelefonosPerfilActualizadosEvent(
        int perfilId,
        string userId,
        string? telefono1,
        string? telefono2)
    {
        PerfilId = perfilId;
        UserId = userId;
        Telefono1 = telefono1;
        Telefono2 = telefono2;
    }
}
