using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;
using MiGenteEnLinea.Domain.Entities.Empleados;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetRecibosByEmpleado;

public class GetRecibosByEmpleadoQueryHandler : IRequestHandler<GetRecibosByEmpleadoQuery, GetRecibosResult>
{
    private readonly IApplicationDbContext _context;

    public GetRecibosByEmpleadoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetRecibosResult> Handle(GetRecibosByEmpleadoQuery request, CancellationToken cancellationToken)
    {
        // PASO 1: Validar que empleado existe y pertenece al empleador
        var empleadoExists = await _context.Empleados
            .AsNoTracking()
            .AnyAsync(e => e.EmpleadoId == request.EmpleadoId && e.UserId == request.UserId, cancellationToken);

        if (!empleadoExists)
            throw new NotFoundException(nameof(Empleado), request.EmpleadoId);

        // PASO 2: Construir query base
        var query = _context.RecibosHeader
            .AsNoTracking()
            .Where(r => r.EmpleadoId == request.EmpleadoId && r.UserId == request.UserId);

        // PASO 3: Filtrar por estado (solo activos si se solicita)
        if (request.SoloActivos)
        {
            query = query.Where(r => r.Estado != 3); // Excluir anulados (Estado=3)
        }

        // PASO 4: Contar total
        var totalRecords = await query.CountAsync(cancellationToken);

        // PASO 5: Paginar y ordenar (más recientes primero)
        var recibos = await query
            .OrderByDescending(r => r.FechaPago)
            .ThenByDescending(r => r.PagoId)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(r => new ReciboListDto
            {
                PagoId = r.PagoId,
                FechaPago = r.FechaPago,
                FechaRegistro = r.FechaRegistro,
                TotalPercepciones = r.TotalIngresos, // Propiedad correcta: TotalIngresos (no TotalPercepciones)
                TotalDeducciones = r.TotalDeducciones,
                NetoPagar = r.NetoPagar,
                Estado = r.Estado
            })
            .ToListAsync(cancellationToken);

        // PASO 6: Construir resultado paginado
        return new GetRecibosResult
        {
            Recibos = recibos,
            TotalRecords = totalRecords,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)request.PageSize)
        };
    }
}
