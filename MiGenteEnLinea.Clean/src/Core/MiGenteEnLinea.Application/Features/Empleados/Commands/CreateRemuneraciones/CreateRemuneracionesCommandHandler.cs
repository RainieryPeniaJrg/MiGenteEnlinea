using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Empleados;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CreateRemuneraciones;

/// <summary>
/// Handler para CreateRemuneracionesCommand.
/// Implementa guardarOtrasRemuneraciones() del Legacy (EmpleadosService.cs línea 646-654).
/// </summary>
/// <remarks>
/// ESTRATEGIA DDD:
/// - Usa Remuneracion.Crear() factory method para cada item
/// - Valida cada remuneración mediante factory (lanza excepciones si inválido)
/// - Batch insert con AddRange() para mejor performance
/// - Logging estructurado
/// 
/// DIFERENCIAS CON LEGACY:
/// - Legacy: Sin validaciones (asume datos válidos)
/// - Clean: Validaciones en factory method
/// - Legacy: No logging
/// - Clean: Logging estructurado en cada paso
/// 
/// PARIDAD LEGACY:
/// - Siempre retorna true (mismo comportamiento)
/// - Batch insert (AddRange + SaveChanges)
/// </remarks>
public class CreateRemuneracionesCommandHandler : IRequestHandler<CreateRemuneracionesCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CreateRemuneracionesCommandHandler> _logger;

    public CreateRemuneracionesCommandHandler(
        IApplicationDbContext context,
        ILogger<CreateRemuneracionesCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(CreateRemuneracionesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creando {Count} remuneraciones para EmpleadoId: {EmpleadoId}, UserId: {UserId}",
            request.Remuneraciones.Count,
            request.EmpleadoId,
            request.UserId);

        // PASO 1: Crear entidades usando DDD factory method
        var remuneraciones = request.Remuneraciones
            .Select(dto => Remuneracion.Crear(
                userId: request.UserId,
                empleadoId: request.EmpleadoId,
                descripcion: dto.Descripcion,
                monto: dto.Monto
            ))
            .ToList();

        _logger.LogInformation(
            "Entidades Remuneracion creadas exitosamente: {Count}",
            remuneraciones.Count);

        // PASO 2: Batch insert (Legacy: db.Remuneraciones.AddRange(rem))
        await _context.Set<Remuneracion>().AddRangeAsync(remuneraciones, cancellationToken);

        // PASO 3: SaveChanges (Legacy: db.SaveChanges())
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Remuneraciones guardadas exitosamente para EmpleadoId: {EmpleadoId}",
            request.EmpleadoId);

        // PASO 4: Return true (Legacy parity)
        return true;
    }
}
