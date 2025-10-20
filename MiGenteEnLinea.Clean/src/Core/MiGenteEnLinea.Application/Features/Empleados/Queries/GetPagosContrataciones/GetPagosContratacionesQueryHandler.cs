using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetPagosContrataciones;

public class GetPagosContratacionesQueryHandler : IRequestHandler<GetPagosContratacionesQuery, List<PagoContratacionDto>>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<GetPagosContratacionesQueryHandler> _logger;

    public GetPagosContratacionesQueryHandler(
        ILegacyDataService legacyDataService,
        ILogger<GetPagosContratacionesQueryHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<List<PagoContratacionDto>> Handle(GetPagosContratacionesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting pagos contrataciones for ContratacionId: {ContratacionId}, DetalleId: {DetalleId}",
            request.ContratacionId,
            request.DetalleId);

        var result = await _legacyDataService.GetPagosContratacionesAsync(
            request.ContratacionId,
            request.DetalleId,
            cancellationToken);

        _logger.LogInformation(
            "Found {Count} pagos contrataciones for ContratacionId: {ContratacionId}, DetalleId: {DetalleId}",
            result.Count,
            request.ContratacionId,
            request.DetalleId);

        return result;
    }
}
