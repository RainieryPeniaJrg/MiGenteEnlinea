using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CreateEmpleadoTemporal;

public class CreateEmpleadoTemporalCommandHandler : IRequestHandler<CreateEmpleadoTemporalCommand, int>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<CreateEmpleadoTemporalCommandHandler> _logger;

    public CreateEmpleadoTemporalCommandHandler(
        ILegacyDataService legacyDataService,
        ILogger<CreateEmpleadoTemporalCommandHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<int> Handle(CreateEmpleadoTemporalCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating EmpleadoTemporal for UserId: {UserId}, Name: {Nombre} {Apellido}",
            request.UserId,
            request.Nombre,
            request.Apellido);

        var contratacionId = await _legacyDataService.CreateEmpleadoTemporalAsync(request, cancellationToken);

        _logger.LogInformation(
            "EmpleadoTemporal created successfully. ContratacionId: {ContratacionId}",
            contratacionId);

        return contratacionId;
    }
}
