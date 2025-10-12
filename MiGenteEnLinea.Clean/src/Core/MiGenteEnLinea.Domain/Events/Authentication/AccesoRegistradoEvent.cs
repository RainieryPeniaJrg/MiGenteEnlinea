using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Authentication;

/// <summary>
/// Evento que se dispara cuando un usuario inicia sesión exitosamente
/// </summary>
public sealed class AccesoRegistradoEvent : DomainEvent
{
    public int CredencialId { get; }
    public string UserId { get; }
    public DateTime FechaAcceso { get; }
    public string? IpAddress { get; }

    public AccesoRegistradoEvent(int credencialId, string userId, DateTime fechaAcceso, string? ipAddress = null)
    {
        CredencialId = credencialId;
        UserId = userId;
        FechaAcceso = fechaAcceso;
        IpAddress = ipAddress;
    }
}
