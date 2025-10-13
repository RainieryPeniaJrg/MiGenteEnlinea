using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Application.Features.Authentication.Commands.ChangePassword;
using MiGenteEnLinea.Application.Features.Authentication.Commands.Login;
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
    /// Autenticar usuario con email y contraseña
    /// </summary>
    /// <param name="command">Credenciales de login</param>
    /// <returns>Resultado de autenticación con datos de sesión</returns>
    /// <response code="200">Login exitoso - Retorna datos del usuario y su plan</response>
    /// <response code="401">Credenciales inválidas o cuenta inactiva</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <remarks>
    /// Códigos de StatusCode en respuesta:
    /// - 2: Login exitoso
    /// - 0: Credenciales inválidas
    /// - -1: Cuenta inactiva
    /// 
    /// Sample request:
    /// 
    ///     POST /api/auth/login
    ///     {
    ///        "email": "usuario@example.com",
    ///        "password": "MiPassword123"
    ///     }
    /// 
    /// </remarks>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResult>> Login([FromBody] LoginCommand command)
    {
        _logger.LogInformation("POST /api/auth/login - Email: {Email}", command.Email);

        var result = await _mediator.Send(command);

        if (result.StatusCode == 0)
        {
            _logger.LogWarning("Login fallido - Credenciales inválidas para: {Email}", command.Email);
            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        if (result.StatusCode == -1)
        {
            _logger.LogWarning("Login fallido - Cuenta inactiva para: {Email}", command.Email);
            return Unauthorized(new { message = "Cuenta inactiva. Por favor active su cuenta." });
        }

        _logger.LogInformation("Login exitoso - UserId: {UserId}", result.UserId);
        return Ok(result);
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
}
