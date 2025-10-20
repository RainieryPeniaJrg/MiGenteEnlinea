using MediatR;

namespace MiGenteEnLinea.Application.Features.Nominas.Commands.ExportarNominaCsv;

/// <summary>
/// Command CQRS: Exporta nómina a formato CSV
/// </summary>
public record ExportarNominaCsvCommand : IRequest<ExportarNominaCsvResult>
{
    /// <summary>
    /// UserId del empleador
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Período de pago (YYYY-MM)
    /// </summary>
    public string Periodo { get; init; } = string.Empty;

    /// <summary>
    /// Incluir recibos anulados
    /// </summary>
    public bool IncluirAnulados { get; init; }
}

/// <summary>
/// Resultado del export CSV
/// </summary>
public record ExportarNominaCsvResult
{
    public byte[] FileContent { get; init; } = Array.Empty<byte>();
    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = "text/csv";
    public int TotalRecibos { get; init; }
}
