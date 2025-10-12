using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Nominas;

/// <summary>
/// Evento de dominio que se dispara cuando se desactiva una deducción TSS.
/// Útil para notificar que una deducción ya no debe aplicarse en nóminas futuras.
/// </summary>
public sealed class DeduccionTssDesactivadaEvent : DomainEvent
{
    /// <summary>
    /// Identificador de la deducción TSS desactivada.
    /// </summary>
    public int DeduccionId { get; }

    /// <summary>
    /// Descripción de la deducción desactivada.
    /// </summary>
    public string Descripcion { get; }

    /// <summary>
    /// Fecha en que se desactivó la deducción.
    /// </summary>
    public DateTime FechaDesactivacion { get; }

    public DeduccionTssDesactivadaEvent(
        int deduccionId,
        string descripcion,
        DateTime fechaDesactivacion)
    {
        DeduccionId = deduccionId;
        Descripcion = descripcion;
        FechaDesactivacion = fechaDesactivacion;
    }
}
