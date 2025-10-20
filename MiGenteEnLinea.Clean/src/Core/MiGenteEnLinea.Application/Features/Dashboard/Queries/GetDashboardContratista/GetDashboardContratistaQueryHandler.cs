using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Dashboard.Queries.GetDashboardContratista;

/// <summary>
/// Handler para obtener el dashboard completo del Contratista.
/// Ejecuta 11 queries en paralelo para máxima performance.
/// </summary>
public class GetDashboardContratistaQueryHandler : IRequestHandler<GetDashboardContratistaQuery, DashboardContratistaDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetDashboardContratistaQueryHandler> _logger;

    public GetDashboardContratistaQueryHandler(
        IApplicationDbContext context,
        ILogger<GetDashboardContratistaQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<DashboardContratistaDto> Handle(
        GetDashboardContratistaQuery request,
        CancellationToken cancellationToken)
    {
        var fechaReferencia = request.FechaReferencia ?? DateTime.UtcNow;

        _logger.LogInformation(
            "Iniciando obtención de dashboard para Contratista UserId: {UserId}, FechaReferencia: {Fecha}",
            request.UserId,
            fechaReferencia);

        try
        {
            // ===== PASO 1: Ejecutar todas las queries en paralelo =====
            var perfilTask = ObtenerPerfil(request.UserId, cancellationToken);
            var calificacionesTask = ObtenerMetricasCalificaciones(request.UserId, cancellationToken);
            var contratacionesTask = ObtenerMetricasContrataciones(request.UserId, fechaReferencia, cancellationToken);
            var ingresosTask = ObtenerMetricasIngresos(request.UserId, fechaReferencia, cancellationToken);
            var suscripcionTask = ObtenerInfoSuscripcion(request.UserId, fechaReferencia, cancellationToken);
            var actividadTask = ObtenerMetricasActividad(request.UserId, fechaReferencia, cancellationToken);
            var historialTask = ObtenerUltimasContrataciones(request.UserId, cancellationToken);
            var evolucionTask = ObtenerEvolucionIngresos(request.UserId, fechaReferencia, cancellationToken);
            var distribucionTask = ObtenerDistribucionCalificaciones(request.UserId, cancellationToken);
            var serviciosTask = ObtenerServiciosFrecuentes(request.UserId, cancellationToken);
            var tiempoRespuestaTask = ObtenerTiempoRespuesta(request.UserId, cancellationToken);

            await Task.WhenAll(
                perfilTask, calificacionesTask, contratacionesTask, ingresosTask, suscripcionTask,
                actividadTask, historialTask, evolucionTask, distribucionTask, serviciosTask, tiempoRespuestaTask);

            // ===== PASO 2: Combinar resultados =====
            var dashboard = new DashboardContratistaDto
            {
                // Perfil
                NombreCompleto = perfilTask.Result.NombreCompleto,
                Titulo = perfilTask.Result.Titulo,
                ImagenUrl = perfilTask.Result.ImagenUrl,
                TotalServicios = perfilTask.Result.TotalServicios,
                ServiciosActivos = perfilTask.Result.ServiciosActivos,

                // Calificaciones
                PromedioCalificacion = calificacionesTask.Result.Promedio,
                TotalCalificaciones = calificacionesTask.Result.Total,
                Calificaciones5Estrellas = calificacionesTask.Result.Calificaciones5,
                Calificaciones4Estrellas = calificacionesTask.Result.Calificaciones4,
                Calificaciones3Estrellas = calificacionesTask.Result.Calificaciones3,
                CalificacionesBajas = calificacionesTask.Result.CalificacionesBajas,
                PorcentajeRecomendacion = calificacionesTask.Result.PorcentajeRecomendacion,

                // Contrataciones
                ContratacionesPendientes = contratacionesTask.Result.Pendientes,
                ContratacionesAceptadas = contratacionesTask.Result.Aceptadas,
                ContratacionesEnProgreso = contratacionesTask.Result.EnProgreso,
                ContratacionesCompletadas = contratacionesTask.Result.Completadas,
                ContratacionesCompletadasEsteMes = contratacionesTask.Result.CompletadasMes,
                ContratacionesCompletadasEsteAno = contratacionesTask.Result.CompletadasAno,
                ContratacionesCanceladas = contratacionesTask.Result.Canceladas,
                TasaExito = contratacionesTask.Result.TasaExito,

                // Ingresos
                IngresosMesActual = ingresosTask.Result.IngresosMes,
                IngresosAnoActual = ingresosTask.Result.IngresosAno,
                IngresosHistoricos = ingresosTask.Result.IngresosHistoricos,
                IngresoPromedioContratacion = ingresosTask.Result.IngresoPromedio,
                IngresosPendientes = ingresosTask.Result.IngresosPendientes,

                // Suscripción
                SuscripcionPlan = suscripcionTask.Result.PlanNombre,
                SuscripcionVencimiento = suscripcionTask.Result.Vencimiento,
                SuscripcionActiva = suscripcionTask.Result.Activa,
                DiasRestantesSuscripcion = suscripcionTask.Result.DiasRestantes,

                // Actividad
                UltimoLogin = actividadTask.Result.UltimoLogin,
                DiasSinCompletarTrabajo = actividadTask.Result.DiasSinCompletar,
                ContratacionesProximasVencer = actividadTask.Result.ProximasVencer,
                ContratacionesAtrasadas = actividadTask.Result.Atrasadas,

                // Historial y gráficos
                UltimasContrataciones = historialTask.Result,
                EvolucionIngresos = evolucionTask.Result,
                DistribucionCalificaciones = distribucionTask.Result,
                ServiciosMasFrecuentes = serviciosTask.Result,
                TiempoRespuesta = tiempoRespuestaTask.Result
            };

            _logger.LogInformation(
                "Dashboard obtenido - Contrataciones: {Completadas}, Calificación: {Calificacion:F2}, Ingresos Mes: {IngresosMes:C}",
                dashboard.ContratacionesCompletadas,
                dashboard.PromedioCalificacion,
                dashboard.IngresosMesActual);

            return dashboard;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener dashboard para UserId: {UserId}", request.UserId);
            throw;
        }
    }

    // ========================================
    // MÉTODOS PRIVADOS - QUERIES INDIVIDUALES
    // ========================================

    private async Task<PerfilResult> ObtenerPerfil(string userId, CancellationToken ct)
    {
        var contratista = await _context.Contratistas
            .Where(c => c.UserId == userId)
            .Select(c => new
            {
                c.Nombre,
                c.Apellido,
                c.Titulo,
                c.ImagenUrl
            })
            .FirstOrDefaultAsync(ct);

        var servicios = await _context.ContratistasServicios
            .Where(cs => _context.Contratistas.Any(c => c.Id == cs.ContratistaId && c.UserId == userId))
            .Select(cs => cs.DetalleServicio ?? "Otro")
            .Distinct()
            .ToListAsync(ct);

        return new PerfilResult
        {
            NombreCompleto = contratista != null
                ? $"{contratista.Nombre} {contratista.Apellido}".Trim()
                : "Contratista",
            Titulo = contratista?.Titulo ?? "Sin título",
            ImagenUrl = contratista?.ImagenUrl,
            TotalServicios = servicios.Count(),
            ServiciosActivos = servicios
        };
    }

    private async Task<CalificacionesResult> ObtenerMetricasCalificaciones(string userId, CancellationToken ct)
    {
        // Obtener identificación del contratista desde UserId
        var contratistaIdentificacion = await _context.Contratistas
            .Where(c => c.UserId == userId)
            .Select(c => c.Identificacion)
            .FirstOrDefaultAsync(ct);

        if (string.IsNullOrEmpty(contratistaIdentificacion))
        {
            return new CalificacionesResult();
        }

        // Obtener calificaciones y calcular promedio de 4 dimensiones
        var calificaciones = await _context.Calificaciones
            .Where(c => c.ContratistaIdentificacion == contratistaIdentificacion)
            .Select(c => new
            {
                PromedioCalificacion = (c.Puntualidad + c.Cumplimiento + c.Conocimientos + c.Recomendacion) / 4.0m
            })
            .ToListAsync(ct);

        if (!calificaciones.Any())
        {
            return new CalificacionesResult();
        }

        var total = calificaciones.Count;
        var promedio = calificaciones.Average(c => c.PromedioCalificacion);

        var cal5 = calificaciones.Count(c => c.PromedioCalificacion >= 4.5m);
        var cal4 = calificaciones.Count(c => c.PromedioCalificacion >= 3.5m && c.PromedioCalificacion < 4.5m);
        var cal3 = calificaciones.Count(c => c.PromedioCalificacion >= 2.5m && c.PromedioCalificacion < 3.5m);
        var calBajas = calificaciones.Count(c => c.PromedioCalificacion < 2.5m);

        var recomendacion = total > 0
            ? ((decimal)(cal5 + cal4) / total) * 100
            : 0;

        return new CalificacionesResult
        {
            Total = total,
            Promedio = promedio,
            Calificaciones5 = cal5,
            Calificaciones4 = cal4,
            Calificaciones3 = cal3,
            CalificacionesBajas = calBajas,
            PorcentajeRecomendacion = recomendacion
        };
    }

    private async Task<ContratacionesResult> ObtenerMetricasContrataciones(
        string userId, DateTime fechaReferencia, CancellationToken ct)
    {
        // Obtener ContratistaId
        var contratistaId = await _context.Contratistas
            .Where(c => c.UserId == userId)
            .Select(c => c.Id)
            .FirstOrDefaultAsync(ct);

        if (contratistaId == 0)
        {
            return new ContratacionesResult();
        }

        // Todas las contrataciones del contratista (asumiendo que hay FK en DetalleContratacion)
        // NOTA: Necesitamos agregar ContratistaId a DetalleContratacion en el futuro
        // Por ahora, usamos ContratacionId como proxy
        var contrataciones = await _context.DetalleContrataciones
            .ToListAsync(ct); // TODO: Filtrar por ContratistaId cuando esté disponible

        var inicioMes = new DateTime(fechaReferencia.Year, fechaReferencia.Month, 1);
        var inicioAno = new DateTime(fechaReferencia.Year, 1, 1);

        var pendientes = contrataciones.Count(c => c.Estatus == 1);
        var aceptadas = contrataciones.Count(c => c.Estatus == 2);
        var enProgreso = contrataciones.Count(c => c.Estatus == 3);
        var completadas = contrataciones.Count(c => c.Estatus == 4);
        var canceladas = contrataciones.Count(c => c.Estatus == 5 || c.Estatus == 6);

        var completadasMes = contrataciones.Count(c =>
            c.Estatus == 4 && c.FechaFinalizacionReal.HasValue &&
            c.FechaFinalizacionReal.Value >= inicioMes &&
            c.FechaFinalizacionReal.Value < fechaReferencia);

        var completadasAno = contrataciones.Count(c =>
            c.Estatus == 4 && c.FechaFinalizacionReal.HasValue &&
            c.FechaFinalizacionReal.Value >= inicioAno &&
            c.FechaFinalizacionReal.Value < fechaReferencia);

        var totalConSolucion = completadas + canceladas;
        var tasaExito = totalConSolucion > 0
            ? ((decimal)completadas / totalConSolucion) * 100
            : 0;

        return new ContratacionesResult
        {
            Pendientes = pendientes,
            Aceptadas = aceptadas,
            EnProgreso = enProgreso,
            Completadas = completadas,
            CompletadasMes = completadasMes,
            CompletadasAno = completadasAno,
            Canceladas = canceladas,
            TasaExito = tasaExito
        };
    }

    private async Task<IngresosResult> ObtenerMetricasIngresos(
        string userId, DateTime fechaReferencia, CancellationToken ct)
    {
        var contratistaId = await _context.Contratistas
            .Where(c => c.UserId == userId)
            .Select(c => c.Id)
            .FirstOrDefaultAsync(ct);

        if (contratistaId == 0)
        {
            return new IngresosResult();
        }

        var contrataciones = await _context.DetalleContrataciones
            .ToListAsync(ct); // TODO: Filtrar por ContratistaId

        var inicioMes = new DateTime(fechaReferencia.Year, fechaReferencia.Month, 1);
        var inicioAno = new DateTime(fechaReferencia.Year, 1, 1);

        var completadas = contrataciones.Where(c => c.Estatus == 4).ToList();

        var ingresosMes = completadas
            .Where(c => c.FechaFinalizacionReal.HasValue &&
                       c.FechaFinalizacionReal.Value >= inicioMes &&
                       c.FechaFinalizacionReal.Value < fechaReferencia)
            .Sum(c => c.MontoAcordado);

        var ingresosAno = completadas
            .Where(c => c.FechaFinalizacionReal.HasValue &&
                       c.FechaFinalizacionReal.Value >= inicioAno &&
                       c.FechaFinalizacionReal.Value < fechaReferencia)
            .Sum(c => c.MontoAcordado);

        var ingresosHistoricos = completadas.Sum(c => c.MontoAcordado);

        var ingresoPromedio = completadas.Any()
            ? completadas.Average(c => c.MontoAcordado)
            : 0;

        var ingresosPendientes = contrataciones
            .Where(c => c.Estatus == 2 || c.Estatus == 3) // Aceptadas + En Progreso
            .Sum(c => c.MontoAcordado);

        return new IngresosResult
        {
            IngresosMes = ingresosMes,
            IngresosAno = ingresosAno,
            IngresosHistoricos = ingresosHistoricos,
            IngresoPromedio = ingresoPromedio,
            IngresosPendientes = ingresosPendientes
        };
    }

    private async Task<SuscripcionResult> ObtenerInfoSuscripcion(
        string userId, DateTime fechaReferencia, CancellationToken ct)
    {
        var suscripcion = await _context.Suscripciones
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.FechaInicio)
            .FirstOrDefaultAsync(ct);

        if (suscripcion == null)
        {
            return new SuscripcionResult
            {
                PlanNombre = "Sin Plan",
                Activa = false,
                DiasRestantes = 0
            };
        }

        var plan = await _context.PlanesContratistas
            .Where(p => p.PlanId == suscripcion.PlanId)
            .Select(p => p.NombrePlan)
            .FirstOrDefaultAsync(ct);

        var vencimiento = suscripcion.Vencimiento.ToDateTime(TimeOnly.MinValue);
        var activa = vencimiento > fechaReferencia;
        var diasRestantes = (int)(vencimiento - fechaReferencia).TotalDays;

        return new SuscripcionResult
        {
            PlanNombre = plan ?? "Plan Básico",
            Vencimiento = vencimiento,
            Activa = activa,
            DiasRestantes = diasRestantes
        };
    }

    private async Task<ActividadResult> ObtenerMetricasActividad(
        string userId, DateTime fechaReferencia, CancellationToken ct)
    {
        var contratistaId = await _context.Contratistas
            .Where(c => c.UserId == userId)
            .Select(c => c.Id)
            .FirstOrDefaultAsync(ct);

        // Último login (de Credencial.UltimoAcceso si existe)
        var ultimoLogin = await _context.Credenciales
            .Where(c => c.UserId == userId)
            .Select(c => c.UltimoAcceso)
            .FirstOrDefaultAsync(ct);

        // Última contratación completada
        var ultimaCompletada = await _context.DetalleContrataciones
            .Where(c => c.Estatus == 4 && c.FechaFinalizacionReal.HasValue)
            .OrderByDescending(c => c.FechaFinalizacionReal)
            .Select(c => c.FechaFinalizacionReal)
            .FirstOrDefaultAsync(ct);

        var diasSinCompletar = ultimaCompletada.HasValue
            ? (int)(fechaReferencia - ultimaCompletada.Value).TotalDays
            : (int?)null;

        var hoy = DateOnly.FromDateTime(fechaReferencia);
        var en7Dias = hoy.AddDays(7);

        var proximasVencer = await _context.DetalleContrataciones
            .CountAsync(c => c.Estatus == 3 && // En Progreso
                            c.FechaFinal >= hoy &&
                            c.FechaFinal <= en7Dias, ct);

        var atrasadas = await _context.DetalleContrataciones
            .CountAsync(c => c.Estatus == 3 && // En Progreso
                            c.FechaFinal < hoy, ct);

        return new ActividadResult
        {
            UltimoLogin = ultimoLogin,
            DiasSinCompletar = diasSinCompletar,
            ProximasVencer = proximasVencer,
            Atrasadas = atrasadas
        };
    }

    private async Task<List<ContratacionRecienteDto>> ObtenerUltimasContrataciones(
        string userId, CancellationToken ct)
    {
        var contratistaId = await _context.Contratistas
            .Where(c => c.UserId == userId)
            .Select(c => c.Id)
            .FirstOrDefaultAsync(ct);

        var contrataciones = await _context.DetalleContrataciones
            .OrderByDescending(c => c.FechaInicio)
            .Take(10)
            .ToListAsync(ct);

        var resultado = new List<ContratacionRecienteDto>();

        foreach (var c in contrataciones)
        {
            // Obtener calificación si existe (promedio de 4 dimensiones)
            var calificacion = c.CalificacionId.HasValue
                ? await _context.Calificaciones
                    .Where(cal => cal.Id == c.CalificacionId.Value)
                    .Select(cal => (decimal?)((cal.Puntualidad + cal.Cumplimiento + cal.Conocimientos + cal.Recomendacion) / 4.0m))
                    .FirstOrDefaultAsync(ct)
                : null;

            resultado.Add(new ContratacionRecienteDto
            {
                DetalleId = c.DetalleId,
                DescripcionCorta = c.DescripcionCorta,
                FechaInicio = c.FechaInicio.ToDateTime(TimeOnly.MinValue),
                FechaFinal = c.FechaFinal.ToDateTime(TimeOnly.MinValue),
                MontoAcordado = c.MontoAcordado,
                Estado = ObtenerNombreEstado(c.Estatus),
                PorcentajeAvance = c.PorcentajeAvance,
                Calificado = c.Calificado,
                CalificacionRecibida = calificacion
            });
        }

        return resultado;
    }

    private async Task<List<IngresosEvolucionDto>> ObtenerEvolucionIngresos(
        string userId, DateTime fechaReferencia, CancellationToken ct)
    {
        var contratistaId = await _context.Contratistas
            .Where(c => c.UserId == userId)
            .Select(c => c.Id)
            .FirstOrDefaultAsync(ct);

        var fechaInicio = fechaReferencia.AddMonths(-6);

        var contrataciones = await _context.DetalleContrataciones
            .Where(c => c.Estatus == 4 && // Completadas
                       c.FechaFinalizacionReal.HasValue &&
                       c.FechaFinalizacionReal.Value >= fechaInicio)
            .ToListAsync(ct);

        var evolucion = contrataciones
            .GroupBy(c => new
            {
                Ano = c.FechaFinalizacionReal!.Value.Year,
                Mes = c.FechaFinalizacionReal!.Value.Month
            })
            .Select(g => new
            {
                g.Key.Ano,
                g.Key.Mes,
                TotalIngresos = g.Sum(c => c.MontoAcordado),
                CantidadContrataciones = g.Count()
            })
            .OrderBy(x => x.Ano).ThenBy(x => x.Mes)
            .ToList();

        var meses = new[] { "", "Ene", "Feb", "Mar", "Abr", "May", "Jun",
                            "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

        return evolucion.Select(e => new IngresosEvolucionDto
        {
            Mes = $"{meses[e.Mes]} {e.Ano}",
            Ano = e.Ano,
            NumeroMes = e.Mes,
            TotalIngresos = e.TotalIngresos,
            CantidadContrataciones = e.CantidadContrataciones
        }).ToList();
    }

    private async Task<List<CalificacionDistribucionDto>> ObtenerDistribucionCalificaciones(
        string userId, CancellationToken ct)
    {
        var contratistaIdentificacion = await _context.Contratistas
            .Where(c => c.UserId == userId)
            .Select(c => c.Identificacion)
            .FirstOrDefaultAsync(ct);

        if (string.IsNullOrEmpty(contratistaIdentificacion))
        {
            return new List<CalificacionDistribucionDto>();
        }

        // Calcular promedio de 4 dimensiones para cada calificación
        var calificaciones = await _context.Calificaciones
            .Where(c => c.ContratistaIdentificacion == contratistaIdentificacion)
            .Select(c => (c.Puntualidad + c.Cumplimiento + c.Conocimientos + c.Recomendacion) / 4.0m)
            .ToListAsync(ct);

        if (!calificaciones.Any())
        {
            return new List<CalificacionDistribucionDto>();
        }

        var total = calificaciones.Count;

        // Agrupar en rangos (1-5 estrellas) redondeando
        var distribucion = calificaciones
            .Select(c => (int)Math.Round(c))
            .GroupBy(c => c)
            .Select(g => new
            {
                Estrellas = g.Key,
                Cantidad = g.Count()
            })
            .OrderBy(x => x.Estrellas)
            .ToList();

        return distribucion.Select(d => new CalificacionDistribucionDto
        {
            Estrellas = d.Estrellas,
            Cantidad = d.Cantidad,
            Porcentaje = ((decimal)d.Cantidad / total) * 100
        }).ToList();
    }

    private async Task<List<ServicioFrecuenciaDto>> ObtenerServiciosFrecuentes(
        string userId, CancellationToken ct)
    {
        var contratistaId = await _context.Contratistas
            .Where(c => c.UserId == userId)
            .Select(c => c.Id)
            .FirstOrDefaultAsync(ct);

        // Obtener servicios del contratista
        var servicios = await _context.ContratistasServicios
            .Where(cs => cs.ContratistaId == contratistaId)
            .Select(cs => cs.DetalleServicio ?? "Otro")
            .ToListAsync(ct);

        // Contar contrataciones por servicio (basado en DescripcionCorta)
        // Esto es aproximado - idealmente DetalleContratacion debería tener ServicioId
        var contrataciones = await _context.DetalleContrataciones
            .Where(c => c.Estatus == 4) // Solo completadas
            .ToListAsync(ct);

        var frecuencias = servicios.Select(servicio => new
        {
            Servicio = servicio,
            Cantidad = contrataciones.Count(c =>
                c.DescripcionCorta.Contains(servicio, StringComparison.OrdinalIgnoreCase)),
            Ingreso = contrataciones
                .Where(c => c.DescripcionCorta.Contains(servicio, StringComparison.OrdinalIgnoreCase))
                .Sum(c => c.MontoAcordado)
        })
        .Where(x => x.Cantidad > 0)
        .OrderByDescending(x => x.Cantidad)
        .Take(5)
        .ToList();

        var totalContrataciones = frecuencias.Sum(f => f.Cantidad);

        return frecuencias.Select(f => new ServicioFrecuenciaDto
        {
            Servicio = f.Servicio,
            CantidadContrataciones = f.Cantidad,
            IngresoTotal = f.Ingreso,
            Porcentaje = totalContrataciones > 0
                ? ((decimal)f.Cantidad / totalContrataciones) * 100
                : 0
        }).ToList();
    }

    private async Task<TiempoRespuestaDto> ObtenerTiempoRespuesta(
        string userId, CancellationToken ct)
    {
        var contratistaId = await _context.Contratistas
            .Where(c => c.UserId == userId)
            .Select(c => c.Id)
            .FirstOrDefaultAsync(ct);

        var todasPropuestas = await _context.DetalleContrataciones
            .ToListAsync(ct);

        var propuestasRespondidas = todasPropuestas
            .Where(c => c.Estatus != 1) // No pendientes
            .ToList();

        // Calcular tiempo de respuesta (aproximado con FechaInicioReal)
        var tiemposRespuesta = propuestasRespondidas
            .Where(c => c.FechaInicioReal.HasValue)
            .Select(c =>
            {
                var fechaCreacion = c.FechaInicio.ToDateTime(TimeOnly.MinValue);
                var fechaRespuesta = c.FechaInicioReal!.Value;
                return (fechaRespuesta - fechaCreacion).TotalDays;
            })
            .ToList();

        var promedioRespuesta = tiemposRespuesta.Any()
            ? (decimal)tiemposRespuesta.Average()
            : 0;

        var respuestasRapidas = tiemposRespuesta.Count(t => t < 1); // Menos de 24 horas

        var aceptadas = todasPropuestas.Count(c => c.Estatus == 2 || c.Estatus == 3 || c.Estatus == 4);
        var totalPropuestas = todasPropuestas.Count;
        var tasaAceptacion = totalPropuestas > 0
            ? ((decimal)aceptadas / totalPropuestas) * 100
            : 0;

        var sinResponder = todasPropuestas.Count(c => c.Estatus == 1);

        return new TiempoRespuestaDto
        {
            PromedioRespuestaDias = promedioRespuesta,
            TasaAceptacion = tasaAceptacion,
            RespuestasRapidas = respuestasRapidas,
            PropuestasSinResponder = sinResponder
        };
    }

    // ========================================
    // CLASES AUXILIARES (INTERNAL RESULTS)
    // ========================================

    private class PerfilResult
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
        public int TotalServicios { get; set; }
        public List<string> ServiciosActivos { get; set; } = new();
    }

    private class CalificacionesResult
    {
        public int Total { get; set; }
        public decimal Promedio { get; set; }
        public int Calificaciones5 { get; set; }
        public int Calificaciones4 { get; set; }
        public int Calificaciones3 { get; set; }
        public int CalificacionesBajas { get; set; }
        public decimal PorcentajeRecomendacion { get; set; }
    }

    private class ContratacionesResult
    {
        public int Pendientes { get; set; }
        public int Aceptadas { get; set; }
        public int EnProgreso { get; set; }
        public int Completadas { get; set; }
        public int CompletadasMes { get; set; }
        public int CompletadasAno { get; set; }
        public int Canceladas { get; set; }
        public decimal TasaExito { get; set; }
    }

    private class IngresosResult
    {
        public decimal IngresosMes { get; set; }
        public decimal IngresosAno { get; set; }
        public decimal IngresosHistoricos { get; set; }
        public decimal IngresoPromedio { get; set; }
        public decimal IngresosPendientes { get; set; }
    }

    private class SuscripcionResult
    {
        public string PlanNombre { get; set; } = string.Empty;
        public DateTime? Vencimiento { get; set; }
        public bool Activa { get; set; }
        public int DiasRestantes { get; set; }
    }

    private class ActividadResult
    {
        public DateTime? UltimoLogin { get; set; }
        public int? DiasSinCompletar { get; set; }
        public int ProximasVencer { get; set; }
        public int Atrasadas { get; set; }
    }

    private static string ObtenerNombreEstado(int estatus) => estatus switch
    {
        1 => "Pendiente",
        2 => "Aceptada",
        3 => "En Progreso",
        4 => "Completada",
        5 => "Cancelada",
        6 => "Rechazada",
        _ => "Desconocido"
    };
}
