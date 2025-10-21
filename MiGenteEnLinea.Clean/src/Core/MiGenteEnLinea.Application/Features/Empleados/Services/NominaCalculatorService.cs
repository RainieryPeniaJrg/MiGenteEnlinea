using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;
using MiGenteEnLinea.Domain.Entities.Empleados;

namespace MiGenteEnLinea.Application.Features.Empleados.Services;

/// <summary>
/// Servicio para cálculos complejos de nómina.
/// Extrae toda la lógica de armarNovedad() del Legacy (fichaEmpleado.aspx.cs líneas 177-340).
/// </summary>
public class NominaCalculatorService : INominaCalculatorService
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<NominaCalculatorService> _logger;

    // Constantes críticas del Legacy
    private const decimal DIVIDENDO_FRACCION_QUINCENAL = 23.83m; // Días promedio por quincena

    public NominaCalculatorService(
        IApplicationDbContext context,
        ILogger<NominaCalculatorService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<NominaCalculoResult> CalcularNominaAsync(
        int empleadoId,
        DateTime fechaPago,
        string tipoConcepto, // "Salario" o "Regalia"
        bool esFraccion,
        bool aplicarTss,
        CancellationToken cancellationToken = default)
    {
        // PASO 1: Obtener empleado
        var empleado = await _context.Empleados
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EmpleadoId == empleadoId, cancellationToken)
            ?? throw new NotFoundException(nameof(Empleado), empleadoId);

        var result = new NominaCalculoResult();

        // PASO 2: Calcular dividendo según período de pago
        int dividendo = CalcularDividendo(empleado.PeriodoPago, tipoConcepto);

        // PASO 3: Calcular salario base (con fracción si aplica)
        decimal montoSalario = esFraccion
            ? CalcularSalarioFraccion(empleado.Salario, empleado.FechaInicio, fechaPago)
            : empleado.Salario / dividendo;

        // PASO 4: Agregar concepto de salario a percepciones
        var conceptoSalario = new ConceptoNomina
        {
            Descripcion = DeterminarDescripcionSalario(tipoConcepto, esFraccion),
            Monto = montoSalario,
            EmpleadoId = empleadoId
        };
        result.Percepciones.Add(conceptoSalario);

        // PASO 5: Agregar remuneraciones extras (solo si es Salario, no Regalía)
        if (tipoConcepto == "Salario")
        {
            var remuneracionesExtras = ObtenerRemuneracionesExtras(
                empleado, 
                dividendo, 
                esFraccion, 
                empleado.FechaInicio, 
                fechaPago);
            result.Percepciones.AddRange(remuneracionesExtras);
        }

        // PASO 6: Calcular y agregar deducciones TSS (si aplica)
        if (aplicarTss)
        {
            var deducciones = await CalcularDeduccionesTssAsync(
                empleado.Salario,
                dividendo,
                esFraccion,
                empleado.FechaInicio,
                fechaPago,
                empleadoId,
                cancellationToken);
            result.Deducciones.AddRange(deducciones);
        }

        _logger.LogInformation(
            "Nómina calculada: EmpleadoId={EmpleadoId}, Percepciones={TotalPercepciones:C}, " +
            "Deducciones={TotalDeducciones:C}, Neto={NetoPagar:C}",
            empleadoId, result.TotalPercepciones, result.TotalDeducciones, result.NetoPagar);

        return result;
    }

    /// <summary>
    /// Calcula el dividendo según el período de pago del empleado.
    /// Legacy: fichaEmpleado.aspx.cs líneas 189-203
    /// </summary>
    private int CalcularDividendo(int periodoPago, string tipoConcepto)
    {
        // Solo aplicar dividendo si es Salario (no Regalía)
        if (tipoConcepto != "Salario")
            return 1;

        return periodoPago switch
        {
            1 => 4, // Semanal: 4 semanas por mes
            2 => 2, // Quincenal: 2 quincenas por mes
            3 => 1, // Mensual: 1 mes
            _ => 1
        };
    }

    /// <summary>
    /// Calcula el salario proporcional por días trabajados (fracción).
    /// Legacy: fichaEmpleado.aspx.cs líneas 205-220
    /// </summary>
    private decimal CalcularSalarioFraccion(decimal salario, DateOnly? fechaInicio, DateTime fechaPago)
    {
        if (!fechaInicio.HasValue)
        {
            // Si no hay fecha de inicio, usar salario completo dividido por dividendo quincenal
            return salario / DIVIDENDO_FRACCION_QUINCENAL;
        }

        // Calcular días trabajados desde la fecha de inicio hasta el pago
        var dtFechaInicio = new DateTime(fechaInicio.Value.Year, fechaInicio.Value.Month, fechaInicio.Value.Day);
        var dtFechaPago = new DateTime(fechaPago.Year, fechaPago.Month, fechaPago.Day);
        var diferencia = dtFechaPago - dtFechaInicio;
        int diasTrabajados = diferencia.Days;

        // Fórmula crítica: (salario / 23.83) * días trabajados
        return (salario / DIVIDENDO_FRACCION_QUINCENAL) * diasTrabajados;
    }

    /// <summary>
    /// Determina la descripción del concepto de salario.
    /// Legacy: fichaEmpleado.aspx.cs líneas 222-238
    /// </summary>
    private string DeterminarDescripcionSalario(string tipoConcepto, bool esFraccion)
    {
        if (tipoConcepto == "Regalia")
        {
            return esFraccion ? "Fracción de Regalía Pascual" : "Regalía Pascual";
        }

        return esFraccion ? "Fracción de Salario" : "Salario Bruto";
    }

    /// <summary>
    /// Obtiene las remuneraciones extras del empleado (bonos, comisiones).
    /// Legacy: fichaEmpleado.aspx.cs líneas 252-276
    /// </summary>
    private List<ConceptoNomina> ObtenerRemuneracionesExtras(
        Empleado empleado,
        int dividendo,
        bool esFraccion,
        DateOnly? fechaInicio,
        DateTime fechaPago)
    {
        var remuneraciones = new List<ConceptoNomina>();

        // Slot 1
        if (!string.IsNullOrWhiteSpace(empleado.RemuneracionExtra1) && empleado.MontoExtra1.HasValue)
        {
            remuneraciones.Add(CrearConceptoRemuneracion(
                empleado.RemuneracionExtra1,
                empleado.MontoExtra1.Value,
                empleado.EmpleadoId,
                dividendo,
                esFraccion,
                fechaInicio,
                fechaPago));
        }

        // Slot 2
        if (!string.IsNullOrWhiteSpace(empleado.RemuneracionExtra2) && empleado.MontoExtra2.HasValue)
        {
            remuneraciones.Add(CrearConceptoRemuneracion(
                empleado.RemuneracionExtra2,
                empleado.MontoExtra2.Value,
                empleado.EmpleadoId,
                dividendo,
                esFraccion,
                fechaInicio,
                fechaPago));
        }

        // Slot 3
        if (!string.IsNullOrWhiteSpace(empleado.RemuneracionExtra3) && empleado.MontoExtra3.HasValue)
        {
            remuneraciones.Add(CrearConceptoRemuneracion(
                empleado.RemuneracionExtra3,
                empleado.MontoExtra3.Value,
                empleado.EmpleadoId,
                dividendo,
                esFraccion,
                fechaInicio,
                fechaPago));
        }

        return remuneraciones;
    }

    /// <summary>
    /// Crea un concepto de nómina para una remuneración extra.
    /// Aplica fracción si corresponde.
    /// </summary>
    private ConceptoNomina CrearConceptoRemuneracion(
        string descripcion,
        decimal monto,
        int empleadoId,
        int dividendo,
        bool esFraccion,
        DateOnly? fechaInicio,
        DateTime fechaPago)
    {
        decimal montoCalculado;

        if (esFraccion)
        {
            // Calcular fracción usando misma lógica que salario
            if (!fechaInicio.HasValue)
            {
                montoCalculado = monto / DIVIDENDO_FRACCION_QUINCENAL;
            }
            else
            {
                var dtFechaInicio = new DateTime(fechaInicio.Value.Year, fechaInicio.Value.Month, fechaInicio.Value.Day);
                var dtFechaPago = new DateTime(fechaPago.Year, fechaPago.Month, fechaPago.Day);
                var diferencia = dtFechaPago - dtFechaInicio;
                int diasTrabajados = diferencia.Days;
                montoCalculado = (monto / DIVIDENDO_FRACCION_QUINCENAL) * diasTrabajados;
            }
        }
        else
        {
            montoCalculado = monto / dividendo;
        }

        return new ConceptoNomina
        {
            Descripcion = descripcion,
            Monto = montoCalculado,
            EmpleadoId = empleadoId
        };
    }

    /// <summary>
    /// Calcula las deducciones TSS (seguridad social) del empleado.
    /// Legacy: fichaEmpleado.aspx.cs líneas 285-318
    /// ⚠️ CRÍTICO: Las deducciones SIEMPRE son negativas (* -1)
    /// </summary>
    private async Task<List<ConceptoNomina>> CalcularDeduccionesTssAsync(
        decimal salario,
        int dividendo,
        bool esFraccion,
        DateOnly? fechaInicio,
        DateTime fechaPago,
        int empleadoId,
        CancellationToken cancellationToken)
    {
        var deducciones = new List<ConceptoNomina>();

        // Obtener configuración de deducciones TSS desde la base de datos
        var deduccionesTss = await _context.DeduccionesTss
            .AsNoTracking()
            .Where(d => d.Activa) // Solo deducciones activas (propiedad verificada: Activa, no Activo)
            .ToListAsync(cancellationToken);

        if (deduccionesTss == null || !deduccionesTss.Any())
        {
            _logger.LogWarning("No se encontraron deducciones TSS activas en la base de datos");
            return deducciones;
        }

        foreach (var deduccion in deduccionesTss)
        {
            decimal montoDeduccion;
            string descripcionDeduccion = deduccion.Descripcion;

            if (esFraccion)
            {
                // Calcular fracción de deducción
                decimal salarioFraccion;
                if (!fechaInicio.HasValue)
                {
                    salarioFraccion = salario / DIVIDENDO_FRACCION_QUINCENAL;
                }
                else
                {
                    var dtFechaInicio = new DateTime(fechaInicio.Value.Year, fechaInicio.Value.Month, fechaInicio.Value.Day);
                    var dtFechaPago = new DateTime(fechaPago.Year, fechaPago.Month, fechaPago.Day);
                    var diferencia = dtFechaPago - dtFechaInicio;
                    int diasTrabajados = diferencia.Days;
                    salarioFraccion = (salario / DIVIDENDO_FRACCION_QUINCENAL) * diasTrabajados;
                }

                // Fórmula: (salarioFraccion * porcentaje / 100) * -1
                montoDeduccion = (salarioFraccion * (deduccion.Porcentaje / 100)) * -1;
                descripcionDeduccion = $"Fracción de {deduccion.Descripcion}";
            }
            else
            {
                // Fórmula: (salario * porcentaje / 100) * -1
                montoDeduccion = (salario * (deduccion.Porcentaje / 100)) * -1;
            }

            deducciones.Add(new ConceptoNomina
            {
                Descripcion = descripcionDeduccion,
                Monto = montoDeduccion, // Ya es negativo
                EmpleadoId = empleadoId
            });
        }

        return deducciones;
    }
}
