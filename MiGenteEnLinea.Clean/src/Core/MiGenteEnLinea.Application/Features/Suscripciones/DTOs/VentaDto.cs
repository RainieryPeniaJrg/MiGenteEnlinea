namespace MiGenteEnLinea.Application.Features.Suscripciones.DTOs;

/// <summary>
/// DTO para representar una venta/transacción en las respuestas de la API.
/// </summary>
/// <remarks>
/// Incluye propiedades computadas como MetodoPagoTexto y EstadoTexto
/// para facilitar la visualización en el frontend sin necesidad de mapeo adicional.
/// </remarks>
public record VentaDto
{
    /// <summary>
    /// ID único de la venta.
    /// </summary>
    public int VentaId { get; init; }

    /// <summary>
    /// ID del usuario que realizó la compra (Credencial.Id).
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Fecha y hora de la transacción.
    /// </summary>
    public DateTime FechaTransaccion { get; init; }

    /// <summary>
    /// Método de pago utilizado (código).
    /// 1=Tarjeta de Crédito, 4=Otro, 5=Sin Pago/Gratis.
    /// </summary>
    public int MetodoPago { get; init; }

    /// <summary>
    /// Descripción legible del método de pago.
    /// Propiedad computada: "Tarjeta de Crédito", "Otro", "Sin Pago".
    /// </summary>
    public string MetodoPagoTexto { get; init; } = string.Empty;

    /// <summary>
    /// ID del plan comprado.
    /// </summary>
    public int PlanId { get; init; }

    /// <summary>
    /// Monto total pagado en DOP (Pesos Dominicanos).
    /// </summary>
    public decimal Precio { get; init; }

    /// <summary>
    /// Estado de la transacción (código).
    /// 2=Aprobado, 3=Error, 4=Rechazado.
    /// </summary>
    public int Estado { get; init; }

    /// <summary>
    /// Descripción legible del estado de la transacción.
    /// Propiedad computada: "Aprobado", "Error", "Rechazado".
    /// </summary>
    public string EstadoTexto { get; init; } = string.Empty;

    /// <summary>
    /// ID de transacción generado por Cardnet (gateway de pago).
    /// Null si el pago no fue procesado por Cardnet.
    /// </summary>
    public string? IdTransaccion { get; init; }

    /// <summary>
    /// Últimos 4 dígitos de la tarjeta utilizada.
    /// Null si no se usó tarjeta o pago fue rechazado.
    /// </summary>
    public string? UltimosDigitosTarjeta { get; init; }

    /// <summary>
    /// Comentario adicional sobre la transacción.
    /// Puede incluir motivo de rechazo, detalles de error, etc.
    /// </summary>
    public string? Comentario { get; init; }

    /// <summary>
    /// Dirección IP desde donde se realizó el pago.
    /// Usado para auditoría y prevención de fraude.
    /// </summary>
    public string? DireccionIp { get; init; }
}
