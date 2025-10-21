using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Features.Contratistas.Common;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Contratistas.Queries.GetServiciosContratista;

/// <summary>
/// Handler: Obtiene todos los servicios de un contratista
/// </summary>
public class GetServiciosContratistaQueryHandler : IRequestHandler<GetServiciosContratistaQuery, List<ServicioContratistaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetServiciosContratistaQueryHandler> _logger;

    public GetServiciosContratistaQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetServiciosContratistaQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<ServicioContratistaDto>> Handle(GetServiciosContratistaQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obteniendo servicios del contratista. ContratistaId: {ContratistaId}", request.ContratistaId);

        // USAR REPOSITORIO: GetByContratistaIdAsync ya devuelve ordenado por Orden
        var servicios = await _unitOfWork.ContratistasServicios
            .GetByContratistaIdAsync(request.ContratistaId, cancellationToken);

        // MAPEAR A DTO
        var serviciosDto = servicios.Select(s => new ServicioContratistaDto
        {
            ServicioId = s.ServicioId,
            ContratistaId = s.ContratistaId,
            DetalleServicio = s.DetalleServicio,
            Activo = s.Activo,
            AniosExperiencia = s.AniosExperiencia,
            TarifaBase = s.TarifaBase,
            Orden = s.Orden,
            Certificaciones = s.Certificaciones
        }).ToList();

        _logger.LogInformation(
            "Servicios obtenidos exitosamente. ContratistaId: {ContratistaId}, Total: {Total}",
            request.ContratistaId, serviciosDto.Count);

        return serviciosDto;
    }
}
