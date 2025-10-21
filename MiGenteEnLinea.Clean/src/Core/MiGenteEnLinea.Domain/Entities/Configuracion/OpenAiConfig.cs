namespace MiGenteEnLinea.Domain.Entities.Configuracion;

/// <summary>
/// Entidad OpenAiConfig - Configuración del bot OpenAI para el "abogado virtual"
/// 
/// Migrado desde: Legacy Entity OpenAi_Config (tabla: OpenAi_Config)
/// 
/// **SECURITY WARNING:**
/// Esta entidad contiene API keys y configuraciones sensibles.
/// 
/// **Recomendaciones de Seguridad:**
/// 1. Esta tabla debería eliminarse y moverse a appsettings.json
/// 2. Usar Azure Key Vault o similar para secretos
/// 3. Implementar IOpenAiService en Infrastructure Layer
/// 4. No exponer estos datos en endpoints públicos
/// 
/// **Actual Implementation:**
/// Por compatibilidad con Legacy, se mantiene en base de datos temporalmente.
/// El endpoint GET /api/configuracion/openai debe ser protegido con autorización.
/// </summary>
public class OpenAiConfig
{
    /// <summary>
    /// ID de configuración (PK)
    /// Legacy: id (int, PK)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// API Key de OpenAI (sensible)
    /// Legacy: OpenAIApiKey (string)
    /// ⚠️ SECURITY: Este campo contiene información sensible
    /// </summary>
    public string OpenAIApiKey { get; set; } = string.Empty;

    /// <summary>
    /// URL del API de OpenAI
    /// Legacy: OpenAIApiUrl (string)
    /// Ejemplo: "https://api.openai.com/v1"
    /// </summary>
    public string OpenAIApiUrl { get; set; } = string.Empty;
}
