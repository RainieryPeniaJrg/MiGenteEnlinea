namespace MiGenteEnLinea.Infrastructure.Services;

/// <summary>
/// Configuración SMTP para envío de emails (desde appsettings.json)
/// </summary>
public class EmailSettings
{
    public const string SectionName = "EmailSettings";

    /// <summary>
    /// Servidor SMTP (ej: smtp.gmail.com, smtp.sendgrid.net)
    /// </summary>
    public string SmtpServer { get; set; } = string.Empty;

    /// <summary>
    /// Puerto SMTP (587 para TLS, 465 para SSL, 25 sin encriptación)
    /// </summary>
    public int SmtpPort { get; set; } = 587;

    /// <summary>
    /// Usuario SMTP (email completo para Gmail)
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña SMTP (App Password para Gmail, API Key para SendGrid)
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Email del remitente (ej: noreply@migenteenlinea.com)
    /// </summary>
    public string FromEmail { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del remitente mostrado en el email
    /// </summary>
    public string FromName { get; set; } = "MiGente En Línea";

    /// <summary>
    /// Habilitar SSL/TLS (recomendado: true)
    /// </summary>
    public bool EnableSsl { get; set; } = true;

    /// <summary>
    /// Timeout de conexión SMTP en milisegundos (default: 30 segundos)
    /// </summary>
    public int Timeout { get; set; } = 30000;

    /// <summary>
    /// Número máximo de intentos de reenvío si falla
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Delay entre reintentos en milisegundos (exponential backoff)
    /// </summary>
    public int RetryDelayMilliseconds { get; set; } = 2000;

    /// <summary>
    /// Validar configuración al inicializar
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(SmtpServer))
            throw new InvalidOperationException("SmtpServer no configurado en appsettings.json");

        if (SmtpPort <= 0 || SmtpPort > 65535)
            throw new InvalidOperationException($"SmtpPort inválido: {SmtpPort}");

        if (string.IsNullOrWhiteSpace(Username))
            throw new InvalidOperationException("Username SMTP no configurado");

        if (string.IsNullOrWhiteSpace(Password))
            throw new InvalidOperationException("Password SMTP no configurado");

        if (string.IsNullOrWhiteSpace(FromEmail))
            throw new InvalidOperationException("FromEmail no configurado");

        if (!FromEmail.Contains('@'))
            throw new InvalidOperationException($"FromEmail inválido: {FromEmail}");
    }
}
