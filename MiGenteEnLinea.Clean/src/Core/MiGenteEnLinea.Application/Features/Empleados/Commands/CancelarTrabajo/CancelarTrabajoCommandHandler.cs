using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CancelarTrabajo;

public class CancelarTrabajoCommandHandler : IRequestHandler<CancelarTrabajoCommand, bool>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<CancelarTrabajoCommandHandler> _logger;

    public CancelarTrabajoCommandHandler(
        ILegacyDataService legacyDataService,
        ILogger<CancelarTrabajoCommandHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<bool> Handle(CancelarTrabajoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Cancelando trabajo temporal: ContratacionId={ContratacionId}, DetalleId={DetalleId}",
            request.ContratacionId,
            request.DetalleId);

        var result = await _legacyDataService.CancelarTrabajoAsync(
            request.ContratacionId,
            request.DetalleId,
            cancellationToken);

        _logger.LogInformation(
            "Trabajo temporal cancelado (estatus=3): ContratacionId={ContratacionId}, DetalleId={DetalleId}",
            request.ContratacionId,
            request.DetalleId);
        
        return result;
    }
}
