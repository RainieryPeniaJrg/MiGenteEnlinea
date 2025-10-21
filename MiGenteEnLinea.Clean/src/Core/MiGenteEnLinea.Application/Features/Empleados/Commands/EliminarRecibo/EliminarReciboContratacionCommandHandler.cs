using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.EliminarRecibo;

public class EliminarReciboContratacionCommandHandler : IRequestHandler<EliminarReciboContratacionCommand, bool>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<EliminarReciboContratacionCommandHandler> _logger;

    public EliminarReciboContratacionCommandHandler(
        ILegacyDataService legacyDataService,
        ILogger<EliminarReciboContratacionCommandHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<bool> Handle(EliminarReciboContratacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Eliminando recibo de contratación: PagoId={PagoId}", request.PagoId);

        var result = await _legacyDataService.EliminarReciboContratacionAsync(
            request.PagoId,
            cancellationToken);

        _logger.LogInformation("Recibo de contratación eliminado (Header + Detalle): PagoId={PagoId}", request.PagoId);
        
        return result;
    }
}
