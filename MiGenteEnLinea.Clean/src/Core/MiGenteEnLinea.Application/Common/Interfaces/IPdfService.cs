namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Servicio para generación de documentos PDF
/// 
/// CONTEXTO DE NEGOCIO:
/// - Genera PDFs desde HTML (contratos, recibos, documentos legales)
/// - Usado para: Contratos de trabajo, Recibos de pago, Autorizaciones TSS
/// - Convierte templates HTML con datos dinámicos a PDFs descargables
/// 
/// MAPEO CON LEGACY:
/// - Legacy: Utilitario.ConvertHtmlToPdf(htmlContent) usando iText.Html2pdf
/// - Clean: PdfService con iText 8.0 (versión moderna)
/// </summary>
public interface IPdfService
{
    /// <summary>
    /// Convierte contenido HTML a PDF
    /// </summary>
    /// <param name="htmlContent">Contenido HTML completo (incluyendo estilos inline)</param>
    /// <returns>Byte array del PDF generado</returns>
    /// <exception cref="ArgumentNullException">Si htmlContent es null</exception>
    /// <exception cref="InvalidOperationException">Si la conversión falla</exception>
    byte[] ConvertHtmlToPdf(string htmlContent);

    /// <summary>
    /// Convierte contenido HTML a PDF con opciones avanzadas
    /// </summary>
    /// <param name="htmlContent">Contenido HTML completo</param>
    /// <param name="pageSize">Tamaño de página (A4, Letter, Legal)</param>
    /// <param name="margins">Márgenes en milímetros (top, right, bottom, left)</param>
    /// <returns>Byte array del PDF generado</returns>
    byte[] ConvertHtmlToPdf(string htmlContent, string pageSize = "A4", (float top, float right, float bottom, float left)? margins = null);

    /// <summary>
    /// Genera PDF de contrato de trabajo
    /// </summary>
    /// <param name="empleadorNombre">Nombre del empleador</param>
    /// <param name="empleadorRnc">RNC del empleador</param>
    /// <param name="empleadoNombre">Nombre del empleado</param>
    /// <param name="empleadoCedula">Cédula del empleado</param>
    /// <param name="puesto">Puesto del empleado</param>
    /// <param name="salario">Salario mensual</param>
    /// <param name="fechaInicio">Fecha de inicio del contrato</param>
    /// <returns>Byte array del PDF generado</returns>
    byte[] GenerarContratoTrabajo(
        string empleadorNombre,
        string empleadorRnc,
        string empleadoNombre,
        string empleadoCedula,
        string puesto,
        decimal salario,
        DateTime fechaInicio);

    /// <summary>
    /// Genera PDF de recibo de pago
    /// </summary>
    /// <param name="reciboId">ID del recibo</param>
    /// <param name="empleadorNombre">Nombre del empleador</param>
    /// <param name="empleadoNombre">Nombre del empleado</param>
    /// <param name="periodo">Período de pago</param>
    /// <param name="salarioBruto">Salario bruto</param>
    /// <param name="deducciones">Deducciones totales</param>
    /// <param name="salarioNeto">Salario neto a pagar</param>
    /// <returns>Byte array del PDF generado</returns>
    byte[] GenerarReciboPago(
        int reciboId,
        string empleadorNombre,
        string empleadoNombre,
        string periodo,
        decimal salarioBruto,
        decimal deducciones,
        decimal salarioNeto);

    /// <summary>
    /// Genera PDF de autorización TSS (Seguridad Social)
    /// </summary>
    /// <param name="empleadorNombre">Nombre del empleador</param>
    /// <param name="empleadorRnc">RNC del empleador</param>
    /// <param name="empleados">Lista de empleados a autorizar</param>
    /// <returns>Byte array del PDF generado</returns>
    byte[] GenerarAutorizacionTSS(
        string empleadorNombre,
        string empleadorRnc,
        List<(string Nombre, string Cedula, decimal Salario)> empleados);
}
