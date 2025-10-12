using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Configuracion;

/// <summary>
/// Evento que se dispara cuando se crea una configuración de correo
/// </summary>
public sealed class ConfigCorreoCreadaEvent : DomainEvent
{
    public int ConfigId { get; }
    public string Email { get; }
    public string Servidor { get; }
    public int Puerto { get; }

    public ConfigCorreoCreadaEvent(int configId, string email, string servidor, int puerto)
    {
        ConfigId = configId;
        Email = email;
        Servidor = servidor;
        Puerto = puerto;
    }
}
