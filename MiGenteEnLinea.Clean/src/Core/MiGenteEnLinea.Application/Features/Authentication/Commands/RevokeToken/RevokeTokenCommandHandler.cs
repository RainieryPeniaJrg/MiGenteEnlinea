using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.RevokeToken;

/// <summary>
/// Handler para RevokeTokenCommand
/// </summary>
/// <remarks>
/// Delega la revocación de tokens a IIdentityService para mantener Clean Architecture.
/// 
/// COMPORTAMIENTO:
/// - Si el token existe y está activo → lo revoca
/// - Si el token existe y ya está revocado → idempotente (no falla)
/// - Si el token no existe → lanza UnauthorizedAccessException
/// </remarks>
public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, Unit>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<RevokeTokenCommandHandler> _logger;

    public RevokeTokenCommandHandler(
        IIdentityService identityService,
        ILogger<RevokeTokenCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing revoke token request from IP: {IpAddress}", request.IpAddress);

        try
        {
            // Delegar revocación completa a IIdentityService
            // Incluye: validación, update database, audit logging
            await _identityService.RevokeTokenAsync(
                refreshToken: request.RefreshToken,
                ipAddress: request.IpAddress,
                reason: request.Reason ?? "User logout"
            );

            _logger.LogInformation("Refresh token revoked successfully from IP: {IpAddress}", request.IpAddress);

            return Unit.Value;
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Revoke token failed from IP {IpAddress}: {Message}", request.IpAddress, ex.Message);
            throw;
        }
    }
}
