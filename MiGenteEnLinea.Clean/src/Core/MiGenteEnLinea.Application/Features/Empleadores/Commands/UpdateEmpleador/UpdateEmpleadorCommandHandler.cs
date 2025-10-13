using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.UpdateEmpleador;

/// <summary>
/// Handler: Procesa la actualización del perfil de Empleador
/// </summary>
public sealed class UpdateEmpleadorCommandHandler : IRequestHandler<UpdateEmpleadorCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<UpdateEmpleadorCommandHandler> _logger;

    public UpdateEmpleadorCommandHandler(
        IApplicationDbContext context,
        ILogger<UpdateEmpleadorCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la actualización del empleador
    /// </summary>
    /// <exception cref="InvalidOperationException">Si empleador no existe</exception>
    public async Task<bool> Handle(UpdateEmpleadorCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Actualizando empleador para userId: {UserId}", request.UserId);

        // ============================================
        // PASO 1: Buscar empleador por userId
        // ============================================
        var empleador = await _context.Empleadores
            .FirstOrDefaultAsync(e => e.UserId == request.UserId, cancellationToken);

        if (empleador == null)
        {
            _logger.LogWarning("Empleador no encontrado para userId: {UserId}", request.UserId);
            throw new InvalidOperationException($"Empleador no encontrado para usuario {request.UserId}");
        }

        // ============================================
        // PASO 2: Actualizar con método de dominio
        // ============================================
        // El método ActualizarPerfil() de la entidad Empleador maneja:
        // - Validaciones de longitud
        // - Trim de strings
        // - Levanta eventos de dominio (PerfilActualizadoEvent)
        empleador.ActualizarPerfil(
            habilidades: request.Habilidades,
            experiencia: request.Experiencia,
            descripcion: request.Descripcion
        );

        // ============================================
        // PASO 3: Guardar cambios
        // ============================================
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Empleador actualizado exitosamente. EmpleadorId: {EmpleadorId}, UserId: {UserId}",
            empleador.Id, request.UserId);

        return true;
    }
}
