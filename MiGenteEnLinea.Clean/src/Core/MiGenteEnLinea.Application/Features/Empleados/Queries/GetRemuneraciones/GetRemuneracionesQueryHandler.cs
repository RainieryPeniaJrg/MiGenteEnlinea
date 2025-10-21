using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetRemuneraciones;

public class GetRemuneracionesQueryHandler : IRequestHandler<GetRemuneracionesQuery, List<RemuneracionDto>>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<GetRemuneracionesQueryHandler> _logger;

    public GetRemuneracionesQueryHandler(
        ILegacyDataService legacyDataService,
        ILogger<GetRemuneracionesQueryHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene remuneraciones adicionales de un empleado
    /// Replica EmpleadosService.obtenerRemuneraciones()
    /// </summary>
    public async Task<List<RemuneracionDto>> Handle(GetRemuneracionesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Obteniendo remuneraciones - UserId: {UserId}, EmpleadoId: {EmpleadoId}",
            request.UserId,
            request.EmpleadoId);

        // Legacy: return db.Remuneraciones.Where(x => x.userID == userID && x.empleadoID == empleadoID).ToList();
        var remuneraciones = await _legacyDataService.GetRemuneracionesAsync(
            request.UserId,
            request.EmpleadoId,
            cancellationToken);

        _logger.LogInformation("Remuneraciones encontradas: {Count}", remuneraciones.Count);

        return remuneraciones;
    }
}
