namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Interfaz para el servicio de envío de emails
/// </summary>
/// <remarks>
/// Implementación en Infrastructure usando SMTP (Gmail/SendGrid)
/// </remarks>
public interface IEmailService
{
    /// <summary>
    /// Envía un email de bienvenida/registro con link de activación
    /// </summary>
    /// <param name="nombre">Nombre del usuario</param>
    /// <param name="email">Email destino</param>
    /// <param name="subject">Asunto del email</param>
    /// <param name="activationUrl">URL de activación de cuenta</param>
    /// <returns>Task</returns>
    Task SendWelcomeEmailAsync(string nombre, string email, string subject, string activationUrl);

    /// <summary>
    /// Envía un email de restablecimiento de contraseña
    /// </summary>
    /// <param name="nombre">Nombre del usuario</param>
    /// <param name="email">Email destino</param>
    /// <param name="resetUrl">URL para restablecer contraseña</param>
    /// <returns>Task</returns>
    Task SendPasswordResetEmailAsync(string nombre, string email, string resetUrl);

    /// <summary>
    /// Envía un email genérico
    /// </summary>
    /// <param name="to">Email destino</param>
    /// <param name="subject">Asunto</param>
    /// <param name="body">Cuerpo del mensaje (HTML)</param>
    /// <returns>Task</returns>
    Task SendEmailAsync(string to, string subject, string body);
}
