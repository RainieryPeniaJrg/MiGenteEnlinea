using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contratistas;

/// <summary>
/// Evento de dominio que se dispara cuando un contratista cambia su foto de perfil principal.
/// </summary>
public sealed class ContratistaFotoPrincipalCambiadaEvent : DomainEvent
{
    public int ImagenId { get; }
    public int ContratistaId { get; }
    public string NuevaImagenUrl { get; }

    public ContratistaFotoPrincipalCambiadaEvent(int imagenId, int contratistaId, string nuevaImagenUrl)
    {
        ImagenId = imagenId;
        ContratistaId = contratistaId;
        NuevaImagenUrl = nuevaImagenUrl;
    }
}
