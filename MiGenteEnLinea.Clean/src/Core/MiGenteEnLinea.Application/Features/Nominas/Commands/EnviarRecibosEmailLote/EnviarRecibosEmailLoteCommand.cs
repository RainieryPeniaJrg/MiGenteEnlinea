using MediatR;

namespace MiGenteEnLinea.Application.Features.Nominas.Commands.EnviarRecibosEmailLote;

/// <summary>
/// Command para enviar recibos de nómina por email en lote.
/// 
/// CASOS DE USO:
/// - Empleador procesa nómina y envía recibos a todos los empleados automáticamente
/// - Reenvío de recibos específicos que fallaron en envío anterior
/// - Envío programado de recibos (scheduler job)
/// 
/// WORKFLOW:
/// 1. Valida que todos los recibos existen
/// 2. Genera PDF de cada recibo (reutiliza GenerarRecibosPdfLoteCommand logic)
/// 3. Obtiene email de cada empleado
/// 4. Envía email con PDF adjunto
/// 5. Marca recibos como enviados (opcional - usando Notas field)
/// 6. Retorna contadores de éxito/fallos con errores detallados
/// </summary>
public record EnviarRecibosEmailLoteCommand : IRequest<EnviarRecibosEmailLoteResult>
{
    /// <summary>
    /// IDs de los recibos a enviar por email
    /// </summary>
    public List<int> ReciboIds { get; init; } = new();

    /// <summary>
    /// Asunto del email (opcional - usa default si no se provee)
    /// </summary>
    public string? Asunto { get; init; }

    /// <summary>
    /// Mensaje adicional en el cuerpo del email (opcional)
    /// </summary>
    public string? MensajeAdicional { get; init; }

    /// <summary>
    /// Incluir detalle completo en el PDF (ingresos y deducciones)
    /// </summary>
    public bool IncluirDetalleCompleto { get; init; } = true;

    /// <summary>
    /// Copiar al empleador en el email (BCC)
    /// </summary>
    public bool CopiarEmpleador { get; init; } = false;
}

/// <summary>
/// Resultado del envío masivo de recibos por email
/// </summary>
public record EnviarRecibosEmailLoteResult
{
    /// <summary>
    /// Cantidad de emails enviados exitosamente
    /// </summary>
    public int EmailsEnviados { get; set; }

    /// <summary>
    /// Cantidad de emails que fallaron
    /// </summary>
    public int EmailsFallidos { get; set; }

    /// <summary>
    /// Lista de recibos procesados con status
    /// </summary>
    public List<ReciboEmailDto> RecibosEnviados { get; set; } = new();

    /// <summary>
    /// Lista de errores detallados por recibo
    /// </summary>
    public List<string> Errores { get; set; } = new();

    /// <summary>
    /// Indica si todos los envíos fueron exitosos
    /// </summary>
    public bool Exitoso => Errores.Count == 0;

    /// <summary>
    /// Tamaño total de PDFs enviados (en bytes)
    /// </summary>
    public long TotalBytesEnviados { get; set; }
}

/// <summary>
/// DTO con información de un recibo enviado por email
/// </summary>
public record ReciboEmailDto
{
    public int ReciboId { get; init; }
    public int EmpleadoId { get; init; }
    public string EmpleadoNombre { get; init; } = string.Empty;
    public string EmpleadoEmail { get; init; } = string.Empty;
    public bool Enviado { get; init; }
    public DateTime? FechaEnvio { get; init; }
    public string? ErrorMensaje { get; init; }
    public long TamanoPdf { get; init; }
}
