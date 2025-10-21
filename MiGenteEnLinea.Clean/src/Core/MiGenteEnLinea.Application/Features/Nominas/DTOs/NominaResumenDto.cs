namespace MiGenteEnLinea.Application.Features.Nominas.DTOs;

/// <summary>
/// DTO con resumen completo de nómina por período
/// </summary>
public class NominaResumenDto
{
    public int EmpleadorId { get; set; }
    public string Periodo { get; set; } = string.Empty;
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    
    // Totales generales
    public int TotalEmpleados { get; set; }
    public decimal TotalSalarioBruto { get; set; }
    public decimal TotalDeducciones { get; set; }
    public decimal TotalSalarioNeto { get; set; }
    
    // Desglose de deducciones por tipo (ej: AFP, SFS, ISR, etc.)
    public Dictionary<string, decimal> DeduccionesPorTipo { get; set; } = new();
    
    // Estadísticas
    public int RecibosGenerados { get; set; }
    public int RecibosAnulados { get; set; }
    public decimal PromedioSalarioBruto { get; set; }
    public decimal PromedioSalarioNeto { get; set; }
    
    // Detalle por empleado (opcional)
    public List<NominaEmpleadoDto> DetalleEmpleados { get; set; } = new();
}

/// <summary>
/// DTO con resumen de nómina para un empleado específico
/// </summary>
public class NominaEmpleadoDto
{
    public int EmpleadoId { get; set; }
    public string NombreEmpleado { get; set; } = string.Empty;
    
    public int TotalRecibos { get; set; }
    public decimal TotalSalarioBruto { get; set; }
    public decimal TotalDeducciones { get; set; }
    public decimal TotalSalarioNeto { get; set; }
    
    public decimal PromedioSalarioBruto { get; set; }
    public decimal PromedioSalarioNeto { get; set; }
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
    public decimal MasaSalarial { get; set; }
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
