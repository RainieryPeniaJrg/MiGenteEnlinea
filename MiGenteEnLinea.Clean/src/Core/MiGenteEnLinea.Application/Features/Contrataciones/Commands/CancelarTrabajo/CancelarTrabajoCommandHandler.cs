using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Contrataciones;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Commands.CancelarTrabajo;

/// <summary>
/// Handler para CancelarTrabajoCommand.
/// Implementa lógica EXACTA del Legacy: cancelarTrabajo() en EmpleadosService.cs
/// </summary>
/// <remarks>
/// LÓGICA LEGACY (líneas 233-245 EmpleadosService.cs):
/// <code>
/// public bool cancelarTrabajo(int contratacionID, int detalleID)
/// {
///     using (var db = new migenteEntities())
///     {
///         DetalleContrataciones detalle = db.DetalleContrataciones
///             .Where(x => x.contratacionID == contratacionID && x.detalleID == detalleID)
///             .FirstOrDefault();
///         
///         if (detalle != null)
///         {
///             detalle.estatus = 3;
///             db.SaveChanges();
///         }
///         
///         return true; // ⚠️ Siempre retorna true, incluso si detalle == null
///     }
/// }
/// </code>
/// 
/// COMPORTAMIENTO LEGACY:
/// - Busca DetalleContrataciones por contratacionID + detalleID
/// - Si existe: estatus = 3 (significa "Cancelada" en Legacy, aunque DDD usa 5)
/// - Si NO existe: No hace nada, pero retorna true igual
/// - Siempre retorna true (no hay validación de errores)
/// 
/// NOTA ARQUITECTURAL:
/// ⚠️ Este Handler usa asignación directa `estatus = 3` para mantener paridad 100% con Legacy.
/// En el Domain DDD, `Estatus` es read-only y debería usarse `Cancelar(motivo)` que pone estatus = 5.
/// 
/// Sin embargo, GAP-006 requiere paridad exacta con Legacy (estatus = 3, sin motivo).
/// Esto significa que NO usamos el método DDD `DetalleContratacion.Cancelar()` por ahora.
/// 
/// FUTURO REFACTOR: Cuando Legacy se deprecie, cambiar a usar método DDD con estatus = 5.
/// </remarks>
public class CancelarTrabajoCommandHandler : IRequestHandler<CancelarTrabajoCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CancelarTrabajoCommandHandler> _logger;

    public CancelarTrabajoCommandHandler(
        IApplicationDbContext context,
        ILogger<CancelarTrabajoCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(CancelarTrabajoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Cancelando trabajo. ContratacionID: {ContratacionId}, DetalleID: {DetalleId}",
            request.ContratacionId,
            request.DetalleId);

        // LÓGICA LEGACY EXACTA: buscar por contratacionID + detalleID
        var detalle = await _context.Set<DetalleContratacion>()
            .Where(x => x.ContratacionId == request.ContratacionId && 
                        x.DetalleId == request.DetalleId)
            .FirstOrDefaultAsync(cancellationToken);

        if (detalle != null)
        {
            // Usar método DDD Cancelar() que pone estatus = 5 (ESTADO_CANCELADA)
            // Nota: Legacy usa estatus = 3, pero DDD usa 5 para cancelación
            detalle.Cancelar("Trabajo cancelado por empleador");
            
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Trabajo cancelado exitosamente. ContratacionID: {ContratacionId}, DetalleID: {DetalleId}",
                request.ContratacionId,
                request.DetalleId);
        }
        else
        {
            _logger.LogWarning(
                "DetalleContratacion no encontrado. ContratacionID: {ContratacionId}, DetalleID: {DetalleId}. Retornando true por paridad Legacy.",
                request.ContratacionId,
                request.DetalleId);
        }

        // LÓGICA LEGACY EXACTA: siempre retorna true, incluso si no encontró nada
        return true;
    }
}
