using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.CreateSuscripcion;

/// <summary>
/// Handler para crear una nueva suscripción.
/// </summary>
/// <remarks>
/// Lógica migrada desde: SuscripcionesService.guardarSuscripcion()
/// 
/// Flujo:
/// 1. Validar que el plan existe
/// 2. Calcular fecha de vencimiento (FechaInicio + Plan.Duracion meses)
/// 3. Cancelar suscripción activa previa si existe
/// 4. Crear nueva suscripción usando Suscripcion.Create()
/// 5. Guardar en base de datos
/// </remarks>
public class CreateSuscripcionCommandHandler : IRequestHandler<CreateSuscripcionCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CreateSuscripcionCommandHandler> _logger;

    public CreateSuscripcionCommandHandler(
        IApplicationDbContext context,
        ILogger<CreateSuscripcionCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> Handle(CreateSuscripcionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creando suscripción para usuario {UserId} con plan {PlanId}",
            request.UserId,
            request.PlanId);

        // PASO 1: Validar que el plan existe (puede ser Empleador o Contratista)
        var planEmpleador = await _context.PlanesEmpleadores
            .FirstOrDefaultAsync(p => p.PlanId == request.PlanId, cancellationToken);

        var planContratista = await _context.PlanesContratistas
            .FirstOrDefaultAsync(p => p.PlanId == request.PlanId, cancellationToken);

        if (planEmpleador == null && planContratista == null)
        {
            throw new NotFoundException($"Plan con ID {request.PlanId} no encontrado");
        }

        // Obtener duración del plan (en meses)
        // En legacy, la duración está hardcoded a 1 mes
        var duracionMeses = 1;

        // PASO 2: Cancelar suscripción activa previa si existe
        var suscripcionActiva = await _context.Suscripciones
            .Where(s => s.UserId == request.UserId && !s.Cancelada)
            .FirstOrDefaultAsync(cancellationToken);

        if (suscripcionActiva != null)
        {
            _logger.LogInformation(
                "Cancelando suscripción activa previa {SuscripcionId} para usuario {UserId}",
                suscripcionActiva.Id,
                request.UserId);

            suscripcionActiva.Cancelar("Reemplazada por nueva suscripción");
        }

        // PASO 3: Crear nueva suscripción
        var nuevaSuscripcion = Suscripcion.Create(
            userId: request.UserId,
            planId: request.PlanId,
            duracionMeses: duracionMeses);

        _context.Suscripciones.Add(nuevaSuscripcion);

        // PASO 4: Guardar cambios
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Suscripción {SuscripcionId} creada exitosamente para usuario {UserId}. Vence: {Vencimiento}",
            nuevaSuscripcion.Id,
            request.UserId,
            nuevaSuscripcion.Vencimiento);

        return nuevaSuscripcion.Id;
    }
}
