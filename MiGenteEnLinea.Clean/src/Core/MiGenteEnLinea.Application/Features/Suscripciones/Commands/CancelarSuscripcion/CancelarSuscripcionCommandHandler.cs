using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.CancelarSuscripcion;

/// <summary>
/// Handler para cancelar una suscripción activa.
/// </summary>
/// <remarks>
/// Funcionalidad nueva (no existe en Legacy).
/// 
/// Flujo:
/// 1. Buscar suscripción activa del usuario
/// 2. Cancelar usando método de dominio Cancelar()
/// 3. Guardar cambios
/// 4. Log del motivo
/// </remarks>
public class CancelarSuscripcionCommandHandler : IRequestHandler<CancelarSuscripcionCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CancelarSuscripcionCommandHandler> _logger;

    public CancelarSuscripcionCommandHandler(
        IApplicationDbContext context,
        ILogger<CancelarSuscripcionCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(CancelarSuscripcionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Cancelando suscripción de usuario {UserId}. Motivo: {Motivo}",
            request.UserId,
            request.MotivoCancelacion ?? "No especificado");

        // PASO 1: Buscar suscripción activa
        var suscripcion = await _context.Suscripciones
            .Where(s => s.UserId == request.UserId && !s.Cancelada)
            .FirstOrDefaultAsync(cancellationToken);

        if (suscripcion == null)
        {
            throw new NotFoundException($"No se encontró suscripción activa para usuario {request.UserId}");
        }

        // PASO 2: Cancelar usando método de dominio
        suscripcion.Cancelar(request.MotivoCancelacion ?? "Cancelación solicitada");

        // PASO 3: Guardar cambios
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Suscripción {SuscripcionId} cancelada exitosamente para usuario {UserId}",
            suscripcion.Id,
            request.UserId);

        return Unit.Value;
    }
}
