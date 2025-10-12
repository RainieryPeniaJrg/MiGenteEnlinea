using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Suscripciones;

namespace MiGenteEnLinea.Domain.Entities.Suscripciones;

/// <summary>
/// Entidad Suscripcion - Gestiona las suscripciones de usuarios a planes de servicio
/// Una suscripción vincula un usuario (Empleador o Contratista) con un plan de pago
/// 
/// MAPEO CON LEGACY:
/// - Tabla: Suscripciones (nombre legacy plural)
/// - Columnas: suscripcionID, userID, planID, vencimiento
/// 
/// NOTAS DE NEGOCIO:
/// - Un usuario puede tener solo una suscripción activa a la vez
/// - Las suscripciones tienen fecha de vencimiento
/// - Cuando vence, el usuario pierde acceso a funcionalidades premium
/// - Los planes son diferentes para Empleadores vs Contratistas
/// - Un planID puede referirse a Planes_empleadores o Planes_Contratistas
/// </summary>
public sealed class Suscripcion : AggregateRoot
{
    /// <summary>
    /// Identificador único de la suscripción
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Identificador del usuario (FK a Credencial.UserId)
    /// Puede ser un Empleador o un Contratista
    /// </summary>
    public string UserId { get; private set; }

    /// <summary>
    /// Identificador del plan contratado
    /// Puede referirse a Planes_empleadores.planID o Planes_Contratistas.planID
    /// según el tipo de usuario
    /// </summary>
    public int PlanId { get; private set; }

    /// <summary>
    /// Fecha de vencimiento de la suscripción
    /// Después de esta fecha, la suscripción se considera vencida
    /// </summary>
    public DateOnly Vencimiento { get; private set; }

    /// <summary>
    /// Fecha de inicio de la suscripción
    /// Se establece cuando se crea o renueva
    /// </summary>
    public DateTime FechaInicio { get; private set; }

    /// <summary>
    /// Indica si la suscripción fue cancelada por el usuario
    /// </summary>
    public bool Cancelada { get; private set; }

    /// <summary>
    /// Fecha en que se canceló la suscripción (si aplica)
    /// </summary>
    public DateTime? FechaCancelacion { get; private set; }

    /// <summary>
    /// Razón de cancelación (opcional)
    /// </summary>
    public string? RazonCancelacion { get; private set; }

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor
    private Suscripcion() { }
#pragma warning restore CS8618

