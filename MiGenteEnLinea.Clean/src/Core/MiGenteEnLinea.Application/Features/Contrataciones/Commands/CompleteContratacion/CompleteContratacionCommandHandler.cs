using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Domain.Entities.Contrataciones;
using MiGenteEnLinea.Domain.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.CompleteContratacion;

public class CompleteContratacionCommandHandler : IRequestHandler<CompleteContratacionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompleteContratacionCommandHandler> _logger;

    public CompleteContratacionCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CompleteContratacionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(CompleteContratacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Completing contratacion {DetalleId}", request.DetalleId);

        var contratacion = await _unitOfWork.DetallesContrataciones
            .GetByIdAsync(request.DetalleId, cancellationToken);

        if (contratacion == null)
            throw new NotFoundException(nameof(DetalleContratacion), request.DetalleId);

        try
        {
            contratacion.Completar();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Contratacion {DetalleId} completed successfully. Amount: {Monto}",
                contratacion.DetalleId,
                contratacion.MontoAcordado);

            return Unit.Value;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot complete contratacion {DetalleId}", request.DetalleId);
            throw;
        }
    }
}
