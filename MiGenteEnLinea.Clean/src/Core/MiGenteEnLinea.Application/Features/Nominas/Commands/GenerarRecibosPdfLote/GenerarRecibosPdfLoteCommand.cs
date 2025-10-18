using MediatR;

namespace MiGenteEnLinea.Application.Features.Nominas.Commands.GenerarRecibosPdfLote;

/// <summary>
/// Command para generar recibos de nómina en PDF en lote.
/// Utiliza PdfService de LOTE 5.3 para generación masiva.
/// </summary>
public record GenerarRecibosPdfLoteCommand : IRequest<GenerarRecibosPdfLoteResult>
{
    /// <summary>
    /// Lista de IDs de recibos a generar en PDF
    /// </summary>
    public List<int> ReciboIds { get; init; } = new();

    /// <summary>
    /// Ruta donde guardar los PDFs (opcional, por defecto: temp)
    /// </summary>
    public string? RutaDestino { get; init; }

    /// <summary>
    /// Comprimir los PDFs en un archivo ZIP
    /// </summary>
    public bool ComprimirEnZip { get; init; } = true;

    /// <summary>
    /// Nombre del archivo ZIP (si ComprimirEnZip = true)
    /// </summary>
    public string? NombreArchivoZip { get; init; }
}

/// <summary>
/// Resultado de la generación de PDFs en lote
/// </summary>
public record GenerarRecibosPdfLoteResult
{
    public int PdfsGenerados { get; init; }
    public int PdfsError { get; init; }
    public string? RutaArchivoZip { get; init; }
    public List<string> RutasArchivos { get; init; } = new();
    public List<string> Errores { get; init; } = new();
    public bool Exitoso => PdfsError == 0;
}
