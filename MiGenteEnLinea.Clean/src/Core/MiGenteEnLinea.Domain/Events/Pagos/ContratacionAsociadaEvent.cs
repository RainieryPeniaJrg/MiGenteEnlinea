using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Pagos;

/// <summary>
/// Evento que se dispara cuando se asocia un recibo a una contratación
/// </summary>
public sealed class ContratacionAsociadaEvent : DomainEvent
{
    public int PagoId { get; }
    public int? ContratacionAnterior { get; }
    public int ContratacionNueva { get; }

    public ContratacionAsociadaEvent(int pagoId, int? contratacionAnterior, int contratacionNueva)
    {
        PagoId = pagoId;
        ContratacionAnterior = contratacionAnterior;
        ContratacionNueva = contratacionNueva;
    }
}
