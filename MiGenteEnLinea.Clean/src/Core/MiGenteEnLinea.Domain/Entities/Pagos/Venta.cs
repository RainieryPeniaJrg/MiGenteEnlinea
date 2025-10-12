using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Pagos;

namespace MiGenteEnLinea.Domain.Entities.Pagos;

/// <summary>
/// Representa una transacción de venta de suscripción procesada a través de la pasarela de pagos.
/// Incluye información del pago, plan comprado, y detalles de la transacción con Cardnet.
/// </summary>
public sealed class Venta : AggregateRoot
{
    /// <summary>
    /// Identificador único de la venta.
    /// </summary>
    public int VentaId { get; private set; }

    /// <summary>
    /// ID del usuario que realizó la compra (Empleador o Contratista).
    /// </summary>
    public string UserId { get; private set; } = string.Empty;

    /// <summary>
    /// Fecha y hora de la transacción.
    /// </summary>
    public DateTime FechaTransaccion { get; private set; }

    /// <summary>
    /// Método de pago utilizado.
    /// 1 = Tarjeta de Crédito, 2 = Tarjeta de Débito, 3 = Transferencia, 4 = Otro
    /// </summary>
    public int MetodoPago { get; private set; }

    /// <summary>
    /// ID del plan comprado (PlanEmpleador o PlanContratista).
    /// </summary>
    public int PlanId { get; private set; }

    /// <summary>
    /// Precio total pagado en DOP (Pesos Dominicanos).
    /// </summary>
    public decimal Precio { get; private set; }

    /// <summary>
    /// Comentario o nota adicional sobre la venta.
    /// </summary>
    public string? Comentario { get; private set; }

    /// <summary>
    /// ID de transacción devuelto por Cardnet.
    /// </summary>
    public string? IdTransaccion { get; private set; }

    /// <summary>
    /// Clave de idempotencia para evitar transacciones duplicadas.
    /// </summary>
    public string? IdempotencyKey { get; private set; }

    /// <summary>
    /// Últimos 4 dígitos de la tarjeta utilizada (enmascarada).
    /// </summary>
    public string? UltimosDigitosTarjeta { get; private set; }

    /// <summary>
    /// Dirección IP desde la cual se realizó la compra.
    /// </summary>
    public string? DireccionIp { get; private set; }

    /// <summary>
    /// Estado de la venta.
    /// 1 = Pendiente, 2 = Aprobada, 3 = Rechazada, 4 = Reembolsada
    /// </summary>
    public int Estado { get; private set; }

    // Constructor privado para EF Core
    private Venta() { }

    /// <summary>
    /// Crea una nueva venta (transacción inicial en estado Pendiente).
    /// </summary>
    /// <param name="userId">ID del usuario comprador</param>
    /// <param name="planId">ID del plan comprado</param>
    /// <param name="precio">Precio total</param>
    /// <param name="metodoPago">Método de pago (1-4)</param>
    /// <param name="idempotencyKey">Clave de idempotencia</param>
    /// <param name="direccionIp">IP del cliente</param>
    /// <returns>Nueva instancia de Venta</returns>
    /// <exception cref="ArgumentException">Si los parámetros no son válidos</exception>
    public static Venta Create(
        string userId,
        int planId,
        decimal precio,
        int metodoPago,
        string idempotencyKey,
        string? direccionIp = null)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("El ID de usuario es requerido", nameof(userId));

        if (planId <= 0)
            throw new ArgumentException("El ID de plan debe ser mayor a 0", nameof(planId));

        if (precio < 0)
            throw new ArgumentException("El precio no puede ser negativo", nameof(precio));

        if (metodoPago < 1 || metodoPago > 4)
            throw new ArgumentException("El método de pago debe estar entre 1 y 4", nameof(metodoPago));

        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new ArgumentException("La clave de idempotencia es requerida", nameof(idempotencyKey));

        if (idempotencyKey.Length > 100)
            throw new ArgumentException("La clave de idempotencia no puede exceder 100 caracteres", nameof(idempotencyKey));

        var venta = new Venta
        {
            UserId = userId.Trim(),
            PlanId = planId,
            Precio = precio,
            MetodoPago = metodoPago,
            IdempotencyKey = idempotencyKey.Trim(),
            DireccionIp = direccionIp?.Trim(),
            FechaTransaccion = DateTime.UtcNow,
            Estado = 1 // Pendiente
        };

        venta.RaiseDomainEvent(new VentaCreadaEvent(
            venta.VentaId,
            venta.UserId,
            venta.PlanId,
            venta.Precio,
            venta.IdempotencyKey));

