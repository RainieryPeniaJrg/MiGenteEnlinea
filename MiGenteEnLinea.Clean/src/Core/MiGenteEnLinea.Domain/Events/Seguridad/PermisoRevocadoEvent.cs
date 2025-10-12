using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se otorga un permiso específico a un usuario
/// </summary>
public sealed class PermisoOtorgadoEvent : DomainEvent
{
    public int PermisoId { get; }
    public string UserId { get; }
    public int PermisoOtorgado { get; }
    public int AtributosActuales { get; }

    public PermisoOtorgadoEvent(int permisoId, string userId, int permisoOtorgado, int atributosActuales)
    {
        PermisoId = permisoId;
        UserId = userId;
        PermisoOtorgado = permisoOtorgado;
        AtributosActuales = atributosActuales;
    }
}
