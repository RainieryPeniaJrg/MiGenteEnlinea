using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetRemuneracionesByEmpleado;

/// <summary>
/// Handler para GetRemuneracionesByEmpleadoQuery.
/// Legacy: EmpleadosService.obtenerRemuneraciones(userID, empleadoID)
/// Retorna los 3 slots de remuneraciones extras del empleado.
/// </summary>
public class GetRemuneracionesByEmpleadoQueryHandler : IRequestHandler<GetRemuneracionesByEmpleadoQuery, List<RemuneracionSlotDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRemuneracionesByEmpleadoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RemuneracionSlotDto>> Handle(GetRemuneracionesByEmpleadoQuery request, CancellationToken cancellationToken)
    {
        // PASO 1: Buscar empleado y validar propiedad
        var empleado = await _context.Empleados
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EmpleadoId == request.EmpleadoId && 
                                     e.UserId == request.UserId,
                                cancellationToken);

        if (empleado == null)
        {
            throw new NotFoundException("Empleado", request.EmpleadoId);
        }

        // PASO 2: Construir lista con los 3 slots de remuneraciones
        // Siempre retorna 3 elementos, incluso si están vacíos
        var remuneraciones = new List<RemuneracionSlotDto>
        {
            new RemuneracionSlotDto
            {
                Numero = 1,
                Descripcion = empleado.RemuneracionExtra1,
                Monto = empleado.MontoExtra1
            },
            new RemuneracionSlotDto
            {
                Numero = 2,
                Descripcion = empleado.RemuneracionExtra2,
                Monto = empleado.MontoExtra2
            },
            new RemuneracionSlotDto
            {
                Numero = 3,
                Descripcion = empleado.RemuneracionExtra3,
                Monto = empleado.MontoExtra3
            }
        };

        return remuneraciones;
    }
}
