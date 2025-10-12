using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contratistas;

/// <summary>
/// Evento de dominio que se dispara cuando un contratista agrega una nueva foto a su portafolio.
/// </summary>
public sealed class ContratistaFotoAgregadaEvent : DomainEvent
{
    public int ImagenId { get; }
    public int ContratistaId { get; }
    public string ImagenUrl { get; }
    public string? TipoFoto { get; }
    public bool EsPrincipal { get; }

    public ContratistaFotoAgregadaEvent(
        int imagenId,
        int contratistaId,
        string imagenUrl,
        string? tipoFoto,
        bool esPrincipal)
    {
        ImagenId = imagenId;
        ContratistaId = contratistaId;
        ImagenUrl = imagenUrl;
        TipoFoto = tipoFoto;
        EsPrincipal = esPrincipal;
    }
}
