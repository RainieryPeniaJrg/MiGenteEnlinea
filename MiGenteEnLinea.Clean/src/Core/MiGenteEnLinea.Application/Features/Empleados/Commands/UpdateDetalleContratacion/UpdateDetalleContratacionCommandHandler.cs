using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.UpdateDetalleContratacion;

public class UpdateDetalleContratacionCommandHandler : IRequestHandler<UpdateDetalleContratacionCommand, bool>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<UpdateDetalleContratacionCommandHandler> _logger;

    public UpdateDetalleContratacionCommandHandler(
        ILegacyDataService legacyDataService,
        ILogger<UpdateDetalleContratacionCommandHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateDetalleContratacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Updating DetalleContratacion for ContratacionId: {ContratacionId}",
            request.ContratacionId);

        var result = await _legacyDataService.UpdateDetalleContratacionAsync(request, cancellationToken);

        _logger.LogInformation(
            "DetalleContratacion updated successfully for ContratacionId: {ContratacionId}",
            request.ContratacionId);

        return result;
    }
}
