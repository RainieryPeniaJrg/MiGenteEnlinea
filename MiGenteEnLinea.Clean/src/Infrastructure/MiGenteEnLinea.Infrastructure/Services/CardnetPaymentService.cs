using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;
using RestSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Infrastructure.Services;

/// <summary>
/// Implementación del servicio de pagos con Cardnet República Dominicana.
/// Integración con API de Cardnet para procesamiento de pagos con tarjetas de crédito/débito.
/// </summary>
public class CardnetPaymentService : IPaymentService
{
    private readonly ILogger<CardnetPaymentService> _logger;
    private readonly MiGenteDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    // Constantes de Cardnet (hardcoded del Legacy)
    private const string TOKEN_CARDNET = "454500350001"; // Token estático del Legacy
    private const string CURRENCY_DOP = "214"; // Código de Peso Dominicano
    private const string ENVIRONMENT = "ECommerce"; // Tipo de ambiente

    public CardnetPaymentService(
        ILogger<CardnetPaymentService> logger,
        MiGenteDbContext context,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Genera una idempotency key llamando al endpoint de Cardnet.
    /// Esta key previene transacciones duplicadas si la petición se envía múltiples veces.
    /// </summary>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Idempotency key en formato "ikey:XXXXXXXX"</returns>
    public async Task<string> GenerateIdempotencyKeyAsync(CancellationToken ct = default)
    {
        try
        {
            var config = await GetConfigurationAsync(ct);

            // Endpoint: POST /idempotency-keys (sin /transactions/ en la ruta)
            var idempotencyUrl = config.BaseUrl.Replace("/transactions/", "/idenpotency-keys");

            _logger.LogInformation(
                "Generando idempotency key. URL: {Url}, MerchantID: {MerchantId}",
                idempotencyUrl,
                config.MerchantId);

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            var request = new HttpRequestMessage(HttpMethod.Post, idempotencyUrl);
            request.Headers.Add("Accept", "text/plain");
            request.Content = new StringContent("", System.Text.Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request, ct);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(ct);
                _logger.LogError(
                    "Error generando idempotency key. Status: {Status}, Response: {Response}",
                    response.StatusCode,
                    errorContent);
                throw new InvalidOperationException($"Error al generar idempotency key: {response.StatusCode}");
            }

            var plainTextResponse = await response.Content.ReadAsStringAsync(ct);

            // Respuesta esperada: "ikey:a1b2c3d4-e5f6-7890-abcd-ef1234567890"
            if (string.IsNullOrWhiteSpace(plainTextResponse) || !plainTextResponse.StartsWith("ikey:"))
            {
                _logger.LogError("Formato de respuesta inválido: {Response}", plainTextResponse);
                throw new InvalidOperationException($"Formato de idempotency key inválido: {plainTextResponse}");
            }

            // Remover prefijo "ikey:" para obtener el GUID limpio
            var idempotencyKey = plainTextResponse.Substring("ikey:".Length);

            _logger.LogInformation("Idempotency key generada exitosamente: {Key}", idempotencyKey);

            return idempotencyKey;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción al generar idempotency key");
            throw;
        }
    }

