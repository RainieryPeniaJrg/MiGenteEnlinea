# 📧 PLAN DE EJECUCIÓN 1: EMAIL SERVICE IMPLEMENTATION

**Prioridad:** 🔴 **CRÍTICA - BLOQUEANTE**  
**Esfuerzo Estimado:** 1 día (6-8 horas)  
**Estado:** ⏳ PENDIENTE  
**Bloqueante Para:** RegisterCommand, ActivateAccountCommand, PasswordResetCommand (futuro)

---

## 🎯 OBJETIVO

Implementar servicio completo de envío de emails para desbloquear funcionalidad existente que está comentada en el código. Actualmente `RegisterCommand` NO puede enviar emails de activación de cuenta.

---

## 📊 ANÁLISIS DE SITUACIÓN ACTUAL

### ❌ Problema Identificado

**Ubicación:** `Application/Features/Authentication/Commands/Register/RegisterCommandHandler.cs` línea 58

```csharp
// BLOQUEADO: Este código NO funciona porque _emailService no está registrado
await _emailService.SendActivationEmailAsync(
    credencial.Email,
    perfil.UsuarioId,
    activationToken: perfil.UsuarioId // Simplificado por ahora
);
```

**Estado en DependencyInjection.cs línea 143:**
```csharp
// TODO: Agregar cuando se migren del legacy
// services.AddScoped<IEmailService, EmailService>(); ← COMENTADO!
```

### ✅ Lo Que Ya Existe

1. ✅ Interface `IEmailService` mencionada en código
2. ✅ Configuración Email en `appsettings.json` (incompleta)
3. ✅ Referencia en múltiples Commands (Register, etc.)

### ❌ Lo Que Falta

1. ❌ Interface `IEmailService` completa (en Application/Common/Interfaces/)
2. ❌ Clase `EmailService` (en Infrastructure/Services/)
3. ❌ Clase `EmailSettings` (en Infrastructure/Options/)
4. ❌ Templates HTML de emails (4 templates)
5. ❌ Registro en DI
6. ❌ NuGet package (MailKit)

---

## 📋 PLAN DE IMPLEMENTACIÓN

### ⏱️ FASE 1: Análisis y Configuración (30 min)

**Objetivo:** Preparar configuración y estructura base

#### Paso 1.1: Leer Legacy EmailService (15 min)

**Acción:**
```bash
# Leer servicio Legacy para entender configuración
```

**Archivos a Analizar:**
- `Codigo Fuente Mi Gente/MiGente_Front/Services/EmailService.cs`
- `Codigo Fuente Mi Gente/MiGente_Front/Services/EmailSender.cs`
- Legacy `Web.config` (configuración SMTP)
- Legacy `MailTemplates/` (templates existentes)

**Información a Extraer:**
- Configuración SMTP actual
- Estructura de emails existentes
- Métodos de envío utilizados
- Templates HTML que ya funcionan

#### Paso 1.2: Instalar NuGet Package (5 min)

**Comando:**
```powershell
cd src/Infrastructure/MiGenteEnLinea.Infrastructure
dotnet add package MailKit --version 4.3.0
dotnet add package MimeKit --version 4.3.0
```

**Verificar:**
```powershell
dotnet build --no-restore
# Debe compilar sin errores
```

#### Paso 1.3: Actualizar appsettings.json (10 min)

**Ubicación:** `src/Presentation/MiGenteEnLinea.API/appsettings.json`

