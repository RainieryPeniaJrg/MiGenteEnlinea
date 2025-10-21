using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetTodasCalificaciones;

public class GetTodasCalificacionesQueryHandler : IRequestHandler<GetTodasCalificacionesQuery, List<CalificacionVistaDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetTodasCalificacionesQueryHandler> _logger;

    public GetTodasCalificacionesQueryHandler(
        IApplicationDbContext context,
        ILogger<GetTodasCalificacionesQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las calificaciones
    /// Replica CalificacionesService.getTodas()
    /// </summary>
    public async Task<List<CalificacionVistaDto>> Handle(GetTodasCalificacionesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obteniendo todas las calificaciones");

        // TODO: Implementar cuando vista VCalificaciones exista o crear join apropiado
        // Legacy: return db.VCalificaciones.ToList();
        
        _logger.LogWarning("GetTodasCalificaciones no completamente implementado - retornando lista vacía");
        
        return new List<CalificacionVistaDto>();
    }
}
