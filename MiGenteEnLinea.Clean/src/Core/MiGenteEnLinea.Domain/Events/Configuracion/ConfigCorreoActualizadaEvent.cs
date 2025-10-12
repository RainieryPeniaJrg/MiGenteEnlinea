using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Configuracion;

/// <summary>
/// Evento que se dispara cuando se actualiza la configuración de correo
/// </summary>
public sealed class ConfigCorreoActualizadaEvent : DomainEvent
{
    public int ConfigId { get; }
    public string Campo { get; }
    public string ValorAnterior { get; }
    public string ValorNuevo { get; }

    public ConfigCorreoActualizadaEvent(int configId, string campo, string valorAnterior, string valorNuevo)
    {
        ConfigId = configId;
        Campo = campo;
        ValorAnterior = valorAnterior;
        ValorNuevo = valorNuevo;
    }
}
