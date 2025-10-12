using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Empleadores;

/// <summary>
/// Evento de dominio: Se dispara cuando un empleador actualiza su foto de perfil
/// 
/// CASOS DE USO:
/// - Invalidar cache de imagen de perfil
/// - Generar thumbnails de diferentes tamaños
/// - Migrar imagen a Azure Blob Storage (futuro)
/// - Registrar en auditoría
/// </summary>
public sealed class FotoActualizadaEvent : DomainEvent
{
    /// <summary>
    /// ID del empleador que actualizó su foto
    /// </summary>
    public int EmpleadorId { get; }

    public FotoActualizadaEvent(int empleadorId)
    {
        EmpleadorId = empleadorId;
    }
}
