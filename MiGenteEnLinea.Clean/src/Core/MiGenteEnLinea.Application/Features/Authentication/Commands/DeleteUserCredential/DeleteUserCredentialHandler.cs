using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.DeleteUserCredential;

/// <summary>
/// Handler para eliminar una credencial específica de un usuario
/// </summary>
public class DeleteUserCredentialHandler : IRequestHandler<DeleteUserCredentialCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DeleteUserCredentialHandler> _logger;

    public DeleteUserCredentialHandler(
        IApplicationDbContext context,
        ILogger<DeleteUserCredentialHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        DeleteUserCredentialCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Eliminando credencial {CredentialId} del usuario {UserId}",
            request.CredentialId,
            request.UserId);

        // Validar que la credencial existe y pertenece al usuario
        var credential = await _context.Credenciales
            .Where(c => c.Id == request.CredentialId && c.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (credential == null)
        {
            throw new NotFoundException(
                $"Credencial {request.CredentialId} no encontrada para usuario {request.UserId}");
        }

        // MEJORA sobre Legacy: Validar que no es la última credencial activa
        // Legacy no tenía esta validación, causaba problemas si usuario eliminaba su única credencial
        var activeCredentialsCount = await _context.Credenciales
            .Where(c => c.UserId == request.UserId && c.Activo == true)
            .CountAsync(cancellationToken);

        if (activeCredentialsCount <= 1 && credential.Activo == true)
        {
            throw new ValidationException(
                "No se puede eliminar la única credencial activa del usuario. " +
                "El usuario debe tener al menos una credencial activa para mantener acceso al sistema.");
        }

        // Eliminar credencial (mismo patrón que Legacy: db.Credenciales.Remove)
        _context.Credenciales.Remove(credential);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Credencial {CredentialId} eliminada exitosamente. Usuario {UserId} tiene {Count} credenciales restantes",
            request.CredentialId,
            request.UserId,
            activeCredentialsCount - (credential.Activo == true ? 1 : 0));

        return Unit.Value;
    }
}
