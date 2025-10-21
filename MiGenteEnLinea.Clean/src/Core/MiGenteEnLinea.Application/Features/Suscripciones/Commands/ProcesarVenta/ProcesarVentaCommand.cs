using MediatR;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.ProcesarVenta;

/// <summary>
/// Comando para procesar una venta con pago (Cardnet).
/// </summary>
/// <remarks>
/// Migrado desde: PaymentService.Payment() + SuscripcionesService.guardarSuscripcion()
/// Flow completo:
/// 1. Validar plan existe y obtener precio
/// 2. Generar idempotency key (Cardnet)
/// 3. Procesar pago con Cardnet API
/// 4. Si ResponseCode == "00" (aprobado):
///    - Crear registro Venta con estado Aprobado
///    - Crear o renovar Suscripción
///    - Retornar VentaId
/// 5. Si ResponseCode != "00" (rechazado):
///    - Crear registro Venta con estado Rechazado
///    - Lanzar PaymentRejectedException
/// </remarks>
public record ProcesarVentaCommand : IRequest<int>
{
    /// <summary>
    /// ID del usuario que realiza la compra.
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// ID del plan a suscribir.
    /// </summary>
    public int PlanId { get; init; }

    /// <summary>
    /// Número de tarjeta de crédito (encriptado en legacy, en clean manejado por Cardnet).
    /// </summary>
    public string CardNumber { get; init; } = string.Empty;

    /// <summary>
    /// CVV de la tarjeta (3-4 dígitos).
    /// </summary>
    public string Cvv { get; init; } = string.Empty;

    /// <summary>
    /// Fecha de expiración (formato: MMYY, ej: "1225" = Diciembre 2025).
    /// </summary>
    public string ExpirationDate { get; init; } = string.Empty;

    /// <summary>
    /// Dirección IP del cliente (para antifraude Cardnet).
    /// </summary>
    public string? ClientIp { get; init; }

    /// <summary>
    /// Número de referencia (generado por frontend o backend).
    /// </summary>
    public string? ReferenceNumber { get; init; }

    /// <summary>
    /// Número de factura/invoice (generado por frontend o backend).
    /// </summary>
    public string? InvoiceNumber { get; init; }
}
