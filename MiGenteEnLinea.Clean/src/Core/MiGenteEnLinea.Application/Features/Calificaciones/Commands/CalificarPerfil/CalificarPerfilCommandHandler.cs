using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Calificaciones;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Commands.CalificarPerfil;

public class CalificarPerfilCommandHandler : IRequestHandler<CalificarPerfilCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CalificarPerfilCommandHandler> _logger;

    public CalificarPerfilCommandHandler(
        IApplicationDbContext context,
        ILogger<CalificarPerfilCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> Handle(CalificarPerfilCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creando calificación - EmpleadorUserId: {EmpleadorUserId}, ContratistaIdentificacion: {ContratistaIdentificacion}",
            request.EmpleadorUserId,
            request.ContratistaIdentificacion);

        // Legacy: db.Calificaciones.Add(cal); db.SaveChanges(); return cal;
        // Domain: Usa factory method Calificacion.Create() con validaciones de negocio
        var calificacion = Calificacion.Create(
            request.EmpleadorUserId,
            request.ContratistaIdentificacion,
            request.ContratistaNombre,
            request.Puntualidad,
            request.Cumplimiento,
            request.Conocimientos,
            request.Recomendacion);

        _context.Calificaciones.Add(calificacion);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Calificación creada con ID: {CalificacionId}", calificacion.Id);

        return calificacion.Id;
    }
}
