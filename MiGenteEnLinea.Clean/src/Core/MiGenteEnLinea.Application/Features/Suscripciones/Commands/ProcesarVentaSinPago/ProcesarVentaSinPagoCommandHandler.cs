using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Suscripciones;
using MiGenteEnLinea.Domain.Entities.Pagos;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Commands.ProcesarVentaSinPago;

/// <summary>
/// Handler para procesar venta sin pago (planes gratuitos).
/// </summary>
/// <remarks>
/// Flujo:
/// 1. Validar que el plan existe y es gratuito (Precio = 0)
/// 2. Crear registro de venta con MetodoPago = "Gratuito"
/// 3. Crear o renovar suscripción
/// 4. Guardar cambios
/// </remarks>
public class ProcesarVentaSinPagoCommandHandler : IRequestHandler<ProcesarVentaSinPagoCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProcesarVentaSinPagoCommandHandler> _logger;

    public ProcesarVentaSinPagoCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ProcesarVentaSinPagoCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<int> Handle(ProcesarVentaSinPagoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Procesando venta sin pago para usuario {UserId}, plan {PlanId}. Motivo: {Motivo}",
            request.UserId,
            request.PlanId,
            request.Motivo ?? "No especificado");

        // PASO 1: Validar que el plan existe
        var planEmpleador = await _unitOfWork.PlanesEmpleadores.GetByIdAsync(request.PlanId, cancellationToken);
        var planContratista = await _unitOfWork.PlanesContratistas.GetByIdAsync(request.PlanId, cancellationToken);

        if (planEmpleador == null && planContratista == null)
        {
            throw new NotFoundException($"Plan con ID {request.PlanId} no encontrado");
        }

        var plan = (object?)planEmpleador ?? planContratista!;
        var precio = planEmpleador?.Precio ?? planContratista!.Precio;
        // Duración hardcoded a 1 mes (como en Legacy)
        var duracionMeses = 1;

        // Validar que el plan es gratuito
        if (precio > 0)
        {
            throw new ValidationException($"El plan {request.PlanId} no es gratuito (Precio: {precio:C})");
        }

        // PASO 2: Crear registro de venta
        var venta = Venta.Create(
            userId: request.UserId,
            planId: request.PlanId,
            precio: 0,
            metodoPago: 4, // Otro (plan gratuito)
            idempotencyKey: $"FREE-{Guid.NewGuid():N}",
            direccionIp: null);

        // Marcar como aprobado inmediatamente (no requiere pago)
        venta.Aprobar(
            idTransaccion: $"FREE-{DateTime.UtcNow:yyyyMMddHHmmss}",
            ultimosDigitosTarjeta: null,
            comentario: request.Motivo ?? "Plan gratuito");

        await _unitOfWork.Ventas.AddAsync(venta, cancellationToken);

        // PASO 3: Crear o renovar suscripción
        var suscripcionExistente = await _unitOfWork.Suscripciones
            .GetNoCanceladaByUserIdAsync(request.UserId, cancellationToken);

        if (suscripcionExistente != null)
        {
            // Renovar existente
            _logger.LogInformation(
                "Renovando suscripción existente {SuscripcionId} por {Meses} meses",
                suscripcionExistente.Id,
                duracionMeses);

            // Renovar usa el método de dominio que maneja la lógica de vencimiento
            suscripcionExistente.Renovar(duracionMeses);
        }
        else
        {
            // Crear nueva
            _logger.LogInformation(
                "Creando nueva suscripción para usuario {UserId}, plan {PlanId}",
                request.UserId,
                request.PlanId);

            var nuevaSuscripcion = Suscripcion.Create(
                userId: request.UserId,
                planId: request.PlanId,
                duracionMeses: duracionMeses);

            await _unitOfWork.Suscripciones.AddAsync(nuevaSuscripcion, cancellationToken);
        }

        // PASO 4: Guardar cambios
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Venta gratuita {VentaId} procesada exitosamente para usuario {UserId}",
            venta.VentaId,
            request.UserId);

        return venta.VentaId;
    }
}
