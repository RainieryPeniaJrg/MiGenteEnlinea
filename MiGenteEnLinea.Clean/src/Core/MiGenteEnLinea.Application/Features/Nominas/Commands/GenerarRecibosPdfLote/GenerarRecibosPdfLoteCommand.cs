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
    /// Incluir detalle completo de ingresos y deducciones
    /// </summary>
    public bool IncluirDetalleCompleto { get; init; } = true;
}

/// <summary>
/// Resultado de la generación de PDFs en lote
/// </summary>
public record GenerarRecibosPdfLoteResult
{
    public int PdfsExitosos { get; set; }
    public int PdfsFallidos { get; set; }
    public List<ReciboPdfDto> PdfsGenerados { get; set; } = new();
    public List<string> Errores { get; set; } = new();
    public bool Exitoso => Errores.Count == 0;
}

/// <summary>
/// DTO con información de un PDF generado
/// </summary>
public record ReciboPdfDto
{
    public int ReciboId { get; init; }
    public int EmpleadoId { get; init; }
    public string EmpleadoNombre { get; init; } = string.Empty;
    public byte[] PdfBytes { get; init; } = Array.Empty<byte>();
    public string Periodo { get; init; } = string.Empty;
    public DateTime FechaGeneracion { get; init; }
    public long TamanioBytes => PdfBytes.Length;
}
