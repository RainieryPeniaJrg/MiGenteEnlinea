using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Infrastructure.Services;

/// <summary>
/// ⚠️ MOCK TEMPORAL: Implementación temporal de INominaCalculatorService para permitir startup de la API.
/// TODO: Reemplazar con NominaCalculatorService completo cuando se migre la lógica de armarNovedad().
/// </summary>
public class MockNominaCalculatorService : INominaCalculatorService
{
    private readonly ILogger<MockNominaCalculatorService> _logger;

    public MockNominaCalculatorService(ILogger<MockNominaCalculatorService> logger)
    {
        _logger = logger;
        _logger.LogWarning("⚠️ USANDO MOCK NOMINA CALCULATOR SERVICE - No calcula nóminas reales");
    }

    public Task<NominaCalculoResult> CalcularNominaAsync(
        int empleadoId,
        DateTime fechaPago,
        string tipoConcepto,
        bool esFraccion,
        bool aplicarTss,
        CancellationToken cancellationToken = default)
    {
        _logger.LogWarning(
            "Mock: Simulando cálculo de nómina. EmpleadoId: {EmpleadoId}, FechaPago: {FechaPago}, Concepto: {Concepto}",
            empleadoId,
            fechaPago,
            tipoConcepto);

        // Simular resultado básico con estructura real
        var result = new NominaCalculoResult
        {
            Percepciones = new List<ConceptoNomina>
            {
                new ConceptoNomina
                {
                    EmpleadoId = empleadoId,
                    Descripcion = "Salario Base (MOCK)",
                    Monto = 25000.00m
                }
            },
            Deducciones = new List<ConceptoNomina>() // Sin deducciones en mock
        };

        _logger.LogInformation(
            "Mock: Nómina calculada. Total Percepciones: {Percepciones}, Total Deducciones: {Deducciones}, Neto: {Neto}",
            result.TotalPercepciones,
            result.TotalDeducciones,
            result.NetoPagar);

        return Task.FromResult(result);
    }
}
