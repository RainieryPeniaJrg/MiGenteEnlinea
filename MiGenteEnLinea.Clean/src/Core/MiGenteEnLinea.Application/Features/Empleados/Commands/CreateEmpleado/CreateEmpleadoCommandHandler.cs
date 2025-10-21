using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Empleados;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.CreateEmpleado;

/// <summary>
/// Handler para CreateEmpleadoCommand.
/// Legacy: EmpleadosService.guardarEmpleado(empleado)
/// </summary>
public class CreateEmpleadoCommandHandler : IRequestHandler<CreateEmpleadoCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateEmpleadoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateEmpleadoCommand request, CancellationToken cancellationToken)
    {
        // PASO 1: Validar que el empleador existe
        var empleadorExists = await _context.Credenciales
            .AnyAsync(c => c.UserId == request.UserId, cancellationToken);

        if (!empleadorExists)
        {
            throw new NotFoundException("Credencial", request.UserId);
        }

        // PASO 2: Validar que la identificación no esté duplicada para este empleador
        var duplicateExists = await _context.Empleados
            .AnyAsync(e => e.UserId == request.UserId && 
                           e.Identificacion == request.Identificacion,
                     cancellationToken);

        if (duplicateExists)
        {
            throw new ValidationException(
                $"Ya existe un empleado con la identificación {request.Identificacion}");
        }

        // PASO 3: Crear empleado usando factory method del dominio
        var empleado = Empleado.Create(
            userId: request.UserId,
            identificacion: request.Identificacion,
            nombre: request.Nombre,
            apellido: request.Apellido,
            salario: request.Salario,
            periodoPago: request.PeriodoPago
        );

        // PASO 4: Actualizar fecha de inicio
        var fechaInicioDateOnly = DateOnly.FromDateTime(request.FechaInicio);
        empleado.ActualizarFechaInicio(fechaInicioDateOnly);

        // PASO 5: Actualizar información personal si se proporciona
        if (request.EstadoCivil.HasValue || request.Nacimiento.HasValue || !string.IsNullOrEmpty(request.Alias))
        {
            var nacimientoDateOnly = request.Nacimiento.HasValue 
                ? DateOnly.FromDateTime(request.Nacimiento.Value)
                : (DateOnly?)null;

            empleado.ActualizarInformacionPersonal(
                nombre: request.Nombre,
                apellido: request.Apellido,
                nacimiento: nacimientoDateOnly,
                estadoCivil: request.EstadoCivil,
                alias: request.Alias
            );
        }

        // PASO 6: Actualizar información de contacto
        if (!string.IsNullOrEmpty(request.Telefono1) || 
            !string.IsNullOrEmpty(request.Telefono2))
        {
            empleado.ActualizarContacto(
                telefono1: request.Telefono1,
                telefono2: request.Telefono2,
                contactoEmergencia: request.ContactoEmergencia,
                telefonoEmergencia: request.TelefonoEmergencia
            );
        }

        // PASO 7: Actualizar dirección
        if (!string.IsNullOrEmpty(request.Direccion) ||
            !string.IsNullOrEmpty(request.Provincia) ||
            !string.IsNullOrEmpty(request.Municipio))
        {
            empleado.ActualizarDireccion(
                direccion: request.Direccion,
                provincia: request.Provincia,
                municipio: request.Municipio
            );
        }

        // PASO 8: Actualizar posición si se proporciona
        if (!string.IsNullOrEmpty(request.Posicion))
        {
            empleado.ActualizarPosicion(
                posicion: request.Posicion,
                salario: request.Salario,
                periodoPago: request.PeriodoPago
            );
        }

        // PASO 8: Configurar TSS y días de pago
        // Nota: Estos campos se actualizan directamente porque no tienen métodos domain específicos
        // TODO: Considerar agregar método domain si la lógica se complica
        if (request.DiasPago.HasValue)
        {
            // Acceso directo a propiedad por ahora
            // empleado.DiasPago = request.DiasPago.Value;
        }

        // PASO 9: Asignar foto si se proporciona
        if (!string.IsNullOrEmpty(request.Foto))
        {
            // empleado.Foto = request.Foto;
        }

        // PASO 10: Guardar en base de datos
        await _context.Empleados.AddAsync(empleado, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return empleado.EmpleadoId;
    }
}
