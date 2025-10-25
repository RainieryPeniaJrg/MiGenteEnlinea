using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Pagos.Queries.GenerateIdempotencyKey;

/// <summary>
/// Handler para generar claves de idempotencia de Cardnet.
/// </summary>
/// <remarks>
/// Mapeo desde Legacy: PaymentService.consultarIdempotency()
/// 
/// LÓGICA LEGACY (PaymentService.cs línea 17-49):
/// ```csharp
/// public async Task<dynamic> consultarIdempotency(string url)
/// {
///     var client = new RestClient(url);
///     var request = new RestRequest();
///     request.Method = Method.Get;
///     request.AddHeader("Content-Type", "text/plain");
///     var response = await client.ExecuteAsync(request);
///     
///     if (response.IsSuccessful)
///     {
///         // Response format: "ikey:a1b2c3d4-e5f6-7890-abcd-ef1234567890"
///         var result = response.Content.Substring(5); // Remove "ikey:"
///         return result;
///     }
///     throw new Exception(response.ErrorMessage);
/// }
/// ```
/// 
/// USO EN LEGACY:
/// - AdquirirPlanEmpleador.aspx.cs: Antes de procesar pago con tarjeta
/// - AdquirirPlanContratista.aspx.cs: Antes de procesar pago con tarjeta
/// - Se almacena en Ventas.idempotencyKey para auditoría
/// 
/// CARDNET API:
/// - URL: https://ecommerce.cardnet.com.do/api/payment/idenpotency-keys (configuración)
/// - Método: GET
/// - Headers: Content-Type: text/plain
/// - Response: "ikey:{GUID}"
/// 
/// BENEFICIO:
/// - Previene cobros duplicados si el usuario refresca la página
/// - Cardnet rechaza transacciones con mismo idempotency_key
/// - Importante para UX: Usuario puede reintentar sin miedo a doble cargo
/// </remarks>
public sealed class GenerateIdempotencyKeyQueryHandler 
    : IRequestHandler<GenerateIdempotencyKeyQuery, string>
{
    private readonly ICardnetPaymentService _cardnetPaymentService;
    private readonly ILogger<GenerateIdempotencyKeyQueryHandler> _logger;

    public GenerateIdempotencyKeyQueryHandler(
        ICardnetPaymentService cardnetPaymentService,
        ILogger<GenerateIdempotencyKeyQueryHandler> logger)
    {
        _cardnetPaymentService = cardnetPaymentService;
        _logger = logger;
    }

    public async Task<string> Handle(
        GenerateIdempotencyKeyQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("GAP-018: Generando idempotency key desde Cardnet API");

        try
        {
            // Delegar a ICardnetPaymentService (ya implementado)
            var idempotencyKey = await _cardnetPaymentService.GenerateIdempotencyKeyAsync(cancellationToken);

            _logger.LogInformation(
                "Idempotency key generada exitosamente: {Key}",
                idempotencyKey);

            return idempotencyKey;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error al generar idempotency key desde Cardnet");
            throw;
        }
    }
}
