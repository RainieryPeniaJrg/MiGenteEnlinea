using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contratistas;

/// <summary>
/// Evento de dominio: Se dispara cuando un contratista actualiza su información de contacto
/// 
/// CASOS DE USO:
/// - Invalidar cache de contacto
/// - Notificar al contratista de cambio en datos sensibles
/// - Registrar en auditoría de seguridad
/// - Validar nuevos números de teléfono
/// - Enviar confirmación por WhatsApp si está habilitado
/// </summary>
public sealed class ContactoActualizadoEvent : DomainEvent
{
    /// <summary>
    /// ID del contratista que actualizó su contacto
    /// </summary>
    public int ContratistaId { get; }

    public ContactoActualizadoEvent(int contratistaId)
    {
        ContratistaId = contratistaId;
    }
}
