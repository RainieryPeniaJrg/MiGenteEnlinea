namespace MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

/// <summary>
/// DTO para vista de calificaciones (equivalente a VCalificaciones del Legacy)
/// </summary>
public class CalificacionVistaDto
{
    public int CalificacionId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public int Puntuacion { get; set; }
    public string? Comentario { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string NombreCalificador { get; set; } = string.Empty;
    public string ApellidoCalificador { get; set; } = string.Empty;
}
