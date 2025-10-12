using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se crean permisos para un usuario
/// </summary>
public sealed class PermisosCreadosEvent : DomainEvent
{
    public int PermisoId { get; }
    public string UserId { get; }
    public int Atributos { get; }

    public PermisosCreadosEvent(int permisoId, string userId, int atributos)
    {
        PermisoId = permisoId;
        UserId = userId;
        Atributos = atributos;
    }
}
