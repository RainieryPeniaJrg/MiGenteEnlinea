using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CalificarContratacion;

public class CalificarContratacionCommandHandler : IRequestHandler<CalificarContratacionCommand, bool>
{
    private readonly ILegacyDataService _legacyDataService;
    private readonly ILogger<CalificarContratacionCommandHandler> _logger;

    public CalificarContratacionCommandHandler(
        ILegacyDataService legacyDataService,
        ILogger<CalificarContratacionCommandHandler> logger)
    {
        _legacyDataService = legacyDataService;
        _logger = logger;
    }

    public async Task<bool> Handle(CalificarContratacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Calificando contratación {ContratacionId} con calificación {CalificacionId}",
            request.ContratacionId,
            request.CalificacionId);

        var result = await _legacyDataService.CalificarContratacionAsync(
            request.ContratacionId,
            request.CalificacionId,
            cancellationToken);

        if (result)
        {
            _logger.LogInformation(
                "Contratación {ContratacionId} calificada exitosamente",
                request.ContratacionId);
        }
        else
        {
            _logger.LogWarning(
                "No se encontró la contratación {ContratacionId} para calificar",
                request.ContratacionId);
        }

        return result;
    }
}
