using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetPlanesEmpleadores;

/// <summary>
/// Handler para GetPlanesEmpleadoresQuery.
/// </summary>
/// <remarks>
/// LÓGICA LEGACY:
/// - SuscripcionesService.obtenerPlanes()
/// - Retorna lista completa de planes de empleadores
/// - Usado en: AdquirirPlan.aspx, Checkout.aspx, etc.
/// </remarks>
public class GetPlanesEmpleadoresQueryHandler : IRequestHandler<GetPlanesEmpleadoresQuery, List<PlanEmpleador>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetPlanesEmpleadoresQueryHandler> _logger;

    public GetPlanesEmpleadoresQueryHandler(
        IApplicationDbContext context,
        ILogger<GetPlanesEmpleadoresQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<PlanEmpleador>> Handle(GetPlanesEmpleadoresQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Obteniendo planes de empleadores. SoloActivos: {SoloActivos}",
            request.SoloActivos);

        var query = _context.PlanesEmpleadores.AsQueryable();

        // Filtrar solo activos si se solicita
        if (request.SoloActivos)
        {
            query = query.Where(p => p.Activo);
        }

        // Ordenar por precio ascendente (del más barato al más caro)
        var planes = await query
            .OrderBy(p => p.Precio)
            .ToListAsync(cancellationToken);

        _logger.LogInformation(
            "Se encontraron {Count} planes de empleadores",
            planes.Count);

        return planes;
    }
}
