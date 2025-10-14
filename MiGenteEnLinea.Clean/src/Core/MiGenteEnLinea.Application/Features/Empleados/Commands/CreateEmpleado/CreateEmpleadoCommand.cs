using MediatR;
using System;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CreateEmpleado;

/// <summary>
/// Command para crear un nuevo empleado permanente.
/// Legacy: EmpleadosService.guardarEmpleado(empleado)
/// </summary>
public record CreateEmpleadoCommand : IRequest<int>
{
    public string UserId { get; init; } = string.Empty;
    public string Identificacion { get; init; } = string.Empty;
    public string Nombre { get; init; } = string.Empty;
    public string Apellido { get; init; } = string.Empty;
    public string? Alias { get; init; }
    
    // Información Personal
    public int? EstadoCivil { get; init; }
    public DateTime? Nacimiento { get; init; }
    
    // Contacto
    public string? Telefono1 { get; init; }
    public string? Telefono2 { get; init; }
    public string? Direccion { get; init; }
    public string? Provincia { get; init; }
    public string? Municipio { get; init; }
    
    // Información Laboral
    public DateTime FechaInicio { get; init; }
    public string? Posicion { get; init; }
    public decimal Salario { get; init; }
    public int PeriodoPago { get; init; }
    public int? DiasPago { get; init; }
    public bool Tss { get; init; }
    
    // Emergencia
    public string? ContactoEmergencia { get; init; }
    public string? TelefonoEmergencia { get; init; }
    
    // Foto (base64)
    public string? Foto { get; init; }
}
