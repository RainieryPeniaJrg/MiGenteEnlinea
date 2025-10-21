using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Domain.Entities.Contrataciones;
using MiGenteEnLinea.Domain.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.RejectContratacion;

/// <summary>
/// Handler para rechazar una propuesta de contratación.
/// </summary>
public class RejectContratacionCommandHandler : IRequestHandler<RejectContratacionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RejectContratacionCommandHandler> _logger;

    public RejectContratacionCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<RejectContratacionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(RejectContratacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Rejecting contratacion {DetalleId} with reason: {Motivo}",
            request.DetalleId,
            request.Motivo);

        // Buscar contratación
        var contratacion = await _unitOfWork.DetallesContrataciones
            .GetByIdAsync(request.DetalleId, cancellationToken);

        if (contratacion == null)
        {
            _logger.LogWarning("Contratacion not found with ID: {DetalleId}", request.DetalleId);
            throw new NotFoundException(nameof(DetalleContratacion), request.DetalleId);
        }

        try
        {
            // Llamar método del Domain (valida estado)
            contratacion.Rechazar(request.Motivo);

            // Guardar cambios
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Contratacion {DetalleId} rejected successfully. Reason: {Motivo}",
                contratacion.DetalleId,
                request.Motivo);

            return Unit.Value;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "Cannot reject contratacion {DetalleId}. Current status: {Status}",
                request.DetalleId,
                contratacion.ObtenerNombreEstado());
            throw;
        }
    }
}
