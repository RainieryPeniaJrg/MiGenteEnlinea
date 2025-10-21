using MediatR;
using System;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.UpdateEmpleado;

/// <summary>
/// Command para actualizar un empleado permanente existente.
/// Legacy: EmpleadosService.actualizarEmpleado(empleado)
/// Nota: Solo actualiza campos no nulos (partial update)
/// </summary>
public record UpdateEmpleadoCommand : IRequest<bool>
{
    public int EmpleadoId { get; init; }
    public string UserId { get; init; } = string.Empty;
    
    // Información Personal (opcional)
    public string? Nombre { get; init; }
    public string? Apellido { get; init; }
    public string? Alias { get; init; }
    public int? EstadoCivil { get; init; }
    public DateTime? Nacimiento { get; init; }
    
    // Contacto (opcional)
    public string? Telefono1 { get; init; }
    public string? Telefono2 { get; init; }
    public string? Direccion { get; init; }
    public string? Provincia { get; init; }
    public string? Municipio { get; init; }
    
    // Información Laboral (opcional)
    public DateTime? FechaInicio { get; init; }
    public string? Posicion { get; init; }
    public decimal? Salario { get; init; }
    public int? PeriodoPago { get; init; }
    public int? DiasPago { get; init; }
    public bool? Tss { get; init; }
    
    // Emergencia (opcional)
    public string? ContactoEmergencia { get; init; }
    public string? TelefonoEmergencia { get; init; }
    
    // Foto (opcional)
    public string? Foto { get; init; }
}
