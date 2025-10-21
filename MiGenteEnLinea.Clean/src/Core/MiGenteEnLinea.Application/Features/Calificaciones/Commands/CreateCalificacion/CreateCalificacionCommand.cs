using MediatR;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Commands.CreateCalificacion;

/// <summary>
/// Command: Crear nueva calificación con 4 dimensiones de evaluación
/// Mapea a: CalificacionesService.calificarPerfil() + Domain.Calificacion.Create()
/// 
/// Las calificaciones son INMUTABLES (no se pueden editar ni eliminar).
/// Implementa validación de duplicados (1 calificación por empleador-contratista).
/// </summary>
public record CreateCalificacionCommand : IRequest<int>
{
    /// <summary>
    /// ID del empleador que califica
    /// </summary>
    public string EmpleadorUserId { get; init; } = string.Empty;

    /// <summary>
    /// Identificación del contratista calificado (RNC o Cédula)
    /// </summary>
    public string ContratistaIdentificacion { get; init; } = string.Empty;

    /// <summary>
    /// Nombre completo del contratista (para desnormalización)
    /// </summary>
    public string ContratistaNombre { get; init; } = string.Empty;

    /// <summary>
    /// Calificación de puntualidad (1-5 estrellas)
    /// </summary>
    public int Puntualidad { get; init; }

    /// <summary>
    /// Calificación de cumplimiento (1-5 estrellas)
    /// </summary>
    public int Cumplimiento { get; init; }

    /// <summary>
    /// Calificación de conocimientos (1-5 estrellas)
    /// </summary>
    public int Conocimientos { get; init; }

    /// <summary>
    /// Calificación de recomendación (1-5 estrellas)
    /// </summary>
    public int Recomendacion { get; init; }
}
