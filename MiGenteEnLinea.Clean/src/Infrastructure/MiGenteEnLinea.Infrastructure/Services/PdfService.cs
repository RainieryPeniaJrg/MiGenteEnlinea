using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Infrastructure.Services;

/// <summary>
/// Implementación del servicio de generación de PDFs
/// Usa iText 8.0 para conversión HTML a PDF
/// 
/// LÓGICA COPIADA DE: Legacy Utilitario.ConvertHtmlToPdf()
/// </summary>
public class PdfService : IPdfService
{
    private readonly ILogger<PdfService> _logger;
    private readonly INumeroEnLetrasService _numeroEnLetrasService;

    public PdfService(
        ILogger<PdfService> logger,
        INumeroEnLetrasService numeroEnLetrasService)
    {
        _logger = logger;
        _numeroEnLetrasService = numeroEnLetrasService;
    }

    /// <summary>
    /// Convierte HTML a PDF (método básico)
    /// LÓGICA EXACTA DEL LEGACY: Utilitario.ConvertHtmlToPdf()
    /// </summary>
    public byte[] ConvertHtmlToPdf(string htmlContent)
    {
        if (string.IsNullOrWhiteSpace(htmlContent))
            throw new ArgumentNullException(nameof(htmlContent), "El contenido HTML no puede estar vacío");

        try
        {
            _logger.LogInformation("Convirtiendo HTML a PDF. Longitud HTML: {Length} caracteres", htmlContent.Length);

            // LÓGICA EXACTA DEL LEGACY:
            // using (MemoryStream pdfStream = new MemoryStream())
            // {
            //     HtmlConverter.ConvertToPdf(htmlContent, pdfStream);
            //     return pdfStream.ToArray();
            // }

            using var pdfStream = new MemoryStream();
            HtmlConverter.ConvertToPdf(htmlContent, pdfStream);
            
            var pdfBytes = pdfStream.ToArray();
            
            _logger.LogInformation("PDF generado exitosamente. Tamaño: {Size} bytes", pdfBytes.Length);
            
            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al convertir HTML a PDF");
            throw new InvalidOperationException("Error al generar el PDF", ex);
        }
    }

    /// <summary>
    /// Convierte HTML a PDF con opciones avanzadas
    /// MEJORA sobre Legacy: Permite configurar tamaño de página y márgenes
    /// </summary>
    public byte[] ConvertHtmlToPdf(
        string htmlContent, 
        string pageSize = "A4", 
        (float top, float right, float bottom, float left)? margins = null)
    {
        if (string.IsNullOrWhiteSpace(htmlContent))
            throw new ArgumentNullException(nameof(htmlContent), "El contenido HTML no puede estar vacío");

        try
        {
            _logger.LogInformation(
                "Convirtiendo HTML a PDF con opciones personalizadas. PageSize: {PageSize}, Margins: {Margins}", 
                pageSize, 
                margins);

            using var pdfStream = new MemoryStream();
            using var pdfWriter = new PdfWriter(pdfStream);
            using var pdfDocument = new PdfDocument(pdfWriter);

            // Configurar tamaño de página
            var pageSizeObj = pageSize.ToUpper() switch
            {
                "A4" => PageSize.A4,
                "LETTER" => PageSize.LETTER,
                "LEGAL" => PageSize.LEGAL,
                _ => PageSize.A4
            };
            pdfDocument.SetDefaultPageSize(pageSizeObj);

            // Configurar propiedades
            var converterProperties = new ConverterProperties();

            // Convertir HTML a PDF
            HtmlConverter.ConvertToPdf(htmlContent, pdfDocument, converterProperties);

            var pdfBytes = pdfStream.ToArray();
            
            _logger.LogInformation("PDF generado exitosamente con opciones. Tamaño: {Size} bytes", pdfBytes.Length);
            
            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al convertir HTML a PDF con opciones");
            throw new InvalidOperationException("Error al generar el PDF", ex);
        }
    }

    /// <summary>
    /// Genera PDF de contrato de trabajo
    /// Usa template HTML predefinido
    /// </summary>
    public byte[] GenerarContratoTrabajo(
        string empleadorNombre,
        string empleadorRnc,
        string empleadoNombre,
        string empleadoCedula,
        string puesto,
        decimal salario,
        DateTime fechaInicio)
    {
        _logger.LogInformation(
            "Generando contrato de trabajo. Empleador: {Empleador}, Empleado: {Empleado}, Puesto: {Puesto}",
            empleadorNombre,
            empleadoNombre,
            puesto);

        var htmlTemplate = GetContratoTrabajoTemplate(
            empleadorNombre,
            empleadorRnc,
            empleadoNombre,
            empleadoCedula,
            puesto,
            salario,
            fechaInicio);

        return ConvertHtmlToPdf(htmlTemplate);
    }

