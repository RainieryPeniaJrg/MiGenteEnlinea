using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se actualiza la información del gerente o representante legal
/// </summary>
public sealed class InformacionGerenteActualizadaEvent : DomainEvent
{
    public int PerfilesInfoId { get; }
    public string UserId { get; }
    public string? CedulaGerente { get; }
    public string? NombreGerente { get; }
    public string? ApellidoGerente { get; }
    public string? DireccionGerente { get; }

    public InformacionGerenteActualizadaEvent(
        int perfilesInfoId,
        string userId,
        string? cedulaGerente,
        string? nombreGerente,
        string? apellidoGerente,
        string? direccionGerente)
    {
        PerfilesInfoId = perfilesInfoId;
        UserId = userId;
        CedulaGerente = cedulaGerente;
        NombreGerente = nombreGerente;
        ApellidoGerente = apellidoGerente;
        DireccionGerente = direccionGerente;
    }
}
