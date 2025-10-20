using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Nominas.Commands.ExportarNominaCsv;

/// <summary>
/// Handler para exportar nómina a CSV
/// </summary>
public class ExportarNominaCsvCommandHandler : IRequestHandler<ExportarNominaCsvCommand, ExportarNominaCsvResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ExportarNominaCsvCommandHandler> _logger;

    public ExportarNominaCsvCommandHandler(
        IApplicationDbContext context,
        ILogger<ExportarNominaCsvCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ExportarNominaCsvResult> Handle(
        ExportarNominaCsvCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Exportando nómina a CSV. UserId: {UserId}, Período: {Periodo}",
            request.UserId, request.Periodo);

        // Parse período (formato: YYYY-MM)
        var parts = request.Periodo.Split('-');
        if (parts.Length != 2 || !int.TryParse(parts[0], out var year) || !int.TryParse(parts[1], out var month))
        {
            throw new ArgumentException("Formato de período inválido. Use YYYY-MM");
        }

        var fechaInicio = new DateOnly(year, month, 1);
        var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

        // Convertir DateOnly a DateTime para comparación (SQL Server FechaPago es DateTime)
        var fechaInicioDateTime = fechaInicio.ToDateTime(TimeOnly.MinValue);
        var fechaFinDateTime = fechaFin.ToDateTime(TimeOnly.MaxValue);

        // Obtener recibos del período
        var query = _context.RecibosHeader
            .Include(r => r.Detalles)
            .Where(r => r.UserId == request.UserId &&
                       r.FechaPago >= fechaInicioDateTime &&
                       r.FechaPago <= fechaFinDateTime);

        if (!request.IncluirAnulados)
        {
            query = query.Where(r => r.Estado != 0); // 0 = anulado
        }

        var recibos = await query
            .OrderBy(r => r.FechaPago)
            .ThenBy(r => r.EmpleadoId)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Recibos encontrados para exportar: {Count}", recibos.Count());

        // Generar CSV
        var csv = new StringBuilder();

        // Header
        csv.AppendLine("PagoID,EmpleadoID,FechaPago,PeriodoInicio,PeriodoFin,TotalIngresos,TotalDeducciones,NetoPagar,Estado,Concepto,Monto");

        // Rows
        foreach (var recibo in recibos)
        {
            // Agregar línea principal del recibo
            csv.AppendLine($"{recibo.PagoId}," +
                          $"{recibo.EmpleadoId}," +
                          $"{recibo.FechaPago:yyyy-MM-dd}," +
                          $"{recibo.PeriodoInicio:yyyy-MM-dd}," +
                          $"{recibo.PeriodoFin:yyyy-MM-dd}," +
                          $"{recibo.TotalIngresos:F2}," +
                          $"{recibo.TotalDeducciones:F2}," +
                          $"{recibo.NetoPagar:F2}," +
                          $"{(recibo.Estado == 2 ? "Pagado" : recibo.Estado == 3 ? "Anulado" : "Pendiente")}," +
                          $"\"INGRESOS TOTALES\"," +
                          $"{recibo.TotalIngresos:F2}");

            // Agregar líneas de deducciones
            if (recibo.Detalles != null)
            {
                foreach (var detalle in recibo.Detalles.Where(d => d.TipoConcepto == 2)) // Solo deducciones
                {
                    csv.AppendLine($"{recibo.PagoId}," +
                                  $"{recibo.EmpleadoId}," +
                                  $"{recibo.FechaPago:yyyy-MM-dd}," +
                                  $"{recibo.PeriodoInicio:yyyy-MM-dd}," +
                                  $"{recibo.PeriodoFin:yyyy-MM-dd}," +
                                  $",,," + // Columnas vacías (totales ya en fila principal)
                                  $"{(recibo.Estado == 2 ? "Pagado" : recibo.Estado == 3 ? "Anulado" : "Pendiente")}," +
                                  $"\"{detalle.Concepto}\"," +
                                  $"{detalle.Monto:F2}");
                }
            }
        }

        var fileContent = Encoding.UTF8.GetBytes(csv.ToString());
        var fileName = $"Nomina_{request.Periodo.Replace("-", "_")}_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";

        _logger.LogInformation("CSV generado exitosamente. Tamaño: {Size} bytes", fileContent.Length);

        return new ExportarNominaCsvResult
        {
            FileContent = fileContent,
            FileName = fileName,
            ContentType = "text/csv",
            TotalRecibos = recibos.Count()
        };
    }
}