**Cambio:**
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "noreply@migenteonline.com",
    "Password": "USE_USER_SECRETS_IN_DEV",
    "FromName": "MiGente En Línea",
    "FromEmail": "noreply@migenteonline.com",
    "EnableSsl": true,
    "DefaultTimeout": 30,
    "RetryCount": 3
  }
}
```

**Configurar User Secrets (desarrollo):**
```powershell
cd src/Presentation/MiGenteEnLinea.API
dotnet user-secrets set "EmailSettings:Username" "tu_email@gmail.com"
dotnet user-secrets set "EmailSettings:Password" "tu_app_password"
```

**Nota:** Para Gmail, usar "App Password" (no contraseña normal):
1. Ir a Google Account → Security
2. Enable 2-Step Verification
3. Generate App Password para "Mail"

---

### ⏱️ FASE 2: Implementación de Clases Core (2 horas)

#### Paso 2.1: Crear IEmailService Interface (15 min)

**Ubicación:** `src/Core/MiGenteEnLinea.Application/Common/Interfaces/IEmailService.cs`

**Crear archivo:**
```csharp
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Servicio para envío de correos electrónicos
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Envía email de activación de cuenta con link único
    /// </summary>
    /// <param name="toEmail">Email del destinatario</param>
    /// <param name="userId">ID del usuario (para generar link)</param>
    /// <param name="activationToken">Token de activación único</param>
    Task SendActivationEmailAsync(string toEmail, string userId, string activationToken);

    /// <summary>
    /// Envía email de bienvenida después de activar cuenta
    /// </summary>
    /// <param name="toEmail">Email del destinatario</param>
    /// <param name="nombreCompleto">Nombre completo del usuario</param>
    /// <param name="tipoUsuario">Tipo: "Empleador" o "Contratista"</param>
    Task SendWelcomeEmailAsync(string toEmail, string nombreCompleto, string tipoUsuario);

    /// <summary>
    /// Envía email de reseteo de contraseña (FUTURO)
    /// </summary>
    Task SendPasswordResetEmailAsync(string toEmail, string resetToken);

    /// <summary>
    /// Envía email de confirmación de pago
    /// </summary>
    /// <param name="toEmail">Email del destinatario</param>
    /// <param name="ventaId">ID de la venta</param>
    /// <param name="monto">Monto pagado</param>
    /// <param name="planNombre">Nombre del plan adquirido</param>
    Task SendPaymentConfirmationEmailAsync(
        string toEmail, 
        int ventaId, 
        decimal monto, 
        string planNombre);

    /// <summary>
    /// Envía email genérico con HTML personalizado
    /// </summary>
    Task SendEmailAsync(
        string toEmail, 
        string subject, 
        string htmlBody, 
        string? plainTextBody = null);
}
```

#### Paso 2.2: Crear EmailSettings Class (10 min)

**Ubicación:** `src/Infrastructure/MiGenteEnLinea.Infrastructure/Options/EmailSettings.cs`

**Crear archivo:**
```csharp
namespace MiGenteEnLinea.Infrastructure.Options;

/// <summary>
/// Configuración del servicio de email (SMTP)
/// </summary>
public class EmailSettings
{
    public const string SectionName = "EmailSettings";

    /// <summary>
    /// Servidor SMTP (ej: smtp.gmail.com)
    /// </summary>
    public string SmtpServer { get; set; } = string.Empty;

    /// <summary>
    /// Puerto SMTP (587 para TLS, 465 para SSL)
    /// </summary>
    public int SmtpPort { get; set; } = 587;

    /// <summary>
    /// Usuario de autenticación SMTP
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña de autenticación SMTP
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del remitente (aparece en el email)
    /// </summary>
    public string FromName { get; set; } = "MiGente En Línea";

    /// <summary>
    /// Email del remitente
    /// </summary>
    public string FromEmail { get; set; } = string.Empty;

    /// <summary>
    /// Usar SSL/TLS
    /// </summary>
    public bool EnableSsl { get; set; } = true;

    /// <summary>
    /// Timeout en segundos para conexión SMTP
    /// </summary>
    public int DefaultTimeout { get; set; } = 30;

    /// <summary>
    /// Número de reintentos en caso de fallo
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Validar configuración
    /// </summary>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(SmtpServer) &&
               SmtpPort > 0 &&
               !string.IsNullOrWhiteSpace(Username) &&
               !string.IsNullOrWhiteSpace(Password) &&
               !string.IsNullOrWhiteSpace(FromEmail);
    }
}
```

#### Paso 2.3: Crear EmailService Implementation (1.5 horas)

**Ubicación:** `src/Infrastructure/MiGenteEnLinea.Infrastructure/Services/EmailService.cs`

**Crear archivo completo:**
```csharp
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Infrastructure.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Infrastructure.Services;

