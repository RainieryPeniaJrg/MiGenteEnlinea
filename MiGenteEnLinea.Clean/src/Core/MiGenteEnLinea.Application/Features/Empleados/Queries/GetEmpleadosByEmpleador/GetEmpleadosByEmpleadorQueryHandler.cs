using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Common.Models;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetEmpleadosByEmpleador;

/// <summary>
/// Handler para GetEmpleadosByEmpleadorQuery.
/// Legacy: EmpleadosService.getEmpleados(userID) + colaboradores.aspx.cs GetColaboradores WebMethod
/// Implementa paginación, búsqueda y filtrado por estado.
/// </summary>
public class GetEmpleadosByEmpleadorQueryHandler 
    : IRequestHandler<GetEmpleadosByEmpleadorQuery, PaginatedList<EmpleadoListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetEmpleadosByEmpleadorQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<EmpleadoListDto>> Handle(
        GetEmpleadosByEmpleadorQuery request, 
        CancellationToken cancellationToken)
    {
        // Query base
        var query = _context.Empleados
            .AsNoTracking()
            .Where(e => e.UserId == request.UserId);

        // Filtro por estado activo (si se especifica)
        if (request.SoloActivos.HasValue)
        {
            query = query.Where(e => e.Activo == request.SoloActivos.Value);
        }

        // Búsqueda por término (nombre, apellido o identificación)
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTermLower = request.SearchTerm.ToLower();
            query = query.Where(e => 
                e.Nombre.ToLower().Contains(searchTermLower) ||
                e.Apellido.ToLower().Contains(searchTermLower) ||
                e.Identificacion.Contains(request.SearchTerm)
            );
        }

        // Ordenar por fecha de registro descendente (más recientes primero)
        query = query.OrderByDescending(e => e.FechaRegistro);

        // Proyección a DTO
        var dtoQuery = query.Select(e => new EmpleadoListDto
        {
            EmpleadoId = e.EmpleadoId,
            Identificacion = e.Identificacion,
            Nombre = e.Nombre,
            Apellido = e.Apellido,
            Posicion = e.Posicion,
            Salario = e.Salario,
            PeriodoPago = e.PeriodoPago,
            DiasPago = e.DiasPago,
            FechaInicio = e.FechaInicio,
            Activo = e.Activo,
            Foto = e.Foto,
            FechaSalida = e.FechaSalida
        });

        // Aplicar paginación
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await dtoQuery
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<EmpleadoListDto>(
            items, 
            totalCount, 
            request.PageIndex, 
            request.PageSize
        );
    }
}
