using MediatR;
using MiGenteEnLinea.Application.Features.Contratistas.Common;

namespace MiGenteEnLinea.Application.Features.Contratistas.Queries.SearchContratistas;

/// <summary>
/// Query: Busca contratistas con filtros y paginación
/// </summary>
/// <remarks>
/// LÓGICA LEGACY: 
/// - ContratistasService.getTodasUltimos20() → sin filtros, top 20
/// - ContratistasService.getConCriterio(palabrasClave, zona) → con filtros
/// FUNCIONALIDAD: Permite búsqueda con múltiples criterios
/// </remarks>
/// <param name="SearchTerm">Término de búsqueda (busca en Titulo, Presentacion, Sector)</param>
/// <param name="Provincia">Filtro por provincia (null o "Cualquier Ubicacion" = sin filtro)</param>
/// <param name="Sector">Filtro por sector económico (null = sin filtro)</param>
/// <param name="ExperienciaMinima">Años mínimos de experiencia (null = sin filtro)</param>
/// <param name="SoloActivos">Filtrar solo contratistas activos (default: true)</param>
/// <param name="PageIndex">Número de página (1-based, default: 1)</param>
/// <param name="PageSize">Cantidad de resultados por página (default: 10, max: 100)</param>
public record SearchContratistasQuery(
    string? SearchTerm = null,
    string? Provincia = null,
    string? Sector = null,
    int? ExperienciaMinima = null,
    bool SoloActivos = true,
    int PageIndex = 1,
    int PageSize = 10
) : IRequest<SearchContratistasResult>;

/// <summary>
/// Resultado de la búsqueda con metadatos de paginación
/// </summary>
/// <param name="Contratistas">Lista de contratistas encontrados</param>
/// <param name="TotalRecords">Total de registros que cumplen con los filtros</param>
/// <param name="PageIndex">Página actual</param>
/// <param name="PageSize">Tamaño de página</param>
public record SearchContratistasResult(
    List<ContratistaDto> Contratistas,
    int TotalRecords,
    int PageIndex,
    int PageSize
)
{
    /// <summary>
    /// Total de páginas disponibles (calculado)
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);

    /// <summary>
    /// ¿Hay página anterior?
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>
    /// ¿Hay página siguiente?
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;
}
