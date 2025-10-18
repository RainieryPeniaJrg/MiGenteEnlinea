namespace MiGenteEnLinea.Infrastructure.Utilities;

/// <summary>
/// Convertidor de números a letras en español (República Dominicana)
/// 
/// LÓGICA COPIADA EXACTA DE: Legacy NumeroEnLetras.cs
/// 
/// CONTEXTO DE NEGOCIO:
/// - Usado en contratos de trabajo para escribir salarios en letras
/// - Usado en recibos de pago para montos legales
/// - Formato dominicano: "CINCO MIL PESOS DOMINICANOS 50/100"
/// 
/// EJEMPLOS:
/// - 5250.50m → "CINCO MIL DOSCIENTOS CINCUENTA PESOS DOMINICANOS 50/100"
/// - 1000.00m → "MIL PESOS DOMINICANOS 00/100"
/// - 15000.25m → "QUINCE MIL PESOS DOMINICANOS 25/100"
/// </summary>
public static class NumberToWordsConverter
{
    /// <summary>
    /// Convierte un número decimal a palabras (formato de dinero)
    /// LÓGICA EXACTA DEL LEGACY: NumeroEnLetras.NumerosALetras(decimal)
    /// </summary>
    /// <param name="number">Número a convertir</param>
    /// <returns>Texto en formato "XXXX PESOS DOMINICANOS XX/100"</returns>
    public static string ConvertirALetras(this decimal number)
    {
        var entero = Convert.ToInt64(Math.Truncate(number));
        var decimales = Convert.ToInt32(Math.Round((number - entero) * 100, 2));

        string dec = $" PESOS DOMINICANOS {decimales:00} /100";
        var res = ConvertirALetras(Convert.ToDouble(entero)) + dec;
        
        return res;
    }

    /// <summary>
    /// Convierte un número entero a palabras
    /// LÓGICA EXACTA DEL LEGACY: NumeroEnLetras.NumerosALetras(double)
    /// </summary>
    private static string ConvertirALetras(double value)
    {
        string num2Text;
        value = Math.Truncate(value);

        if (value == 0) num2Text = "CERO";
        else if (value == 1) num2Text = "UNO";
        else if (value == 2) num2Text = "DOS";
        else if (value == 3) num2Text = "TRES";
        else if (value == 4) num2Text = "CUATRO";
        else if (value == 5) num2Text = "CINCO";
        else if (value == 6) num2Text = "SEIS";
        else if (value == 7) num2Text = "SIETE";
        else if (value == 8) num2Text = "OCHO";
        else if (value == 9) num2Text = "NUEVE";
        else if (value == 10) num2Text = "DIEZ";
        else if (value == 11) num2Text = "ONCE";
        else if (value == 12) num2Text = "DOCE";
        else if (value == 13) num2Text = "TRECE";
        else if (value == 14) num2Text = "CATORCE";
        else if (value == 15) num2Text = "QUINCE";
        else if (value < 20) num2Text = "DIECI" + ConvertirALetras(value - 10);
        else if (value == 20) num2Text = "VEINTE";
        else if (value < 30) num2Text = "VEINTI" + (value % 10 == 1 ? "UN" : ConvertirALetras(value - 20));
        else if (value == 30) num2Text = "TREINTA";
        else if (value == 40) num2Text = "CUARENTA";
        else if (value == 50) num2Text = "CINCUENTA";
        else if (value == 60) num2Text = "SESENTA";
        else if (value == 70) num2Text = "SETENTA";
        else if (value == 80) num2Text = "OCHENTA";
        else if (value == 90) num2Text = "NOVENTA";
        else if (value < 100) num2Text = ConvertirALetras(Math.Truncate(value / 10) * 10) + " Y " + ConvertirALetras(value % 10);
        else if (value == 100) num2Text = "CIEN";
        else if (value < 200) num2Text = "CIENTO " + ConvertirALetras(value - 100);
        else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800))
            num2Text = ConvertirALetras(Math.Truncate(value / 100)) + "CIENTOS";
        else if (value == 500) num2Text = "QUINIENTOS";
        else if (value == 700) num2Text = "SETECIENTOS";
        else if (value == 900) num2Text = "NOVECIENTOS";
        else if (value < 1000) num2Text = ConvertirALetras(Math.Truncate(value / 100) * 100) + " " + ConvertirALetras(value % 100);
        else if (value == 1000) num2Text = "MIL";
        else if (value < 2000) num2Text = "MIL " + ConvertirALetras(value % 1000);
        else if (value < 1000000)
        {
            num2Text = ConvertirALetras(Math.Truncate(value / 1000)) + " MIL";
            if ((value % 1000) > 0)
            {
                num2Text = num2Text + " " + ConvertirALetras(value % 1000);
            }
        }
        else if (value == 1000000) num2Text = "UN MILLON";
        else if (value < 2000000) num2Text = "UN MILLON " + ConvertirALetras(value % 1000000);
        else if (value < 1000000000000)
        {
            num2Text = ConvertirALetras(Math.Truncate(value / 1000000)) + " MILLONES ";
            if ((value - Math.Truncate(value / 1000000) * 1000000) > 0)
            {
                num2Text = num2Text + " " + ConvertirALetras(value - Math.Truncate(value / 1000000) * 1000000);
            }
        }
        else if (value == 1000000000000) num2Text = "UN BILLON";
        else if (value < 2000000000000) num2Text = "UN BILLON " + ConvertirALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
        else
        {
            num2Text = ConvertirALetras(Math.Truncate(value / 1000000000000)) + " BILLONES";
            if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0)
            {
                num2Text = num2Text + " " + ConvertirALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
        }

        return num2Text;
    }
}
