namespace MiGenteEnLinea.Application.Features.Nominas.DTOs;

/// <summary>
/// DTO con resumen completo de nómina por período
/// </summary>
public class NominaResumenDto
{
    public int EmpleadorId { get; set; }
    public string Periodo { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    
    // Totales generales
    public int TotalEmpleados { get; set; }
    public int TotalRecibos { get; set; }
    public decimal TotalIngresos { get; set; }
    public decimal TotalDeducciones { get; set; }
    public decimal TotalNeto { get; set; }
    
    // Deducciones TSS
    public decimal TotalAFP { get; set; }
    public decimal TotalSFS { get; set; }
    public decimal TotalInfotep { get; set; }
    
    // Promedios
    public decimal PromedioSalario { get; set; }
    public decimal PromedioDeducciones { get; set; }
    
    // Detalle por empleado (opcional)
    public List<EmpleadoNominaResumenDto> DetalleEmpleados { get; set; } = new();
}

/// <summary>
/// DTO con resumen de nómina para un empleado específico
/// </summary>
public class EmpleadoNominaResumenDto
{
    public int EmpleadoId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    
    public int CantidadRecibos { get; set; }
    public decimal TotalIngresos { get; set; }
    public decimal TotalDeducciones { get; set; }
    public decimal TotalNeto { get; set; }
    
    public DateTime? UltimoPago { get; set; }
}

/// <summary>
/// DTO para estadísticas avanzadas de nómina
/// </summary>
public class EstadisticasNominaDto
{
    public string Periodo { get; set; } = string.Empty;
    
    // Métricas generales
    public int TotalEmpleadosActivos { get; set; }
    public int TotalEmpleadosInactivos { get; set; }
    public decimal MasaScalarial { get; set; }
    public decimal CostoTotalEmpresa { get; set; } // Incluye contribuciones patronales
    
    // Distribución salarial
    public decimal SalarioMinimo { get; set; }
    public decimal SalarioMaximo { get; set; }
    public decimal SalarioPromedio { get; set; }
    public decimal SalarioMediano { get; set; }
    
    // Deducciones
    public decimal TotalDeduccionesLegales { get; set; }
    public decimal TotalDeduccionesVoluntarias { get; set; }
    
    // Tendencias (comparación con período anterior)
    public decimal VariacionPorcentualNomina { get; set; }
    public int VariacionCantidadEmpleados { get; set; }
}
