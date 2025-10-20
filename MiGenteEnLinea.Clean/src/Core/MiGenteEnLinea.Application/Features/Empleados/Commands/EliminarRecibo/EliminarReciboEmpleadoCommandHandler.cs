using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.EliminarRecibo;

public class EliminarReciboEmpleadoCommandHandler : IRequestHandler<EliminarReciboEmpleadoCommand, bool>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<EliminarReciboEmpleadoCommandHandler> _logger;

    public EliminarReciboEmpleadoCommandHandler(
        ILegacyDataService legacyDataService,
        ILogger<EliminarReciboEmpleadoCommandHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<bool> Handle(EliminarReciboEmpleadoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Eliminando recibo de empleado: PagoId={PagoId}", request.PagoId);

        var result = await _legacyDataService.EliminarReciboEmpleadoAsync(
            request.PagoId,
            cancellationToken);

        _logger.LogInformation("Recibo de empleado eliminado (Header + Detalle): PagoId={PagoId}", request.PagoId);
        
        return result;
    }
}
