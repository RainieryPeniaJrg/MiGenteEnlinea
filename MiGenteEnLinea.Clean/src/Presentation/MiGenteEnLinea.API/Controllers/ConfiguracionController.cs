using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Features.Configuracion.Queries.GetOpenAiConfig;

namespace MiGenteEnLinea.API.Controllers;

/// <summary>
/// Controller para configuración del sistema
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ConfiguracionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ConfiguracionController> _logger;

    public ConfiguracionController(
        IMediator mediator,
        ILogger<ConfiguracionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Method #49: Obtener configuración del bot OpenAI
    /// </summary>
    /// <remarks>
    /// ⚠️ **SECURITY WARNING:** Este endpoint expone API keys en la respuesta.
    /// 
    /// Migrado desde: BotServices.getOpenAI() - línea 11
    /// 
    /// **Legacy Code:**
    /// ```csharp
    /// public OpenAi_Config getOpenAI()
    /// {
    ///     using (var db = new migenteEntities())
    ///     {
    ///         return db.OpenAi_Config.FirstOrDefault();
    ///     }
    /// }
    /// ```
    /// 
    /// **Business Rules:**
    /// - Retorna la configuración activa del bot OpenAI
    /// - Usado por el "abogado virtual" en el frontend
    /// - Solo debe haber 1 registro en la tabla OpenAi_Config
    /// 
    /// **Security Concerns:**
    /// - Este endpoint expone API keys sensibles
    /// - DEBE ser protegido con autorización en producción
    /// - RECOMENDACIÓN: Mover configuración a Backend (appsettings.json o Key Vault)
    /// 
    /// **Response:**
    /// - 200 OK: Configuración encontrada (retorna OpenAiConfigDto)
    /// - 404 Not Found: No hay configuración en BD
    /// - 500 Internal Server Error: Error al procesar solicitud
    /// 
    /// **Use Cases:**
    /// - Inicialización del chat bot en frontend
    /// - Obtención de API key y URL para llamadas a OpenAI
    /// - DEPRECADO: Migrar a IOpenAiService en Infrastructure
    /// </remarks>
    /// <response code="200">Configuración encontrada</response>
    /// <response code="404">Configuración no encontrada</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet("openai")]
    [AllowAnonymous] // ⚠️ TEMPORAL - En producción debe ser [Authorize]
    [ProducesResponseType(typeof(OpenAiConfigDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOpenAiConfig()
    {
        try
        {
            _logger.LogWarning(
                "⚠️ SECURITY WARNING: GetOpenAiConfig endpoint called from IP: {IP}. " +
                "Este endpoint expone API keys. Considerar mover a Backend configuration.",
                HttpContext.Connection.RemoteIpAddress);

            var query = new GetOpenAiConfigQuery();
            var config = await _mediator.Send(query);

            if (config == null)
            {
                _logger.LogWarning("Configuración OpenAI no encontrada en base de datos");
                return NotFound(new 
                { 
                    message = "Configuración OpenAI no encontrada. " +
                             "Contacte al administrador del sistema." 
                });
            }

            return Ok(config);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración OpenAI");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = "Error al procesar la solicitud" });
        }
    }
}
