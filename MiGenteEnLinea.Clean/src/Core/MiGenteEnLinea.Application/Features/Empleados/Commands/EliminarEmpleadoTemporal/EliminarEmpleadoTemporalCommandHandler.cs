using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.EliminarEmpleadoTemporal;

/// <summary>
/// Handler para eliminar un empleado temporal con todos sus recibos asociados.
/// Migrado desde: EmpleadosService.eliminarEmpleadoTemporal(int contratacionID) - line 298
/// </summary>
public class EliminarEmpleadoTemporalCommandHandler 
    : IRequestHandler<EliminarEmpleadoTemporalCommand, bool>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<EliminarEmpleadoTemporalCommandHandler> _logger;

    public EliminarEmpleadoTemporalCommandHandler(
        ILegacyDataService legacyDataService,
        ILogger<EliminarEmpleadoTemporalCommandHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<bool> Handle(
        EliminarEmpleadoTemporalCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Eliminando empleado temporal y sus recibos: ContratacionId={ContratacionId}",
            request.ContratacionId);

        var result = await _legacyDataService.EliminarEmpleadoTemporalAsync(
            request.ContratacionId,
            cancellationToken);

        _logger.LogInformation(
            "Empleado temporal eliminado (recibos + empleado): ContratacionId={ContratacionId}",
            request.ContratacionId);

        return result;
    }
}