        return venta;
    }

    /// <summary>
    /// Marca la venta como aprobada con la información de la transacción.
    /// </summary>
    public void Aprobar(string idTransaccion, string? ultimosDigitosTarjeta = null, string? comentario = null)
    {
        if (Estado != 1) // Solo puede aprobar si está Pendiente
            throw new InvalidOperationException($"No se puede aprobar una venta en estado {ObtenerEstadoTexto()}");

        if (string.IsNullOrWhiteSpace(idTransaccion))
            throw new ArgumentException("El ID de transacción es requerido", nameof(idTransaccion));

        if (idTransaccion.Length > 100)
            throw new ArgumentException("El ID de transacción no puede exceder 100 caracteres", nameof(idTransaccion));

        IdTransaccion = idTransaccion.Trim();
        UltimosDigitosTarjeta = ultimosDigitosTarjeta?.Trim();
        Comentario = comentario?.Trim();
        Estado = 2; // Aprobada

        RaiseDomainEvent(new VentaAprobadaEvent(
            VentaId,
            UserId,
            PlanId,
            Precio,
            IdTransaccion));
    }

    /// <summary>
    /// Marca la venta como rechazada.
    /// </summary>
    public void Rechazar(string motivo)
    {
        if (Estado != 1) // Solo puede rechazar si está Pendiente
            throw new InvalidOperationException($"No se puede rechazar una venta en estado {ObtenerEstadoTexto()}");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("El motivo de rechazo es requerido", nameof(motivo));

        Comentario = $"RECHAZADA: {motivo.Trim()}";
        Estado = 3; // Rechazada

        RaiseDomainEvent(new VentaRechazadaEvent(VentaId, UserId, motivo));
    }

    /// <summary>
    /// Marca la venta como reembolsada.
    /// </summary>
    public void Reembolsar(string motivo)
    {
        if (Estado != 2) // Solo puede reembolsar si está Aprobada
            throw new InvalidOperationException("Solo se pueden reembolsar ventas aprobadas");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("El motivo de reembolso es requerido", nameof(motivo));

        Comentario = $"{Comentario ?? ""} | REEMBOLSO: {motivo.Trim()}";
        Estado = 4; // Reembolsada

        RaiseDomainEvent(new VentaReembolsadaEvent(
            VentaId,
            UserId,
            PlanId,
            Precio,
            motivo));
    }

    /// <summary>
    /// Actualiza el comentario de la venta.
    /// </summary>
    public void ActualizarComentario(string comentario)
    {
        Comentario = comentario?.Trim();
    }

    /// <summary>
    /// Obtiene el texto del estado actual.
    /// </summary>
    public string ObtenerEstadoTexto() => Estado switch
    {
        1 => "Pendiente",
        2 => "Aprobada",
        3 => "Rechazada",
        4 => "Reembolsada",
        _ => "Desconocido"
    };

    /// <summary>
    /// Obtiene el texto del método de pago.
    /// </summary>
    public string ObtenerMetodoPagoTexto() => MetodoPago switch
    {
        1 => "Tarjeta de Crédito",
        2 => "Tarjeta de Débito",
        3 => "Transferencia Bancaria",
        4 => "Otro",
        _ => "Desconocido"
    };

    /// <summary>
    /// Verifica si la venta fue exitosa (aprobada).
    /// </summary>
    public bool FueExitosa() => Estado == 2;

    /// <summary>
    /// Verifica si la venta está pendiente de procesamiento.
    /// </summary>
    public bool EstaPendiente() => Estado == 1;

    /// <summary>
    /// Verifica si la venta puede ser reembolsada.
    /// </summary>
    public bool PuedeSerReembolsada() => Estado == 2; // Solo aprobadas

    /// <summary>
    /// Verifica si la transacción fue procesada con tarjeta.
    /// </summary>
    public bool EsPagoConTarjeta() => MetodoPago == 1 || MetodoPago == 2;

    /// <summary>
    /// Obtiene un resumen de la venta.
    /// </summary>
    public string ObtenerResumen()
    {
        var tarjeta = !string.IsNullOrWhiteSpace(UltimosDigitosTarjeta) ? $" - ****{UltimosDigitosTarjeta}" : "";
        return $"Venta #{VentaId} - {ObtenerEstadoTexto()} - RD${Precio:N2} - {ObtenerMetodoPagoTexto()}{tarjeta}";
    }

    /// <summary>
    /// Calcula los días transcurridos desde la compra.
    /// </summary>
    public int CalcularDiasDesdeCompra()
    {
        return (DateTime.UtcNow - FechaTransaccion).Days;
    }

    /// <summary>
    /// Verifica si la venta es elegible para reembolso según política (ej: 30 días).
    /// </summary>
    public bool EsElegibleParaReembolso(int diasMaximos = 30)
    {
        return PuedeSerReembolsada() && CalcularDiasDesdeCompra() <= diasMaximos;
    }
}
