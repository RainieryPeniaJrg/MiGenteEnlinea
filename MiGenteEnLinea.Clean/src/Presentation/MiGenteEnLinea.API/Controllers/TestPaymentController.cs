using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.API.Controllers;

/// <summary>
/// ⚠️ CONTROLADOR TEMPORAL PARA TESTING - REMOVER EN PRODUCCIÓN
/// Controller para probar CardnetPaymentService en Swagger UI.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TestPaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<TestPaymentController> _logger;

    public TestPaymentController(
        IPaymentService paymentService,
        ILogger<TestPaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// TEST 1: Genera una idempotency key desde Cardnet.
    /// </summary>
    /// <returns>Idempotency key generada.</returns>
    /// <response code="200">Key generada exitosamente.</response>
    /// <response code="500">Error al generar key.</response>
    [HttpPost("idempotency-key")]
    [ProducesResponseType(typeof(IdempotencyKeyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IdempotencyKeyResponse>> GenerateIdempotencyKey()
    {
        try
        {
            _logger.LogInformation("TEST: Generando idempotency key...");

            var idempotencyKey = await _paymentService.GenerateIdempotencyKeyAsync();

            _logger.LogInformation("TEST: Idempotency key generada: {Key}", idempotencyKey);

            return Ok(new IdempotencyKeyResponse
            {
                IdempotencyKey = idempotencyKey,
                Success = true,
                Message = "Idempotency key generada exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TEST: Error al generar idempotency key");
            return StatusCode(500, new
            {
                success = false,
                message = "Error al generar idempotency key",
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// TEST 2: Procesa un pago de prueba con Cardnet.
    /// </summary>
    /// <param name="request">Datos del pago de prueba.</param>
    /// <returns>Resultado del pago.</returns>
    /// <response code="200">Pago procesado (puede estar aprobado o rechazado).</response>
    /// <response code="500">Error al procesar pago.</response>
    /// <remarks>
    /// TARJETAS DE PRUEBA CARDNET:
    /// - Visa Aprobada: 4111111111111111, CVV: 123, Exp: 12/25
    /// - MasterCard Aprobada: 5500000000000004, CVV: 456, Exp: 06/26
    /// - Visa Rechazada: 4111111111111112, CVV: 789, Exp: 03/24
    /// 
    /// IMPORTANTE:
    /// - El cardNumber se envía en PLAINTEXT (sin encriptar)
    /// - El sistema envía via HTTPS a Cardnet
    /// - NO se almacena el número completo en DB (solo últimos 4 dígitos)
    /// </remarks>
    [HttpPost("process-payment")]
    [ProducesResponseType(typeof(PaymentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaymentResult>> ProcessPayment([FromBody] TestPaymentRequest request)
    {
        try
        {
            _logger.LogInformation(
                "TEST: Procesando pago de prueba. Monto: {Amount}, Últimos 4: {Last4}",
                request.Amount,
                MaskCardNumber(request.CardNumber));

            var paymentRequest = new PaymentRequest
            {
                Amount = request.Amount,
                CardNumber = request.CardNumber,
                CVV = request.CVV,
                ExpirationDate = request.ExpirationDate,
                ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                ReferenceNumber = $"TEST-{DateTime.UtcNow:yyyyMMddHHmmss}",
                InvoiceNumber = $"INV-TEST-{Guid.NewGuid().ToString()[..8]}"
            };

            var result = await _paymentService.ProcessPaymentAsync(paymentRequest);

            if (result.Success)
            {
                _logger.LogInformation(
                    "TEST: Pago APROBADO. ApprovalCode: {ApprovalCode}, PnRef: {PnRef}",
                    result.ApprovalCode,
                    result.TransactionReference);
            }
            else
            {
                _logger.LogWarning(
                    "TEST: Pago RECHAZADO. Código: {Code}, Descripción: {Description}",
                    result.ResponseCode,
                    result.ResponseDescription);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TEST: Error al procesar pago");
            return StatusCode(500, new
            {
                success = false,
                message = "Error al procesar pago",
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    /// <summary>
    /// TEST 3: Obtiene la configuración actual del gateway de pagos.
    /// </summary>
    /// <returns>Configuración de Cardnet.</returns>
    /// <response code="200">Configuración obtenida exitosamente.</response>
    /// <response code="500">Error al obtener configuración.</response>
    [HttpGet("gateway-config")]
    [ProducesResponseType(typeof(PaymentGatewayConfig), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaymentGatewayConfig>> GetGatewayConfig()
    {
        try
        {
            _logger.LogInformation("TEST: Obteniendo configuración del gateway...");

            var config = await _paymentService.GetConfigurationAsync();

            _logger.LogInformation(
                "TEST: Configuración obtenida. MerchantID: {MerchantId}, IsTest: {IsTest}",
                config.MerchantId,
                config.IsTest);

            return Ok(config);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TEST: Error al obtener configuración");
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener configuración del gateway",
                error = ex.Message
            });
        }
    }

    private static string MaskCardNumber(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 4)
        {
            return "****";
        }

        var last4 = cardNumber.Substring(cardNumber.Length - 4);
        return $"****-****-****-{last4}";
    }
}

/// <summary>
/// Request para testing de pagos.
/// </summary>
public class TestPaymentRequest
{
    /// <summary>
    /// Monto a cobrar (ej: 100.50).
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Número de tarjeta completo (ej: 4111111111111111).
    /// IMPORTANTE: Se envía en PLAINTEXT via HTTPS.
    /// </summary>
    public string CardNumber { get; set; } = null!;

    /// <summary>
    /// CVV de la tarjeta (3-4 dígitos).
    /// </summary>
    public string CVV { get; set; } = null!;

    /// <summary>
    /// Fecha de expiración en formato MM/YY (ej: 12/25).
    /// </summary>
    public string ExpirationDate { get; set; } = null!;
}

/// <summary>
/// Respuesta de generación de idempotency key.
/// </summary>
public class IdempotencyKeyResponse
{
    /// <summary>
    /// Idempotency key generada por Cardnet.
    /// </summary>
    public string IdempotencyKey { get; set; } = null!;

    /// <summary>
    /// Indica si la generación fue exitosa.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Mensaje descriptivo.
    /// </summary>
    public string Message { get; set; } = null!;
}
