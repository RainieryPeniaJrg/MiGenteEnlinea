using MediatR;

namespace MiGenteEnLinea.Application.Features.Nominas.Commands.ProcessContractPayment;

/// <summary>
/// Command para procesar pago de contratación (empleados temporales).
/// Implementa procesarPagoContratacion() del Legacy (EmpleadosService.cs línea 170-204).
/// </summary>
/// <remarks>
/// LÓGICA LEGACY EXACTA:
/// 1. Insertar Empleador_Recibos_Header_Contrataciones
/// 2. Insertar Empleador_Recibos_Detalle_Contrataciones (con pagoID del header)
/// 3. SI Concepto == "Pago Final" → UPDATE DetalleContrataciones.estatus = 2
/// 
/// GAP-005: Agregar lógica de update estatus
/// </remarks>
public record ProcessContractPaymentCommand : IRequest<int>
{
    public string UserId { get; init; } = string.Empty;
    public int ContratacionId { get; init; }
    public int DetalleId { get; init; }
    public DateTime FechaRegistro { get; init; }
    public DateTime FechaPago { get; init; }
    public string ConceptoPago { get; init; } = string.Empty;
    public int Tipo { get; init; }
    
    public List<DetalleReciboContratacion> Detalles { get; init; } = new();
}

public record DetalleReciboContratacion
{
    public string Concepto { get; init; } = string.Empty;
    public decimal Monto { get; init; }
}
