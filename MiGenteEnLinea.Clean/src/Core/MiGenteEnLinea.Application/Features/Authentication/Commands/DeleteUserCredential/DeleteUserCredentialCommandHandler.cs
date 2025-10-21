using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.DeleteUserCredential;

/// <summary>
/// Handler para eliminar una credencial de usuario
/// Migrado de: LoginService.borrarUsuario(string userID, int credencialID) - line 108
/// </summary>
public class DeleteUserCredentialCommandHandler : IRequestHandler<DeleteUserCredentialCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DeleteUserCredentialCommandHandler> _logger;

    public DeleteUserCredentialCommandHandler(
        IApplicationDbContext context,
        ILogger<DeleteUserCredentialCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteUserCredentialCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Eliminando credencial {CredentialId} del usuario {UserId}",
            request.CredentialId,
            request.UserId);

        // PASO 1: Buscar credencial
        var credencial = await _context.Credenciales
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.Id == request.CredentialId, cancellationToken);

        if (credencial == null)
        {
            throw new NotFoundException($"Credencial {request.CredentialId} no encontrada para usuario {request.UserId}");
        }

        // PASO 2: Validar que no sea la última credencial activa (MEJORA sobre Legacy)
        var totalCredencialesActivas = await _context.Credenciales
            .CountAsync(x => x.UserId == request.UserId && x.Activo, cancellationToken);

        if (totalCredencialesActivas <= 1)
        {
            throw new ValidationException("No se puede eliminar la última credencial activa del usuario");
        }

        // PASO 3: Eliminar credencial (Legacy: db.Credenciales.Remove(result))
        _context.Credenciales.Remove(credencial);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Credencial {CredentialId} eliminada exitosamente para usuario {UserId}",
            request.CredentialId,
            request.UserId);

        return Unit.Value;
    }
}
