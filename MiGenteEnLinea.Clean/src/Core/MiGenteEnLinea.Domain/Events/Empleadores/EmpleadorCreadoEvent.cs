using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Empleadores;

/// <summary>
/// Evento de dominio: Se dispara cuando se crea un nuevo perfil de empleador
/// 
/// CASOS DE USO:
/// - Enviar email de bienvenida al empleador
/// - Registrar en analytics/auditoría
/// - Notificar a administradores de nuevo empleador
/// - Inicializar configuraciones por defecto del empleador
/// </summary>
public sealed class EmpleadorCreadoEvent : DomainEvent
{
    /// <summary>
    /// ID del empleador creado
    /// </summary>
    public int EmpleadorId { get; }

    /// <summary>
    /// ID del usuario asociado (FK a Credencial)
    /// </summary>
    public string UserId { get; }

    public EmpleadorCreadoEvent(int empleadorId, string userId)
    {
        EmpleadorId = empleadorId;
        UserId = userId;
    }
}
