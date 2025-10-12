using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se revoca un permiso específico de un usuario
/// </summary>
public sealed class PermisoRevocadoEvent : DomainEvent
{
    public int PermisoId { get; }
    public string UserId { get; }
    public int PermisoRevocado { get; }
    public int AtributosActuales { get; }

    public PermisoRevocadoEvent(int permisoId, string userId, int permisoRevocado, int atributosActuales)
    {
        PermisoId = permisoId;
        UserId = userId;
        PermisoRevocado = permisoRevocado;
        AtributosActuales = atributosActuales;
    }
}
