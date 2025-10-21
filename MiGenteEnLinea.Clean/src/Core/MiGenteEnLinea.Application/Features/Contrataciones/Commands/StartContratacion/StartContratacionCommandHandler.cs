using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Domain.Entities.Contrataciones;
using MiGenteEnLinea.Domain.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.StartContratacion;

public class StartContratacionCommandHandler : IRequestHandler<StartContratacionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<StartContratacionCommandHandler> _logger;

    public StartContratacionCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<StartContratacionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(StartContratacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting contratacion {DetalleId}", request.DetalleId);

        var contratacion = await _unitOfWork.DetallesContrataciones
            .GetByIdAsync(request.DetalleId, cancellationToken);

        if (contratacion == null)
            throw new NotFoundException(nameof(DetalleContratacion), request.DetalleId);

        try
        {
            contratacion.IniciarTrabajo();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Contratacion {DetalleId} started successfully at {FechaInicio}",
                contratacion.DetalleId,
                DateTime.Now);

            return Unit.Value;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot start contratacion {DetalleId}", request.DetalleId);
            throw;
        }
    }
}
