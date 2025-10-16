namespace MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

/// <summary>
/// DTO para promedio de calificaciones con distribución
/// 
/// NOTA: El promedio se calcula sobre el PromedioGeneral de cada calificación,
/// que es el promedio de las 4 dimensiones (Puntualidad, Cumplimiento, Conocimientos, Recomendación).
/// La distribución por estrellas se redondea del promedio general.
/// </summary>
public class PromedioCalificacionDto
{
    /// <summary>
    /// Identificación de la persona calificada
    /// </summary>
    public string Identificacion { get; set; } = string.Empty;

    /// <summary>
    /// Promedio general de calificaciones (1-5)
    /// </summary>
    public decimal PromedioGeneral { get; set; }

    /// <summary>
    /// Total de calificaciones recibidas
    /// </summary>
    public int TotalCalificaciones { get; set; }

    /// <summary>
    /// Cantidad de calificaciones de 5 estrellas
    /// </summary>
    public int Calificaciones5Estrellas { get; set; }

    /// <summary>
    /// Cantidad de calificaciones de 4 estrellas
    /// </summary>
    public int Calificaciones4Estrellas { get; set; }

    /// <summary>
    /// Cantidad de calificaciones de 3 estrellas
    /// </summary>
    public int Calificaciones3Estrellas { get; set; }

    /// <summary>
    /// Cantidad de calificaciones de 2 estrellas
    /// </summary>
    public int Calificaciones2Estrellas { get; set; }

    /// <summary>
    /// Cantidad de calificaciones de 1 estrella
    /// </summary>
    public int Calificaciones1Estrella { get; set; }

    /// <summary>
    /// Porcentaje de calificaciones positivas (4-5 estrellas)
    /// </summary>
    public decimal PorcentajePositivas
    {
        get
        {
            if (TotalCalificaciones == 0) return 0;
            return Math.Round(
                ((decimal)(Calificaciones5Estrellas + Calificaciones4Estrellas) / TotalCalificaciones) * 100,
                2
            );
        }
    }

    /// <summary>
    /// Porcentaje de calificaciones negativas (1-2 estrellas)
    /// </summary>
    public decimal PorcentajeNegativas
    {
        get
        {
            if (TotalCalificaciones == 0) return 0;
            return Math.Round(
                ((decimal)(Calificaciones1Estrella + Calificaciones2Estrellas) / TotalCalificaciones) * 100,
                2
            );
        }
    }
}
