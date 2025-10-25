namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Servicio para convertir números a su representación en letras (español).
/// Utilizado para generar documentos legales con montos escritos.
/// </summary>
public interface INumeroEnLetrasService
{
    /// <summary>
    /// Convierte un número decimal a su representación en letras en español.
    /// </summary>
    /// <param name="numero">Número a convertir (puede incluir decimales)</param>
    /// <param name="incluirMoneda">Si true, incluye "PESOS DOMINICANOS" y formato de centavos</param>
    /// <returns>Representación del número en letras (mayúsculas)</returns>
    /// <example>
    /// ConvertirALetras(1234.56, true) → "MIL DOSCIENTOS TREINTA Y CUATRO PESOS DOMINICANOS 56 /100"
    /// ConvertirALetras(100, false) → "CIEN"
    /// </example>
    string ConvertirALetras(decimal numero, bool incluirMoneda = true);

    /// <summary>
    /// Convierte un número entero a su representación en letras (versión simplificada para números pequeños).
    /// </summary>
    /// <param name="numero">Número entero entre 0 y 10,000</param>
    /// <returns>Representación del número en letras (mayúsculas)</returns>
    /// <exception cref="ArgumentOutOfRangeException">Si el número está fuera del rango 0-10,000</exception>
    string ConvertirEnteroALetras(long numero);
}
