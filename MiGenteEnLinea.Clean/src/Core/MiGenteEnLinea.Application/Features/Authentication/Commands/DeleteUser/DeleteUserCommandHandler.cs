using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.DeleteUser;

/// <summary>
/// Handler para DeleteUserCommand.
/// Implementa SOFT DELETE marcando el usuario como inactivo.
/// RÉPLICA EXACTA de LoginService.borrarUsuario() del Legacy.
/// </summary>
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(
        IApplicationDbContext context,
        ILogger<DeleteUserCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // ================================================================================
            // LÓGICA LEGACY: LoginService.borrarUsuario()
            // ================================================================================
            // Legacy: db.Credenciales.Where(a => a.userID == userID && a.id == credencialID).FirstOrDefault()
            // Legacy: db.Credenciales.Remove(result) - HARD DELETE

            var credencial = await _context.Credenciales
                .FirstOrDefaultAsync(c => c.UserId == request.UserID && c.Id == request.CredencialID, cancellationToken);

            if (credencial == null)
            {
                _logger.LogWarning(
                    "Intento de eliminar credencial inexistente. UserID: {UserID}, CredencialID: {CredencialID}",
                    request.UserID,
                    request.CredencialID);

                return false;
            }

            // ================================================================================
            // MEJORA vs Legacy: SOFT DELETE en lugar de HARD DELETE
            // ================================================================================
            // Legacy usaba Remove() que borraba físicamente el registro
            // Clean usa Desactivar() que marca Activo = false
            credencial.Desactivar();

            // Guardar cambios (interceptor actualizará ModifiedAt automáticamente)
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Usuario eliminado (soft delete) exitosamente. UserID: {UserID}, CredencialID: {CredencialID}",
                request.UserID,
                request.CredencialID);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error al eliminar usuario. UserID: {UserID}, CredencialID: {CredencialID}",
                request.UserID,
                request.CredencialID);

            return false;
        }
    }
}
