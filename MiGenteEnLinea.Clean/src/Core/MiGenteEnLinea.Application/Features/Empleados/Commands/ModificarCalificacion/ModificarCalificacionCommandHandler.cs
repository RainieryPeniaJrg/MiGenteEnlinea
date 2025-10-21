using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.ModificarCalificacion;

public class ModificarCalificacionCommandHandler : IRequestHandler<ModificarCalificacionCommand, bool>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<ModificarCalificacionCommandHandler> _logger;

    public ModificarCalificacionCommandHandler(
        ILegacyDataService legacyDataService,
        ILogger<ModificarCalificacionCommandHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<bool> Handle(ModificarCalificacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Modificando calificación {CalificacionId}",
            request.CalificacionId);

        var result = await _legacyDataService.ModificarCalificacionAsync(
            request,
            cancellationToken);

        if (result)
        {
            _logger.LogInformation(
                "Calificación {CalificacionId} modificada exitosamente",
                request.CalificacionId);
        }
        else
        {
            _logger.LogWarning(
                "No se encontró la calificación {CalificacionId} para modificar",
                request.CalificacionId);
        }

        return result;
    }
}
