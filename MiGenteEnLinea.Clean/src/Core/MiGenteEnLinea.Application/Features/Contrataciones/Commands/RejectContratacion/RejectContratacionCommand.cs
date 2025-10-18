using MediatR;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.RejectContratacion;

/// <summary>
/// Command para que un contratista rechace una propuesta de contratación.
/// 
/// CONTEXTO DE NEGOCIO:
/// - El contratista recibe propuesta pero no puede aceptarla (no disponible, términos no convenientes, etc.)
/// - Debe proporcionar un motivo del rechazo
/// - Estado cambia de: Pendiente → Rechazada
/// - El empleador recibe notificación con el motivo
/// 
/// FLUJO:
/// 1. Contratista accede a propuesta
/// 2. Click en "Rechazar Propuesta"
/// 3. Sistema solicita motivo del rechazo
/// 4. Sistema valida que esté en estado Pendiente
/// 5. Sistema cambia estado a Rechazada y guarda motivo
/// 6. Domain Event ContratacionRechazadaEvent se dispara
/// 7. Sistema notifica al empleador con motivo
/// </summary>
public record RejectContratacionCommand : IRequest<Unit>
{
    /// <summary>
    /// ID del detalle de contratación a rechazar
    /// </summary>
    public int DetalleId { get; init; }

    /// <summary>
    /// Motivo del rechazo (requerido).
    /// Ejemplos: "No estoy disponible en esas fechas", 
    /// "El monto no es suficiente para el trabajo solicitado",
    /// "No cuento con las herramientas necesarias"
    /// </summary>
    public string Motivo { get; init; } = string.Empty;
}
