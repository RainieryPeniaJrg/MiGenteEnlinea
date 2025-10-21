using MediatR;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetVistaContratacionTemporal;

/// <summary>
/// Handler para GetVistaContratacionTemporalQuery
/// Obtiene vista completa de contratación temporal desde base de datos
/// </summary>
public class GetVistaContratacionTemporalQueryHandler : IRequestHandler<GetVistaContratacionTemporalQuery, VistaContratacionTemporalDto?>
{
    private readonly ILegacyDataService _legacyDataService;

    public GetVistaContratacionTemporalQueryHandler(ILegacyDataService legacyDataService)
    {
        _legacyDataService = legacyDataService;
    }

    public async Task<VistaContratacionTemporalDto?> Handle(
        GetVistaContratacionTemporalQuery request,
        CancellationToken cancellationToken)
    {
        return await _legacyDataService.GetVistaContratacionTemporalAsync(
            request.ContratacionId,
            request.UserId,
            cancellationToken);
    }
}
