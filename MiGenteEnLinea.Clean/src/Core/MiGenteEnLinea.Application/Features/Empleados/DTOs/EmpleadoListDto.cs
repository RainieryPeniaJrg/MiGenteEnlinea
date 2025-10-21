using System;

namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// DTO resumido para listado de empleados.
/// Optimizado para grids y listas con paginación.
/// </summary>
public class EmpleadoListDto
{
    public int EmpleadoId { get; set; }
    public string Identificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string NombreCompleto => $"{Nombre} {Apellido}";
    public string? Posicion { get; set; }
    public decimal Salario { get; set; }
    public int PeriodoPago { get; set; }
    public string PeriodoPagoDescripcion => PeriodoPago switch
    {
        1 => "Semanal",
        2 => "Quincenal",
        3 => "Mensual",
        _ => "N/A"
    };
    public int? DiasPago { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public bool Activo { get; set; }
    public string? Foto { get; set; }
    
    // Solo para inactivos
    public DateTime? FechaSalida { get; set; }
}
