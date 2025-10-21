using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Calificaciones;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Commands.CreateCalificacion;

/// <summary>
/// Handler: Crear nueva calificación con 4 dimensiones
/// Usa Domain.Calificacion.Create() para encapsulación y domain events
/// </summary>
public class CreateCalificacionCommandHandler : IRequestHandler<CreateCalificacionCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CreateCalificacionCommandHandler> _logger;

    public CreateCalificacionCommandHandler(
        IApplicationDbContext context,
        ILogger<CreateCalificacionCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> Handle(CreateCalificacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creando calificación: Empleador {EmpleadorUserId} califica a {ContratistaIdentificacion} ({ContratistaNombre}). " +
            "Puntualidad={Puntualidad}, Cumplimiento={Cumplimiento}, Conocimientos={Conocimientos}, Recomendacion={Recomendacion}",
            request.EmpleadorUserId,
            request.ContratistaIdentificacion,
            request.ContratistaNombre,
            request.Puntualidad,
            request.Cumplimiento,
            request.Conocimientos,
            request.Recomendacion
        );

        // Validar que no exista una calificación duplicada del mismo empleador al mismo contratista
        var existeCalificacion = await _context.Calificaciones
            .AnyAsync(
                c => c.EmpleadorUserId == request.EmpleadorUserId && 
                     c.ContratistaIdentificacion == request.ContratistaIdentificacion,
                cancellationToken
            );

        if (existeCalificacion)
        {
            _logger.LogWarning(
                "Empleador {EmpleadorUserId} ya ha calificado a {ContratistaIdentificacion}. Calificación duplicada rechazada.",
                request.EmpleadorUserId,
                request.ContratistaIdentificacion
            );
            throw new InvalidOperationException(
                "Ya has calificado a esta persona. Las calificaciones son inmutables y no se pueden modificar."
            );
        }

        // Usar el Factory Method del Domain para crear la calificación
        // Esto encapsula las validaciones y levanta domain events
        var calificacion = Calificacion.Create(
            empleadorUserId: request.EmpleadorUserId,
            contratistaIdentificacion: request.ContratistaIdentificacion,
            contratistaNombre: request.ContratistaNombre,
            puntualidad: request.Puntualidad,
            cumplimiento: request.Cumplimiento,
            conocimientos: request.Conocimientos,
            recomendacion: request.Recomendacion
        );

        _context.Calificaciones.Add(calificacion);
        await _context.SaveChangesAsync(cancellationToken);

        var promedio = calificacion.ObtenerPromedioGeneral();
        var categoria = calificacion.ObtenerCategoria();

        _logger.LogInformation(
            "Calificación creada exitosamente: ID={CalificacionId}, Promedio={Promedio:F2}, Categoría={Categoria}",
            calificacion.Id,
            promedio,
            categoria
        );

        return calificacion.Id;
    }
}
