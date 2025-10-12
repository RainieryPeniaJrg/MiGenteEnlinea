using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contratistas;

/// <summary>
/// Evento de dominio: Se dispara cuando un contratista actualiza su perfil
/// 
/// CASOS DE USO:
/// - Invalidar cache del perfil
/// - Notificar a empleadores que siguen al contratista
/// - Registrar en auditoría de cambios
/// - Actualizar índices de búsqueda
/// - Recalcular score de completitud del perfil
/// </summary>
public sealed class PerfilContratistaActualizadoEvent : DomainEvent
{
    /// <summary>
    /// ID del contratista que actualizó su perfil
    /// </summary>
    public int ContratistaId { get; }

    public PerfilContratistaActualizadoEvent(int contratistaId)
    {
        ContratistaId = contratistaId;
    }
}