    /// <summary>
    /// Genera PDF de recibo de pago
    /// </summary>
    public byte[] GenerarReciboPago(
        int reciboId,
        string empleadorNombre,
        string empleadoNombre,
        string periodo,
        decimal salarioBruto,
        decimal deducciones,
        decimal salarioNeto)
    {
        _logger.LogInformation(
            "Generando recibo de pago. ReciboID: {ReciboId}, Empleado: {Empleado}, Período: {Periodo}",
            reciboId,
            empleadoNombre,
            periodo);

        var htmlTemplate = GetReciboPagoTemplate(
            reciboId,
            empleadorNombre,
            empleadoNombre,
            periodo,
            salarioBruto,
            deducciones,
            salarioNeto);

        return ConvertHtmlToPdf(htmlTemplate);
    }

    /// <summary>
    /// Genera PDF de autorización TSS
    /// </summary>
    public byte[] GenerarAutorizacionTSS(
        string empleadorNombre,
        string empleadorRnc,
        List<(string Nombre, string Cedula, decimal Salario)> empleados)
    {
        _logger.LogInformation(
            "Generando autorización TSS. Empleador: {Empleador}, Total empleados: {Count}",
            empleadorNombre,
            empleados.Count);

        var htmlTemplate = GetAutorizacionTSSTemplate(empleadorNombre, empleadorRnc, empleados);

        return ConvertHtmlToPdf(htmlTemplate);
    }

    #region Templates HTML

