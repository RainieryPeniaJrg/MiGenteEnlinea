using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Contratistas.Common;

namespace MiGenteEnLinea.Application.Features.Contratistas.Queries.GetServiciosContratista;

/// <summary>
/// Handler: Obtiene todos los servicios de un contratista
/// </summary>
public class GetServiciosContratistaQueryHandler : IRequestHandler<GetServiciosContratistaQuery, List<ServicioContratistaDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetServiciosContratistaQueryHandler> _logger;

    public GetServiciosContratistaQueryHandler(
        IApplicationDbContext context,
        ILogger<GetServiciosContratistaQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ServicioContratistaDto>> Handle(GetServiciosContratistaQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obteniendo servicios del contratista. ContratistaId: {ContratistaId}", request.ContratistaId);

        var servicios = await _context.ContratistasServicios
            .AsNoTracking()
            .Where(s => s.ContratistaId == request.ContratistaId)
            .OrderBy(s => s.Orden)
            .ThenBy(s => s.ServicioId)
            .Select(s => new ServicioContratistaDto
            {
                ServicioId = s.ServicioId,
                ContratistaId = s.ContratistaId,
                DetalleServicio = s.DetalleServicio,
                Activo = s.Activo,
                AniosExperiencia = s.AniosExperiencia,
                TarifaBase = s.TarifaBase,
                Orden = s.Orden,
                Certificaciones = s.Certificaciones
            })
            .ToListAsync(cancellationToken);

        _logger.LogInformation(
            "Servicios obtenidos exitosamente. ContratistaId: {ContratistaId}, Total: {Total}",
            request.ContratistaId, servicios.Count);

        return servicios;
    }
}
