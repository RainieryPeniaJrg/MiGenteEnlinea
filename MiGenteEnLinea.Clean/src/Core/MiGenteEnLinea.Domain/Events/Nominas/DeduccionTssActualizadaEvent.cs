using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Nominas;

/// <summary>
/// Evento de dominio que se dispara cuando se actualiza una deducción TSS.
/// Útil para recalcular nóminas existentes o notificar cambios en la legislación.
/// </summary>
public sealed class DeduccionTssActualizadaEvent : DomainEvent
{
    /// <summary>
    /// Identificador de la deducción TSS actualizada.
    /// </summary>
    public int DeduccionId { get; }

    /// <summary>
    /// Descripción de la deducción.
    /// </summary>
    public string Descripcion { get; }

    /// <summary>
    /// Porcentaje anterior.
    /// </summary>
    public decimal PorcentajeAnterior { get; }

    /// <summary>
    /// Porcentaje nuevo.
    /// </summary>
    public decimal PorcentajeNuevo { get; }

    /// <summary>
    /// Fecha en que se actualizó la deducción.
    /// </summary>
    public DateTime FechaActualizacion { get; }

    public DeduccionTssActualizadaEvent(
        int deduccionId,
        string descripcion,
        decimal porcentajeAnterior,
        decimal porcentajeNuevo,
        DateTime fechaActualizacion)
    {
        DeduccionId = deduccionId;
        Descripcion = descripcion;
        PorcentajeAnterior = porcentajeAnterior;
        PorcentajeNuevo = porcentajeNuevo;
        FechaActualizacion = fechaActualizacion;
    }
}
