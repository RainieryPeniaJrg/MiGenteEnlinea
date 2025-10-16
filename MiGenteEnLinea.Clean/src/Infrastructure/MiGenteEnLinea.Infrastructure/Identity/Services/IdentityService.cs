using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Infrastructure.Identity.Services;

/// <summary>
/// Implementación del servicio de identidad usando ASP.NET Core Identity
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly MiGenteDbContext _context;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService,
        MiGenteDbContext context,
        ILogger<IdentityService> logger)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _context = context;
        _logger = logger;
    }

    public async Task<AuthenticationResultDto> LoginAsync(string email, string password, string ipAddress)
    {
        // 1. Buscar usuario por email
        var user = await _userManager.FindByEmailAsync(email);
        
        if (user == null)
        {
            _logger.LogWarning("Login failed: User not found with email {Email}", email);
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // 2. Verificar contraseña
        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        
        if (!passwordValid)
        {
            _logger.LogWarning("Login failed: Invalid password for user {UserId}", user.Id);
            
            // Registrar intento fallido (Identity maneja lockout automáticamente)
            await _userManager.AccessFailedAsync(user);
            
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // 3. Verificar si la cuenta está confirmada/activa
        if (!user.EmailConfirmed)
        {
            _logger.LogWarning("Login failed: Account not confirmed for user {UserId}", user.Id);
            throw new UnauthorizedAccessException("La cuenta no está activa. Por favor, verifica tu correo electrónico.");
        }

        // 4. Verificar si la cuenta está bloqueada
        if (await _userManager.IsLockedOutAsync(user))
        {
            _logger.LogWarning("Login failed: Account is locked out for user {UserId}", user.Id);
            throw new UnauthorizedAccessException("La cuenta está bloqueada debido a múltiples intentos fallidos. Intenta nuevamente más tarde.");
        }

        // 5. Obtener roles del usuario
        var roles = await _userManager.GetRolesAsync(user);

        // 6. Generar access token JWT
        var accessToken = _jwtTokenService.GenerateAccessToken(
            userId: user.Id,
            email: user.Email!,
            tipo: user.Tipo,
            nombreCompleto: user.NombreCompleto,
            planId: user.PlanID,
            roles: roles
        );

        // 7. Generar refresh token
        var refreshTokenData = _jwtTokenService.GenerateRefreshToken(ipAddress);

        // 8. Guardar refresh token en base de datos
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenData.Token,
            Expires = refreshTokenData.Expires,
            Created = DateTime.UtcNow,
            CreatedByIp = refreshTokenData.CreatedByIp
        };

        user.RefreshTokens.Add(refreshTokenEntity);

        // Actualizar último login
        user.UltimoLogin = DateTime.UtcNow;

        // Resetear contador de intentos fallidos
        await _userManager.ResetAccessFailedCountAsync(user);

        // Guardar cambios
        await _userManager.UpdateAsync(user);

        _logger.LogInformation("Login successful for user {UserId} ({Email}) from IP {IpAddress}", 
            user.Id, user.Email, ipAddress);

        // 9. Retornar resultado
        return new AuthenticationResultDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenData.Token,
            AccessTokenExpires = DateTime.UtcNow.AddMinutes(15), // TODO: Obtener de JwtSettings
            RefreshTokenExpires = refreshTokenData.Expires,
            User = new UserInfoDto
            {
                UserId = user.Id,
                Email = user.Email!,
                NombreCompleto = user.NombreCompleto,
                Tipo = user.Tipo,
                PlanId = user.PlanID,
                VencimientoPlan = user.VencimientoPlan,
                Roles = roles.ToList()
            }
        };
    }

    public async Task<AuthenticationResultDto> RefreshTokenAsync(string refreshToken, string ipAddress)
    {
        // 1. Buscar refresh token en base de datos
        var tokenEntity = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (tokenEntity == null)
        {
            _logger.LogWarning("Refresh token not found: {Token}", refreshToken);
            throw new UnauthorizedAccessException("Refresh token inválido");
        }

        // 2. Verificar si el token está activo
        if (!tokenEntity.IsActive)
        {
            _logger.LogWarning("Refresh token is not active: {Token}", refreshToken);
            throw new UnauthorizedAccessException("Refresh token expirado o revocado");
        }

        var user = tokenEntity.User;

        // 3. Generar nuevo access token
        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = _jwtTokenService.GenerateAccessToken(
            userId: user.Id,
            email: user.Email!,
            tipo: user.Tipo,
            nombreCompleto: user.NombreCompleto,
            planId: user.PlanID,
            roles: roles
        );

        // 4. Rotar refresh token (invalidar el viejo, crear uno nuevo)
        var newRefreshTokenData = _jwtTokenService.GenerateRefreshToken(ipAddress);

        // Marcar el token viejo como reemplazado
        tokenEntity.Revoked = DateTime.UtcNow;
        tokenEntity.RevokedByIp = ipAddress;
        tokenEntity.ReplacedByToken = newRefreshTokenData.Token;
        tokenEntity.ReasonRevoked = "Replaced by new token";

        // Crear nuevo refresh token
        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshTokenData.Token,
            Expires = newRefreshTokenData.Expires,
            Created = DateTime.UtcNow,
            CreatedByIp = newRefreshTokenData.CreatedByIp
        };

        user.RefreshTokens.Add(newRefreshTokenEntity);

        // Guardar cambios
        await _context.SaveChangesAsync();

        _logger.LogInformation("Refresh token rotated for user {UserId}", user.Id);

        return new AuthenticationResultDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshTokenData.Token,
            AccessTokenExpires = DateTime.UtcNow.AddMinutes(15),
            RefreshTokenExpires = newRefreshTokenData.Expires,
            User = new UserInfoDto
            {
                UserId = user.Id,
                Email = user.Email!,
                NombreCompleto = user.NombreCompleto,
                Tipo = user.Tipo,
                PlanId = user.PlanID,
                VencimientoPlan = user.VencimientoPlan,
                Roles = roles.ToList()
            }
        };
    }

    public async Task RevokeTokenAsync(string refreshToken, string ipAddress, string? reason = null)
    {
        var tokenEntity = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (tokenEntity == null)
        {
            _logger.LogWarning("Refresh token not found for revocation: {Token}", refreshToken);
            throw new UnauthorizedAccessException("Refresh token inválido");
        }

        if (!tokenEntity.IsActive)
        {
            _logger.LogWarning("Refresh token is already inactive: {Token}", refreshToken);
            return; // Ya está revocado, no hacer nada
        }

        // Revocar token
        tokenEntity.Revoked = DateTime.UtcNow;
        tokenEntity.RevokedByIp = ipAddress;
        tokenEntity.ReasonRevoked = reason ?? "User logout";

        await _context.SaveChangesAsync();

        _logger.LogInformation("Refresh token revoked for user {UserId}", tokenEntity.UserId);
    }

    public async Task<string> RegisterAsync(string email, string password, string nombreCompleto, string tipo)
    {
        // Verificar si el usuario ya existe
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("El email ya está registrado");
        }

        // Crear nuevo usuario
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = false, // Requiere confirmación
            NombreCompleto = nombreCompleto,
            Tipo = tipo,
            PlanID = 0, // Sin plan inicialmente
            FechaCreacion = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("User registration failed: {Errors}", errors);
            throw new InvalidOperationException($"Error al registrar usuario: {errors}");
        }

        _logger.LogInformation("User registered successfully: {Email}", email);

        return user.Id;
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null;
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new InvalidOperationException("Usuario no encontrado");
        }

        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        return result.Succeeded;
    }
}
