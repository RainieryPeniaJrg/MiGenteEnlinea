using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.RenovarSuscripcion;

/// <summary>
/// Handler para renovar una suscripción existente.
/// </summary>
/// <remarks>
/// Funcionalidad nueva (no existe en Legacy).
/// 
/// Flujo:
/// 1. Buscar suscripción activa del usuario
/// 2. Calcular nueva fecha de vencimiento
/// 3. Renovar usando método de dominio Renovar()
/// 4. Guardar cambios
/// 5. Log del motivo si se proporcionó
/// </remarks>
public class RenovarSuscripcionCommandHandler : IRequestHandler<RenovarSuscripcionCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<RenovarSuscripcionCommandHandler> _logger;

    public RenovarSuscripcionCommandHandler(
        IApplicationDbContext context,
        ILogger<RenovarSuscripcionCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(RenovarSuscripcionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Renovando suscripción de usuario {UserId} por {Meses} meses. Motivo: {Motivo}",
            request.UserId,
            request.MesesExtension,
            request.Motivo ?? "No especificado");

        // PASO 1: Buscar suscripción activa
        var suscripcion = await _context.Suscripciones
            .Where(s => s.UserId == request.UserId && !s.Cancelada)
            .FirstOrDefaultAsync(cancellationToken);

        if (suscripcion == null)
        {
            throw new NotFoundException($"No se encontró suscripción activa para usuario {request.UserId}");
        }

        // PASO 2: Renovar usando método de dominio
        // El método Renovar() internamente maneja si está vencida o no
        suscripcion.Renovar(request.MesesExtension);

        // PASO 3: Guardar cambios
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Suscripción {SuscripcionId} renovada exitosamente. Nuevo vencimiento: {Vencimiento}",
            suscripcion.Id,
            suscripcion.Vencimiento);

        return Unit.Value;
    }
}
