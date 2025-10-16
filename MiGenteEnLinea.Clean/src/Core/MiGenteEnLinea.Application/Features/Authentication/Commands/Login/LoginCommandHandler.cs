using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Login;

/// <summary>
/// Handler para el comando de login
/// </summary>
/// <remarks>
/// Delega la autenticación a IIdentityService para mantener Clean Architecture
/// </remarks>
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResultDto>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IIdentityService identityService,
        ILogger<LoginCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<AuthenticationResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing login request for email: {Email}", request.Email);

        // Delegar autenticación completa a IIdentityService
        var result = await _identityService.LoginAsync(
            email: request.Email,
            password: request.Password,
            ipAddress: request.IpAddress
        );

        _logger.LogInformation("Login successful for user: {UserId}", result.User.UserId);

        return result;
    }
}
