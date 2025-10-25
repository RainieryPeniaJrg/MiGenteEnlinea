using MediatR;

namespace MiGenteEnLinea.Application.Features.Utilitarios.Queries.ConvertirNumeroALetras;

/// <summary>
/// Query para convertir un número decimal a texto en español.
/// </summary>
/// <remarks>
/// Mapeo desde Legacy: NumeroEnLetras.NumerosALetras(decimal) extension method
/// Caso de uso: Generación de PDFs legales (contratos, recibos, nómina)
/// </remarks>
public sealed record ConvertirNumeroALetrasQuery : IRequest<string>
{
    /// <summary>
    /// Número a convertir (puede incluir decimales).
    /// </summary>
    public decimal Numero { get; init; }

    /// <summary>
    /// Si es true, agrega formato de moneda "PESOS DOMINICANOS XX/100".
    /// Si es false, solo retorna el número en texto.
    /// </summary>
    public bool IncluirMoneda { get; init; } = true;
}
