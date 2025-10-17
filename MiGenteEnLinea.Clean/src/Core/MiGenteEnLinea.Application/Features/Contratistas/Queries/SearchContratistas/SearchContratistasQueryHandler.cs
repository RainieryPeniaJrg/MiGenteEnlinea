using MediatR;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Features.Contratistas.Common;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Contratistas;

namespace MiGenteEnLinea.Application.Features.Contratistas.Queries.SearchContratistas;

/// <summary>
/// Handler: Busca contratistas con filtros y paginación
/// </summary>
public class SearchContratistasQueryHandler : IRequestHandler<SearchContratistasQuery, SearchContratistasResult>
{
    private readonly IContratistaRepository _contratistaRepository;
    private readonly ILogger<SearchContratistasQueryHandler> _logger;

    public SearchContratistasQueryHandler(
        IContratistaRepository contratistaRepository,
        ILogger<SearchContratistasQueryHandler> logger)
    {
        _contratistaRepository = contratistaRepository;
        _logger = logger;
    }

    public async Task<SearchContratistasResult> Handle(SearchContratistasQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Buscando contratistas. SearchTerm: {SearchTerm}, Provincia: {Provincia}, PageIndex: {PageIndex}",
            request.SearchTerm, request.Provincia, request.PageIndex);

        // Validar y ajustar PageSize
        var pageSize = request.PageSize;
        if (pageSize > 100) pageSize = 100;
        if (pageSize < 1) pageSize = 10;

        // Validar y ajustar PageIndex
        var pageIndex = request.PageIndex;
        if (pageIndex < 1) pageIndex = 1;

        // BUSCAR usando Repository con proyección DTO
        var (contratistas, totalRecords) = await _contratistaRepository.SearchProjectedAsync<ContratistaDto>(
            searchTerm: request.SearchTerm,
            provincia: request.Provincia,
            sector: request.Sector,
            experienciaMinima: request.ExperienciaMinima,
            soloActivos: request.SoloActivos,
            pageNumber: pageIndex,
            pageSize: pageSize,
            selector: c => new ContratistaDto
            {
                ContratistaId = c.Id,
                UserId = c.UserId,
                FechaIngreso = c.FechaIngreso,
                Titulo = c.Titulo,
                Tipo = c.Tipo,
                Identificacion = c.Identificacion,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                NombreCompleto = $"{c.Nombre} {c.Apellido}",
                Sector = c.Sector,
                Experiencia = c.Experiencia,
                Presentacion = c.Presentacion,
                Telefono1 = c.Telefono1,
                Whatsapp1 = c.Whatsapp1,
                Telefono2 = c.Telefono2,
                Whatsapp2 = c.Whatsapp2,
                Email = c.Email != null ? c.Email.Value : null,
                Activo = c.Activo,
                Provincia = c.Provincia,
                NivelNacional = c.NivelNacional,
                ImagenUrl = c.ImagenUrl,
                // Campos calculados
                TieneWhatsApp = (c.Telefono1 != null && c.Whatsapp1) || (c.Telefono2 != null && c.Whatsapp2),
                PerfilCompleto = !string.IsNullOrWhiteSpace(c.UserId) &&
                                 !string.IsNullOrWhiteSpace(c.Nombre) &&
                                 !string.IsNullOrWhiteSpace(c.Apellido) &&
                                 !string.IsNullOrWhiteSpace(c.Titulo) &&
                                 !string.IsNullOrWhiteSpace(c.Presentacion) &&
                                 !string.IsNullOrWhiteSpace(c.Telefono1) &&
                                 !string.IsNullOrWhiteSpace(c.Provincia),
                PuedeRecibirTrabajos = c.Activo &&
                                       !string.IsNullOrWhiteSpace(c.Telefono1) &&
                                       (!string.IsNullOrWhiteSpace(c.Presentacion) || !string.IsNullOrWhiteSpace(c.Titulo))
            },
            ct: cancellationToken);

        _logger.LogInformation(
            "Búsqueda completada. Total: {TotalRecords}, Página: {PageIndex}/{TotalPages}",
            totalRecords, pageIndex, (int)Math.Ceiling(totalRecords / (double)pageSize));

        return new SearchContratistasResult(contratistas.ToList(), totalRecords, pageIndex, pageSize);
    }
}
