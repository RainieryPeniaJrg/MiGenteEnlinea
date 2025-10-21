using MediatR;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetTodosLosTemporales;

/// <summary>
/// Handler para GetTodosLosTemporalesQuery
/// Obtiene todos los empleados temporales con transformación de nombres según tipo
/// </summary>
public class GetTodosLosTemporalesQueryHandler : IRequestHandler<GetTodosLosTemporalesQuery, List<EmpleadoTemporalDto>>
{
    private readonly ILegacyDataService _legacyDataService;

    public GetTodosLosTemporalesQueryHandler(ILegacyDataService legacyDataService)
    {
        _legacyDataService = legacyDataService;
    }

    public async Task<List<EmpleadoTemporalDto>> Handle(
        GetTodosLosTemporalesQuery request,
        CancellationToken cancellationToken)
    {
        return await _legacyDataService.GetTodosLosTemporalesAsync(
            request.UserId,
            cancellationToken);
    }
}
