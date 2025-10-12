using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se actualizan los permisos completos de un usuario
/// </summary>
public sealed class PermisosActualizadosEvent : DomainEvent
{
    public int PermisoId { get; }
    public string UserId { get; }
    public int AtributosAnteriores { get; }
    public int AtributosNuevos { get; }

    public PermisosActualizadosEvent(int permisoId, string userId, int atributosAnteriores, int atributosNuevos)
    {
        PermisoId = permisoId;
        UserId = userId;
        AtributosAnteriores = atributosAnteriores;
        AtributosNuevos = atributosNuevos;
    }
}
