using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.RefreshToken;

/// <summary>
/// Handler para RefreshTokenCommand
/// </summary>
/// <remarks>
/// Delega la renovación de tokens a IIdentityService para mantener Clean Architecture.
/// 
/// IMPORTANTE: Token Rotation (seguridad)
/// - El refresh token viejo se revoca automáticamente
/// - Se crea un nuevo refresh token
/// - El refresh token viejo NO puede volver a usarse
/// - Si se detecta uso de token revocado → posible ataque → considerar revocar todos los tokens del usuario
/// </remarks>
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthenticationResultDto>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IIdentityService identityService,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<AuthenticationResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing refresh token request from IP: {IpAddress}", request.IpAddress);

        try
        {
            // Delegar renovación completa a IIdentityService
            // Incluye: validación, token rotation, audit logging
            var result = await _identityService.RefreshTokenAsync(
                refreshToken: request.RefreshToken,
                ipAddress: request.IpAddress
            );

            _logger.LogInformation("Refresh token successful for user: {UserId}", result.User.UserId);

            return result;
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Refresh token failed from IP {IpAddress}: {Message}", request.IpAddress, ex.Message);
            throw;
        }
    }
}
