using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Dashboard.Queries.GetDashboardEmpleador;

/// <summary>
/// Handler para obtener las métricas del dashboard del empleador.
/// </summary>
/// <remarks>
/// Este handler ejecuta múltiples queries en paralelo para optimizar performance:
/// 1. Métricas de empleados (activos/inactivos)
/// 2. Métricas de nómina (mes/año)
/// 3. Información de suscripción
/// 4. Actividad reciente (recibos, contrataciones, calificaciones)
/// 5. Historial de pagos
/// 
/// IMPORTANTE: Los resultados deben cachearse (5-15 min) para evitar queries costosas en cada request.
/// </remarks>
public class GetDashboardEmpleadorQueryHandler : IRequestHandler<GetDashboardEmpleadorQuery, DashboardEmpleadorDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetDashboardEmpleadorQueryHandler> _logger;

    public GetDashboardEmpleadorQueryHandler(
        IApplicationDbContext context,
        ILogger<GetDashboardEmpleadorQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<DashboardEmpleadorDto> Handle(
        GetDashboardEmpleadorQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching dashboard metrics for UserId: {UserId}", request.UserId);

        var fechaReferencia = request.FechaReferencia ?? DateTime.UtcNow;
        var inicioMes = new DateTime(fechaReferencia.Year, fechaReferencia.Month, 1);
        var finMes = inicioMes.AddMonths(1).AddDays(-1);
        var inicioAno = new DateTime(fechaReferencia.Year, 1, 1);

        try
        {
            // Ejecutar queries en paralelo para mejor performance
            var empleadosTask = ObtenerMetricasEmpleados(request.UserId, cancellationToken);
            var nominaTask = ObtenerMetricasNomina(request.UserId, inicioMes, finMes, inicioAno, fechaReferencia, cancellationToken);
            var suscripcionTask = ObtenerInfoSuscripcion(request.UserId, fechaReferencia, cancellationToken);
            var actividadTask = ObtenerMetricasActividad(request.UserId, inicioMes, finMes, cancellationToken);
            var pagosTask = ObtenerUltimosPagos(request.UserId, cancellationToken);

            await Task.WhenAll(empleadosTask, nominaTask, suscripcionTask, actividadTask, pagosTask);

            var empleados = await empleadosTask;
            var nomina = await nominaTask;
            var suscripcion = await suscripcionTask;
            var actividad = await actividadTask;
            var pagos = await pagosTask;

            var dashboard = new DashboardEmpleadorDto
            {
                // Empleados
                TotalEmpleados = empleados.Total,
                EmpleadosActivos = empleados.Activos,
                EmpleadosInactivos = empleados.Inactivos,

                // Nómina
                NominaMesActual = nomina.MesActual,
                NominaAnoActual = nomina.AnoActual,
                ProximaNominaFecha = nomina.ProximaFecha,
                ProximaNominaMonto = nomina.ProximoMonto,
                TotalPagosHistoricos = nomina.TotalHistorico,

                // Suscripción
                SuscripcionPlan = suscripcion.Plan,
                SuscripcionVencimiento = suscripcion.Vencimiento,
                SuscripcionActiva = suscripcion.Activa,
                DiasRestantesSuscripcion = suscripcion.DiasRestantes,

                // Actividad
                RecibosGeneradosEsteMes = actividad.RecibosEsteMes,
                ContratacionesTemporalesActivas = actividad.ContratacionesActivas,
                ContratacionesTemporalesCompletadas = actividad.ContratacionesCompletadas,
                CalificacionesPendientes = actividad.CalificacionesPendientes,
                CalificacionesCompletadas = actividad.CalificacionesCompletadas,

                // Historial
                UltimosPagos = pagos
            };

            _logger.LogInformation(
                "Dashboard metrics fetched successfully - Empleados: {Empleados}, Nómina Mes: {NominaMes:C}",
                empleados.Total,
                nomina.MesActual);

            return dashboard;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching dashboard metrics for UserId: {UserId}", request.UserId);
            throw;
        }
    }

    // ========================================
    // 📊 MÉTRICAS DE EMPLEADOS
    // ========================================

    private async Task<(int Total, int Activos, int Inactivos)> ObtenerMetricasEmpleados(
        string userId,
        CancellationToken cancellationToken)
    {
        var empleados = await _context.Empleados
            .Where(e => e.UserId == userId)
            .Select(e => new { e.Activo })
            .ToListAsync(cancellationToken);

        var total = empleados.Count;
        var activos = empleados.Count(e => e.Activo);
        var inactivos = total - activos;

        return (total, activos, inactivos);
    }

    // ========================================
    // 💰 MÉTRICAS DE NÓMINA
    // ========================================

    private async Task<(
        decimal MesActual,
        decimal AnoActual,
        DateTime? ProximaFecha,
        decimal ProximoMonto,
        decimal TotalHistorico)> ObtenerMetricasNomina(
        string userId,
        DateTime inicioMes,
        DateTime finMes,
        DateTime inicioAno,
        DateTime fechaReferencia,
        CancellationToken cancellationToken)
    {
        // Nómina del mes actual
        var nominaMes = await _context.RecibosHeader
            .Where(r => r.UserId == userId &&
                        r.FechaPago >= inicioMes &&
                        r.FechaPago <= finMes)
            .SumAsync(r => r.NetoPagar, cancellationToken);

        // Nómina del año actual
        var nominaAno = await _context.RecibosHeader
            .Where(r => r.UserId == userId &&
                        r.FechaPago >= inicioAno &&
                        r.FechaPago <= fechaReferencia)
            .SumAsync(r => r.NetoPagar, cancellationToken);

        // Total histórico de pagos
        var totalHistorico = await _context.RecibosHeader
            .Where(r => r.UserId == userId)
            .SumAsync(r => r.NetoPagar, cancellationToken);

        // Calcular próxima nómina estimada
        var empleadosActivos = await _context.Empleados
            .Where(e => e.UserId == userId && e.Activo)
            .ToListAsync(cancellationToken);

        var proximoMonto = empleadosActivos.Sum(e => e.Salario);

        // Calcular próxima fecha basándose en el período más común
        DateTime? proximaFecha = null;
        if (empleadosActivos.Any())
        {
            var periodoPredominante = empleadosActivos
                .GroupBy(e => e.DiasPago)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key;

            if (periodoPredominante.HasValue)
            {
                // DiasPago: 7=Semanal, 15=Quincenal, 30=Mensual
                proximaFecha = fechaReferencia.AddDays(periodoPredominante.Value);
            }
        }

        return (nominaMes, nominaAno, proximaFecha, proximoMonto, totalHistorico);
    }

    // ========================================
    // 📅 INFORMACIÓN DE SUSCRIPCIÓN
    // ========================================

    private async Task<(
        string Plan,
        DateTime? Vencimiento,
        bool Activa,
        int DiasRestantes)> ObtenerInfoSuscripcion(
        string userId,
        DateTime fechaReferencia,
        CancellationToken cancellationToken)
    {
        // Buscar empleador para obtener suscripción
        var empleador = await _context.Empleadores
            .Where(e => e.UserId == userId)
            .Select(e => new
            {
                e.Id,
                EmpleadorId = e.Id  // Alias para compatibilidad
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (empleador == null)
        {
            return ("Sin Plan", null, false, 0);
        }

        // Buscar suscripción activa
        var suscripcion = await _context.Suscripciones
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.Vencimiento)
            .Join(_context.PlanesEmpleadores,
                s => s.PlanId,
                p => p.PlanId,
                (s, p) => new
                {
                    s.Vencimiento,
                    PlanNombre = p.Nombre ?? "Plan Básico"
                })
            .FirstOrDefaultAsync(cancellationToken);

        if (suscripcion == null)
        {
            return ("Sin Suscripción", null, false, 0);
        }

        var vencimiento = suscripcion.Vencimiento.ToDateTime(TimeOnly.MinValue);
        var activa = vencimiento > fechaReferencia;
        var diasRestantes = (int)(vencimiento - fechaReferencia).TotalDays;

        return (suscripcion.PlanNombre, vencimiento, activa, diasRestantes);
    }

    // ========================================
    // 📊 MÉTRICAS DE ACTIVIDAD RECIENTE
    // ========================================

    private async Task<(
        int RecibosEsteMes,
        int ContratacionesActivas,
        int ContratacionesCompletadas,
        int CalificacionesPendientes,
        int CalificacionesCompletadas)> ObtenerMetricasActividad(
        string userId,
        DateTime inicioMes,
        DateTime finMes,
        CancellationToken cancellationToken)
    {
        // Recibos generados este mes
        var recibosEsteMes = await _context.RecibosHeader
            .Where(r => r.UserId == userId &&
                        r.FechaPago >= inicioMes &&
                        r.FechaPago <= finMes)
            .CountAsync(cancellationToken);

        // NOTA: EmpleadosTemporales no está disponible en Domain Layer actual
        // Esto se implementará en LOTE futuro cuando se complete la migración
        var contratacionesActivas = 0;
        var contratacionesCompletadas = 0;

        // Calificaciones (simplificado - sin filtro por EmpleadoTemporal)
        var calificacionesPendientes = 0; // Requiere EmpleadosTemporales
        
        var calificacionesCompletadas = await _context.Calificaciones
            .Where(c => c.EmpleadorUserId == userId)
            .CountAsync(cancellationToken);

        return (
            recibosEsteMes,
            contratacionesActivas,
            contratacionesCompletadas,
            calificacionesPendientes,
            calificacionesCompletadas);
    }

    // ========================================
    // 📋 HISTORIAL DE PAGOS RECIENTES
    // ========================================

    private async Task<List<PagoRecienteDto>> ObtenerUltimosPagos(
        string userId,
        CancellationToken cancellationToken)
    {
        var ultimosPagos = await _context.RecibosHeader
            .Where(r => r.UserId == userId && r.FechaPago.HasValue)
            .OrderByDescending(r => r.FechaPago)
            .Take(10)
            .Join(
                _context.Empleados,
                recibo => recibo.EmpleadoId,
                empleado => empleado.EmpleadoId,
                (recibo, empleado) => new PagoRecienteDto
                {
                    ReciboId = recibo.PagoId,
                    Fecha = recibo.FechaPago!.Value,
                    Monto = recibo.NetoPagar,
                    EmpleadoNombre = empleado.Nombre + " " + empleado.Apellido,
                    Concepto = recibo.ConceptoPago ?? "Pago de Nómina",
                    Estado = "Completado"
                })
            .ToListAsync(cancellationToken);

        return ultimosPagos;
    }
}
