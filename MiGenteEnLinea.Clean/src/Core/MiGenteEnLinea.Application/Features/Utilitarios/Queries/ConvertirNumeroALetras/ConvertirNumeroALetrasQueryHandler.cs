using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Utilitarios.Queries.ConvertirNumeroALetras;

/// <summary>
/// Handler para convertir números a texto en español.
/// </summary>
/// <remarks>
/// Mapeo desde Legacy: NumeroEnLetras.NumerosALetras(decimal)
/// 
/// LÓGICA LEGACY:
/// ```csharp
/// public static string NumerosALetras(this decimal numberAsString)
/// {
///     var entero = Convert.ToInt64(Math.Truncate(numberAsString));
///     var decimales = Convert.ToInt32(Math.Round((numberAsString - entero) * 100, 2));
///     if (decimales > 0)
///     {
///         dec = $" PESOS DOMINICANOS {decimales:0,0} /100";
///     }
///     var res = NumerosALetras(Convert.ToDouble(entero)) + dec;
///     return res;
/// }
/// ```
/// 
/// USO EN LEGACY:
/// - ContratoPersonaFisica.html: Salario en texto
/// - ReciboEmpleado.aspx: Monto pagado en texto
/// - PDF generation: iText con montos legales
/// 
/// COMPATIBILIDAD:
/// - Usa INumeroEnLetrasService (port del Legacy)
/// - Mantiene formato EXACTO: "MIL PESOS DOMINICANOS 50/100"
/// - Decimales siempre con 2 dígitos: 5 → "05/100"
/// </remarks>
public sealed class ConvertirNumeroALetrasQueryHandler 
    : IRequestHandler<ConvertirNumeroALetrasQuery, string>
{
    private readonly INumeroEnLetrasService _numeroEnLetrasService;
    private readonly ILogger<ConvertirNumeroALetrasQueryHandler> _logger;

    public ConvertirNumeroALetrasQueryHandler(
        INumeroEnLetrasService numeroEnLetrasService,
        ILogger<ConvertirNumeroALetrasQueryHandler> logger)
    {
        _numeroEnLetrasService = numeroEnLetrasService;
        _logger = logger;
    }

    public Task<string> Handle(
        ConvertirNumeroALetrasQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Convirtiendo número {Numero} a letras (IncluirMoneda={IncluirMoneda})",
            request.Numero,
            request.IncluirMoneda);

        try
        {
            // Delegar a INumeroEnLetrasService (ya implementado)
            var texto = _numeroEnLetrasService.ConvertirALetras(
                request.Numero, 
                request.IncluirMoneda);

            _logger.LogDebug(
                "Conversión exitosa: {Numero} → {Texto}",
                request.Numero,
                texto);

            return Task.FromResult(texto);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error al convertir número {Numero} a letras",
                request.Numero);
            throw;
        }
    }
}
