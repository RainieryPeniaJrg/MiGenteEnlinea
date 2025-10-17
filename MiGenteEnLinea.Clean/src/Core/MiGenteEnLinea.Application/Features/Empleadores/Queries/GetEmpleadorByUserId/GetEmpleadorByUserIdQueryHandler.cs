using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleadores.DTOs;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Empleadores;

namespace MiGenteEnLinea.Application.Features.Empleadores.Queries.GetEmpleadorByUserId;

/// <summary>
/// Handler: Obtiene un Empleador por UserId
/// </summary>
public sealed class GetEmpleadorByUserIdQueryHandler : IRequestHandler<GetEmpleadorByUserIdQuery, EmpleadorDto?>
{
    private readonly IEmpleadorRepository _empleadorRepository;
    private readonly ILogger<GetEmpleadorByUserIdQueryHandler> _logger;

    public GetEmpleadorByUserIdQueryHandler(
        IEmpleadorRepository empleadorRepository,
        ILogger<GetEmpleadorByUserIdQueryHandler> logger)
    {
        _empleadorRepository = empleadorRepository;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la consulta del empleador por userId
    /// </summary>
    public async Task<EmpleadorDto?> Handle(GetEmpleadorByUserIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando empleador por userId: {UserId}", request.UserId);

        var empleador = await _empleadorRepository.GetByUserIdProjectedAsync(
            request.UserId,
            e => new EmpleadorDto
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
            },
            cancellationToken);

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
