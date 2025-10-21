using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Domain.Entities.Contratistas;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.AddServicio;

/// <summary>
/// Handler: Agrega un servicio al perfil de un contratista
/// </summary>
public class AddServicioCommandHandler : IRequestHandler<AddServicioCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddServicioCommandHandler> _logger;

    public AddServicioCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<AddServicioCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<int> Handle(AddServicioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Agregando servicio al contratista. ContratistaId: {ContratistaId}, Detalle: {Detalle}",
            request.ContratistaId, request.DetalleServicio);

        // 1. VALIDAR: Contratista existe
        var contratista = await _unitOfWork.Contratistas
            .GetByIdAsync(request.ContratistaId, cancellationToken);

        if (contratista == null)
        {
            _logger.LogWarning("Contratista no encontrado. ContratistaId: {ContratistaId}", request.ContratistaId);
            throw new InvalidOperationException($"No existe un contratista con ID {request.ContratistaId}");
        }

        // 2. CREAR SERVICIO usando Factory Method de dominio
        var servicio = ContratistaServicio.Agregar(
            contratistaId: request.ContratistaId,
            detalleServicio: request.DetalleServicio
        );

        // 3. AGREGAR AL REPOSITORIO
        await _unitOfWork.ContratistasServicios.AddAsync(servicio, cancellationToken);

        // 4. GUARDAR CAMBIOS
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Servicio agregado exitosamente. ServicioId: {ServicioId}, ContratistaId: {ContratistaId}",
            servicio.ServicioId, request.ContratistaId);

        return servicio.ServicioId;
    }
}
