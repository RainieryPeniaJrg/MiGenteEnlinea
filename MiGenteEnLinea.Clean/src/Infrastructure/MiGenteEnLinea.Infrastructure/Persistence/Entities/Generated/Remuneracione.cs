using System;
using System.Collections.Generic;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

/// <summary>
/// Entidad generada: Remuneraciones
/// Tabla Legacy: Remuneraciones (otras remuneraciones adicionales al salario base)
/// Migrado desde: MiGente_Front.Data.Remuneraciones
/// 
/// Representa ingresos adicionales al salario base como:
/// - Bonos
/// - Comisiones  
/// - Horas extras
/// - Incentivos
/// </summary>
public partial class Remuneracione
{
    /// <summary>
    /// ID único de la remuneración
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID del usuario empleador (FK a Credencial.UserId)
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Descripción del concepto de remuneración
    /// Ejemplos: "Bono mensual", "Comisión ventas", "Horas extras"
    /// </summary>
    public string Descripcion { get; set; } = null!;

    /// <summary>
    /// Monto de la remuneración adicional
    /// </summary>
    public decimal? Monto { get; set; }

    /// <summary>
    /// ID del empleado (FK a Empleado.EmpleadoId)
    /// </summary>
    public int? EmpleadoId { get; set; }

    // Navigation properties
    public virtual Empleado? Empleado { get; set; }
}
