using System;

namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// DTO con información detallada de un empleado permanente.
/// Incluye campos calculados y toda la información necesaria para la vista de detalle.
/// </summary>
public class EmpleadoDetalleDto
{
    public int EmpleadoId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string NombreCompleto => $"{Nombre} {Apellido}";
    public string? Alias { get; set; }
    
    // Información Personal
    public int? EstadoCivil { get; set; }
    public DateOnly? Nacimiento { get; set; }
    public int? Edad
    {
        get
        {
            if (!Nacimiento.HasValue)
                return null;

            var today = DateOnly.FromDateTime(DateTime.Now);
            var age = today.Year - Nacimiento.Value.Year;
            if (Nacimiento.Value > today.AddYears(-age))
                age--;
            return age;
        }
    }
    
    // Contacto
    public string? Telefono1 { get; set; }
    public string? Telefono2 { get; set; }
    public string? Direccion { get; set; }
    public string? Provincia { get; set; }
    public string? Municipio { get; set; }
    
    // Información Laboral
    public DateTime FechaRegistro { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public string? Posicion { get; set; }
    public decimal Salario { get; set; }
    public int PeriodoPago { get; set; }
    public string PeriodoPagoDescripcion => PeriodoPago switch
    {
        1 => "Semanal",
        2 => "Quincenal",
        3 => "Mensual",
        _ => "No especificado"
    };
    
    public int? DiasPago { get; set; }
    public bool InscritoTss { get; set; }
    public bool Activo { get; set; }
    
    // Baja (si aplica)
    public DateTime? FechaSalida { get; set; }
    public string? MotivoBaja { get; set; }
    public decimal? Prestaciones { get; set; }
    
    // Emergencia
    public string? ContactoEmergencia { get; set; }
    public string? TelefonoEmergencia { get; set; }
    
    // Foto
    public string? Foto { get; set; }
    
    // Campos Calculados
    public decimal SalarioMensual { get; set; }
    public int Antiguedad { get; set; }
    public bool RequiereActualizacionFoto { get; set; }
    
    // Remuneraciones Extras
    public string? DescripcionExtra1 { get; set; }
    public decimal? MontoExtra1 { get; set; }
    public string? DescripcionExtra2 { get; set; }
    public decimal? MontoExtra2 { get; set; }
    public string? DescripcionExtra3 { get; set; }
    public decimal? MontoExtra3 { get; set; }
    
    // Auditoría
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
