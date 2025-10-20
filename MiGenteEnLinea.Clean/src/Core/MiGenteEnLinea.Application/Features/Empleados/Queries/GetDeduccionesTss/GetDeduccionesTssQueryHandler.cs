using MediatR;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetDeduccionesTss;

/// <summary>
/// Handler para GetDeduccionesTssQuery.
/// Migrado de: EmpleadosService.deducciones() - Line 680
/// Retorna el catálogo completo de deducciones TSS sin filtros.
/// </summary>
public class GetDeduccionesTssQueryHandler : IRequestHandler<GetDeduccionesTssQuery, List<DeduccionTssDto>>
{
    private readonly ILegacyDataService _legacyDataService;

    public GetDeduccionesTssQueryHandler(ILegacyDataService legacyDataService)
    {
        _legacyDataService = legacyDataService;
    }

    public async Task<List<DeduccionTssDto>> Handle(GetDeduccionesTssQuery request, CancellationToken cancellationToken)
    {
        // Legacy: return db.Deducciones_TSS.ToList();
        return await _legacyDataService.GetDeduccionesTssAsync(cancellationToken);
    }
}
