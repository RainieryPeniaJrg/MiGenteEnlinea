using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetPlanesContratistas;

/// <summary>
/// Handler para GetPlanesContratistasQuery.
/// </summary>
/// <remarks>
/// LÓGICA LEGACY:
/// - SuscripcionesService.obtenerPlanesContratistas()
/// - Retorna lista completa de planes de contratistas
/// - Usado en: AdquirirPlanContratista.aspx, CheckoutContratista.aspx
/// </remarks>
public class GetPlanesContratistasQueryHandler : IRequestHandler<GetPlanesContratistasQuery, List<PlanContratista>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetPlanesContratistasQueryHandler> _logger;

    public GetPlanesContratistasQueryHandler(
        IApplicationDbContext context,
        ILogger<GetPlanesContratistasQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<PlanContratista>> Handle(GetPlanesContratistasQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Obteniendo planes de contratistas. SoloActivos: {SoloActivos}",
            request.SoloActivos);

        var query = _context.PlanesContratistas.AsQueryable();

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
            "Se encontraron {Count} planes de contratistas",
            planes.Count);

        return planes;
    }
}
