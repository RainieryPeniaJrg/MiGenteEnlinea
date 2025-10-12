using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se actualiza la identificación de un perfil
/// </summary>
public sealed class IdentificacionActualizadaEvent : DomainEvent
{
    public int PerfilesInfoId { get; }
    public string UserId { get; }
    public string IdentificacionAnterior { get; }
    public string IdentificacionNueva { get; }
    public int? TipoIdentificacion { get; }

    public IdentificacionActualizadaEvent(
        int perfilesInfoId,
        string userId,
        string identificacionAnterior,
        string identificacionNueva,
        int? tipoIdentificacion)
    {
        PerfilesInfoId = perfilesInfoId;
        UserId = userId;
        IdentificacionAnterior = identificacionAnterior;
        IdentificacionNueva = identificacionNueva;
        TipoIdentificacion = tipoIdentificacion;
    }
}
