using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Contrataciones;
using MiGenteEnLinea.Domain.Entities.Empleados;
using MiGenteEnLinea.Domain.Entities.Pagos;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.EliminarEmpleadoTemporal;

/// <summary>
/// Handler para eliminar un empleado temporal con cascade delete manual.
/// Implementa eliminarEmpleadoTemporal() del Legacy (EmpleadosService.cs línea 299-357).
/// </summary>
/// <remarks>
/// ANÁLISIS DE CONFIGURACIÓN EF CORE:
/// 
/// EmpleadoTemporalConfiguration.cs:
/// - Relación 1: EmpleadoTemporal → DetalleContratacion (1:N)
///   OnDelete(DeleteBehavior.Restrict) ⚠️ Sin auto-cascade
/// 
/// - Relación 2: EmpleadoTemporal → EmpleadorRecibosHeaderContratacione (1:N)
///   OnDelete(DeleteBehavior.Restrict) ⚠️ Sin auto-cascade
/// 
/// ORDEN DE ELIMINACIÓN OBLIGATORIO (por FK constraints):
/// 1. EmpleadorRecibosDetalleContrataciones (nietos - FK a Header)
/// 2. EmpleadorRecibosHeaderContrataciones (hijos - FK a EmpleadoTemporal)
/// 3. EmpleadoTemporal (root)
/// 
/// DDD PATTERN:
/// - Dominio NO tiene método Eliminar() → Operación de infraestructura
/// - Usar Repository pattern (context.Remove)
/// - Transacción única (no múltiples SaveChanges como Legacy)
/// - Respeta DeleteBehavior.Restrict haciendo delete manual
/// 
/// DIFERENCIAS CON LEGACY:
/// - Legacy: Múltiples DbContext + SaveChanges en cada paso (anti-pattern)
/// - Clean: Transacción única con SaveChanges al final
/// - Legacy: Loop foreach con SaveChanges interno
/// - Clean: Batch RemoveRange para mejor performance
/// </remarks>
public class EliminarEmpleadoTemporalCommandHandler : IRequestHandler<EliminarEmpleadoTemporalCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<EliminarEmpleadoTemporalCommandHandler> _logger;

    public EliminarEmpleadoTemporalCommandHandler(
        IApplicationDbContext context,
        ILogger<EliminarEmpleadoTemporalCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Elimina EmpleadoTemporal con cascade delete manual en orden correcto
    /// </summary>
    public async Task<bool> Handle(EliminarEmpleadoTemporalCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Iniciando eliminación de EmpleadoTemporal con ContratacionId: {ContratacionId}",
            request.ContratacionId);

        // PASO 1: Verificar si existe EmpleadoTemporal
        var empleadoTemporal = await _context.Set<EmpleadoTemporal>()
            .FirstOrDefaultAsync(e => e.ContratacionId == request.ContratacionId, cancellationToken);

        if (empleadoTemporal == null)
        {
            _logger.LogWarning(
                "EmpleadoTemporal no encontrado con ContratacionId: {ContratacionId}",
                request.ContratacionId);
            
            // Legacy retorna true incluso si no encuentra el registro
            return true;
        }

        // PASO 2: Buscar todos los Headers de recibos asociados
        var recibosHeaders = await _context.Set<EmpleadorRecibosHeaderContratacione>()
            .Where(h => h.ContratacionId == request.ContratacionId)
            .ToListAsync(cancellationToken);

        _logger.LogInformation(
            "Encontrados {Count} recibos headers para ContratacionId: {ContratacionId}",
            recibosHeaders.Count,
            request.ContratacionId);

        // PASO 3: Para cada Header, eliminar sus Detalles (nietos primero)
        foreach (var header in recibosHeaders)
        {
            // 3a. Buscar detalles del recibo
            var recibosDetalles = await _context.Set<EmpleadorRecibosDetalleContratacione>()
                .Where(d => d.PagoId == header.PagoId)
                .ToListAsync(cancellationToken);

            if (recibosDetalles.Any())
            {
                _logger.LogInformation(
                    "Eliminando {Count} detalles de recibo para PagoId: {PagoId}",
                    recibosDetalles.Count,
                    header.PagoId);

                // 3b. Eliminar detalles (batch delete - mejor performance que Legacy)
                _context.Set<EmpleadorRecibosDetalleContratacione>().RemoveRange(recibosDetalles);
            }
        }

        // PASO 4: Eliminar todos los Headers (hijos después de nietos)
        if (recibosHeaders.Any())
        {
            _logger.LogInformation(
                "Eliminando {Count} headers de recibos para ContratacionId: {ContratacionId}",
                recibosHeaders.Count,
                request.ContratacionId);

            _context.Set<EmpleadorRecibosHeaderContratacione>().RemoveRange(recibosHeaders);
        }

        // PASO 5: Eliminar EmpleadoTemporal (root al final)
        _logger.LogInformation(
            "Eliminando EmpleadoTemporal con ContratacionId: {ContratacionId}",
            request.ContratacionId);

        _context.Set<EmpleadoTemporal>().Remove(empleadoTemporal);

        // PASO 6: SaveChanges UNA SOLA VEZ (transacción atómica)
        // Nota: Legacy hace SaveChanges múltiples veces, pero esto es más seguro
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "EmpleadoTemporal eliminado exitosamente. ContratacionId: {ContratacionId}",
            request.ContratacionId);

        // Legacy siempre retorna true
        return true;
    }
}
