using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.RemoveServicio;

/// <summary>
/// Handler: Elimina un servicio del perfil de un contratista
/// </summary>
public class RemoveServicioCommandHandler : IRequestHandler<RemoveServicioCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveServicioCommandHandler> _logger;

    public RemoveServicioCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<RemoveServicioCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(RemoveServicioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Removiendo servicio. ServicioId: {ServicioId}, ContratistaId: {ContratistaId}",
            request.ServicioId, request.ContratistaId);

        // 1. BUSCAR SERVICIO
        var servicio = await _unitOfWork.ContratistasServicios
            .GetByIdAsync(request.ServicioId, cancellationToken);

        if (servicio == null || servicio.ContratistaId != request.ContratistaId)
        {
            _logger.LogWarning(
                "Servicio no encontrado o no pertenece al contratista. ServicioId: {ServicioId}, ContratistaId: {ContratistaId}",
                request.ServicioId, request.ContratistaId);
            throw new InvalidOperationException(
                $"No existe el servicio {request.ServicioId} para el contratista {request.ContratistaId}");
        }

        // 2. REMOVER SERVICIO (Physical delete - igual que Legacy)
        _unitOfWork.ContratistasServicios.Remove(servicio);

        // 3. GUARDAR CAMBIOS
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Servicio removido exitosamente. ServicioId: {ServicioId}, ContratistaId: {ContratistaId}",
            request.ServicioId, request.ContratistaId);
    }
}
