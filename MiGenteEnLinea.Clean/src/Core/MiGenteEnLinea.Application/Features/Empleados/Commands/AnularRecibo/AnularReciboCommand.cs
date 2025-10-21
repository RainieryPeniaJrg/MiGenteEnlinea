using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.AnularRecibo;

/// <summary>
/// Command para anular un recibo de pago.
/// Mapea: EmpleadosService.eliminarReciboEmpleado()
/// 
/// ⚠️ MEJORA vs Legacy: Cambia de hard delete a soft delete (Estado=3 "Anulado")
/// Legacy: db.Empleador_Recibos_Header.Remove() + db.Empleador_Recibos_Detalle.RemoveRange()
/// Clean: header.Anular() - soft delete con metadata
/// </summary>
public record AnularReciboCommand : IRequest<Unit>
{
    /// <summary>
    /// GUID del empleador que anula el recibo
    /// </summary>
    public string UserId { get; init; } = null!;

    /// <summary>
    /// ID del recibo (PagoId) a anular
    /// </summary>
    public int PagoId { get; init; }

    /// <summary>
    /// Motivo de la anulación (opcional pero recomendado)
    /// </summary>
    public string? MotivoAnulacion { get; init; }
}
