using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Nominas.Commands.GenerarRecibosPdfLote;

/// <summary>
/// Handler para generar PDFs de recibos en lote.
/// 
/// LÓGICA DE NEGOCIO:
/// 1. Obtiene todos los recibos por sus IDs
/// 2. Para cada recibo, genera PDF usando IPdfService
/// 3. Recopila errores sin detener el proceso completo
/// 4. Retorna lista de PDFs generados (byte arrays)
/// </summary>
public class GenerarRecibosPdfLoteCommandHandler : IRequestHandler<GenerarRecibosPdfLoteCommand, GenerarRecibosPdfLoteResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPdfService _pdfService;
    private readonly ILogger<GenerarRecibosPdfLoteCommandHandler> _logger;

    public GenerarRecibosPdfLoteCommandHandler(
        IUnitOfWork unitOfWork,
        IPdfService pdfService,
        ILogger<GenerarRecibosPdfLoteCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _pdfService = pdfService;
        _logger = logger;
    }

    public async Task<GenerarRecibosPdfLoteResult> Handle(
        GenerarRecibosPdfLoteCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Generando PDFs en lote - Recibos: {Count}",
            request.ReciboIds.Count);

        var result = new GenerarRecibosPdfLoteResult
        {
            PdfsGenerados = new List<ReciboPdfDto>(),
            Errores = new List<string>()
        };

        int exitosos = 0;
        int fallidos = 0;

        foreach (var reciboId in request.ReciboIds)
        {
            try
            {
                // Obtener recibo con detalles y empleado
                var recibo = await _unitOfWork.RecibosHeader.GetWithDetallesAsync(reciboId, cancellationToken);
                if (recibo == null)
                {
                    result.Errores.Add($"Recibo {reciboId} no encontrado");
                    fallidos++;
                    continue;
                }

                // Obtener empleado
                var empleado = await _unitOfWork.Empleados.GetByIdAsync(recibo.EmpleadoId);
                if (empleado == null)
                {
                    result.Errores.Add($"Empleado {recibo.EmpleadoId} no encontrado para recibo {reciboId}");
                    fallidos++;
                    continue;
                }

                // Obtener empleador usando IEmpleadorRepository
                var empleador = await _unitOfWork.Empleadores.FirstOrDefaultAsync(
                    e => e.UserId == recibo.UserId,
                    cancellationToken);
                
                if (empleador == null)
                {
                    result.Errores.Add($"Empleador {recibo.UserId} no encontrado para recibo {reciboId}");
                    fallidos++;
                    continue;
                }

                // Generar PDF usando el servicio
                // NOTA: Empleador no tiene nombre/razón social en dominio actual, usando UserId
                var pdfBytes = _pdfService.GenerarReciboPago(
                    reciboId: recibo.PagoId,
                    empleadorNombre: $"Empleador #{empleador.Id}", // TODO: Agregar nombre empresa a Empleador entity
                    empleadoNombre: empleado.NombreCompleto,
                    periodo: recibo.ConceptoPago,
                    salarioBruto: recibo.TotalIngresos,
                    deducciones: recibo.TotalDeducciones,
                    salarioNeto: recibo.NetoPagar
                );

                // Agregar a resultado
                result.PdfsGenerados.Add(new ReciboPdfDto
                {
                    ReciboId = recibo.PagoId,
                    EmpleadoId = recibo.EmpleadoId,
                    EmpleadoNombre = empleado.NombreCompleto,
                    PdfBytes = pdfBytes,
                    Periodo = recibo.ConceptoPago,
                    FechaGeneracion = DateTime.UtcNow
                });

                exitosos++;

                _logger.LogInformation(
                    "PDF generado - Recibo: {ReciboId}, Empleado: {EmpleadoId}, Size: {Size} bytes",
                    recibo.PagoId,
                    recibo.EmpleadoId,
                    pdfBytes.Length);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error generando PDF para recibo {ReciboId}",
                    reciboId);

                result.Errores.Add($"Error generando PDF para recibo {reciboId}: {ex.Message}");
                fallidos++;
            }
        }

        result.PdfsExitosos = exitosos;
        result.PdfsFallidos = fallidos;

        _logger.LogInformation(
            "PDFs generados - Exitosos: {Exitosos}, Fallidos: {Fallidos}",
            exitosos,
            fallidos);

        return result;
    }
}