/// <summary>
/// Servicio de envío de emails usando MailKit/SMTP
/// </summary>
public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;
    private readonly string _templatesBasePath;

    public EmailService(
        IOptions<EmailSettings> settings,
        ILogger<EmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        // Validar configuración
        if (!_settings.IsValid())
        {
            _logger.LogError("EmailSettings inválido. Revisar configuración en appsettings.json");
            throw new InvalidOperationException("EmailSettings no está configurado correctamente");
        }

        // Path a templates (ajustar según estructura de proyecto)
        _templatesBasePath = Path.Combine(AppContext.BaseDirectory, "Templates", "Email");
    }

    /// <inheritdoc/>
    public async Task SendActivationEmailAsync(string toEmail, string userId, string activationToken)
    {
        _logger.LogInformation("Enviando email de activación a: {Email}", toEmail);

        var activationUrl = $"https://migenteonline.com/activar?userId={userId}&token={activationToken}";
        
        var htmlBody = GetActivationEmailTemplate(activationUrl);
        var plainText = $"Activa tu cuenta visitando: {activationUrl}";

        await SendEmailAsync(
            toEmail,
            "Activa tu cuenta - MiGente En Línea",
            htmlBody,
            plainText);

        _logger.LogInformation("Email de activación enviado exitosamente a: {Email}", toEmail);
    }

    /// <inheritdoc/>
    public async Task SendWelcomeEmailAsync(string toEmail, string nombreCompleto, string tipoUsuario)
    {
        _logger.LogInformation("Enviando email de bienvenida a: {Email}", toEmail);

        var htmlBody = GetWelcomeEmailTemplate(nombreCompleto, tipoUsuario);
        var plainText = $"¡Bienvenido {nombreCompleto}! Tu cuenta de {tipoUsuario} ha sido activada.";

        await SendEmailAsync(
            toEmail,
            "¡Bienvenido a MiGente En Línea!",
            htmlBody,
            plainText);

        _logger.LogInformation("Email de bienvenida enviado exitosamente a: {Email}", toEmail);
    }

    /// <inheritdoc/>
    public async Task SendPasswordResetEmailAsync(string toEmail, string resetToken)
    {
        _logger.LogInformation("Enviando email de reseteo de contraseña a: {Email}", toEmail);

        var resetUrl = $"https://migenteonline.com/reset-password?token={resetToken}";
        
        var htmlBody = GetPasswordResetEmailTemplate(resetUrl);
        var plainText = $"Resetea tu contraseña visitando: {resetUrl}";

        await SendEmailAsync(
            toEmail,
            "Reseteo de contraseña - MiGente En Línea",
            htmlBody,
            plainText);

        _logger.LogInformation("Email de reseteo enviado exitosamente a: {Email}", toEmail);
    }

    /// <inheritdoc/>
    public async Task SendPaymentConfirmationEmailAsync(
        string toEmail, 
        int ventaId, 
        decimal monto, 
        string planNombre)
    {
        _logger.LogInformation("Enviando confirmación de pago a: {Email}", toEmail);

        var htmlBody = GetPaymentConfirmationTemplate(ventaId, monto, planNombre);
        var plainText = $"Tu pago de RD${monto:N2} para el plan {planNombre} ha sido procesado. ID: {ventaId}";

        await SendEmailAsync(
            toEmail,
            $"Confirmación de Pago - Venta #{ventaId}",
            htmlBody,
            plainText);

        _logger.LogInformation("Email de confirmación enviado exitosamente a: {Email}", toEmail);
    }

    /// <inheritdoc/>
    public async Task SendEmailAsync(
        string toEmail, 
        string subject, 
        string htmlBody, 
        string? plainTextBody = null)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
            throw new ArgumentException("Email destinatario es requerido", nameof(toEmail));

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject es requerido", nameof(subject));

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlBody,
            TextBody = plainTextBody ?? StripHtml(htmlBody)
        };

        message.Body = bodyBuilder.ToMessageBody();

        // Enviar con reintentos
        await SendWithRetryAsync(message);
    }

    /// <summary>
    /// Envía email con política de reintentos
    /// </summary>
    private async Task SendWithRetryAsync(MimeMessage message)
    {
        var attempt = 0;
        var maxAttempts = _settings.RetryCount;

        while (attempt < maxAttempts)
        {
            attempt++;

            try
            {
                using var client = new SmtpClient();
                
                // Timeout de conexión
                client.Timeout = _settings.DefaultTimeout * 1000;

                // Conectar al servidor SMTP
                await client.ConnectAsync(
                    _settings.SmtpServer, 
                    _settings.SmtpPort, 
                    _settings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);

                // Autenticar
                await client.AuthenticateAsync(_settings.Username, _settings.Password);

                // Enviar
                await client.SendAsync(message);

                // Desconectar
                await client.DisconnectAsync(true);

                _logger.LogDebug("Email enviado exitosamente en intento {Attempt}", attempt);
                return; // Éxito!
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                _logger.LogWarning(
                    ex, 
                    "Error al enviar email (intento {Attempt}/{MaxAttempts}): {Message}", 
                    attempt, 
                    maxAttempts, 
                    ex.Message);

                // Esperar antes de reintentar (exponential backoff)
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
            }
            catch (Exception ex)
            {
                // Último intento falló
                _logger.LogError(
                    ex, 
                    "Error FATAL al enviar email después de {MaxAttempts} intentos", 
                    maxAttempts);

                throw new InvalidOperationException(
                    $"No se pudo enviar email después de {maxAttempts} intentos", 
                    ex);
            }
        }
    }

    /// <summary>
    /// Remueve tags HTML para generar plain text
    /// </summary>
    private static string StripHtml(string html)
    {
        return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
    }

    // ========================================
    // TEMPLATES HTML
    // ========================================

    private string GetActivationEmailTemplate(string activationUrl)
    {
        return $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Activa tu cuenta</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #0066cc; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
        .content {{ background-color: #f9f9f9; padding: 30px; border: 1px solid #ddd; }}
        .button {{ display: inline-block; padding: 12px 30px; background-color: #0066cc; color: white; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #777; }}
    </style>
</head>
<body>
    <div class=""header"">
        <h1>¡Bienvenido a MiGente En Línea!</h1>
    </div>
    <div class=""content"">
        <h2>Activa tu cuenta</h2>
        <p>Gracias por registrarte en MiGente En Línea. Para completar tu registro, por favor activa tu cuenta haciendo clic en el botón de abajo:</p>
        
        <div style=""text-align: center;"">
            <a href=""{activationUrl}"" class=""button"">Activar Mi Cuenta</a>
        </div>

        <p>O copia y pega este enlace en tu navegador:</p>
        <p style=""word-break: break-all; color: #0066cc;"">{activationUrl}</p>

        <p><strong>Este enlace expira en 24 horas.</strong></p>
    </div>
    <div class=""footer"">
        <p>Si no creaste esta cuenta, puedes ignorar este email.</p>
        <p>&copy; {DateTime.Now.Year} MiGente En Línea. Todos los derechos reservados.</p>
    </div>
</body>
</html>";
    }

    private string GetWelcomeEmailTemplate(string nombreCompleto, string tipoUsuario)
    {
        return $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>¡Bienvenido!</title>
</head>
<body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;"">
    <div style=""background-color: #28a745; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0;"">
        <h1>¡Cuenta Activada!</h1>
    </div>
    <div style=""background-color: #f9f9f9; padding: 30px; border: 1px solid #ddd;"">
        <h2>¡Hola {nombreCompleto}!</h2>
        <p>Tu cuenta de <strong>{tipoUsuario}</strong> ha sido activada exitosamente.</p>
        
        <p>Ya puedes iniciar sesión y comenzar a usar todos los servicios de MiGente En Línea:</p>
        
        <ul>
            <li>Gestión de empleados</li>
            <li>Procesamiento de nómina</li>
            <li>Reportes y estadísticas</li>
            <li>Y mucho más...</li>
        </ul>

        <div style=""text-align: center; margin: 20px 0;"">
            <a href=""https://migenteonline.com/login"" style=""display: inline-block; padding: 12px 30px; background-color: #28a745; color: white; text-decoration: none; border-radius: 5px;"">Iniciar Sesión</a>
        </div>
    </div>
    <div style=""text-align: center; padding: 20px; font-size: 12px; color: #777;"">
        <p>&copy; {DateTime.Now.Year} MiGente En Línea. Todos los derechos reservados.</p>
    </div>
</body>
</html>";
    }

    private string GetPasswordResetEmailTemplate(string resetUrl)
    {
        return $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <title>Reseteo de Contraseña</title>
</head>
<body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;"">
    <div style=""background-color: #ffc107; color: #333; padding: 20px; text-align: center; border-radius: 5px 5px 0 0;"">
        <h1>Reseteo de Contraseña</h1>
    </div>
    <div style=""background-color: #f9f9f9; padding: 30px; border: 1px solid #ddd;"">
        <p>Has solicitado resetear tu contraseña en MiGente En Línea.</p>
        
        <p>Haz clic en el botón de abajo para crear una nueva contraseña:</p>

        <div style=""text-align: center; margin: 20px 0;"">
            <a href=""{resetUrl}"" style=""display: inline-block; padding: 12px 30px; background-color: #ffc107; color: #333; text-decoration: none; border-radius: 5px; font-weight: bold;"">Resetear Contraseña</a>
        </div>

        <p>O copia y pega este enlace:</p>
        <p style=""word-break: break-all; color: #0066cc;"">{resetUrl}</p>

        <p><strong>Este enlace expira en 1 hora.</strong></p>

        <p style=""color: #dc3545;""><strong>⚠️ Si no solicitaste este cambio, ignora este email y tu contraseña permanecerá sin cambios.</strong></p>
    </div>
    <div style=""text-align: center; padding: 20px; font-size: 12px; color: #777;"">
        <p>&copy; {DateTime.Now.Year} MiGente En Línea. Todos los derechos reservados.</p>
    </div>
</body>
</html>";
    }

    private string GetPaymentConfirmationTemplate(int ventaId, decimal monto, string planNombre)
    {
        return $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <title>Confirmación de Pago</title>
</head>
<body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;"">
    <div style=""background-color: #28a745; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0;"">
        <h1>✓ Pago Exitoso</h1>
    </div>
    <div style=""background-color: #f9f9f9; padding: 30px; border: 1px solid #ddd;"">
        <h2>Confirmación de Pago</h2>
        <p>Tu pago ha sido procesado exitosamente.</p>
        
        <table style=""width: 100%; border-collapse: collapse; margin: 20px 0;"">
            <tr style=""border-bottom: 1px solid #ddd;"">
                <td style=""padding: 10px; font-weight: bold;"">ID de Venta:</td>
                <td style=""padding: 10px;"">{ventaId}</td>
            </tr>
            <tr style=""border-bottom: 1px solid #ddd;"">
                <td style=""padding: 10px; font-weight: bold;"">Plan:</td>
                <td style=""padding: 10px;"">{planNombre}</td>
            </tr>
            <tr style=""border-bottom: 1px solid #ddd;"">
                <td style=""padding: 10px; font-weight: bold;"">Monto:</td>
                <td style=""padding: 10px; color: #28a745; font-size: 18px; font-weight: bold;"">RD${monto:N2}</td>
            </tr>
            <tr>
                <td style=""padding: 10px; font-weight: bold;"">Fecha:</td>
                <td style=""padding: 10px;"">{DateTime.Now:dd/MM/yyyy HH:mm}</td>
            </tr>
        </table>

        <p>Tu suscripción ha sido activada y ya puedes disfrutar de todos los beneficios.</p>

        <div style=""text-align: center; margin: 20px 0;"">
            <a href=""https://migenteonline.com/mi-suscripcion"" style=""display: inline-block; padding: 12px 30px; background-color: #0066cc; color: white; text-decoration: none; border-radius: 5px;"">Ver Mi Suscripción</a>
        </div>
    </div>
    <div style=""text-align: center; padding: 20px; font-size: 12px; color: #777;"">
        <p>Guarda este email como comprobante de pago.</p>
        <p>&copy; {DateTime.Now.Year} MiGente En Línea. Todos los derechos reservados.</p>
    </div>
</body>
</html>";
    }
}
```

---

### ⏱️ FASE 3: Registro en Dependency Injection (15 min)

#### Paso 3.1: Actualizar DependencyInjection.cs

**Ubicación:** `src/Infrastructure/MiGenteEnLinea.Infrastructure/DependencyInjection.cs` línea ~143

**Cambio (DESCOMENTAR y configurar):**

```csharp
// ========================================
// EMAIL SERVICE
// ========================================

// Configuración de Email
services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));

// Validar configuración al startup
services.AddOptions<EmailSettings>()
    .Bind(configuration.GetSection(EmailSettings.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Registrar servicio de email
services.AddScoped<IEmailService, EmailService>();

_logger.LogInformation("EmailService registrado exitosamente");
```

---

### ⏱️ FASE 4: Testing y Validación (2 horas)

#### Paso 4.1: Compilar y Verificar (10 min)

```powershell
cd MiGenteEnLinea.Clean
dotnet build --no-restore

# Debe compilar sin errores
# Expected output: Build succeeded. 0 Error(s)
```

#### Paso 4.2: Crear Unit Test para EmailService (1 hora)

**Ubicación:** `tests/MiGenteEnLinea.Infrastructure.Tests/Services/EmailServiceTests.cs`

**Crear archivo:**
```csharp
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MiGenteEnLinea.Infrastructure.Services;
using MiGenteEnLinea.Infrastructure.Options;

namespace MiGenteEnLinea.Infrastructure.Tests.Services;

public class EmailServiceTests
{
    private readonly Mock<ILogger<EmailService>> _loggerMock;
    private readonly EmailSettings _validSettings;

    public EmailServiceTests()
    {
        _loggerMock = new Mock<ILogger<EmailService>>();
        
        _validSettings = new EmailSettings
        {
            SmtpServer = "smtp.gmail.com",
            SmtpPort = 587,
            Username = "test@gmail.com",
            Password = "test_password",
            FromName = "Test",
            FromEmail = "test@gmail.com",
            EnableSsl = true,
            DefaultTimeout = 30,
            RetryCount = 3
        };
    }

    [Fact]
    public void Constructor_WithInvalidSettings_ThrowsException()
    {
        // Arrange
        var invalidSettings = new EmailSettings(); // Vacío = inválido
        var options = Options.Create(invalidSettings);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            new EmailService(options, _loggerMock.Object));
    }

    [Fact]
    public void Constructor_WithValidSettings_CreatesInstance()
    {
        // Arrange
        var options = Options.Create(_validSettings);

        // Act
        var service = new EmailService(options, _loggerMock.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task SendActivationEmailAsync_WithNullEmail_ThrowsArgumentException()
    {
        // Arrange
        var options = Options.Create(_validSettings);
        var service = new EmailService(options, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await service.SendActivationEmailAsync(null!, "userId123", "token123"));
    }

    // TODO: Agregar más tests cuando tengamos servidor SMTP de prueba
    // - Test de envío real con MailHog (servidor SMTP local para testing)
    // - Test de reintentos
    // - Test de templates HTML
}
```

#### Paso 4.3: Testing Manual con Swagger UI (30 min)

**Escenario 1: Probar RegisterCommand end-to-end**

```bash
# 1. Ejecutar API
cd src/Presentation/MiGenteEnLinea.API
dotnet run

# 2. Abrir Swagger UI
# http://localhost:5015/swagger

# 3. Ejecutar POST /api/auth/register
{
  "email": "test_nuevo@gmail.com",
  "password": "TestPassword123!",
  "nombre": "Test",
  "apellido": "Usuario",
  "tipoUsuario": "Empleador"
}

# 4. Verificar:
# - ✅ Response 201 Created
# - ✅ Logs muestran "Email de activación enviado"
# - ✅ Email llegó a bandeja de entrada
# - ✅ Link de activación funciona
```

**Escenario 2: Probar con email inválido (debe fallar gracefully)**

```json
POST /api/auth/register
{
  "email": "email_invalido_sin_arroba.com",
  "password": "Test123!",
  "nombre": "Test",
  "apellido": "Test",
  "tipoUsuario": "Empleador"
}

// Expected: 400 Bad Request con mensaje de validación
```

#### Paso 4.4: Testing de Configuración SMTP (20 min)

**Opción A: Usar Mailtrap.io (RECOMENDADO para desarrollo)**

1. Crear cuenta gratuita en https://mailtrap.io
2. Obtener credenciales SMTP
3. Actualizar `appsettings.Development.json`:

```json
{
  "EmailSettings": {
    "SmtpServer": "sandbox.smtp.mailtrap.io",
    "SmtpPort": 2525,
    "Username": "tu_mailtrap_username",
    "Password": "tu_mailtrap_password",
    "FromName": "MiGente Dev",
    "FromEmail": "dev@migente.local",
    "EnableSsl": true
  }
}
```

4. Ejecutar tests → Emails aparecerán en Mailtrap inbox (no se envían realmente)

**Opción B: Usar Gmail (para producción)**

Ver instrucciones en Fase 1, Paso 1.3

---

## ✅ CHECKLIST DE COMPLETADO

### Fase 1: Análisis y Configuración

- [ ] Leer Legacy EmailService.cs y EmailSender.cs
- [ ] Instalar MailKit y MimeKit (NuGet)
- [ ] Actualizar appsettings.json con EmailSettings
- [ ] Configurar User Secrets para desarrollo
- [ ] Compilar sin errores

### Fase 2: Implementación

- [ ] Crear IEmailService interface (5 métodos)
- [ ] Crear EmailSettings class
- [ ] Crear EmailService implementation (~450 líneas)
- [ ] Implementar 4 templates HTML inline

### Fase 3: Registro DI

- [ ] Descomentar y configurar registro en DependencyInjection.cs
- [ ] Verificar compilación exitosa

### Fase 4: Testing

- [ ] Crear EmailServiceTests (unit tests)
- [ ] Probar RegisterCommand con Swagger UI
- [ ] Verificar email llegue a bandeja de entrada
- [ ] Probar link de activación funciona
- [ ] Testing con diferentes configuraciones SMTP

---

## 📊 MÉTRICAS DE ÉXITO

| Métrica | Objetivo | Cómo Verificar |
|---------|----------|----------------|
| **Compilación** | 0 errores | `dotnet build` exitoso |
| **Unit Tests** | 80%+ coverage | `dotnet test` todos pasan |
| **Email enviado** | < 5 segundos | Logs muestran tiempo de envío |
| **Reintentos** | 3 intentos máx | Logs muestran reintentos si falla |
| **RegisterCommand** | Funciona end-to-end | Usuario recibe email de activación |

---

## 🚨 TROUBLESHOOTING

### Problema 1: "Authentication failed"

**Causa:** Gmail bloqueando aplicaciones menos seguras

**Solución:**
1. Habilitar 2FA en cuenta Gmail
2. Generar App Password específico
3. Usar App Password en vez de contraseña normal

### Problema 2: "Connection timeout"

**Causa:** Firewall bloqueando puerto SMTP 587/465

**Solución:**
1. Verificar firewall permite conexiones salientes al puerto
2. Probar con puerto alternativo (587 vs 465)
3. Verificar `EnableSsl` está en `true`

### Problema 3: Emails no llegan

**Causa:** Emails marcados como spam

**Solución:**
1. Revisar carpeta de spam
2. Configurar SPF/DKIM en dominio (producción)
3. Usar servicio profesional (SendGrid, Mailgun)

---

## 📁 ARCHIVOS CREADOS (RESUMEN)

```
MiGenteEnLinea.Clean/
├── src/
│   ├── Core/
│   │   └── MiGenteEnLinea.Application/
│   │       └── Common/
│   │           └── Interfaces/
│   │               └── IEmailService.cs (✅ NUEVO - 50 líneas)
│   │
│   └── Infrastructure/
│       └── MiGenteEnLinea.Infrastructure/
│           ├── Options/
│           │   └── EmailSettings.cs (✅ NUEVO - 60 líneas)
│           │
│           ├── Services/
│           │   └── EmailService.cs (✅ NUEVO - 450 líneas)
│           │
│           └── DependencyInjection.cs (✏️ MODIFICADO - +10 líneas)
│
├── tests/
│   └── MiGenteEnLinea.Infrastructure.Tests/
│       └── Services/
│           └── EmailServiceTests.cs (✅ NUEVO - 80 líneas)
│
└── src/Presentation/MiGenteEnLinea.API/
    └── appsettings.json (✏️ MODIFICADO - +12 líneas)
```

**Total Archivos Nuevos:** 4 archivos (~640 líneas)  
**Total Modificados:** 2 archivos (+22 líneas)

---

## 🎯 PRÓXIMOS PASOS (POST-IMPLEMENTACIÓN)

1. ✅ **Implementar LOTE 6: Calificaciones** (siguiente prioridad alta)
2. ⏳ Agregar método `SendNominaReportEmailAsync()` (enviar recibos de pago)
3. ⏳ Crear templates HTML en archivos separados (en vez de inline)
4. ⏳ Migrar a SendGrid/Mailgun para producción (más confiable que SMTP)
5. ⏳ Agregar soporte para attachments (PDFs de recibos)
6. ⏳ Implementar email templates con Razor/Handlebars

---

**Elaborado por:** GitHub Copilot AI Agent  
**Fecha:** 2025-01-13  
**Versión:** 1.0  
**Última Actualización:** 2025-01-13  
**Estado:** ⏳ PENDIENTE DE EJECUCIÓN

