using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Contratistas;

/// <summary>
/// Evento de dominio: Se dispara cuando se crea un nuevo perfil de contratista
/// 
/// CASOS DE USO:
/// - Enviar email de bienvenida al contratista
/// - Registrar en analytics/auditoría
/// - Notificar a administradores de nuevo contratista
/// - Inicializar configuraciones por defecto
/// - Crear registro en sistema de calificaciones
/// </summary>
public sealed class ContratistaCreadoEvent : DomainEvent
{
    /// <summary>
    /// ID del contratista creado
    /// </summary>
    public int ContratistaId { get; }

    /// <summary>
    /// ID del usuario asociado (FK a Credencial)
    /// </summary>
    public string UserId { get; }

    public ContratistaCreadoEvent(int contratistaId, string userId)
    {
        ContratistaId = contratistaId;
        UserId = userId;
    }
}