    /// <summary>
    /// Constructor privado para lógica de creación
    /// </summary>
    private Suscripcion(
        string userId,
        int planId,
        DateOnly vencimiento)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        PlanId = planId;
        Vencimiento = vencimiento;
        FechaInicio = DateTime.UtcNow;
        Cancelada = false;
    }

    /// <summary>
    /// Factory Method: Crea una nueva suscripción
    /// </summary>
    /// <param name="userId">ID del usuario (debe existir en Credenciales)</param>
    /// <param name="planId">ID del plan contratado</param>
    /// <param name="duracionMeses">Duración de la suscripción en meses (default: 1 mes)</param>
    /// <returns>Nueva instancia de Suscripcion</returns>
    /// <exception cref="ArgumentException">Si los datos no son válidos</exception>
    public static Suscripcion Create(
        string userId,
        int planId,
        int duracionMeses = 1)
    {
        // Validaciones de negocio
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId es requerido", nameof(userId));

        if (planId <= 0)
            throw new ArgumentException("PlanId debe ser mayor a 0", nameof(planId));

        if (duracionMeses <= 0)
            throw new ArgumentException("Duración debe ser al menos 1 mes", nameof(duracionMeses));

        if (duracionMeses > 24)
            throw new ArgumentException("Duración no puede exceder 24 meses", nameof(duracionMeses));

        // Calcular fecha de vencimiento
        var fechaVencimiento = DateOnly.FromDateTime(
            DateTime.UtcNow.AddMonths(duracionMeses));

        var suscripcion = new Suscripcion(userId, planId, fechaVencimiento);

        // Levantar evento de dominio
        suscripcion.RaiseDomainEvent(new SuscripcionCreadaEvent(
            suscripcion.Id,
            userId,
            planId,
            fechaVencimiento));

        return suscripcion;
    }

    /// <summary>
    /// Factory Method: Crea una suscripción con fecha de vencimiento específica
    /// Útil para migraciones o casos especiales
    /// </summary>
    public static Suscripcion CreateConFechaEspecifica(
        string userId,
        int planId,
        DateOnly vencimiento)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId es requerido", nameof(userId));

        if (planId <= 0)
            throw new ArgumentException("PlanId debe ser mayor a 0", nameof(planId));

        var suscripcion = new Suscripcion(userId, planId, vencimiento);
        return suscripcion;
    }

    /// <summary>
    /// DOMAIN METHOD: Renueva la suscripción extendiendo su vencimiento
    /// </summary>
    /// <param name="duracionMeses">Meses adicionales (default: 1)</param>
    public void Renovar(int duracionMeses = 1)
    {
        if (duracionMeses <= 0)
            throw new ArgumentException("Duración debe ser al menos 1 mes", nameof(duracionMeses));

        if (duracionMeses > 24)
            throw new ArgumentException("Duración no puede exceder 24 meses", nameof(duracionMeses));

        // Si está vencida, renovar desde hoy
        // Si no está vencida, extender desde fecha de vencimiento actual
        var fechaBase = EstaVencida()
            ? DateTime.UtcNow
            : Vencimiento.ToDateTime(TimeOnly.MinValue);

        Vencimiento = DateOnly.FromDateTime(fechaBase.AddMonths(duracionMeses));
        Cancelada = false; // Si estaba cancelada, se reactiva
        FechaCancelacion = null;
        RazonCancelacion = null;

        // Levantar evento de dominio
        RaiseDomainEvent(new SuscripcionRenovadaEvent(Id, UserId, Vencimiento));
    }

    /// <summary>
    /// DOMAIN METHOD: Cancela la suscripción
    /// </summary>
    /// <param name="razon">Razón de cancelación (opcional)</param>
    public void Cancelar(string? razon = null)
    {
        if (Cancelada)
            throw new InvalidOperationException("La suscripción ya está cancelada");

        Cancelada = true;
        FechaCancelacion = DateTime.UtcNow;
        RazonCancelacion = razon?.Trim();

        // Levantar evento de dominio
        RaiseDomainEvent(new SuscripcionCanceladaEvent(Id, UserId, razon));
    }

    /// <summary>
    /// DOMAIN METHOD: Reactiva una suscripción cancelada
    /// </summary>
    public void Reactivar()
    {
        if (!Cancelada)
            throw new InvalidOperationException("La suscripción no está cancelada");

        if (EstaVencida())
            throw new InvalidOperationException("No se puede reactivar una suscripción vencida. Debe renovarse.");

        Cancelada = false;
        FechaCancelacion = null;
        RazonCancelacion = null;

        // Levantar evento de dominio
        RaiseDomainEvent(new SuscripcionReactivadaEvent(Id, UserId));
    }

    /// <summary>
    /// DOMAIN METHOD: Cambia el plan de la suscripción
    /// </summary>
    /// <param name="nuevoPlanId">ID del nuevo plan</param>
    /// <param name="ajustarVencimiento">Si debe ajustar el vencimiento proporcionalmente</param>
    public void CambiarPlan(int nuevoPlanId, bool ajustarVencimiento = false)
    {
        if (nuevoPlanId <= 0)
            throw new ArgumentException("PlanId debe ser mayor a 0", nameof(nuevoPlanId));

        if (nuevoPlanId == PlanId)
            throw new InvalidOperationException("El nuevo plan es el mismo que el actual");

        if (EstaVencida())
            throw new InvalidOperationException("No se puede cambiar el plan de una suscripción vencida");

        var planAnterior = PlanId;
        PlanId = nuevoPlanId;

        // Si se solicita ajuste, recalcular proporcionalmente
        if (ajustarVencimiento)
        {
            // Aquí se podría implementar lógica de ajuste proporcional
            // Por ahora, solo se cambia el plan sin ajustar fechas
        }

        // Levantar evento de dominio
        RaiseDomainEvent(new PlanCambiadoEvent(Id, UserId, planAnterior, nuevoPlanId));
    }

    /// <summary>
    /// DOMAIN METHOD: Extiende el vencimiento sin cambiar nada más
    /// </summary>
    /// <param name="dias">Días adicionales a agregar</param>
    /// <remarks>
    /// Útil para promociones, compensaciones, o extensiones de cortesía
    /// </remarks>
    public void ExtenderVencimiento(int dias)
    {
        if (dias <= 0)
            throw new ArgumentException("Días debe ser mayor a 0", nameof(dias));

        if (dias > 365)
            throw new ArgumentException("No se puede extender más de 365 días de una vez", nameof(dias));

        var fechaBase = Vencimiento.ToDateTime(TimeOnly.MinValue);
        Vencimiento = DateOnly.FromDateTime(fechaBase.AddDays(dias));

        // Levantar evento de dominio
        RaiseDomainEvent(new VencimientoExtendidoEvent(Id, UserId, dias, Vencimiento));
    }

    /// <summary>
    /// DOMAIN METHOD: Verifica si la suscripción está vencida
    /// </summary>
    /// <returns>True si está vencida, False si aún está vigente</returns>
    public bool EstaVencida()
    {
        return DateOnly.FromDateTime(DateTime.UtcNow) > Vencimiento;
    }

    /// <summary>
    /// DOMAIN METHOD: Verifica si la suscripción está activa
    /// Una suscripción está activa si NO está vencida Y NO está cancelada
    /// </summary>
    public bool EstaActiva()
    {
        return !EstaVencida() && !Cancelada;
    }

    /// <summary>
    /// DOMAIN METHOD: Calcula los días restantes hasta el vencimiento
    /// </summary>
    /// <returns>Días restantes (puede ser negativo si ya venció)</returns>
    public int DiasRestantes()
    {
        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        return Vencimiento.DayNumber - hoy.DayNumber;
    }

    /// <summary>
    /// DOMAIN METHOD: Verifica si la suscripción está por vencer pronto
    /// </summary>
    /// <param name="diasAnticipacion">Días de anticipación (default: 7)</param>
    /// <returns>True si vence en los próximos N días</returns>
    public bool EstaPorVencer(int diasAnticipacion = 7)
    {
        if (EstaVencida())
            return false;

        var diasRestantes = DiasRestantes();
        return diasRestantes > 0 && diasRestantes <= diasAnticipacion;
    }

    /// <summary>
    /// DOMAIN METHOD: Obtiene el tiempo de uso de la suscripción
    /// </summary>
    /// <returns>Días transcurridos desde la creación</returns>
    public int DiasDeUso()
    {
        var hoy = DateTime.UtcNow;
        return (hoy - FechaInicio).Days;
    }
}
