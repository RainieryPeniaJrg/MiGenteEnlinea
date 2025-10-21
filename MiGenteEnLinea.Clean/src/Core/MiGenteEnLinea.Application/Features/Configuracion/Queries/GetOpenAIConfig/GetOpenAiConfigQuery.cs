using MediatR;

namespace MiGenteEnLinea.Application.Features.Configuracion.Queries.GetOpenAiConfig;

/// <summary>
/// Method #49: Query para obtener la configuración del bot OpenAI
/// </summary>
/// <remarks>
/// Migrado desde: BotServices.getOpenAI() - línea 11
/// 
/// **Legacy Code:**
/// <code>
/// public OpenAi_Config getOpenAI()
/// {
///     using (var db = new migenteEntities())
///     {
///         return db.OpenAi_Config.FirstOrDefault();
///     }
/// }
/// </code>
/// 
/// **Business Rules:**
/// - Retorna la configuración activa del bot OpenAI (API key, model, etc.)
/// - Usado por el "abogado virtual" en el frontend
/// - Solo debe haber 1 registro de configuración en la BD
/// 
/// **Decisión Arquitectural:**
/// OPCIÓN B (RECOMENDADA): Mover a Infrastructure Layer como IOpenAiService
/// - No exponer API keys directamente en endpoints públicos
/// - Configuración debe estar en appsettings.json o Key Vault
/// - Este endpoint es TEMPORAL para compatibilidad con Legacy
/// 
/// **Use Cases:**
/// - Inicialización del chat bot en frontend
/// - Obtención de modelo y configuración
/// - DEPRECADO: Migrar a configuración desde Backend
/// </remarks>
public record GetOpenAiConfigQuery : IRequest<OpenAiConfigDto?>;

/// <summary>
/// DTO para configuración de OpenAI
/// Paridad con Legacy: OpenAi_Config (tabla tiene solo id, OpenAIApiKey, OpenAIApiUrl)
/// </summary>
public record OpenAiConfigDto
{
    public int ConfigId { get; init; }
    
    /// <summary>
    /// API Key de OpenAI (sensible)
    /// ⚠️ SECURITY: Este campo expone información sensible
    /// </summary>
    public string? ApiKey { get; init; }
    
    /// <summary>
    /// URL del API de OpenAI
    /// Ejemplo: "https://api.openai.com/v1"
    /// </summary>
    public string? ApiUrl { get; init; }
}
