using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Contratistas;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.AddServicio;

/// <summary>
/// Handler: Agrega un servicio al perfil de un contratista
/// </summary>
public class AddServicioCommandHandler : IRequestHandler<AddServicioCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<AddServicioCommandHandler> _logger;

    public AddServicioCommandHandler(
        IApplicationDbContext context,
        ILogger<AddServicioCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> Handle(AddServicioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Agregando servicio al contratista. ContratistaId: {ContratistaId}, Detalle: {Detalle}",
            request.ContratistaId, request.DetalleServicio);

        // 1. VALIDAR: Contratista existe
        var contratistaExiste = await _context.Contratistas
            .AnyAsync(c => c.Id == request.ContratistaId, cancellationToken);

        if (!contratistaExiste)
        {
            _logger.LogWarning("Contratista no encontrado. ContratistaId: {ContratistaId}", request.ContratistaId);
            throw new InvalidOperationException($"No existe un contratista con ID {request.ContratistaId}");
        }

        // 2. CREAR SERVICIO usando Factory Method de dominio
        var servicio = ContratistaServicio.Agregar(
            contratistaId: request.ContratistaId,
            detalleServicio: request.DetalleServicio
        );

        // 3. AGREGAR A DBCONTEXT
        _context.ContratistasServicios.Add(servicio);

        // 4. GUARDAR CAMBIOS
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Servicio agregado exitosamente. ServicioId: {ServicioId}, ContratistaId: {ContratistaId}",
            servicio.ServicioId, request.ContratistaId);

        return servicio.ServicioId;
    }
}
