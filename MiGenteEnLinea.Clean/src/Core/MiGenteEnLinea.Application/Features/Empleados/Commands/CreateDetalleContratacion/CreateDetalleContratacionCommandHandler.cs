using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CreateDetalleContratacion;

public class CreateDetalleContratacionCommandHandler : IRequestHandler<CreateDetalleContratacionCommand, int>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<CreateDetalleContratacionCommandHandler> _logger;

    public CreateDetalleContratacionCommandHandler(
        ILegacyDataService legacyDataService,
        ILogger<CreateDetalleContratacionCommandHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<int> Handle(CreateDetalleContratacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating DetalleContratacion for ContratacionId: {ContratacionId}",
            request.ContratacionId);

        var detalleId = await _legacyDataService.CreateDetalleContratacionAsync(request, cancellationToken);

        _logger.LogInformation(
            "DetalleContratacion created successfully. DetalleId: {DetalleId}",
            detalleId);

        return detalleId;
    }
}
