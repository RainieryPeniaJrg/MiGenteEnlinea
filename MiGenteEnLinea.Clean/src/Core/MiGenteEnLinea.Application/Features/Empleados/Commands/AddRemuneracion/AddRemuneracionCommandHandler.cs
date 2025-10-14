using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.AddRemuneracion;

/// <summary>
/// Handler para AddRemuneracionCommand.
/// Legacy: EmpleadosService.guardarOtrasRemuneraciones(rem)
/// Agrega o actualiza una remuneración extra en uno de los 3 slots disponibles.
/// </summary>
public class AddRemuneracionCommandHandler : IRequestHandler<AddRemuneracionCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public AddRemuneracionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AddRemuneracionCommand request, CancellationToken cancellationToken)
    {
        // PASO 1: Buscar empleado y validar propiedad
        var empleado = await _context.Empleados
            .FirstOrDefaultAsync(e => e.EmpleadoId == request.EmpleadoId && 
                                     e.UserId == request.UserId,
                                cancellationToken);

        if (empleado == null)
        {
            throw new NotFoundException("Empleado", request.EmpleadoId);
        }

        // PASO 2: Validar que el empleado esté activo
        if (!empleado.Activo)
        {
            throw new ValidationException("No se pueden agregar remuneraciones a un empleado inactivo");
        }

        // PASO 3: Agregar o actualizar remuneración usando método domain
        // El método domain valida automáticamente:
        // - Número entre 1 y 3
        // - Descripción no vacía
        // - Monto no negativo
        empleado.AgregarRemuneracionExtra(
            numero: request.Numero,
            descripcion: request.Descripcion,
            monto: request.Monto
        );

        // PASO 4: Guardar cambios
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
