using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Application.Features.Nominas.Commands.GenerarRecibosPdfLote;
using MiGenteEnLinea.Application.Features.Nominas.Commands.ProcesarNominaLote;
using MiGenteEnLinea.Application.Features.Nominas.DTOs;
using MiGenteEnLinea.Application.Features.Nominas.Queries.GetNominaResumen;

namespace MiGenteEnLinea.API.Controllers;

/// <summary>
/// Controller para gestión avanzada de nómina.
/// 
/// FUNCIONALIDADES:
/// - Procesamiento de nómina en lote (batch processing)
/// - Generación masiva de recibos en PDF
/// - Resúmenes y estadísticas de nómina por período
/// - Exportación de datos a Excel
/// 
/// WORKFLOW TÍPICO:
/// 1. Empleador procesa nómina del período (POST /api/nominas/procesar-lote)
/// 2. Sistema genera recibos para todos los empleados
/// 3. Empleador genera PDFs (POST /api/nominas/generar-pdfs)
/// 4. Empleador consulta resumen (GET /api/nominas/resumen)
/// 5. Opcional: Exportar a Excel o enviar por email
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NominasController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<NominasController> _logger;

    public NominasController(
        IMediator mediator,
        ILogger<NominasController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Procesa nómina para múltiples empleados en lote.
    /// </summary>
    /// <param name="command">Datos del lote de nómina</param>
    /// <returns>Resultado del procesamiento con contadores y errores</returns>
    /// <response code="200">Nómina procesada (puede tener errores parciales)</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="401">No autenticado</response>
    /// <response code="404">Empleador no encontrado</response>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     POST /api/nominas/procesar-lote
    ///     {
    ///       "empleadorId": 1,
    ///       "periodo": "2025-01",
    ///       "fechaPago": "2025-01-15",
    ///       "empleados": [
    ///         {
    ///           "empleadoId": 101,
    ///           "salario": 25000.00,
    ///           "conceptos": [
    ///             { "concepto": "Bono Productividad", "monto": 5000, "esDeduccion": false },
    ///             { "concepto": "Préstamo", "monto": 2000, "esDeduccion": true }
    ///           ]
    ///         }
    ///       ],
    ///       "notas": "Nómina quincenal enero 2025"
    ///     }
    /// 
    /// Respuesta exitosa incluye:
    /// - recibosCreados: Cantidad de recibos generados exitosamente
    /// - empleadosProcesados: Cantidad de empleados procesados
    /// - totalPagado: Monto total neto pagado
    /// - reciboIds: Lista de IDs de recibos generados
    /// - errores: Lista de errores si algunos empleados fallaron
    /// </remarks>
    [HttpPost("procesar-lote")]
    [ProducesResponseType(typeof(ProcesarNominaLoteResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProcesarNominaLoteResult>> ProcesarLote(
        [FromBody] ProcesarNominaLoteCommand command)
    {
        _logger.LogInformation(
            "Processing payroll batch - Employer: {EmpleadorId}, Period: {Periodo}, Employees: {Count}",
            command.EmpleadorId,
            command.Periodo,
            command.Empleados.Count);

        try
        {
            var result = await _mediator.Send(command);

            if (result.Errores.Count > 0)
            {
                _logger.LogWarning(
                    "Payroll batch completed with errors - Success: {Success}, Failed: {Failed}",
                    result.EmpleadosProcesados,
                    result.Errores.Count);
            }

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Employer not found");
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error processing payroll");
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Genera PDFs de recibos de nómina en lote.
    /// </summary>
    /// <param name="command">Lista de IDs de recibos a generar</param>
    /// <returns>PDFs generados con metadata</returns>
    /// <response code="200">PDFs generados (puede tener errores parciales)</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="401">No autenticado</response>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     POST /api/nominas/generar-pdfs
    ///     {
    ///       "reciboIds": [1001, 1002, 1003, 1004],
    ///       "incluirDetalleCompleto": true
    ///     }
    /// 
    /// Respuesta incluye:
    /// - pdfsExitosos/pdfsFallidos: Contadores
    /// - pdfsGenerados: Array de objetos con:
    ///   * reciboId
    ///   * empleadoId, empleadoNombre
    ///   * pdfBytes (base64 encoded PDF)
    ///   * periodo, fechaGeneracion
    ///   * tamanioBytes
    /// - errores: Lista de errores si algunos PDFs fallaron
    /// 
    /// NOTA: Los PDFs se retornan como byte arrays. El cliente debe:
    /// 1. Convertir base64 a bytes
    /// 2. Guardar como archivo .pdf
    /// 3. O mostrar en visor PDF del navegador
    /// </remarks>
    [HttpPost("generar-pdfs")]
    [ProducesResponseType(typeof(GenerarRecibosPdfLoteResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenerarRecibosPdfLoteResult>> GenerarPdfs(
        [FromBody] GenerarRecibosPdfLoteCommand command)
    {
        _logger.LogInformation(
            "Generating PDFs batch - Receipts: {Count}",
            command.ReciboIds.Count);

        try
        {
            var result = await _mediator.Send(command);

            if (result.Errores.Count > 0)
            {
                _logger.LogWarning(
                    "PDF generation completed with errors - Success: {Success}, Failed: {Failed}",
                    result.PdfsExitosos,
                    result.PdfsFallidos);
            }

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error generating PDFs");
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene resumen de nómina por período.
    /// </summary>
    /// <param name="empleadorId">ID del empleador</param>
    /// <param name="periodo">Período (ej: "2025-01")</param>
    /// <param name="fechaInicio">Fecha inicio del período (alternativa)</param>
    /// <param name="fechaFin">Fecha fin del período (alternativa)</param>
    /// <param name="incluirDetalleEmpleados">Incluir detalle por empleado</param>
    /// <returns>Resumen con totales, deducciones, estadísticas</returns>
    /// <response code="200">Resumen generado exitosamente</response>
    /// <response code="400">Parámetros inválidos</response>
    /// <response code="401">No autenticado</response>
    /// <response code="404">Empleador no encontrado</response>
    /// <remarks>
    /// Ejemplos de uso:
    /// 
    ///     GET /api/nominas/resumen?empleadorId=1&amp;periodo=2025-01
    ///     GET /api/nominas/resumen?empleadorId=1&amp;fechaInicio=2025-01-01&amp;fechaFin=2025-01-31
    ///     GET /api/nominas/resumen?empleadorId=1&amp;periodo=2025-Q1&amp;incluirDetalleEmpleados=true
    /// 
    /// Respuesta incluye:
    /// - totalEmpleados: Cantidad de empleados con pagos en el período
    /// - totalSalarioBruto/totalDeducciones/totalSalarioNeto: Sumas totales
    /// - deduccionesPorTipo: Dictionary con breakdown (AFP, SFS, ISR, etc.)
    /// - recibosGenerados/recibosAnulados: Contadores
    /// - promedioSalarioBruto/promedioSalarioNeto: Métricas
    /// - detalleEmpleados: Array opcional con detalle por empleado
    /// </remarks>
    [HttpGet("resumen")]
    [ProducesResponseType(typeof(NominaResumenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NominaResumenDto>> GetResumen(
        [FromQuery] int empleadorId,
        [FromQuery] string? periodo = null,
        [FromQuery] DateTime? fechaInicio = null,
        [FromQuery] DateTime? fechaFin = null,
        [FromQuery] bool incluirDetalleEmpleados = true)
    {
        _logger.LogInformation(
            "Getting payroll summary - Employer: {EmpleadorId}, Period: {Periodo}",
            empleadorId,
            periodo ?? $"{fechaInicio:yyyy-MM-dd} to {fechaFin:yyyy-MM-dd}");

        var query = new GetNominaResumenQuery
        {
            EmpleadorId = empleadorId,
            Periodo = periodo ?? string.Empty,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            IncluirDetalleEmpleados = incluirDetalleEmpleados
        };

        try
        {
            var resumen = await _mediator.Send(query);
            return Ok(resumen);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Employer not found");
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error getting summary");
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Descarga un PDF específico de recibo.
    /// </summary>
    /// <param name="reciboId">ID del recibo</param>
    /// <returns>Archivo PDF para descarga</returns>
    /// <response code="200">PDF generado exitosamente</response>
    /// <response code="404">Recibo no encontrado</response>
    /// <remarks>
    /// Este endpoint retorna el PDF directamente como archivo descargable.
    /// El navegador abrirá el visor PDF o descargará el archivo.
    /// 
    /// Ejemplo:
    ///     GET /api/nominas/recibo/1001/pdf
    /// 
    /// Response headers:
    /// - Content-Type: application/pdf
    /// - Content-Disposition: attachment; filename="recibo-1001.pdf"
    /// </remarks>
    [HttpGet("recibo/{reciboId}/pdf")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DescargarReciboPdf(int reciboId)
    {
        _logger.LogInformation("Downloading PDF for receipt: {ReciboId}", reciboId);

        var command = new GenerarRecibosPdfLoteCommand
        {
            ReciboIds = new List<int> { reciboId },
            IncluirDetalleCompleto = true
        };

        try
        {
            var result = await _mediator.Send(command);

            if (result.PdfsGenerados.Count == 0 || result.Errores.Count > 0)
            {
                return NotFound(new { error = $"No se pudo generar el PDF para el recibo {reciboId}" });
            }

            var pdf = result.PdfsGenerados.First();
            return File(pdf.PdfBytes, "application/pdf", $"recibo-{reciboId}.pdf");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Receipt not found");
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene estadísticas de salud del servicio de nómina.
    /// </summary>
    /// <returns>Información de estado y versión</returns>
    /// <response code="200">Estado del servicio</response>
    [HttpGet("health")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Health()
    {
        return Ok(new
        {
            service = "Nominas API",
            status = "healthy",
            version = "1.0.0",
            timestamp = DateTime.UtcNow,
            features = new[]
            {
                "Batch Payroll Processing",
                "PDF Generation",
                "Payroll Summary",
                "Statistics & Reports"
            }
        });
    }
}
