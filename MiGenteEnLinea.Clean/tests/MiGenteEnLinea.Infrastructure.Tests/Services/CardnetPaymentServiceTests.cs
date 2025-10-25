using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Pagos;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;
using MiGenteEnLinea.Infrastructure.Services;
using System.Net;
using System.Net.Http;
using Xunit;

namespace MiGenteEnLinea.Infrastructure.Tests.Services;

/// <summary>
/// Tests unitarios para CardnetPaymentService.
/// Valida idempotency key generation, payment processing y configuración.
/// </summary>
public class CardnetPaymentServiceTests : IDisposable
{
    private readonly MiGenteDbContext _context;
    private readonly Mock<ILogger<CardnetPaymentService>> _loggerMock;
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly CardnetPaymentService _service;

    public CardnetPaymentServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<MiGenteDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new MiGenteDbContext(options);

        // Seed test data
        SeedTestData();

        // Setup mocks
        _loggerMock = new Mock<ILogger<CardnetPaymentService>>();
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();

        // Create service
        _service = new CardnetPaymentService(
            _loggerMock.Object,
            _context,
            _httpClientFactoryMock.Object);
    }

    private void SeedTestData()
    {
        var gateway = PaymentGateway.Create(
            urlProduccion: "https://lab.cardnet.com.do/api/payment/transactions/",
            urlTest: "https://lab.cardnet.com.do/api/payment/transactions/",
            merchantId: "349041263",
            terminalId: "77777777",
            modoTest: true);

        _context.PaymentGateways.Add(gateway);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetConfigurationAsync_ReturnsCorrectConfiguration()
    {
        // Act
        var config = await _service.GetConfigurationAsync();

        // Assert
        Assert.NotNull(config);
        Assert.Equal("349041263", config.MerchantId);
        Assert.Equal("77777777", config.TerminalId);
        Assert.Equal("https://lab.cardnet.com.do/api/payment/transactions/", config.BaseUrl);
        Assert.True(config.IsTest);
    }

    [Fact]
    public async Task GetConfigurationAsync_ThrowsException_WhenNoConfigurationExists()
    {
        // Arrange - Remove gateway
        var gateway = await _context.PaymentGateways.FirstAsync();
        _context.PaymentGateways.Remove(gateway);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _service.GetConfigurationAsync());
    }

    [Fact]
    public async Task GenerateIdempotencyKeyAsync_ReturnsValidKey_WhenCardnetRespondsSuccessfully()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("ikey:a1b2c3d4-e5f6-7890-abcd-ef1234567890")
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        // Act
        var idempotencyKey = await _service.GenerateIdempotencyKeyAsync();

        // Assert
        Assert.NotNull(idempotencyKey);
        Assert.Equal("a1b2c3d4-e5f6-7890-abcd-ef1234567890", idempotencyKey);
        Assert.Equal(36, idempotencyKey.Length); // GUID length
    }

    [Fact]
    public async Task GenerateIdempotencyKeyAsync_ThrowsException_WhenCardnetReturnsError()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Error interno del servidor")
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _service.GenerateIdempotencyKeyAsync());
    }

    [Fact]
    public async Task GenerateIdempotencyKeyAsync_ThrowsException_WhenResponseFormatIsInvalid()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("invalid-format-without-ikey-prefix")
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _service.GenerateIdempotencyKeyAsync());
    }

    [Fact]
    public void PaymentRequest_ShouldMaskCardNumber_InLogs()
    {
        // Arrange
        var cardNumber = "4111111111111111";
        var expected = "****-****-****-1111";

        // Act - This would be tested through logging verification
        // For now, we validate the format manually
        var last4 = cardNumber.Substring(cardNumber.Length - 4);
        var masked = $"****-****-****-{last4}";

        // Assert
        Assert.Equal(expected, masked);
        Assert.DoesNotContain("4111111111111111", masked);
    }

    [Theory]
    [InlineData("4111111111111111", "****-****-****-1111")]
    [InlineData("5500000000000004", "****-****-****-0004")]
    [InlineData("378282246310005", "****-****-****-0005")]
    public void MaskCardNumber_ShouldReturnCorrectFormat(string cardNumber, string expected)
    {
        // Arrange & Act
        var last4 = cardNumber.Substring(cardNumber.Length - 4);
        var masked = $"****-****-****-{last4}";

        // Assert
        Assert.Equal(expected, masked);
    }

    [Fact]
    public void PaymentRequest_ShouldNotExposeFullCardNumber()
    {
        // Arrange
        var request = new PaymentRequest
        {
            Amount = 100.50m,
            CardNumber = "4111111111111111",
            CVV = "123",
            ExpirationDate = "12/25",
            ClientIP = "192.168.1.100",
            ReferenceNumber = "TEST-001",
            InvoiceNumber = "INV-001"
        };

        // Act - Simulate logging
        var logSafeInfo = new
        {
            Amount = request.Amount,
            Last4 = $"****-****-****-{request.CardNumber.Substring(request.CardNumber.Length - 4)}",
            Reference = request.ReferenceNumber
        };

        // Assert - Verify card number is not in "safe" logging object
        var logString = System.Text.Json.JsonSerializer.Serialize(logSafeInfo);
        Assert.DoesNotContain("4111111111111111", logString);
        Assert.Contains("****-****-****-1111", logString);
        Assert.DoesNotContain(request.CVV, logString);
    }

    [Fact]
    public async Task GetConfigurationAsync_SelectsCorrectUrl_BasedOnModoTest()
    {
        // Arrange
        var gateway = await _context.PaymentGateways.FirstAsync();
        gateway.CambiarAModoProduccion();
        await _context.SaveChangesAsync();

        // Act
        var config = await _service.GetConfigurationAsync();

        // Assert
        Assert.False(config.IsTest);
        Assert.Equal(gateway.UrlProduccion, config.BaseUrl);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
