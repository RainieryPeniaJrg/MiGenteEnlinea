using MediatR;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.CancelContratacion;

/// <summary>
/// Command para cancelar una contratación.
/// 
/// CONTEXTO:
/// - Puede cancelarse desde cualquier estado EXCEPTO Completada
/// - Debe proporcionar motivo de cancelación
/// - Puede cancelar: empleador o contratista
/// - Estado cambia a: Cancelada
/// </summary>
public record CancelContratacionCommand : IRequest<Unit>
{
    public int DetalleId { get; init; }
    
    /// <summary>
    /// Motivo de la cancelación (requerido).
    /// Ejemplos: "Cliente canceló el proyecto", 
    /// "No puedo continuar por razones personales",
    /// "Problema con materiales no disponibles"
    /// </summary>
    public string Motivo { get; init; } = string.Empty;
}
