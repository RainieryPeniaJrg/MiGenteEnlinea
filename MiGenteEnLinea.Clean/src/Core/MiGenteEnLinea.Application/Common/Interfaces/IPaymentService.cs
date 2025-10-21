using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Common.Interfaces
{
    /// <summary>
    /// Interface para servicios de procesamiento de pagos
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Genera una idempotency key para evitar transacciones duplicadas.
        /// </summary>
        /// <param name="ct">Token de cancelación</param>
        /// <returns>Idempotency key generada por Cardnet</returns>
        Task<string> GenerateIdempotencyKeyAsync(CancellationToken ct = default);

        /// <summary>
        /// Procesa un pago con el gateway de pago (Cardnet)
        /// </summary>
        /// <param name="request">Datos del pago a procesar</param>
        /// <param name="ct">Token de cancelación</param>
        /// <returns>Resultado del procesamiento del pago</returns>
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken ct = default);

        /// <summary>
        /// Obtiene la configuración del gateway de pago desde la base de datos
        /// </summary>
        /// <param name="ct">Token de cancelación</param>
        /// <returns>Configuración del gateway</returns>
        Task<PaymentGatewayConfig> GetConfigurationAsync(CancellationToken ct = default);
    }

    /// <summary>
    /// Request para procesar un pago
    /// </summary>
    public record PaymentRequest
    {
        /// <summary>
        /// Monto a cobrar
        /// </summary>
        public decimal Amount { get; init; }

        /// <summary>
        /// Número de tarjeta (encriptado)
        /// </summary>
        public string CardNumber { get; init; } = null!;

        /// <summary>
        /// CVV de la tarjeta
        /// </summary>
        public string CVV { get; init; } = null!;

        /// <summary>
        /// Fecha de expiración (formato: MM/YY)
        /// </summary>
        public string ExpirationDate { get; init; } = null!;

        /// <summary>
        /// IP del cliente
        /// </summary>
        public string ClientIP { get; init; } = null!;

        /// <summary>
        /// Número de referencia único para la transacción
        /// </summary>
        public string ReferenceNumber { get; init; } = null!;

        /// <summary>
        /// Número de factura
        /// </summary>
        public string InvoiceNumber { get; init; } = null!;
    }

    /// <summary>
    /// Resultado del procesamiento de un pago
    /// </summary>
    public record PaymentResult
    {
        /// <summary>
        /// Indica si el pago fue exitoso
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Código de respuesta del gateway (ej: "00" = aprobado)
        /// </summary>
        public string ResponseCode { get; init; } = null!;

        /// <summary>
        /// Descripción del código de respuesta
        /// </summary>
        public string ResponseDescription { get; init; } = null!;

        /// <summary>
        /// Código de aprobación (si fue aprobado)
        /// </summary>
        public string? ApprovalCode { get; init; }

        /// <summary>
        /// Referencia de la transacción en el gateway
        /// </summary>
        public string? TransactionReference { get; init; }

        /// <summary>
        /// Clave de idempotencia utilizada
        /// </summary>
        public string IdempotencyKey { get; init; } = null!;
    }

    /// <summary>
    /// Configuración del gateway de pago
    /// </summary>
    public record PaymentGatewayConfig
    {
        /// <summary>
        /// ID del comerciante en Cardnet
        /// </summary>
        public string MerchantId { get; init; } = null!;

        /// <summary>
        /// ID de la terminal en Cardnet
        /// </summary>
        public string TerminalId { get; init; } = null!;

        /// <summary>
        /// URL base del API de Cardnet
        /// </summary>
        public string BaseUrl { get; init; } = null!;

        /// <summary>
        /// Indica si está en modo de prueba
        /// </summary>
        public bool IsTest { get; init; }
    }
}
