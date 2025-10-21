using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetPromedioCalificacion;

/// <summary>
/// Handler: Calcular promedio y distribución de calificaciones
/// Feature NUEVA: Agregación estadística (no existe en Legacy)
/// </summary>
public class GetPromedioCalificacionQueryHandler 
    : IRequestHandler<GetPromedioCalificacionQuery, PromedioCalificacionDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetPromedioCalificacionQueryHandler> _logger;

    public GetPromedioCalificacionQueryHandler(
        IApplicationDbContext context,
        ILogger<GetPromedioCalificacionQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PromedioCalificacionDto?> Handle(
        GetPromedioCalificacionQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Calculando promedio de calificaciones para Identificacion: {Identificacion}", 
            request.Identificacion);

        // Obtener todas las calificaciones de esta persona
        var calificaciones = await _context.Calificaciones
            .Where(c => c.ContratistaIdentificacion == request.Identificacion)
            .ToListAsync(cancellationToken);

        // Si no hay calificaciones, retornar null
        if (!calificaciones.Any())
        {
            _logger.LogInformation(
                "No se encontraron calificaciones para Identificacion: {Identificacion}", 
                request.Identificacion);
            return null;
        }

        // Calcular estadísticas
        var totalCalificaciones = calificaciones.Count;
        
        // El promedio se calcula sobre el PromedioGeneral de cada calificación
        // (que es el promedio de las 4 dimensiones)
        var promedioGeneral = calificaciones.Average(c => c.ObtenerPromedioGeneral());

        // Distribución por estrellas (redondeando el promedio general)
        var calificaciones1Estrella = calificaciones.Count(c => Math.Round(c.ObtenerPromedioGeneral()) == 1);
        var calificaciones2Estrellas = calificaciones.Count(c => Math.Round(c.ObtenerPromedioGeneral()) == 2);
        var calificaciones3Estrellas = calificaciones.Count(c => Math.Round(c.ObtenerPromedioGeneral()) == 3);
        var calificaciones4Estrellas = calificaciones.Count(c => Math.Round(c.ObtenerPromedioGeneral()) == 4);
        var calificaciones5Estrellas = calificaciones.Count(c => Math.Round(c.ObtenerPromedioGeneral()) == 5);

        var dto = new PromedioCalificacionDto
        {
            Identificacion = request.Identificacion,
            PromedioGeneral = promedioGeneral,
            TotalCalificaciones = totalCalificaciones,
            Calificaciones1Estrella = calificaciones1Estrella,
            Calificaciones2Estrellas = calificaciones2Estrellas,
            Calificaciones3Estrellas = calificaciones3Estrellas,
            Calificaciones4Estrellas = calificaciones4Estrellas,
            Calificaciones5Estrellas = calificaciones5Estrellas
        };

        _logger.LogInformation(
            "Promedio calculado: {Promedio:F2} estrellas ({Total} calificaciones). " +
            "Distribución: 5★={C5}, 4★={C4}, 3★={C3}, 2★={C2}, 1★={C1}. " +
            "Positivas={PorcentajePositivas:F1}%, Negativas={PorcentajeNegativas:F1}%",
            dto.PromedioGeneral, 
            dto.TotalCalificaciones,
            calificaciones5Estrellas,
            calificaciones4Estrellas,
            calificaciones3Estrellas,
            calificaciones2Estrellas,
            calificaciones1Estrella,
            dto.PorcentajePositivas,
            dto.PorcentajeNegativas);

        return dto;
    }
}
