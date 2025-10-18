using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Features.Contrataciones.DTOs;
using MiGenteEnLinea.Domain.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Queries.GetContratacionById;

/// <summary>
/// Handler para obtener una contratación por ID.
/// </summary>
public class GetContratacionByIdQueryHandler : IRequestHandler<GetContratacionByIdQuery, ContratacionDetalleDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetContratacionByIdQueryHandler> _logger;

    public GetContratacionByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetContratacionByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ContratacionDetalleDto?> Handle(
        GetContratacionByIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting contratacion with ID: {DetalleId}", request.DetalleId);

        var contratacion = await _unitOfWork.DetallesContrataciones
            .GetByIdAsync(request.DetalleId, cancellationToken);

        if (contratacion == null)
        {
            _logger.LogWarning("Contratacion not found with ID: {DetalleId}", request.DetalleId);
            return null;
        }

        var dto = _mapper.Map<ContratacionDetalleDto>(contratacion);

        _logger.LogInformation(
            "Retrieved contratacion {DetalleId} - Status: {Status}",
            contratacion.DetalleId,
            contratacion.ObtenerNombreEstado());

        return dto;
    }
}
