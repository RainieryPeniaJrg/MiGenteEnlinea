using MediatR;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetReciboHeaderByPagoId;

/// <summary>
/// Handler para GetReciboHeaderByPagoIdQuery
/// Obtiene recibo header con detalle y empleado por PagoID
/// </summary>
public class GetReciboHeaderByPagoIdQueryHandler : IRequestHandler<GetReciboHeaderByPagoIdQuery, ReciboHeaderCompletoDto?>
{
    private readonly ILegacyDataService _legacyDataService;

    public GetReciboHeaderByPagoIdQueryHandler(ILegacyDataService legacyDataService)
    {
        _legacyDataService = legacyDataService;
    }

    public async Task<ReciboHeaderCompletoDto?> Handle(
        GetReciboHeaderByPagoIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _legacyDataService.GetReciboHeaderByPagoIdAsync(
            request.PagoId,
            cancellationToken);
    }
}
