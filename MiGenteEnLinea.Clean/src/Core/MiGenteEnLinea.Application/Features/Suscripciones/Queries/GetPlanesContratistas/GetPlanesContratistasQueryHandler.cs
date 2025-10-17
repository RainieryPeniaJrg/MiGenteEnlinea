using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Suscripciones;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

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
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPlanesContratistasQueryHandler> _logger;

    public GetPlanesContratistasQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetPlanesContratistasQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<PlanContratista>> Handle(GetPlanesContratistasQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Obteniendo planes de contratistas. SoloActivos: {SoloActivos}",
            request.SoloActivos);

        var planes = request.SoloActivos
            ? await _unitOfWork.PlanesContratistas.GetActivosAsync(cancellationToken)
            : await _unitOfWork.PlanesContratistas.GetAllOrderedByPrecioAsync(cancellationToken);

        _logger.LogInformation(
            "Se encontraron {Count} planes de contratistas",
            planes.Count());

        return planes.ToList();
    }
}
