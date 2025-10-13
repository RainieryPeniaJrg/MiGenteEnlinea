using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleadores.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleadores.Queries.GetEmpleadorByUserId;

/// <summary>
/// Handler: Obtiene un Empleador por UserId
/// </summary>
public sealed class GetEmpleadorByUserIdQueryHandler : IRequestHandler<GetEmpleadorByUserIdQuery, EmpleadorDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetEmpleadorByUserIdQueryHandler> _logger;

    public GetEmpleadorByUserIdQueryHandler(
        IApplicationDbContext context,
        ILogger<GetEmpleadorByUserIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la consulta del empleador por userId
    /// </summary>
    public async Task<EmpleadorDto?> Handle(GetEmpleadorByUserIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando empleador por userId: {UserId}", request.UserId);

        // ============================================
        // Query con AsNoTracking para optimizar lectura
        // ============================================
        var empleador = await _context.Empleadores
            .AsNoTracking()
            .Where(e => e.UserId == request.UserId)
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
            _logger.LogWarning("Empleador no encontrado para userId: {UserId}", request.UserId);
            return null;
        }

        _logger.LogInformation(
            "Empleador encontrado. EmpleadorId: {EmpleadorId}, UserId: {UserId}",
            empleador.EmpleadorId, request.UserId);

        return empleador;
    }
}
