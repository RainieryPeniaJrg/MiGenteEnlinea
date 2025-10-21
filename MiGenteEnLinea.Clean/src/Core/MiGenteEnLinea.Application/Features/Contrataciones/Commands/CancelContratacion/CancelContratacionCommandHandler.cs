using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Domain.Entities.Contrataciones;
using MiGenteEnLinea.Domain.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.CancelContratacion;

public class CancelContratacionCommandHandler : IRequestHandler<CancelContratacionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CancelContratacionCommandHandler> _logger;

    public CancelContratacionCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CancelContratacionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(CancelContratacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Canceling contratacion {DetalleId} with reason: {Motivo}",
            request.DetalleId,
            request.Motivo);

        var contratacion = await _unitOfWork.DetallesContrataciones
            .GetByIdAsync(request.DetalleId, cancellationToken);

        if (contratacion == null)
            throw new NotFoundException(nameof(DetalleContratacion), request.DetalleId);

        try
        {
            contratacion.Cancelar(request.Motivo);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Contratacion {DetalleId} canceled successfully. Reason: {Motivo}",
                contratacion.DetalleId,
                request.Motivo);

            return Unit.Value;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot cancel contratacion {DetalleId}", request.DetalleId);
            throw;
        }
    }
}