    /// <summary>
    /// Template HTML para contrato de trabajo
    /// Basado en: MiGente_Front/HtmlTemplates/ContratoPersonaFisica.html
    /// </summary>
    private string GetContratoTrabajoTemplate(
        string empleadorNombre,
        string empleadorRnc,
        string empleadoNombre,
        string empleadoCedula,
        string puesto,
        decimal salario,
        DateTime fechaInicio)
    {
        var salarioTexto = $"RD$ {salario:N2}";
        // ✅ GAP-020: Convertir salario a letras para documentos legales
        var salarioEnLetras = _numeroEnLetrasService.ConvertirALetras(salario, incluirMoneda: true);
        var fechaTexto = fechaInicio.ToString("dd/MM/yyyy");

        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Contrato de Trabajo</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 40px; line-height: 1.6; }}
        h1 {{ text-align: center; color: #333; }}
        .header {{ text-align: center; margin-bottom: 30px; }}
        .content {{ text-align: justify; margin-bottom: 20px; }}
        .firma {{ margin-top: 60px; }}
        .firma div {{ display: inline-block; width: 45%; text-align: center; }}
        .firma-linea {{ border-top: 1px solid #000; margin-top: 40px; padding-top: 5px; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>CONTRATO DE TRABAJO</h1>
    </div>
    
    <div class='content'>
        <p>
            Entre <strong>{empleadorNombre}</strong>, RNC: <strong>{empleadorRnc}</strong>, 
            en lo adelante denominado <strong>EL EMPLEADOR</strong>,
            y <strong>{empleadoNombre}</strong>, cédula <strong>{empleadoCedula}</strong>, 
            en lo adelante denominado <strong>EL EMPLEADO</strong>, 
            se celebra el presente contrato de trabajo bajo las siguientes cláusulas:
        </p>

        <p><strong>PRIMERA:</strong> 
            EL EMPLEADOR contrata los servicios de EL EMPLEADO para desempeñar el cargo de 
            <strong>{puesto}</strong>.
        </p>

        <p><strong>SEGUNDA:</strong> 
            EL EMPLEADO se obliga a prestar sus servicios personales con la eficiencia y 
            dedicación que requiera el puesto asignado, cumpliendo fielmente con las 
            disposiciones del Código de Trabajo de la República Dominicana.
        </p>

        <p><strong>TERCERA:</strong> 
            EL EMPLEADOR se obliga a pagar a EL EMPLEADO un salario mensual de 
            <strong>{salarioTexto}</strong> ({salarioEnLetras}), pagadero en la forma y periodicidad establecida 
            por la empresa.
        </p>

        <p><strong>CUARTA:</strong> 
            Este contrato entrará en vigor a partir del <strong>{fechaTexto}</strong> 
            y se regirá por las disposiciones del Código de Trabajo vigente en la 
            República Dominicana.
        </p>

        <p><strong>QUINTA:</strong> 
            Cualquiera de las partes podrá dar por terminado el presente contrato, 
            sin responsabilidad alguna, mediante aviso previo de acuerdo a lo establecido 
            en el Código de Trabajo.
        </p>

        <p>
            Hecho y firmado en Santo Domingo, Distrito Nacional, República Dominicana, 
            a los {fechaInicio.Day} días del mes de {fechaInicio:MMMM} del año {fechaInicio.Year}.
        </p>
    </div>

    <div class='firma'>
        <div>
            <div class='firma-linea'>
                EL EMPLEADOR<br>
                {empleadorNombre}<br>
                RNC: {empleadorRnc}
            </div>
        </div>
        <div>
            <div class='firma-linea'>
                EL EMPLEADO<br>
                {empleadoNombre}<br>
                Cédula: {empleadoCedula}
            </div>
        </div>
    </div>
</body>
</html>";
    }

    /// <summary>
    /// Template HTML para recibo de pago
    /// Basado en: MiGente_Front/Empleador/Impresion/recibo.aspx
    /// </summary>
    private string GetReciboPagoTemplate(
        int reciboId,
        string empleadorNombre,
        string empleadoNombre,
        string periodo,
        decimal salarioBruto,
        decimal deducciones,
        decimal salarioNeto)
    {
        // ✅ GAP-020: Convertir salario neto a letras para documentos legales
        var salarioNetoEnLetras = _numeroEnLetrasService.ConvertirALetras(salarioNeto, incluirMoneda: true);

        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Recibo de Pago #{reciboId}</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; }}
        .header {{ text-align: center; margin-bottom: 20px; }}
        .header h1 {{ color: #007bff; margin: 0; }}
        .header h2 {{ color: #666; margin: 5px 0; }}
        .info {{ margin-bottom: 20px; }}
        .info p {{ margin: 5px 0; }}
        table {{ width: 100%; border-collapse: collapse; margin-top: 20px; }}
        th, td {{ border: 1px solid #ddd; padding: 10px; text-align: left; }}
        th {{ background-color: #007bff; color: white; }}
        .total {{ background-color: #f8f9fa; font-weight: bold; }}
        .footer {{ margin-top: 40px; text-align: center; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>RECIBO DE PAGO</h1>
        <h2>#{reciboId}</h2>
        <p>{empleadorNombre}</p>
    </div>

    <div class='info'>
        <p><strong>Empleado:</strong> {empleadoNombre}</p>
        <p><strong>Período:</strong> {periodo}</p>
        <p><strong>Fecha de emisión:</strong> {DateTime.Now:dd/MM/yyyy}</p>
    </div>

    <table>
        <thead>
            <tr>
                <th>Concepto</th>
                <th style='text-align: right;'>Monto</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Salario Bruto</td>
                <td style='text-align: right;'>RD$ {salarioBruto:N2}</td>
            </tr>
            <tr>
                <td>Deducciones (TSS, AFP, SFS)</td>
                <td style='text-align: right;'>RD$ {deducciones:N2}</td>
            </tr>
            <tr class='total'>
                <td><strong>SALARIO NETO A PAGAR</strong></td>
                <td style='text-align: right;'><strong>RD$ {salarioNeto:N2}</strong></td>
            </tr>
        </tbody>
    </table>

    <div style='margin-top: 20px; padding: 10px; background-color: #f8f9fa; border: 1px solid #dee2e6;'>
        <p style='margin: 0; text-align: center;'>
            <strong>MONTO EN LETRAS:</strong> {salarioNetoEnLetras}
        </p>
    </div>

    <div class='footer'>
        <p>Este recibo fue generado electrónicamente por MiGente En Línea</p>
        <p>www.migenteenlinea.com</p>
    </div>
</body>
</html>";
    }

    /// <summary>
    /// Template HTML para autorización TSS
    /// </summary>
    private string GetAutorizacionTSSTemplate(
        string empleadorNombre,
        string empleadorRnc,
        List<(string Nombre, string Cedula, decimal Salario)> empleados)
    {
        var filas = string.Join("", empleados.Select((e, i) => $@"
            <tr>
                <td>{i + 1}</td>
                <td>{e.Nombre}</td>
                <td>{e.Cedula}</td>
                <td style='text-align: right;'>RD$ {e.Salario:N2}</td>
            </tr>"));

        var totalSalarios = empleados.Sum(e => e.Salario);

        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Autorización TSS</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; }}
        .header {{ text-align: center; margin-bottom: 30px; }}
        .header h1 {{ color: #007bff; }}
        .info {{ margin-bottom: 20px; }}
        table {{ width: 100%; border-collapse: collapse; }}
        th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
        th {{ background-color: #007bff; color: white; }}
        .total {{ background-color: #f8f9fa; font-weight: bold; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>AUTORIZACIÓN TESORERÍA DE LA SEGURIDAD SOCIAL (TSS)</h1>
    </div>

    <div class='info'>
        <p><strong>Empleador:</strong> {empleadorNombre}</p>
        <p><strong>RNC:</strong> {empleadorRnc}</p>
        <p><strong>Fecha:</strong> {DateTime.Now:dd/MM/yyyy}</p>
        <p><strong>Total empleados:</strong> {empleados.Count}</p>
    </div>

    <table>
        <thead>
            <tr>
                <th>#</th>
                <th>Nombre Completo</th>
                <th>Cédula</th>
                <th style='text-align: right;'>Salario</th>
            </tr>
        </thead>
        <tbody>
            {filas}
            <tr class='total'>
                <td colspan='3' style='text-align: right;'><strong>TOTAL</strong></td>
                <td style='text-align: right;'><strong>RD$ {totalSalarios:N2}</strong></td>
            </tr>
        </tbody>
    </table>

    <div style='margin-top: 60px; text-align: center;'>
        <div style='border-top: 1px solid #000; width: 300px; margin: 0 auto; padding-top: 10px;'>
            <p><strong>Firma del Empleador</strong></p>
            <p>{empleadorNombre}</p>
            <p>RNC: {empleadorRnc}</p>
        </div>
    </div>
</body>
</html>";
    }

    #endregion
}
