using FluentAssertions;
using MiGenteEnLinea.Infrastructure.Services.Documents;
using Xunit;

namespace MiGenteEnLinea.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests para NumeroEnLetrasService.
/// Garantiza compatibilidad 100% con Legacy NumeroEnLetras.cs
/// </summary>
public class NumeroEnLetrasServiceTests
{
    private readonly NumeroEnLetrasService _service;

    public NumeroEnLetrasServiceTests()
    {
        _service = new NumeroEnLetrasService();
    }

    #region ConvertirALetras (con moneda) Tests

    [Fact]
    public void ConvertirALetras_ConCero_DebeRetornarCeroPesos()
    {
        // Arrange
        decimal numero = 0;

        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: true);

        // Assert
        resultado.Should().Be("CERO PESOS DOMINICANOS 00 /100");
    }

    [Fact]
    public void ConvertirALetras_ConUno_DebeRetornarUnPeso()
    {
        // Arrange
        decimal numero = 1;

        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: true);

        // Assert
        resultado.Should().Be("UNO PESOS DOMINICANOS 00 /100");
    }

    [Fact]
    public void ConvertirALetras_ConCien_DebeRetornarCienPesos()
    {
        // Arrange
        decimal numero = 100;

        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: true);

        // Assert
        resultado.Should().Be("CIEN PESOS DOMINICANOS 00 /100");
    }

    [Fact]
    public void ConvertirALetras_ConMil_DebeRetornarMilPesos()
    {
        // Arrange
        decimal numero = 1000;

        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: true);

        // Assert
        resultado.Should().Be("MIL PESOS DOMINICANOS 00 /100");
    }

    [Fact]
    public void ConvertirALetras_Con1234Punto56_DebeRetornarTextoCompleto()
    {
        // Arrange
        decimal numero = 1234.56m;

        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: true);

        // Assert
        resultado.Should().Be("MIL DOSCIENTOS TREINTA Y CUATRO PESOS DOMINICANOS 56 /100");
    }

    [Fact]
    public void ConvertirALetras_ConUnMillon_DebeRetornarUnMillonPesos()
    {
        // Arrange
        decimal numero = 1000000;

        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: true);

        // Assert
        resultado.Should().Be("UN MILLON PESOS DOMINICANOS 00 /100");
    }

    [Fact]
    public void ConvertirALetras_ConDosMillones_DebeRetornarDosMillonesPesos()
    {
        // Arrange
        decimal numero = 2000000;

        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: true);

        // Assert
        resultado.Should().StartWith("DOS MILLONES");
    }

    [Theory]
    [InlineData(0.50, "CERO PESOS DOMINICANOS 50 /100")]
    [InlineData(0.99, "CERO PESOS DOMINICANOS 99 /100")]
    [InlineData(0.01, "CERO PESOS DOMINICANOS 01 /100")]
    public void ConvertirALetras_ConSoloDecimales_DebeRetornarCeroConCentavos(decimal numero, string esperado)
    {
        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: true);

        // Assert
        resultado.Should().Be(esperado);
    }

    [Theory]
    [InlineData(15, "QUINCE")]
    [InlineData(21, "VEINTIUN")]
    [InlineData(99, "NOVENTA Y NUEVE")]
    [InlineData(500, "QUINIENTOS")]
    [InlineData(700, "SETECIENTOS")]
    [InlineData(900, "NOVECIENTOS")]
    public void ConvertirALetras_CasosEspeciales_DebeRetornarTextoCorrecto(decimal numero, string inicioEsperado)
    {
        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: false);

        // Assert
        resultado.Should().StartWith(inicioEsperado);
    }

    #endregion

    #region ConvertirALetras (sin moneda) Tests

    [Fact]
    public void ConvertirALetras_SinMoneda_ConCero_DebeRetornarSoloCero()
    {
        // Arrange
        decimal numero = 0;

        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: false);

        // Assert
        resultado.Should().Be("CERO");
    }

    [Fact]
    public void ConvertirALetras_SinMoneda_Con1234_DebeRetornarSoloTexto()
    {
        // Arrange
        decimal numero = 1234;

        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: false);

        // Assert
        resultado.Should().Be("MIL DOSCIENTOS TREINTA Y CUATRO");
    }

    #endregion

    #region ConvertirEnteroALetras Tests

    [Fact]
    public void ConvertirEnteroALetras_ConCero_DebeRetornarCero()
    {
        // Act
        var resultado = _service.ConvertirEnteroALetras(0);

        // Assert
        resultado.Should().Be("CERO");
    }

    [Theory]
    [InlineData(1, "UN")]
    [InlineData(10, "DIEZ")]
    [InlineData(15, "QUINCE")]
    [InlineData(20, "VEINTE")]
    [InlineData(100, "CIEN")]
    [InlineData(1000, "UN MIL")]
    [InlineData(10000, "DIEZ MIL")]
    public void ConvertirEnteroALetras_ConNumerosValidos_DebeRetornarTextoCorrecto(long numero, string esperado)
    {
        // Act
        var resultado = _service.ConvertirEnteroALetras(numero);

        // Assert
        resultado.Should().Be(esperado);
    }

    [Theory]
    [InlineData(21, "VEINTE Y UN")]
    [InlineData(55, "CINCUENTA Y CINCO")]
    [InlineData(99, "NOVENTA Y NUEVE")]
    public void ConvertirEnteroALetras_ConDecenasYUnidades_DebeUsarY(long numero, string esperado)
    {
        // Act
        var resultado = _service.ConvertirEnteroALetras(numero);

        // Assert
        resultado.Should().Be(esperado);
    }

    [Theory]
    [InlineData(200, "DOSCIENTOS")]
    [InlineData(500, "QUINIENTOS")]
    [InlineData(700, "SETECIENTOS")]
    [InlineData(900, "NOVECIENTOS")]
    public void ConvertirEnteroALetras_ConCentenas_DebeRetornarTextoCorrecto(long numero, string esperado)
    {
        // Act
        var resultado = _service.ConvertirEnteroALetras(numero);

        // Assert
        resultado.Should().Be(esperado);
    }

    [Fact]
    public void ConvertirEnteroALetras_ConNumeroNegativo_DebeLanzarExcepcion()
    {
        // Act
        Action act = () => _service.ConvertirEnteroALetras(-1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*debe estar entre 0 y 10,000*");
    }

    [Fact]
    public void ConvertirEnteroALetras_ConNumeroMayorA10000_DebeLanzarExcepcion()
    {
        // Act
        Action act = () => _service.ConvertirEnteroALetras(10001);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*debe estar entre 0 y 10,000*");
    }

    #endregion

    #region Edge Cases & Regression Tests

    [Theory]
    [InlineData(16, "DIECISEIS")]
    [InlineData(17, "DIECISIETE")]
    [InlineData(18, "DIECIOCHO")]
    [InlineData(19, "DIECINUEVE")]
    public void ConvertirALetras_ConDieci_DebeUsarDIECI(decimal numero, string esperado)
    {
        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: false);

        // Assert
        resultado.Should().Be(esperado);
    }

    [Theory]
    [InlineData(21, "VEINTIUN")]
    [InlineData(22, "VEINTIDOS")]
    [InlineData(26, "VEINTISEIS")]
    [InlineData(29, "VEINTINUEVE")]
    public void ConvertirALetras_ConVeinti_DebeUsarVEINTI(decimal numero, string esperado)
    {
        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: false);

        // Assert
        resultado.Should().Be(esperado);
    }

    [Fact]
    public void ConvertirALetras_ConCientoUno_DebeRetornarCIENTO()
    {
        // Arrange
        decimal numero = 101;

        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: false);

        // Assert
        resultado.Should().Be("CIENTO UNO");
    }

    [Theory]
    [InlineData(200, "DOSCIENTOS")]
    [InlineData(300, "TRESCIENTOS")]
    [InlineData(400, "CUATROCIENTOS")]
    [InlineData(600, "SEISCIENTOS")]
    [InlineData(800, "OCHOCIENTOS")]
    public void ConvertirALetras_ConCentosExactos_DebeTerminarEnCIENTOS(decimal numero, string esperado)
    {
        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: false);

        // Assert
        resultado.Should().Be(esperado);
    }

    [Fact]
    public void ConvertirALetras_ConRedondeoDecimales_DebeRedondearCorrectamente()
    {
        // Arrange - 1234.565 debería truncarse (no redondear) a 56 centavos
        // porque el Legacy hace Truncate del entero y luego Round de decimales
        decimal numero = 1234.565m;

        // Act
        var resultado = _service.ConvertirALetras(numero, incluirMoneda: true);

        // Assert
        resultado.Should().Contain("56 /100"); // Math.Round((565/1000)*100, 2) = 56
    }

    #endregion
}
