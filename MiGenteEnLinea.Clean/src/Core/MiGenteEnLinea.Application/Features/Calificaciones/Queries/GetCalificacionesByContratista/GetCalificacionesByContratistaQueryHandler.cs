using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Common.Models;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetCalificacionesByContratista;

/// <summary>
/// Handler: Obtener calificaciones paginadas de un contratista
/// Lógica copiada de: CalificacionesService.getById() + View VCalificaciones
/// </summary>
public class GetCalificacionesByContratistaQueryHandler 
    : IRequestHandler<GetCalificacionesByContratistaQuery, PaginatedList<CalificacionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCalificacionesByContratistaQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CalificacionDto>> Handle(
        GetCalificacionesByContratistaQuery request, 
        CancellationToken cancellationToken)
    {
        // Lógica EXACTA del Legacy CalificacionesService.getById()
        var query = _context.Calificaciones
            .Where(c => c.ContratistaIdentificacion == request.Identificacion);

        // Filtro opcional por userId (Legacy: if (userID != null))
        if (!string.IsNullOrEmpty(request.UserId))
        {
            query = query.Where(c => c.EmpleadorUserId == request.UserId);
        }

        // Ordenamiento (Legacy: OrderByDescending(x => x.calificacionID))
        // Por defecto ordena por fecha descendente (más recientes primero)
        // Para ordenar por "rating", se usa el promedio general calculado
        query = (request.OrderBy?.ToLower(), request.OrderDirection?.ToLower()) switch
        {
            ("fecha", "asc") => query.OrderBy(c => c.Fecha).ThenByDescending(c => c.Id),
            ("fecha", "desc") or ("fecha", _) => query.OrderByDescending(c => c.Fecha).ThenByDescending(c => c.Id),
            _ => query.OrderByDescending(c => c.Id) // Default: más recientes primero
        };

        // Proyectar a DTO y paginar
        return await query
            .ProjectTo<CalificacionDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
