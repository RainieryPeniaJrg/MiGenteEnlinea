using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.DesactivarEmpleado;

/// <summary>
/// Handler para DesactivarEmpleadoCommand.
/// Legacy: EmpleadosService.darDeBaja(empleadoID, userID, fechaBaja, prestaciones, motivo)
/// Implementa soft delete con metadata completa de la baja.
/// </summary>
public class DesactivarEmpleadoCommandHandler : IRequestHandler<DesactivarEmpleadoCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DesactivarEmpleadoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DesactivarEmpleadoCommand request, CancellationToken cancellationToken)
    {
        // PASO 1: Obtener empleado
        var empleado = await _context.Empleados
            .FirstOrDefaultAsync(e => e.EmpleadoId == request.EmpleadoId && 
                                     e.UserId == request.UserId,
                                cancellationToken);

        if (empleado == null)
        {
            throw new NotFoundException("Empleado", request.EmpleadoId);
        }

        // PASO 2: Verificar que el empleado esté activo
        if (!empleado.Activo)
        {
            throw new ValidationException("El empleado ya está inactivo");
        }

        // PASO 3: Desactivar usando método domain
        // Este método actualiza: Activo=false, FechaSalida, MotivoBaja, Prestaciones
        // y lanza el evento EmpleadoDesactivadoEvent
        // Nota: La fecha de baja se calcula automáticamente en el dominio (DateTime.UtcNow)
        empleado.Desactivar(
            motivoBaja: request.MotivoBaja,
            prestaciones: request.Prestaciones
        );

        // PASO 4: Guardar cambios
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
