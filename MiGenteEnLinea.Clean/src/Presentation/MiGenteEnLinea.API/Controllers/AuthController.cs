using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Application.Features.Authentication.Commands.ActivateAccount;
using MiGenteEnLinea.Application.Features.Authentication.Commands.ChangePassword;
using MiGenteEnLinea.Application.Features.Authentication.Commands.Login;
using MiGenteEnLinea.Application.Features.Authentication.Commands.RefreshToken;
using MiGenteEnLinea.Application.Features.Authentication.Commands.Register;
using MiGenteEnLinea.Application.Features.Authentication.Commands.RevokeToken;
using MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateProfile;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;
using MiGenteEnLinea.Application.Features.Authentication.Queries.GetCredenciales;
using MiGenteEnLinea.Application.Features.Authentication.Queries.GetPerfil;
using MiGenteEnLinea.Application.Features.Authentication.Queries.GetPerfilByEmail;
using MiGenteEnLinea.Application.Features.Authentication.Queries.ValidarCorreo;

namespace MiGenteEnLinea.API.Controllers;

/// <summary>
/// Controller para autenticación y gestión de usuarios
/// </summary>
/// <remarks>
/// Migrado desde: LoginService.asmx.cs y SuscripcionesService.cs
/// Implementa LOTE 1: AUTHENTICATION & USER MANAGEMENT
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Autenticar usuario con email y contraseña (JWT)
    /// </summary>
    /// <param name="command">Credenciales de login</param>
    /// <returns>Tokens JWT y datos del usuario</returns>
    /// <response code="200">Login exitoso - Retorna access token, refresh token y datos del usuario</response>
    /// <response code="401">Credenciales inválidas o cuenta inactiva</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/auth/login
    ///     {
    ///        "email": "usuario@example.com",
    ///        "password": "MiPassword123",
    ///        "ipAddress": "192.168.1.100"
    ///     }
    /// 
    /// Sample response:
    /// 
    ///     {
    ///        "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    ///        "refreshToken": "a1b2c3d4e5f6...",
    ///        "accessTokenExpires": "2025-01-15T12:30:00Z",
    ///        "refreshTokenExpires": "2025-01-22T11:15:00Z",
    ///        "user": {
    ///            "userId": "550e8400-e29b-41d4-a716-446655440000",
    ///            "email": "usuario@example.com",
    ///            "nombreCompleto": "Juan Pérez",
    ///            "tipo": "1",
    ///            "planId": 2,
    ///            "vencimientoPlan": "2025-12-31T00:00:00Z",
    ///            "roles": ["Empleador"]
    ///        }
    ///     }
    /// 
    /// IMPORTANTE:
    /// - El ipAddress se obtiene automáticamente del HttpContext si no se provee
    /// - El access token expira en 15 minutos
    /// - El refresh token expira en 7 días
    /// - Guardar el refresh token de forma segura para renovar el access token
    /// </remarks>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthenticationResultDto>> Login([FromBody] LoginCommand command)
    {
        _logger.LogInformation("POST /api/auth/login - Email: {Email}", command.Email);

        try
        {
            // Obtener IP del cliente si no se provee
            var ipAddress = string.IsNullOrEmpty(command.IpAddress)
                ? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
                : command.IpAddress;

            // Crear comando con IP
            var loginCommand = command with { IpAddress = ipAddress };

            var result = await _mediator.Send(loginCommand);

            _logger.LogInformation("Login exitoso - UserId: {UserId}", result.User.UserId);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Login fallido: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtener el perfil completo de un usuario
    /// </summary>
    /// <param name="userId">ID del usuario (GUID)</param>
    /// <returns>Datos del perfil del usuario</returns>
    /// <response code="200">Perfil encontrado</response>
    /// <response code="404">Usuario no encontrado</response>
    [HttpGet("perfil/{userId}")]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PerfilDto>> GetPerfil(string userId)
    {
        _logger.LogInformation("GET /api/auth/perfil/{UserId}", userId);

        var result = await _mediator.Send(new GetPerfilQuery(userId));

        if (result == null)
        {
            _logger.LogWarning("Perfil no encontrado para userId: {UserId}", userId);
            return NotFound(new { message = "Perfil no encontrado" });
        }

        return Ok(result);
    }

    /// <summary>
    /// Obtener perfil por email
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <returns>Datos del perfil</returns>
    /// <response code="200">Perfil encontrado</response>
    /// <response code="404">Usuario no encontrado</response>
    [HttpGet("perfil/email/{email}")]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PerfilDto>> GetPerfilByEmail(string email)
    {
        _logger.LogInformation("GET /api/auth/perfil/email/{Email}", email);

        var result = await _mediator.Send(new GetPerfilByEmailQuery(email));

        if (result == null)
        {
            _logger.LogWarning("Perfil no encontrado para email: {Email}", email);
            return NotFound(new { message = "Perfil no encontrado" });
        }

        return Ok(result);
    }

    /// <summary>
    /// Validar si un email ya existe en el sistema
    /// </summary>
    /// <param name="email">Email a validar</param>
    /// <returns>True si el email ya existe (NO disponible), false si está disponible</returns>
    /// <response code="200">Validación completada</response>
    /// <remarks>
    /// Retorna:
    /// - true: Email ya existe (NO disponible para registro)
    /// - false: Email disponible para registro
    /// </remarks>
    [HttpGet("validar-email/{email}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> ValidarCorreo(string email)
    {
        _logger.LogInformation("GET /api/auth/validar-email/{Email}", email);

        var existe = await _mediator.Send(new ValidarCorreoQuery(email));

        return Ok(new { email, existe, disponible = !existe });
    }

    /// <summary>
    /// Obtener todas las credenciales de un usuario
    /// </summary>
    /// <param name="userId">ID del usuario (GUID)</param>
    /// <returns>Lista de credenciales del usuario</returns>
    /// <response code="200">Credenciales encontradas</response>
    [HttpGet("credenciales/{userId}")]
    [ProducesResponseType(typeof(List<CredencialDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CredencialDto>>> GetCredenciales(string userId)
    {
        _logger.LogInformation("GET /api/auth/credenciales/{UserId}", userId);

        var result = await _mediator.Send(new GetCredencialesQuery(userId));

        return Ok(result);
    }

    /// <summary>
    /// Cambiar la contraseña de un usuario
    /// </summary>
    /// <param name="command">Datos para cambio de contraseña</param>
    /// <returns>Resultado de la operación</returns>
    /// <response code="200">Contraseña actualizada exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/auth/change-password
    ///     {
    ///        "email": "usuario@example.com",
    ///        "userId": "550e8400-e29b-41d4-a716-446655440000",
    ///        "newPassword": "NuevaPassword123"
    ///     }
    /// 
    /// </remarks>
    [HttpPost("change-password")]
    [ProducesResponseType(typeof(ChangePasswordResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChangePasswordResult>> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        _logger.LogInformation("POST /api/auth/change-password - Email: {Email}", command.Email);

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            _logger.LogWarning("Cambio de contraseña fallido: {Message}", result.Message);
            return NotFound(result);
        }

        _logger.LogInformation("Contraseña actualizada exitosamente para: {Email}", command.Email);
        return Ok(result);
    }

    /// <summary>
    /// Registrar nuevo usuario en el sistema
    /// </summary>
    /// <param name="command">Datos de registro</param>
    /// <returns>ID del perfil creado</returns>
    /// <response code="201">Usuario registrado exitosamente</response>
    /// <response code="400">Datos inválidos o email ya existe</response>
    /// <response code="500">Error al procesar el registro</response>
    /// <remarks>
    /// Réplica de SuscripcionesService.GuardarPerfil() del Legacy
    /// 
    /// Crea:
    /// - Perfile (Empleador o Contratista según tipo)
    /// - Credencial con contraseña encriptada (BCrypt)
    /// - Contratista (solo si tipo=2)
    /// - Envía email de activación
    /// 
    /// Sample request:
    /// 
    ///     POST /api/auth/register
    ///     {
    ///        "email": "nuevo@example.com",
    ///        "password": "Password123",
    ///        "nombre": "Juan",
    ///        "apellido": "Pérez",
    ///        "tipo": 1,
    ///        "telefono1": "809-555-1234",
    ///        "telefono2": null,
    ///        "usuario": "juanp"
    ///     }
    /// 
    /// Valores de tipo:
    /// - 1 = Empleador
    /// - 2 = Contratista
    /// </remarks>
    [HttpPost("register")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> Register([FromBody] RegisterCommand command)
    {
        _logger.LogInformation("POST /api/auth/register - Email: {Email}, Tipo: {Tipo}", command.Email, command.Tipo);

        try
        {
            var perfilId = await _mediator.Send(command);

            _logger.LogInformation("Usuario registrado exitosamente - PerfilId: {PerfilId}", perfilId);

            return CreatedAtAction(
                nameof(GetPerfil),
                new { userId = perfilId.ToString() },
                new { perfilId, message = "Usuario registrado exitosamente. Por favor revise su correo para activar su cuenta." });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Registro fallido: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Activar cuenta de usuario
    /// </summary>
    /// <param name="command">Datos de activación (UserId y Email)</param>
    /// <returns>Resultado de la activación</returns>
    /// <response code="200">Cuenta activada exitosamente</response>
    /// <response code="400">Datos inválidos o cuenta ya activa</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <remarks>
    /// Réplica de Activar.aspx.cs del Legacy
    /// 
    /// Sample request:
    /// 
    ///     POST /api/auth/activate
    ///     {
    ///        "userId": "550e8400-e29b-41d4-a716-446655440000",
    ///        "email": "usuario@example.com"
    ///     }
    /// 
    /// </remarks>
    [HttpPost("activate")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ActivateAccount([FromBody] ActivateAccountCommand command)
    {
        _logger.LogInformation("POST /api/auth/activate - UserId: {UserId}, Email: {Email}", command.UserId, command.Email);

        try
        {
            var success = await _mediator.Send(command);

            if (!success)
            {
                _logger.LogWarning("Activación fallida - Usuario no encontrado o ya activo: {UserId}", command.UserId);
                return BadRequest(new { message = "No se pudo activar la cuenta. La cuenta ya está activa o los datos son incorrectos." });
            }

            _logger.LogInformation("Cuenta activada exitosamente - UserId: {UserId}", command.UserId);
            return Ok(new { message = "Cuenta activada exitosamente. Ya puede iniciar sesión." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al activar cuenta - UserId: {UserId}", command.UserId);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Actualizar perfil de usuario
    /// </summary>
    /// <param name="userId">ID del usuario (GUID)</param>
    /// <param name="command">Datos a actualizar</param>
    /// <returns>Resultado de la actualización</returns>
    /// <response code="200">Perfil actualizado exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <remarks>
    /// Réplica de LoginService.actualizarPerfil() del Legacy
    /// 
    /// Sample request:
    /// 
    ///     PUT /api/auth/perfil/550e8400-e29b-41d4-a716-446655440000
    ///     {
    ///        "userId": "550e8400-e29b-41d4-a716-446655440000",
    ///        "nombre": "Juan Carlos",
    ///        "apellido": "Pérez González",
    ///        "email": "juan.perez@example.com",
    ///        "telefono1": "809-555-1234",
    ///        "telefono2": "809-555-5678",
    ///        "usuario": "juancp"
    ///     }
    /// 
    /// </remarks>
    [HttpPut("perfil/{userId}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateProfile(string userId, [FromBody] UpdateProfileCommand command)
    {
        if (userId != command.UserId)
        {
            return BadRequest(new { message = "El UserId del path no coincide con el del body" });
        }

        _logger.LogInformation("PUT /api/auth/perfil/{UserId} - Email: {Email}", userId, command.Email);

        try
        {
            var success = await _mediator.Send(command);

            if (!success)
            {
                _logger.LogWarning("Actualización de perfil fallida - Usuario no encontrado: {UserId}", userId);
                return NotFound(new { message = "Usuario no encontrado" });
            }

            _logger.LogInformation("Perfil actualizado exitosamente - UserId: {UserId}", userId);
            return Ok(new { message = "Perfil actualizado exitosamente" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Error de validación al actualizar perfil: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Renovar access token usando refresh token (Token Refresh)
    /// </summary>
    /// <param name="command">Refresh token actual</param>
    /// <returns>Nuevos tokens (access token + refresh token)</returns>
    /// <response code="200">Tokens renovados exitosamente</response>
    /// <response code="401">Refresh token inválido, expirado o revocado</response>
    /// <response code="400">Datos inválidos</response>
    /// <remarks>
    /// IMPORTANTE: Token Rotation (seguridad)
    /// - El refresh token viejo se revoca automáticamente
    /// - Se retorna un nuevo refresh token
    /// - Cada refresh token solo puede usarse UNA VEZ
    /// 
    /// USO:
    /// - Cuando el access token expira (15 minutos)
    /// - NO se requieren credenciales nuevamente
    /// - Experiencia de usuario fluida
    /// 
    /// Sample request:
    /// 
    ///     POST /api/auth/refresh
    ///     {
    ///        "refreshToken": "a1b2c3d4e5f6g7h8...",
    ///        "ipAddress": "192.168.1.100"
    ///     }
    /// 
    /// Sample response (mismo formato que Login):
    /// 
    ///     {
    ///        "accessToken": "eyJhbGciOiJIUzI1NiIs... (NUEVO)",
    ///        "refreshToken": "x9y8z7w6v5u4... (NUEVO)",
    ///        "accessTokenExpires": "2025-01-15T13:00:00Z",
    ///        "refreshTokenExpires": "2025-01-22T12:45:00Z",
    ///        "user": { ... }
    ///     }
    /// 
    /// </remarks>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthenticationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthenticationResultDto>> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        _logger.LogInformation("POST /api/auth/refresh - IP: {IpAddress}", command.IpAddress);

        try
        {
            // Obtener IP del cliente si no se provee
            var ipAddress = string.IsNullOrEmpty(command.IpAddress)
                ? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
                : command.IpAddress;

            // Crear comando con IP
            var refreshCommand = command with { IpAddress = ipAddress };

            var result = await _mediator.Send(refreshCommand);

            _logger.LogInformation("Refresh token exitoso - UserId: {UserId}", result.User.UserId);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Refresh token fallido: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Revocar refresh token (Logout)
    /// </summary>
    /// <param name="command">Refresh token a revocar</param>
    /// <returns>Resultado de la operación</returns>
    /// <response code="204">Token revocado exitosamente</response>
    /// <response code="401">Refresh token inválido</response>
    /// <response code="400">Datos inválidos</response>
    /// <remarks>
    /// USO:
    /// - Logout de usuario (invalida el refresh token)
    /// - Cambio de contraseña (revocar todos los tokens)
    /// - Revocación por admin (seguridad)
    /// 
    /// IMPORTANTE:
    /// - El refresh token revocado NO puede volver a usarse
    /// - El access token actual sigue válido hasta que expire (max 15 min)
    /// - Para logout inmediato, el cliente debe descartar el access token
    /// - La operación es idempotente (revocar token ya revocado no falla)
    /// 
    /// Sample request:
    /// 
    ///     POST /api/auth/revoke
    ///     {
    ///        "refreshToken": "a1b2c3d4e5f6g7h8...",
    ///        "ipAddress": "192.168.1.100",
    ///        "reason": "User logout"
    ///     }
    /// 
    /// </remarks>
    [HttpPost("revoke")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RevokeToken([FromBody] RevokeTokenCommand command)
    {
        _logger.LogInformation("POST /api/auth/revoke - IP: {IpAddress}", command.IpAddress);

        try
        {
            // Obtener IP del cliente si no se provee
            var ipAddress = string.IsNullOrEmpty(command.IpAddress)
                ? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
                : command.IpAddress;

            // Crear comando con IP
            var revokeCommand = command with { IpAddress = ipAddress };

            await _mediator.Send(revokeCommand);

            _logger.LogInformation("Refresh token revocado exitosamente");
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Revoke token fallido: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
    }
}
