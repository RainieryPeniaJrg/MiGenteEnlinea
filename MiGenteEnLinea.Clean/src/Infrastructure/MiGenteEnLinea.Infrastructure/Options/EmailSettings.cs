namespace MiGenteEnLinea.Infrastructure.Options;

/// <summary>
/// Configuración del servicio de email (SMTP).
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// Nombre del remitente que aparecerá en los emails.
    /// </summary>
    public string FromName { get; set; } = "MiGente En Línea";

    /// <summary>
    /// Dirección de email del remitente.
    /// </summary>
    public string FromEmail { get; set; } = string.Empty;

    /// <summary>
    /// Servidor SMTP (ej: smtp.gmail.com, smtp.office365.com).
    /// </summary>
    public string SmtpServer { get; set; } = string.Empty;

    /// <summary>
    /// Puerto SMTP (587 para TLS, 465 para SSL, 25 sin encriptación).
    /// </summary>
    public int SmtpPort { get; set; } = 587;

    /// <summary>
    /// Usar SSL/TLS para la conexión SMTP.
    /// </summary>
    public bool EnableSsl { get; set; } = true;

    /// <summary>
    /// Usuario para autenticación SMTP.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña para autenticación SMTP.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Número máximo de reintentos si falla el envío.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Delay inicial entre reintentos en milisegundos (exponential backoff).
    /// </summary>
    public int RetryDelayMilliseconds { get; set; } = 1000;

    /// <summary>
    /// Timeout para conexión SMTP en milisegundos.
    /// </summary>
    public int Timeout { get; set; } = 30000; // 30 segundos

    /// <summary>
    /// Validar configuración requerida.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(FromEmail))
            throw new InvalidOperationException("EmailSettings:FromEmail es requerido");

        if (string.IsNullOrWhiteSpace(SmtpServer))
            throw new InvalidOperationException("EmailSettings:SmtpServer es requerido");

        if (string.IsNullOrWhiteSpace(Username))
            throw new InvalidOperationException("EmailSettings:Username es requerido");

        if (string.IsNullOrWhiteSpace(Password))
            throw new InvalidOperationException("EmailSettings:Password es requerido");
    }
}
