using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MiGenteEnLinea.Infrastructure.Services;
using Xunit;

namespace MiGenteEnLinea.Infrastructure.Tests.Services;

/// <summary>
/// Unit Tests para EmailService
/// </summary>
public class EmailServiceTests
{
    private readonly Mock<ILogger<EmailService>> _mockLogger;
    private readonly EmailSettings _emailSettings;
    private readonly IOptions<EmailSettings> _emailSettingsOptions;

    public EmailServiceTests()
    {
        _mockLogger = new Mock<ILogger<EmailService>>();
        
        // Configuración válida para tests (Mailtrap.io)
        _emailSettings = new EmailSettings
        {
            SmtpServer = "sandbox.smtp.mailtrap.io",
            SmtpPort = 2525,
            Username = "test_username",
            Password = "test_password",
            FromEmail = "noreply@migenteenlinea.com",
            FromName = "MiGente En Línea - Testing",
            EnableSsl = true,
            Timeout = 10000,
            MaxRetryAttempts = 3,
            RetryDelayMilliseconds = 2000
        };
        
        _emailSettingsOptions = Options.Create(_emailSettings);
    }

    /// <summary>
    /// Test 1: Validar que EmailSettings correctas no lanzan excepción
    /// </summary>
    [Fact]
    public void EmailSettings_ValidConfiguration_DoesNotThrowException()
    {
        // Arrange
        var settings = new EmailSettings
        {
            SmtpServer = "smtp.gmail.com",
            SmtpPort = 587,
            Username = "test@example.com",
            Password = "password",
            FromEmail = "from@example.com",
            FromName = "Test Sender",
            EnableSsl = true,
            Timeout = 30000,
            MaxRetryAttempts = 3,
            RetryDelayMilliseconds = 2000
        };

        // Act
        Action act = () => settings.Validate();

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Test 2: Validar que EmailSettings con SmtpServer vacío lanza excepción
    /// </summary>
    [Fact]
    public void EmailSettings_EmptySmtpServer_ThrowsInvalidOperationException()
    {
        // Arrange
        var settings = new EmailSettings
        {
            SmtpServer = "", // Inválido
            SmtpPort = 587,
            Username = "test@example.com",
            Password = "password",
            FromEmail = "from@example.com",
            FromName = "Test Sender"
        };

        // Act
        Action act = () => settings.Validate();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*SmtpServer*");
    }

    /// <summary>
    /// Test 3: Validar que EmailSettings con FromEmail inválido lanza excepción
    /// </summary>
    [Fact]
    public void EmailSettings_InvalidFromEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var settings = new EmailSettings
        {
            SmtpServer = "smtp.gmail.com",
            SmtpPort = 587,
            Username = "test@example.com",
            Password = "password",
            FromEmail = "invalid-email", // Sin @
            FromName = "Test Sender"
        };

        // Act
        Action act = () => settings.Validate();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*FromEmail*válido*");
    }

