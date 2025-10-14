using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetEmpleadoById;

/// <summary>
/// Handler para GetEmpleadoByIdQuery.
/// Legacy: EmpleadosService.getEmpleadosByID(userID, id)
/// Retorna información completa del empleado incluyendo campos calculados.
/// </summary>
public class GetEmpleadoByIdQueryHandler : IRequestHandler<GetEmpleadoByIdQuery, EmpleadoDetalleDto?>
{
    private readonly IApplicationDbContext _context;

    public GetEmpleadoByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EmpleadoDetalleDto?> Handle(GetEmpleadoByIdQuery request, CancellationToken cancellationToken)
    {
        // Query optimizada con AsNoTracking (solo lectura)
        var empleado = await _context.Empleados
            .AsNoTracking()
            .Where(e => e.UserId == request.UserId && e.EmpleadoId == request.EmpleadoId)
            .Select(e => new EmpleadoDetalleDto
            {
                // Identificación
                EmpleadoId = e.EmpleadoId,
                UserId = e.UserId,
                Identificacion = e.Identificacion,
                
                // Información Personal
                Nombre = e.Nombre,
                Apellido = e.Apellido,
                Alias = e.Alias,
                EstadoCivil = e.EstadoCivil,
                Nacimiento = e.Nacimiento,
                
                // Contacto
                Telefono1 = e.Telefono1,
                Telefono2 = e.Telefono2,
                Direccion = e.Direccion,
                Provincia = e.Provincia,
                Municipio = e.Municipio,
                
                // Información Laboral
                FechaRegistro = e.FechaRegistro,
                FechaInicio = e.FechaInicio,
                Posicion = e.Posicion,
                Salario = e.Salario,
                PeriodoPago = e.PeriodoPago,
                DiasPago = e.DiasPago,
                InscritoTss = e.InscritoTss,
                Activo = e.Activo,
                
                // Baja (si aplica)
                FechaSalida = e.FechaSalida,
                MotivoBaja = e.MotivoBaja,
                Prestaciones = e.Prestaciones,
                
                // Emergencia
                ContactoEmergencia = e.ContactoEmergencia,
                TelefonoEmergencia = e.TelefonoEmergencia,
                
                // Foto
                Foto = e.Foto,
                
                // Remuneraciones Extras
                DescripcionExtra1 = e.RemuneracionExtra1,
                MontoExtra1 = e.MontoExtra1,
                DescripcionExtra2 = e.RemuneracionExtra2,
                MontoExtra2 = e.MontoExtra2,
                DescripcionExtra3 = e.RemuneracionExtra3,
                MontoExtra3 = e.MontoExtra3
            })
            .FirstOrDefaultAsync(cancellationToken);

        // Calcular campos derivados después de la consulta SQL
        if (empleado != null)
        {
            empleado.SalarioMensual = CalcularSalarioMensual(empleado.Salario, empleado.PeriodoPago);
            empleado.Antiguedad = empleado.FechaInicio.HasValue 
                ? CalcularAntiguedad(empleado.FechaInicio.Value) 
                : 0;
            empleado.RequiereActualizacionFoto = CalcularRequiereActualizacionFoto(empleado.FechaRegistro);
        }

        return empleado;
    }

    /// <summary>
    /// Calcula el salario mensual según el período de pago.
    /// Legacy equivalente: Empleado.CalcularSalarioMensual()
    /// </summary>
    private static decimal CalcularSalarioMensual(decimal salario, int periodoPago)
    {
        return periodoPago switch
        {
            1 => salario * 4,      // Semanal: 4 semanas por mes
            2 => salario * 2,      // Quincenal: 2 quincenas por mes
            3 => salario,          // Mensual
            _ => salario
        };
    }

    /// <summary>
    /// Calcula la antigüedad en años.
    /// Legacy equivalente: Empleado.CalcularAntiguedad()
    /// </summary>
    private static int CalcularAntiguedad(DateOnly fechaInicio)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var age = today.Year - fechaInicio.Year;
        if (fechaInicio > today.AddYears(-age)) age--;
        return age;
    }

    /// <summary>
    /// Determina si la foto requiere actualización (más de 1 año).
    /// Legacy equivalente: Empleado.RequiereActualizacionFoto()
    /// </summary>
    private static bool CalcularRequiereActualizacionFoto(DateTime fechaRegistro)
    {
        return (DateTime.Now - fechaRegistro).TotalDays > 365;
    }
}
