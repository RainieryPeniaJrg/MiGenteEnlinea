namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// DTO para remuneraciones adicionales de empleado
/// Migrado desde: Remuneraciones entity (tabla Legacy)
/// 
/// Representa ingresos adicionales al salario base como:
/// - Bonos
/// - Comisiones
/// - Horas extras
/// - Incentivos
/// </summary>
public class RemuneracionDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int EmpleadoId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Monto { get; set; }
}
