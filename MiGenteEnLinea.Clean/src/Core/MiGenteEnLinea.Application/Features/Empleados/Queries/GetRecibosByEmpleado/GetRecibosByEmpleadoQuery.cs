using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetRecibosByEmpleado;

/// <summary>
/// Query para obtener lista de recibos de pago de un empleado.
/// Mapea: EmpleadosService.GetEmpleador_Recibos_Empleado()
/// </summary>
public record GetRecibosByEmpleadoQuery : IRequest<GetRecibosResult>
{
    /// <summary>
    /// GUID del empleador (para validar propiedad)
    /// </summary>
    public string UserId { get; init; } = null!;

    /// <summary>
    /// ID del empleado
    /// </summary>
    public int EmpleadoId { get; init; }

    /// <summary>
    /// Incluir solo recibos activos (excluir anulados). Default: true
    /// </summary>
    public bool SoloActivos { get; init; } = true;

    /// <summary>
    /// Número de página (1-based)
    /// </summary>
    public int PageIndex { get; init; } = 1;

    /// <summary>
    /// Tamaño de página
    /// </summary>
    public int PageSize { get; init; } = 10;
}

/// <summary>
/// Resultado paginado de recibos.
/// </summary>
public record GetRecibosResult
{
    public List<ReciboListDto> Recibos { get; init; } = new();
    public int TotalRecords { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}

/// <summary>
/// DTO simplificado para lista de recibos.
/// </summary>
public record ReciboListDto
{
    public int PagoId { get; init; }
    public DateTime? FechaPago { get; init; } // Nullable porque puede estar pendiente
    public DateTime FechaRegistro { get; init; }
    public decimal TotalPercepciones { get; init; }
    public decimal TotalDeducciones { get; init; }
    public decimal NetoPagar { get; init; }
    public int Estado { get; init; } // 1=Pendiente, 2=Pagado, 3=Anulado
    public string EstadoDescripcion => Estado switch
    {
        1 => "Pendiente",
        2 => "Pagado",
        3 => "Anulado",
        _ => "Desconocido"
    };
}
