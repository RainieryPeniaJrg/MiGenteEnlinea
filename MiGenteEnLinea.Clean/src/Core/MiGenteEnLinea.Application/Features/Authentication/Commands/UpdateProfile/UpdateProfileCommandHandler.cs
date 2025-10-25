using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateProfile;

/// <summary>
/// Handler para UpdateProfileCommand.
/// Réplica SIMPLIFICADA de LoginService.actualizarPerfil() del Legacy.
/// </summary>
public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<UpdateProfileCommandHandler> _logger;

    public UpdateProfileCommandHandler(
        IApplicationDbContext context,
        ILogger<UpdateProfileCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // ================================================================================
            // LÓGICA LEGACY: LoginService.actualizarPerfil(perfilesInfo info, Cuentas cuenta)
            // ================================================================================
            // Legacy actualizaba dos tablas: perfilesInfo y Cuentas
            // Por ahora solo actualizamos Perfiles (tabla unificada en el nuevo modelo)
            
            var perfil = await _context.Perfiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserID, cancellationToken);

            if (perfil == null)
            {
                _logger.LogWarning(
                    "Intento de actualizar perfil inexistente. UserID: {UserID}",
                    request.UserID);
                return false;
            }

            // Actualizar información básica usando método de dominio
            perfil.ActualizarInformacionBasica(
                nombre: request.Nombre,
                apellido: request.Apellido,
                email: request.Email ?? perfil.Email);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Perfil actualizado exitosamente. UserID: {UserID}",
                request.UserID);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error al actualizar perfil. UserID: {UserID}",
                request.UserID);
            return false;
        }
    }
}
