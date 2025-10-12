using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contratistas;

/// <summary>
/// Evento de dominio: Se dispara cuando un contratista actualiza su imagen de perfil
/// 
/// CASOS DE USO:
/// - Invalidar cache de imagen de perfil
/// - Generar thumbnails de diferentes tamaños
/// - Optimizar imagen para web
/// - Registrar en auditoría
/// - Actualizar resultados de búsqueda con nueva imagen
/// </summary>
public sealed class ImagenActualizadaEvent : DomainEvent
{
    /// <summary>
    /// ID del contratista que actualizó su imagen
    /// </summary>
    public int ContratistaId { get; }

    public ImagenActualizadaEvent(int contratistaId)
    {
        ContratistaId = contratistaId;
    }
}
