using System;
using System.Collections.Generic;
using System.Linq;

namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// Resultado del cálculo de nómina con percepciones y deducciones.
/// Generado por INominaCalculatorService.
/// </summary>
public class NominaCalculoResult
{
    public List<ConceptoNomina> Percepciones { get; set; } = new();
    public List<ConceptoNomina> Deducciones { get; set; } = new();

    /// <summary>
    /// Total de percepciones (salario + extras). Valores positivos.
    /// </summary>
    public decimal TotalPercepciones => Percepciones.Sum(x => x.Monto);

    /// <summary>
    /// Total de deducciones (TSS). Valores absolutos (sin signo negativo).
    /// </summary>
    public decimal TotalDeducciones => Deducciones.Sum(x => Math.Abs(x.Monto));

    /// <summary>
    /// Neto a pagar = Percepciones - Deducciones
    /// </summary>
    public decimal NetoPagar => TotalPercepciones - TotalDeducciones;
}

/// <summary>
/// Representa un concepto de nómina (percepción o deducción).
/// </summary>
public class ConceptoNomina
{
    public string Descripcion { get; set; } = null!;
    
    /// <summary>
    /// Monto del concepto.
    /// POSITIVO para percepciones (salario, bonos).
    /// NEGATIVO para deducciones (TSS).
    /// </summary>
    public decimal Monto { get; set; }

    public int EmpleadoId { get; set; }
}
