using System;
using System.Threading;
using System.Threading.Tasks;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Servicio para cálculos complejos de nómina.
/// Extrae la lógica de armarNovedad() del Legacy (fichaEmpleado.aspx.cs).
/// </summary>
public interface INominaCalculatorService
{
    /// <summary>
    /// Calcula la nómina completa de un empleado incluyendo percepciones y deducciones TSS.
    /// </summary>
    /// <param name="empleadoId">ID del empleado</param>
    /// <param name="fechaPago">Fecha del pago</param>
    /// <param name="tipoConcepto">Tipo de concepto: "Salario" o "Regalia"</param>
    /// <param name="esFraccion">True si es fracción de período (días trabajados), False si es período completo</param>
    /// <param name="aplicarTss">True si se deben aplicar deducciones TSS</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Resultado con percepciones y deducciones calculadas</returns>
    Task<NominaCalculoResult> CalcularNominaAsync(
        int empleadoId,
        DateTime fechaPago,
        string tipoConcepto,
        bool esFraccion,
        bool aplicarTss,
        CancellationToken cancellationToken = default);
}
