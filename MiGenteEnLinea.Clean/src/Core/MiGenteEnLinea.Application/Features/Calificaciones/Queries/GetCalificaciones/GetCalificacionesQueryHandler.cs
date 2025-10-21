using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetCalificaciones;

public class GetCalificacionesQueryHandler : IRequestHandler<GetCalificacionesQuery, List<CalificacionVistaDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetCalificacionesQueryHandler> _logger;

    public GetCalificacionesQueryHandler(
        IApplicationDbContext context,
        ILogger<GetCalificacionesQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<CalificacionVistaDto>> Handle(GetCalificacionesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Obteniendo calificaciones - Identificacion: {Identificacion}, UserId: {UserId}",
            request.Identificacion,
            request.UserId);

        // TODO: Implementar lógica real cuando exista vista o join
        // Por ahora retornamos lista vacía
        _logger.LogWarning("GetCalificaciones not fully implemented - returning empty list");
        
        return new List<CalificacionVistaDto>();
    }
}
