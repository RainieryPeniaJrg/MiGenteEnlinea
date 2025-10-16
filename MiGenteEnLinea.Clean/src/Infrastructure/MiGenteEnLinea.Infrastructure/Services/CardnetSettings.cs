namespace MiGenteEnLinea.Infrastructure.Services;

/// <summary>
/// Configuración del gateway de pago Cardnet.
/// Los valores se cargan desde appsettings.json (BaseUrl, IsTest) 
/// y User Secrets (MerchantId, TerminalId) para seguridad PCI compliance.
/// </summary>
public class CardnetSettings
{
    /// <summary>
    /// URL base de la API de Cardnet.
    /// Ejemplo: https://ecommerce.cardnet.com.do/api/payment/
    /// </summary>
    public string BaseUrl { get; set; } = null!;

    /// <summary>
    /// ID del comercio asignado por Cardnet.
    /// NUNCA commitear este valor - usar User Secrets en desarrollo.
    /// </summary>
    public string MerchantId { get; set; } = null!;

    /// <summary>
    /// ID de la terminal del comercio.
    /// NUNCA commitear este valor - usar User Secrets en desarrollo.
    /// </summary>
    public string TerminalId { get; set; } = null!;

    /// <summary>
    /// Indica si se usa el ambiente de prueba (sandbox).
    /// true = ambiente de testing, false = producción.
    /// </summary>
    public bool IsTest { get; set; } = true;
}
