using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Pagos;
using MiGenteEnLinea.Domain.Entities.Contrataciones;

namespace MiGenteEnLinea.Application.Features.Nominas.Commands.ProcessContractPayment;

/// <summary>
/// Handler para ProcessContractPaymentCommand.
/// Implementa lógica EXACTA del Legacy: procesarPagoContratacion() en EmpleadosService.cs
/// </summary>
/// <remarks>
/// LÓGICA LEGACY (líneas 170-204 EmpleadosService.cs):
/// <code>
/// public int procesarPagoContratacion(Empleador_Recibos_Header_Contrataciones header, List<Empleador_Recibos_Detalle_Contrataciones> detalle)
/// {
///     using (var db = new migenteEntities())
///     {
///         db.Empleador_Recibos_Header_Contrataciones.Add(header);
///         db.SaveChanges();
///     }
/// 
///     using (var db1 = new migenteEntities())
///     {
///         foreach (var item in detalle)
///         {
///             item.pagoID = header.pagoID;
///         }
/// 
///         db1.Empleador_Recibos_Detalle_Contrataciones.AddRange(detalle);
///         db1.SaveChanges();
///     }
/// 
///     //update estatus
///     if (detalle.Select(x => x.Concepto).FirstOrDefault() == "Pago Final")
///     {
///         var db3 = new migenteEntities();
///         var det = db3.DetalleContrataciones.Where(X => X.contratacionID == header.contratacionID && X.detalleID==header.detalleID).FirstOrDefault();
///         if (det != null)
///         {
///             det.estatus = 2;
///             db3.SaveChanges();
///         }
///     }
/// 
///     return header.pagoID;
/// }
/// </code>
/// 
/// COMPORTAMIENTO:
/// - Inserta header primero (obtiene pagoID auto-generado)
/// - Inserta detalles con pagoID del header
/// - Si primer detalle tiene Concepto == "Pago Final" → UPDATE estatus = 2 en DetalleContrataciones
/// - Usa 3 DbContexts separados (patrón Legacy)
/// - Retorna pagoID generado
/// 
/// GAP-005 COMPLETADO: ✅ Update estatus cuando Concepto == "Pago Final"
/// 
/// SOLUCIÓN DDD: Usa métodos factory de Domain entities (EmpleadorRecibosHeaderContratacione.Crear, 
/// EmpleadorRecibosDetalleContratacione.Crear, DetalleContratacion.Completar)
/// </remarks>
public class ProcessContractPaymentCommandHandler : IRequestHandler<ProcessContractPaymentCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ProcessContractPaymentCommandHandler> _logger;

    public ProcessContractPaymentCommandHandler(
        IApplicationDbContext context,
        ILogger<ProcessContractPaymentCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> Handle(ProcessContractPaymentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Procesando pago de contratación. ContratacionID: {ContratacionId}, DetalleID: {DetalleId}, ConceptoPago: {ConceptoPago}",
            request.ContratacionId, request.DetalleId, request.ConceptoPago);

        // PASO 1: Insertar Header usando método factory (LÓGICA LEGACY - primer DbContext)
        var header = EmpleadorRecibosHeaderContratacione.Crear(
            userId: request.UserId,
            contratacionId: request.ContratacionId,
            conceptoPago: request.ConceptoPago ?? "Pago de contratación",
            tipo: request.Tipo);

        // Registrar fecha de pago (siempre viene en el request)
        header.RegistrarFechaPago(request.FechaPago);

        _context.EmpleadorRecibosHeaderContrataciones.Add(header);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Header de recibo creado. PagoID: {PagoId}", header.PagoId);

        // PASO 2: Insertar Detalles usando método factory (LÓGICA LEGACY - segundo DbContext)
        var detalles = request.Detalles.Select(d =>
            EmpleadorRecibosDetalleContratacione.Crear(
                pagoId: header.PagoId, // Asignar pagoID generado del header
                concepto: d.Concepto ?? "Concepto de pago",
                monto: d.Monto
            )).ToList();

        _context.EmpleadorRecibosDetalleContrataciones.AddRange(detalles);
        await _context.SaveChangesAsync(cancellationToken);

        // PASO 3: Update estatus si es "Pago Final" (LÓGICA LEGACY - tercer DbContext)
        // GAP-005: Esta es la lógica faltante que se está implementando
        var primerConcepto = request.Detalles.Select(x => x.Concepto).FirstOrDefault();

        if (primerConcepto == "Pago Final")
        {
            var detalleContratacion = await _context.Set<DetalleContratacion>()
                .Where(x => x.ContratacionId == request.ContratacionId && x.DetalleId == request.DetalleId)
                .FirstOrDefaultAsync(cancellationToken);

            if (detalleContratacion != null)
            {
                // Usar método de comportamiento DDD para completar la contratación
                detalleContratacion.Completar();
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                _logger.LogWarning(
                    "DetalleContratacion no encontrado para actualizar estatus. ContratacionID: {ContratacionId}, DetalleID: {DetalleId}",
                    request.ContratacionId, request.DetalleId);
            }
        }

        _logger.LogInformation(
            "Pago de contratación procesado exitosamente. PagoID: {PagoId}",
            header.PagoId);

        return header.PagoId;
    }
}
