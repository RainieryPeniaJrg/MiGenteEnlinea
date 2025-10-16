using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.UpdateSuscripcion;

/// <summary>
/// Handler para actualizar plan y vencimiento de una suscripción.
/// </summary>
/// <remarks>
/// Lógica migrada desde: SuscripcionesService.actualizarSuscripcion()
/// 
/// Flujo:
/// 1. Buscar suscripción activa del usuario
/// 2. Validar que el nuevo plan existe
/// 3. Actualizar usando método de dominio CambiarPlan()
/// 4. Guardar cambios
/// </remarks>
public class UpdateSuscripcionCommandHandler : IRequestHandler<UpdateSuscripcionCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<UpdateSuscripcionCommandHandler> _logger;

    public UpdateSuscripcionCommandHandler(
        IApplicationDbContext context,
        ILogger<UpdateSuscripcionCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateSuscripcionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Actualizando suscripción de usuario {UserId} a plan {PlanId}",
            request.UserId,
            request.NuevoPlanId);

        // PASO 1: Buscar suscripción activa
        var suscripcion = await _context.Suscripciones
            .Where(s => s.UserId == request.UserId && !s.Cancelada)
            .FirstOrDefaultAsync(cancellationToken);

        if (suscripcion == null)
        {
            throw new NotFoundException($"No se encontró suscripción activa para usuario {request.UserId}");
        }

        // PASO 2: Validar que el nuevo plan existe
        var planExiste = await _context.PlanesEmpleadores
            .AnyAsync(p => p.PlanId == request.NuevoPlanId, cancellationToken);

        if (!planExiste)
        {
            planExiste = await _context.PlanesContratistas
                .AnyAsync(p => p.PlanId == request.NuevoPlanId, cancellationToken);
        }

        if (!planExiste)
        {
            throw new NotFoundException($"Plan con ID {request.NuevoPlanId} no encontrado");
        }

        // PASO 3: Actualizar usando métodos de dominio
        // Primero cambiar el plan
        suscripcion.CambiarPlan(request.NuevoPlanId, ajustarVencimiento: false);
        
        // Luego calcular días de diferencia para extender/reducir vencimiento
        var vencimientoActual = suscripcion.Vencimiento.ToDateTime(TimeOnly.MinValue);
        var nuevoVencimiento = request.NuevoVencimiento;
        var diasDiferencia = (int)(nuevoVencimiento - vencimientoActual).TotalDays;
        
        if (diasDiferencia > 0)
        {
            suscripcion.ExtenderVencimiento(diasDiferencia);
        }
        else if (diasDiferencia < 0)
        {
            // Para reducir, necesitamos cancelar y crear nueva (no hay método directo)
            // Por ahora solo loguear warning
            _logger.LogWarning(
                "No se puede reducir vencimiento de {Actual} a {Nuevo}. Se mantiene el vencimiento actual.",
                vencimientoActual,
                nuevoVencimiento);
        }

        // PASO 4: Guardar cambios
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Suscripción {SuscripcionId} actualizada exitosamente. Nuevo plan: {PlanId}, Nuevo vencimiento: {Vencimiento}",
            suscripcion.Id,
            request.NuevoPlanId,
            suscripcion.Vencimiento);

        return Unit.Value;
    }
}
