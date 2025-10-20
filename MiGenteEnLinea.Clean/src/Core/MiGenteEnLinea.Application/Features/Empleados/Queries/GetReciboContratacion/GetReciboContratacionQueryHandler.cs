using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetReciboContratacion;

/// <summary>
/// Handler para obtener un recibo de contratación con su detalle y empleado temporal.
/// Migrado desde: EmpleadosService.GetContratacion_ReciboByPagoID(int pagoID) - line 222
/// </summary>
public class GetReciboContratacionQueryHandler 
    : IRequestHandler<GetReciboContratacionQuery, ReciboContratacionDto?>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<GetReciboContratacionQueryHandler> _logger;

    public GetReciboContratacionQueryHandler(
        ILegacyDataService legacyDataService,
        ILogger<GetReciboContratacionQueryHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<ReciboContratacionDto?> Handle(
        GetReciboContratacionQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Obteniendo recibo de contratación: PagoId={PagoId}",
            request.PagoId);

        var recibo = await _legacyDataService.GetReciboContratacionAsync(
            request.PagoId,
            cancellationToken);

        if (recibo == null)
        {
            _logger.LogWarning(
                "Recibo de contratación no encontrado: PagoId={PagoId}",
                request.PagoId);
            return null;
        }

        _logger.LogInformation(
            "Recibo de contratación obtenido: PagoId={PagoId}, Detalles={DetalleCount}, Total={Total}",
            recibo.PagoId,
            recibo.Detalles.Count,
            recibo.Total);

        return recibo;
    }
}
