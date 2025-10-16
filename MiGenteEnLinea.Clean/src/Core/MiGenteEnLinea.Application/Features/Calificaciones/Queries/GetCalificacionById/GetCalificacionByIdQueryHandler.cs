using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetCalificacionById;

/// <summary>
/// Handler: Obtener una calificación por ID
/// Lógica copiada de: CalificacionesService.getCalificacionByID()
/// </summary>
public class GetCalificacionByIdQueryHandler 
    : IRequestHandler<GetCalificacionByIdQuery, CalificacionDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCalificacionByIdQueryHandler> _logger;

    public GetCalificacionByIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ILogger<GetCalificacionByIdQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CalificacionDto?> Handle(
        GetCalificacionByIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Obteniendo calificación con ID: {CalificacionId}", 
            request.CalificacionId);

        // Lógica EXACTA del Legacy: 
        // db.Calificaciones.Where(x => x.calificacionID == calificacionID)
        //   .OrderByDescending(x => x.calificacionID).FirstOrDefaultAsync()
        //
        // Nota: OrderByDescending es redundante aquí (solo 1 registro con ese ID)
        // pero mantenemos el comportamiento Legacy por compatibilidad
        var calificacion = await _context.Calificaciones
            .Where(c => c.Id == request.CalificacionId)
            .OrderByDescending(c => c.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (calificacion == null)
        {
            _logger.LogWarning(
                "Calificación no encontrada con ID: {CalificacionId}", 
                request.CalificacionId);
            return null;
        }

        var dto = _mapper.Map<CalificacionDto>(calificacion);

        _logger.LogInformation(
            "Calificación encontrada: ID={Id}, Promedio={Promedio}, Categoria={Categoria}, Empleador={EmpleadorUserId}", 
            dto.CalificacionId, 
            dto.PromedioGeneral,
            dto.Categoria,
            dto.EmpleadorUserId);

        return dto;
    }
}
