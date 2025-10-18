using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Servicio para envío de emails con plantillas HTML
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Enviar email de activación de cuenta
    /// </summary>
    /// <param name="toEmail">Email del destinatario</param>
    /// <param name="toName">Nombre del destinatario</param>
    /// <param name="activationUrl">URL de activación (con token)</param>
    Task SendActivationEmailAsync(string toEmail, string toName, string activationUrl);

    /// <summary>
    /// Enviar email de bienvenida después de activar cuenta
    /// </summary>
    /// <param name="toEmail">Email del destinatario</param>
    /// <param name="toName">Nombre del destinatario</param>
    /// <param name="userType">Tipo de usuario ("Empleador" o "Contratista")</param>
    Task SendWelcomeEmailAsync(string toEmail, string toName, string userType);

    /// <summary>
    /// Enviar email de recuperación de contraseña
    /// </summary>
    /// <param name="toEmail">Email del destinatario</param>
    /// <param name="toName">Nombre del destinatario</param>
    /// <param name="resetUrl">URL de reset (con token)</param>
    Task SendPasswordResetEmailAsync(string toEmail, string toName, string resetUrl);

    /// <summary>
    /// Enviar confirmación de pago de suscripción
    /// </summary>
    /// <param name="toEmail">Email del destinatario</param>
    /// <param name="toName">Nombre del destinatario</param>
    /// <param name="planName">Nombre del plan adquirido</param>
    /// <param name="amount">Monto pagado</param>
    /// <param name="transactionId">ID de transacción</param>
    Task SendPaymentConfirmationEmailAsync(
        string toEmail,
        string toName,
        string planName,
        decimal amount,
        string transactionId);

    /// <summary>
    /// Enviar notificación de contratación (nueva, aceptada, rechazada, etc.)
    /// </summary>
    /// <param name="toEmail">Email del destinatario</param>
    /// <param name="toName">Nombre del destinatario</param>
    /// <param name="contractTitle">Título de la contratación</param>
    /// <param name="status">Estado de la contratación</param>
    /// <param name="message">Mensaje adicional</param>
    Task SendContractNotificationEmailAsync(
        string toEmail,
        string toName,
        string contractTitle,
        string status,
        string message);

    /// <summary>
    /// Enviar email genérico (para casos no cubiertos por plantillas)
    /// </summary>
    /// <param name="toEmail">Email del destinatario</param>
    /// <param name="toName">Nombre del destinatario (opcional)</param>
    /// <param name="subject">Asunto del email</param>
    /// <param name="htmlBody">Cuerpo del email en HTML</param>
    /// <param name="plainTextBody">Cuerpo del email en texto plano (fallback)</param>
    Task SendEmailAsync(
        string toEmail,
        string? toName,
        string subject,
        string htmlBody,
        string? plainTextBody = null);
}

