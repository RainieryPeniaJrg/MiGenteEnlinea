using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Suscripciones;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

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
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPlanesEmpleadoresQueryHandler> _logger;

    public GetPlanesEmpleadoresQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetPlanesEmpleadoresQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<PlanEmpleador>> Handle(GetPlanesEmpleadoresQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Obteniendo planes de empleadores. SoloActivos: {SoloActivos}",
            request.SoloActivos);

        var planes = request.SoloActivos
            ? await _unitOfWork.PlanesEmpleadores.GetActivosAsync(cancellationToken)
            : await _unitOfWork.PlanesEmpleadores.GetAllOrderedByPrecioAsync(cancellationToken);

        _logger.LogInformation(
            "Se encontraron {Count} planes de empleadores",
            planes.Count());

        return planes.ToList();
    }
}