    /// <summary>
    /// Procesa un pago con Cardnet usando RestSharp (según Legacy PaymentService.cs).
    /// 
    /// IMPORTANTE SEGURIDAD:
    /// - El card-number viene en PLAINTEXT desde el frontend (no encriptado)
    /// - Se envía a Cardnet tal cual (via HTTPS para seguridad)
    /// - NUNCA loggear el número de tarjeta completo ni el CVV
    /// </summary>
    /// <param name="request">Datos del pago</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Resultado del pago con approval-code y pnRef</returns>
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken ct = default)
    {
        try
        {
            var config = await GetConfigurationAsync(ct);

            // Generar idempotency key primero
            var idempotencyKey = await GenerateIdempotencyKeyAsync(ct);

            // Generar reference-number único (timestamp + random)
            var referenceNumber = $"{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";

            _logger.LogInformation(
                "Procesando pago. Monto: {Amount}, Referencia: {Reference}, Idempotency: {Idempotency}, Últimos 4: {Last4}",
                request.Amount,
                referenceNumber,
                idempotencyKey,
                MaskCardNumber(request.CardNumber));

            // URL del endpoint de sales
            var salesUrl = config.BaseUrl + "sales";

            // Crear cliente RestSharp (igual que Legacy)
            var restClient = new RestClient(salesUrl);
            var restRequest = new RestRequest("", Method.Post);
            restRequest.AddHeader("Content-Type", "application/json");

            // Construir JSON body (estructura exacta del Legacy)
            var jsonBody = new
            {
                amount = request.Amount,
                card_number = request.CardNumber, // ⚠️ PLAINTEXT - NO desencriptar (ya viene directo)
                client_ip = request.ClientIP,
                currency = CURRENCY_DOP, // "214" = Peso Dominicano
                cvv = request.CVV,
                environment = ENVIRONMENT, // "ECommerce"
                expiration_date = request.ExpirationDate, // Formato "MM/YY"
                idempotency_key = idempotencyKey,
                invoice_number = request.InvoiceNumber,
                merchant_id = config.MerchantId,
                reference_number = referenceNumber,
                terminal_id = config.TerminalId,
                token = TOKEN_CARDNET // "454500350001" del Legacy
            };

            restRequest.AddJsonBody(jsonBody);

            _logger.LogDebug(
                "Enviando pago a Cardnet. URL: {Url}, MerchantID: {MerchantId}, TerminalID: {TerminalId}",
                salesUrl,
                config.MerchantId,
                config.TerminalId);

            // Ejecutar request
            var response = await restClient.ExecuteAsync(restRequest, ct);

            if (!response.IsSuccessful)
            {
                var errorMessage = $"Error HTTP al procesar pago: {response.StatusCode}";
                if (!string.IsNullOrWhiteSpace(response.Content))
                {
                    errorMessage += $" - Respuesta: {response.Content}";
                }

                _logger.LogError(
                    "Error en pago. Status: {Status}, Referencia: {Reference}, Error: {Error}",
                    response.StatusCode,
                    referenceNumber,
                    response.ErrorMessage);

                return new PaymentResult
                {
                    Success = false,
                    ResponseCode = ((int)response.StatusCode).ToString(),
                    ResponseDescription = errorMessage,
                    IdempotencyKey = idempotencyKey
                };
            }

            // Parsear respuesta JSON de Cardnet
            var paymentResponse = JsonSerializer.Deserialize<CardnetPaymentResponse>(
                response.Content!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (paymentResponse == null)
            {
                _logger.LogError("No se pudo deserializar respuesta de Cardnet: {Content}", response.Content);
                throw new InvalidOperationException("Respuesta de Cardnet inválida");
            }

            // Mapear respuesta de Cardnet a PaymentResult
            var success = paymentResponse.ResponseCode == "00"; // "00" = Aprobado

            if (success)
            {
                _logger.LogInformation(
                    "Pago APROBADO. Referencia: {Reference}, ApprovalCode: {ApprovalCode}, PnRef: {PnRef}",
                    referenceNumber,
                    paymentResponse.ApprovalCode,
                    paymentResponse.PnRef);
            }
            else
            {
                _logger.LogWarning(
                    "Pago RECHAZADO. Referencia: {Reference}, C\u00f3digo: {Code}, Descripci\u00f3n: {Description}",
                    referenceNumber,
                    paymentResponse.ResponseCode,
                    paymentResponse.ResponseCodeDescription);
            }

            return new PaymentResult
            {
                Success = success,
                ResponseCode = paymentResponse.ResponseCode,
                ResponseDescription = paymentResponse.ResponseCodeDescription,
                ApprovalCode = paymentResponse.ApprovalCode,
                TransactionReference = paymentResponse.PnRef,
                IdempotencyKey = idempotencyKey
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción al procesar pago. Referencia: {Reference}", request.ReferenceNumber);
            throw;
        }
    }

    /// <summary>
    /// Obtiene la configuración de Cardnet desde la tabla PaymentGateway en la base de datos.
    /// </summary>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Configuración del gateway</returns>
    public async Task<PaymentGatewayConfig> GetConfigurationAsync(CancellationToken ct = default)
    {
        try
        {
            // Buscar configuración en la tabla PaymentGateway (debe existir 1 row)
            var gatewayConfig = await _context.PaymentGateways
                .FirstOrDefaultAsync(ct);

            if (gatewayConfig == null)
            {
                _logger.LogError("No se encontró configuración de PaymentGateway en la base de datos");
                throw new InvalidOperationException("Configuración de Cardnet no disponible. Verificar tabla PaymentGateway.");
            }

            // Usar UrlTest o UrlProduccion según el flag 'ModoTest'
            var baseUrl = gatewayConfig.ModoTest
                ? gatewayConfig.UrlTest
                : gatewayConfig.UrlProduccion;

            _logger.LogDebug(
                "Configuración Cardnet cargada. MerchantID: {MerchantId}, IsTest: {IsTest}",
                gatewayConfig.MerchantId,
                gatewayConfig.ModoTest);

            return new PaymentGatewayConfig
            {
                MerchantId = gatewayConfig.MerchantId,
                TerminalId = gatewayConfig.TerminalId,
                BaseUrl = baseUrl,
                IsTest = gatewayConfig.ModoTest
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración de PaymentGateway");
            throw;
        }
    }

    /// <summary>
    /// Enmascara el número de tarjeta para logging seguro (PCI-DSS compliance).
    /// Ejemplo: 4111111111111111 → ****-****-****-1111
    /// </summary>
    /// <param name="cardNumber">Número de tarjeta completo</param>
    /// <returns>Número enmascarado</returns>
    private static string MaskCardNumber(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 4)
        {
            return "****";
        }

        var last4 = cardNumber.Substring(cardNumber.Length - 4);
        return $"****-****-****-{last4}";
    }

    /// <summary>
    /// Modelo de respuesta de Cardnet (según Legacy PaymentService.cs).
    /// Nombres de propiedades en PascalCase para deserialización con PropertyNameCaseInsensitive.
    /// </summary>
    private sealed class CardnetPaymentResponse
    {
        public string IdempotencyKey { get; init; } = null!;
        public string MerchantId { get; init; } = null!;
        public string TerminalId { get; init; } = null!;
        
        /// <summary>
        /// Código de respuesta: "00" = Aprobado, "01" = Rechazado, etc.
        /// </summary>
        public string ResponseCode { get; init; } = null!;
        
        public string ResponseCodeDescription { get; init; } = null!;
        
        /// <summary>
        /// Código de aprobación (solo si fue aprobado)
        /// </summary>
        public string? ApprovalCode { get; init; }
        
        /// <summary>
        /// Referencia de transacción en Cardnet (pnRef)
        /// </summary>
        public string? PnRef { get; init; }
    }
}
