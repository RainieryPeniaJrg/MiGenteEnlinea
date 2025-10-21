using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.DarDeBajaEmpleado;

public class DarDeBajaEmpleadoCommandHandler : IRequestHandler<DarDeBajaEmpleadoCommand, bool>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<DarDeBajaEmpleadoCommandHandler> _logger;

    public DarDeBajaEmpleadoCommandHandler(
        ILegacyDataService legacyDataService,
        ILogger<DarDeBajaEmpleadoCommandHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<bool> Handle(DarDeBajaEmpleadoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Dando de baja empleado: {EmpleadoId}, Fecha: {FechaBaja}, Motivo: {Motivo}",
            request.EmpleadoId,
            request.FechaBaja,
            request.Motivo);

        var result = await _legacyDataService.DarDeBajaEmpleadoAsync(
            request.EmpleadoId,
            request.UserId,
            request.FechaBaja,
            request.Prestaciones,
            request.Motivo,
            cancellationToken);

        _logger.LogInformation("Empleado dado de baja exitosamente: {EmpleadoId}", request.EmpleadoId);
        
        return result;
    }
}
