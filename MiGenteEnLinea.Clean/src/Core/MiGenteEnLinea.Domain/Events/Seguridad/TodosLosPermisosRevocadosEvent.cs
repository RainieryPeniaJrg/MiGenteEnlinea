using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se revocan todos los permisos de un usuario
/// </summary>
public sealed class TodosLosPermisosRevocadosEvent : DomainEvent
{
    public int PermisoId { get; }
    public string UserId { get; }
    public int AtributosAnteriores { get; }

    public TodosLosPermisosRevocadosEvent(int permisoId, string userId, int atributosAnteriores)
    {
        PermisoId = permisoId;
        UserId = userId;
        AtributosAnteriores = atributosAnteriores;
    }
}
