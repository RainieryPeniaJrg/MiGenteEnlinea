using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Empleados.Commands.RemoveRemuneracion;

/// <summary>
/// Handler para RemoveRemuneracionCommand.
/// Legacy: EmpleadosService.quitarRemuneracion(userID, id)
/// Elimina una remuneración extra de uno de los 3 slots disponibles.
/// </summary>
public class RemoveRemuneracionCommandHandler : IRequestHandler<RemoveRemuneracionCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public RemoveRemuneracionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RemoveRemuneracionCommand request, CancellationToken cancellationToken)
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

        // PASO 2: Eliminar remuneración usando método domain
        // El método domain valida automáticamente que el número esté entre 1 y 3
        // y limpia el slot (descripción y monto = null)
        empleado.EliminarRemuneracionExtra(request.Numero);

        // PASO 3: Guardar cambios
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