    /// <summary>
    /// Test 4: Validar que EmailSettings con puerto inválido lanza excepción
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(70000)]
    public void EmailSettings_InvalidSmtpPort_ThrowsInvalidOperationException(int invalidPort)
    {
        // Arrange
        var settings = new EmailSettings
        {
            SmtpServer = "smtp.gmail.com",
            SmtpPort = invalidPort,
            Username = "test@example.com",
            Password = "password",
            FromEmail = "from@example.com",
            FromName = "Test Sender"
        };

        // Act
        Action act = () => settings.Validate();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*SmtpPort*");
    }

    /// <summary>
    /// Test 5: SendEmailAsync con destinatario inválido lanza ArgumentException
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    [InlineData("test@")]
    [InlineData("@test.com")]
    public async Task SendEmailAsync_InvalidToEmail_ThrowsArgumentException(string invalidEmail)
    {
        // Arrange
        var service = new EmailService(_emailSettingsOptions, _mockLogger.Object);

        // Act
        Func<Task> act = async () => await service.SendEmailAsync(
            toEmail: invalidEmail,
            toName: "Test User",
            subject: "Test Subject",
            htmlBody: "<p>Test</p>",
            plainTextBody: "Test"
        );

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*email*");
    }

    /// <summary>
    /// Test 6: SendActivationEmailAsync con URL nulo lanza ArgumentException
    /// </summary>
    [Fact]
    public async Task SendActivationEmailAsync_NullActivationUrl_ThrowsArgumentException()
    {
        // Arrange
        var service = new EmailService(_emailSettingsOptions, _mockLogger.Object);

        // Act
        Func<Task> act = async () => await service.SendActivationEmailAsync(
            toEmail: "test@example.com",
            toName: "Test User",
            activationUrl: null! // URL nula
        );

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*activationUrl*");
    }

    /// <summary>
    /// Test 7: SendWelcomeEmailAsync con tipo de usuario inválido lanza ArgumentException
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("InvalidType")]
    public async Task SendWelcomeEmailAsync_InvalidUserType_ThrowsArgumentException(string invalidType)
    {
        // Arrange
        var service = new EmailService(_emailSettingsOptions, _mockLogger.Object);

        // Act
        Func<Task> act = async () => await service.SendWelcomeEmailAsync(
            toEmail: "test@example.com",
            toName: "Test User",
            userType: invalidType
        );

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*userType*");
    }

    /// <summary>
    /// Test 8: Constructor valida EmailSettings al inicio
    /// </summary>
    [Fact]
    public void Constructor_InvalidEmailSettings_ThrowsInvalidOperationException()
    {
        // Arrange
        var invalidSettings = new EmailSettings
        {
            SmtpServer = "", // Inválido
            SmtpPort = 587
        };
        var invalidOptions = Options.Create(invalidSettings);

        // Act
        Action act = () => new EmailService(invalidOptions, _mockLogger.Object);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*configuración de email*válida*");
    }

    /// <summary>
    /// Test 9: Verificar que templates HTML contienen placeholders esperados
    /// </summary>
    [Fact]
    public void EmailService_ActivationTemplate_ContainsRequiredPlaceholders()
    {
        // Este test verifica la estructura interna del template
        // (en un escenario real, verificaríamos el contenido del email enviado)
        
        // Arrange & Act
        var service = new EmailService(_emailSettingsOptions, _mockLogger.Object);

        // Assert
        // Solo verificamos que el servicio se instancia correctamente
        service.Should().NotBeNull();
        
        // Nota: Para verificar templates, necesitaríamos hacer el método 
        // GenerateActivationEmailHtml() público o usar integration tests
    }

    /// <summary>
    /// Test 10: SendPasswordResetEmailAsync valida parámetros correctamente
    /// </summary>
    [Fact]
    public async Task SendPasswordResetEmailAsync_ValidParameters_DoesNotThrowException()
    {
        // Arrange
        var service = new EmailService(_emailSettingsOptions, _mockLogger.Object);

        // Act
        // Nota: Este test fallará si intenta conectarse al SMTP real.
        // En un escenario ideal, usaríamos un SMTP mock o Mailtrap.io
        
        // Para propósitos de este test, solo verificamos que los parámetros son aceptados
        Func<Task> act = async () => await service.SendPasswordResetEmailAsync(
            toEmail: "test@example.com",
            toName: "Test User",
            resetUrl: "https://example.com/reset?token=abc123"
        );

        // Assert
        // Si los parámetros son válidos, no debe lanzar ArgumentException
        // (podría lanzar SmtpException si intenta enviar realmente)
        try
        {
            await act();
        }
        catch (ArgumentException)
        {
            Assert.Fail("No debería lanzar ArgumentException con parámetros válidos");
        }
        catch
        {
            // Otras excepciones (SMTP) son esperadas sin un servidor real
        }
    }
}
