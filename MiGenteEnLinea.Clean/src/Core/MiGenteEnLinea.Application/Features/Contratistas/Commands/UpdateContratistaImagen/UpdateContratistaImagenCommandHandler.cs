using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.UpdateContratistaImagen;

/// <summary>
/// Handler: Actualiza la imagen de perfil de un contratista
/// </summary>
public class UpdateContratistaImagenCommandHandler : IRequestHandler<UpdateContratistaImagenCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<UpdateContratistaImagenCommandHandler> _logger;

    public UpdateContratistaImagenCommandHandler(
        IApplicationDbContext context,
        ILogger<UpdateContratistaImagenCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(UpdateContratistaImagenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Actualizando imagen de contratista para userId: {UserId}", request.UserId);

        // 1. BUSCAR CONTRATISTA por userId
        var contratista = await _context.Contratistas
            .Where(c => c.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (contratista == null)
        {
            _logger.LogWarning("Contratista no encontrado para userId: {UserId}", request.UserId);
            throw new InvalidOperationException($"No existe un perfil de contratista para el usuario {request.UserId}");
        }

        // 2. ACTUALIZAR IMAGEN usando Domain Method
        try
        {
            contratista.ActualizarImagen(request.ImagenUrl);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "ImagenUrl inválida para userId: {UserId}", request.UserId);
            throw new InvalidOperationException($"La URL de la imagen no es válida: {ex.Message}");
        }

        // 3. GUARDAR CAMBIOS
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Imagen de contratista actualizada exitosamente. ContratistaId: {ContratistaId}, ImagenUrl: {ImagenUrl}",
            contratista.Id, request.ImagenUrl);
    }
}
