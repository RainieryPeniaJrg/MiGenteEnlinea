using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetFichaTemporales;

public class GetFichaTemporalesQueryHandler : IRequestHandler<GetFichaTemporalesQuery, EmpleadoTemporalDto?>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<GetFichaTemporalesQueryHandler> _logger;

    public GetFichaTemporalesQueryHandler(
        ILegacyDataService legacyDataService,
        ILogger<GetFichaTemporalesQueryHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<EmpleadoTemporalDto?> Handle(GetFichaTemporalesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Obteniendo ficha temporal: ContratacionId={ContratacionId}, UserId={UserId}",
            request.ContratacionId,
            request.UserId);

        var result = await _legacyDataService.GetFichaTemporalesAsync(
            request.ContratacionId,
            request.UserId,
            cancellationToken);

        if (result == null)
        {
            _logger.LogWarning(
                "No se encontró ficha temporal con ContratacionId={ContratacionId} y UserId={UserId}",
                request.ContratacionId,
                request.UserId);
        }

        return result;
    }
}
