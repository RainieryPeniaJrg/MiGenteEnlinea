using System;
using System.Collections.Generic;

namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// DTO para recibo de pago detallado con header y líneas de percepciones/deducciones.
/// Usado por GetReciboByIdQuery.
/// </summary>
public record ReciboDetalleDto
{
    // Header
    public int PagoId { get; init; }
    public int EmpleadoId { get; init; }
    public string EmpleadoNombre { get; init; } = null!;
    public string UserId { get; init; } = null!;
    public DateTime? FechaPago { get; init; } // Nullable porque puede estar pendiente de pago
    public DateTime FechaRegistro { get; init; }
    public string? Comentarios { get; init; }
    public int Estado { get; init; } // 1=Pendiente, 2=Pagado, 3=Anulado
    public string? MotivoAnulacion { get; init; }

    // Totales
    public decimal TotalPercepciones { get; init; }
    public decimal TotalDeducciones { get; init; }
    public decimal NetoPagar { get; init; }

    // Detalles
    public List<ReciboLineaDto> Percepciones { get; init; } = new();
    public List<ReciboLineaDto> Deducciones { get; init; } = new();
}

/// <summary>
/// Línea individual de recibo (percepción o deducción).
/// </summary>
public record ReciboLineaDto
{
    public int DetalleId { get; init; }
    public string Descripcion { get; init; } = null!;
    public decimal Monto { get; init; }
}
