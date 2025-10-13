using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.RemoveServicio;

/// <summary>
/// Handler: Elimina un servicio del perfil de un contratista
/// </summary>
public class RemoveServicioCommandHandler : IRequestHandler<RemoveServicioCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<RemoveServicioCommandHandler> _logger;

    public RemoveServicioCommandHandler(
        IApplicationDbContext context,
        ILogger<RemoveServicioCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(RemoveServicioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Removiendo servicio. ServicioId: {ServicioId}, ContratistaId: {ContratistaId}",
            request.ServicioId, request.ContratistaId);

        // 1. BUSCAR SERVICIO con validación de pertenencia
        var servicio = await _context.ContratistasServicios
            .Where(s => s.ServicioId == request.ServicioId && s.ContratistaId == request.ContratistaId)
            .FirstOrDefaultAsync(cancellationToken);

        if (servicio == null)
        {
            _logger.LogWarning(
                "Servicio no encontrado o no pertenece al contratista. ServicioId: {ServicioId}, ContratistaId: {ContratistaId}",
                request.ServicioId, request.ContratistaId);
            throw new InvalidOperationException(
                $"No existe el servicio {request.ServicioId} para el contratista {request.ContratistaId}");
        }

        // 2. REMOVER SERVICIO (Physical delete - igual que Legacy)
        _context.ContratistasServicios.Remove(servicio);

        // 3. GUARDAR CAMBIOS
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Servicio removido exitosamente. ServicioId: {ServicioId}, ContratistaId: {ContratistaId}",
            request.ServicioId, request.ContratistaId);
    }
}
