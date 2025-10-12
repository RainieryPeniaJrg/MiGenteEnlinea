using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Empleadores;

/// <summary>
/// Evento de dominio: Se dispara cuando un empleador actualiza su perfil
/// 
/// CASOS DE USO:
/// - Invalidar cache del perfil
/// - Notificar a contratistas que siguen al empleador
/// - Registrar en auditoría de cambios
/// - Actualizar índices de búsqueda
/// </summary>
public sealed class PerfilActualizadoEvent : DomainEvent
{
    /// <summary>
    /// ID del empleador que actualizó su perfil
    /// </summary>
    public int EmpleadorId { get; }

    public PerfilActualizadoEvent(int empleadorId)
    {
        EmpleadorId = empleadorId;
    }
}
