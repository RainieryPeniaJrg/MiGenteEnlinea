using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ResetPassword;

/// <summary>
/// Handler para resetear contraseña con token
/// </summary>
public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        ILogger<ResetPasswordCommandHandler> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("ResetPassword: Email={Email}", request.Email);

        // Buscar usuario por email
        var credencial = await _context.Credenciales
            .Where(c => c.Email == request.Email && c.Activo)
            .FirstOrDefaultAsync(cancellationToken);

        if (credencial == null)
        {
            _logger.LogWarning("ResetPassword: Email no encontrado o cuenta inactiva - {Email}", request.Email);
            return false;
        }

        // Buscar token válido más reciente para este usuario
        var resetToken = await _context.PasswordResetTokens
            .Where(t => t.UserId == credencial.UserId && 
                       t.Email == request.Email &&
                       t.Token == request.Token)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (resetToken == null)
        {
            _logger.LogWarning("ResetPassword: Token no encontrado - Email={Email}", request.Email);
            return false;
        }

        // Validar token (no usado y no expirado)
        if (!resetToken.ValidateToken(request.Token))
        {
            _logger.LogWarning(
                "ResetPassword: Token inválido - TokenId={TokenId}, IsExpired={IsExpired}, IsUsed={IsUsed}",
                resetToken.Id, resetToken.IsExpired, resetToken.IsUsed);
            return false;
        }
        
        // Hash nueva contraseña
        var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);
        credencial.ActualizarPasswordHash(newPasswordHash);

        // Marcar token como usado
        resetToken.MarkAsUsed();

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("ResetPassword: Contraseña actualizada exitosamente para {Email}", request.Email);
        return true;
    }
}
