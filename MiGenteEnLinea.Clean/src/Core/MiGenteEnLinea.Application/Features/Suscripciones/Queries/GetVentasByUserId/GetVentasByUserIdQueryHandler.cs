using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Pagos;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetVentasByUserId;

/// <summary>
/// Handler para GetVentasByUserIdQuery.
/// </summary>
/// <remarks>
/// NUEVA FUNCIONALIDAD (no existe en Legacy).
/// Retorna historial paginado de ventas/pagos del usuario.
/// Estados: 2=Aprobado, 3=Error, 4=Rechazado
/// </remarks>
public class GetVentasByUserIdQueryHandler : IRequestHandler<GetVentasByUserIdQuery, List<Venta>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetVentasByUserIdQueryHandler> _logger;

    public GetVentasByUserIdQueryHandler(
        IApplicationDbContext context,
        ILogger<GetVentasByUserIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Venta>> Handle(GetVentasByUserIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Obteniendo ventas del usuario {UserId}. Página: {PageNumber}, Tamaño: {PageSize}, SoloAprobadas: {SoloAprobadas}",
            request.UserId, request.PageNumber, request.PageSize, request.SoloAprobadas);

        // Validar parámetros de paginación
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;
        if (pageSize > 100)
        {
            _logger.LogWarning("PageSize {PageSize} excede el máximo permitido (100). Ajustando a 100.", pageSize);
            pageSize = 100;
        }

        var query = _context.Ventas
            .Where(v => v.UserId == request.UserId);

        // Filtrar solo aprobadas si se solicita (Estado = 2)
        if (request.SoloAprobadas)
        {
            query = query.Where(v => v.Estado == 2);
        }

        // Ordenar por fecha descendente (más reciente primero)
        // Paginar resultados
        var ventas = await query
            .OrderByDescending(v => v.FechaTransaccion)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        _logger.LogInformation(
            "Se encontraron {Count} ventas para el usuario {UserId} en la página {PageNumber}",
            ventas.Count, request.UserId, pageNumber);

        // Log de totales (para debugging)
        var totalVentas = await _context.Ventas
            .Where(v => v.UserId == request.UserId)
            .CountAsync(cancellationToken);
        
        _logger.LogInformation(
            "Total de ventas del usuario {UserId}: {Total}. Páginas disponibles: {TotalPaginas}",
            request.UserId, totalVentas, (int)Math.Ceiling(totalVentas / (double)pageSize));

        return ventas;
    }
}
