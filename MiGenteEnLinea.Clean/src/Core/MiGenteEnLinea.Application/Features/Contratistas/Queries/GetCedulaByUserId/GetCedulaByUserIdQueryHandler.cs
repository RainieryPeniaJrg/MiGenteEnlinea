using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Application.Features.Contratistas.Queries.GetCedulaByUserId;

/// <summary>
/// Handler para GetCedulaByUserIdQuery
/// Réplica EXACTA de SuscripcionesService.obtenerCedula() del Legacy
/// GAP-013: Query simple que retorna identificación del contratista
/// </summary>
public sealed class GetCedulaByUserIdQueryHandler : IRequestHandler<GetCedulaByUserIdQuery, string?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCedulaByUserIdQueryHandler> _logger;

    public GetCedulaByUserIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetCedulaByUserIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene la cédula/identificación de un contratista por userId
    /// 
    /// Legacy behavior (SuscripcionesService.cs líneas 177-181):
    /// - Query: db.Contratistas.Where(x => x.userID == userID).Select(x => x.identificacion).FirstOrDefault()
    /// - Retorna string? (puede ser null si no existe)
    /// 
    /// Clean behavior:
    /// - Usa IUnitOfWork.Contratistas
    /// - Retorna identificacion (puede ser null)
    /// </summary>
    public async Task<string?> Handle(GetCedulaByUserIdQuery request, CancellationToken cancellationToken)
    {
        // ================================================================================
        // PASO 1: QUERY CONTRATISTA POR USERID
        // ================================================================================
        // Legacy línea 179: db.Contratistas.Where(x => x.userID == userID).Select(x => x.identificacion).FirstOrDefault()
        var contratistas = await _unitOfWork.Contratistas.GetAllAsync(cancellationToken);
        var cedula = contratistas
            .Where(c => c.UserId == request.UserId)
            .Select(c => c.Identificacion)
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(cedula))
        {
            _logger.LogInformation(
                "No se encontró cédula para contratista. UserId: {UserId}",
                request.UserId);
            return null;
        }

        _logger.LogInformation(
            "Cédula obtenida exitosamente. UserId: {UserId}",
            request.UserId);

        return cedula;
    }
}
