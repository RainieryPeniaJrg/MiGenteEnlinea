using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleadores.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleadores.Queries.GetEmpleadorById;

/// <summary>
/// Handler: Obtiene un Empleador por EmpleadorId
/// </summary>
public sealed class GetEmpleadorByIdQueryHandler : IRequestHandler<GetEmpleadorByIdQuery, EmpleadorDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetEmpleadorByIdQueryHandler> _logger;

    public GetEmpleadorByIdQueryHandler(
        IApplicationDbContext context,
        ILogger<GetEmpleadorByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<EmpleadorDto?> Handle(GetEmpleadorByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando empleador por EmpleadorId: {EmpleadorId}", request.EmpleadorId);

        var empleador = await _context.Empleadores
            .AsNoTracking()
            .Where(e => e.Id == request.EmpleadorId)
            .Select(e => new EmpleadorDto
            {
                EmpleadorId = e.Id,
                UserId = e.UserId,
                FechaPublicacion = e.FechaPublicacion,
                Habilidades = e.Habilidades,
                Experiencia = e.Experiencia,
                Descripcion = e.Descripcion,
                TieneFoto = e.Foto != null && e.Foto.Length > 0,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (empleador == null)
        {
            _logger.LogWarning("Empleador no encontrado. EmpleadorId: {EmpleadorId}", request.EmpleadorId);
            return null;
        }

        return empleador;
    }
}
