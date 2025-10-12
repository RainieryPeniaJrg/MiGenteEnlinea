using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se crea información extendida de un perfil
/// </summary>
public sealed class PerfilesInfoCreadoEvent : DomainEvent
{
    public int PerfilesInfoId { get; }
    public string UserId { get; }
    public string Identificacion { get; }
    public int? TipoIdentificacion { get; }

    public PerfilesInfoCreadoEvent(
        int perfilesInfoId,
        string userId,
        string identificacion,
        int? tipoIdentificacion)
    {
        PerfilesInfoId = perfilesInfoId;
        UserId = userId;
        Identificacion = identificacion;
        TipoIdentificacion = tipoIdentificacion;
    }
}
