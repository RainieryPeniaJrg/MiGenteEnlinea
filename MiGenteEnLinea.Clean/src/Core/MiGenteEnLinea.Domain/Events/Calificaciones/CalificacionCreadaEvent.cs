using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Calificaciones;

/// <summary>
/// Evento de dominio: Se creó una nueva calificación
/// </summary>
public sealed class CalificacionCreadaEvent : DomainEvent
{
    public int CalificacionId { get; }
    public string EmpleadorUserId { get; }
    public string ContratistaIdentificacion { get; }
    public string ContratistaNombre { get; }
    public int Puntualidad { get; }
    public int Cumplimiento { get; }
    public int Conocimientos { get; }
    public int Recomendacion { get; }
    public decimal PromedioGeneral { get; }

    public CalificacionCreadaEvent(
        int calificacionId,
        string empleadorUserId,
        string contratistaIdentificacion,
        string contratistaNombre,
        int puntualidad,
        int cumplimiento,
        int conocimientos,
        int recomendacion,
        decimal promedioGeneral)
    {
        CalificacionId = calificacionId;
        EmpleadorUserId = empleadorUserId;
        ContratistaIdentificacion = contratistaIdentificacion;
        ContratistaNombre = contratistaNombre;
        Puntualidad = puntualidad;
        Cumplimiento = cumplimiento;
        Conocimientos = conocimientos;
        Recomendacion = recomendacion;
        PromedioGeneral = promedioGeneral;
    }
}
