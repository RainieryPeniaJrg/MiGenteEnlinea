using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Suscripciones;

namespace MiGenteEnLinea.Domain.Entities.Suscripciones;

/// <summary>
/// Representa un plan de suscripción para contratistas/proveedores de servicios.
/// Los planes definen el acceso y características disponibles para contratistas.
/// </summary>
public sealed class PlanContratista : AggregateRoot
{
    /// <summary>
    /// Identificador único del plan.
    /// </summary>
    public int PlanId { get; private set; }

    /// <summary>
    /// Nombre del plan (ej: "Básico", "Profesional", "Premium").
    /// </summary>
    public string NombrePlan { get; private set; } = string.Empty;

    /// <summary>
    /// Precio mensual del plan en DOP (Pesos Dominicanos).
    /// </summary>
    public decimal Precio { get; private set; }

    /// <summary>
    /// Indica si el plan está activo y disponible para compra.
    /// </summary>
    public bool Activo { get; private set; }

    // Constructor privado para EF Core
    private PlanContratista() { }

    /// <summary>
    /// Crea un nuevo plan de contratista.
    /// </summary>
    /// <param name="nombrePlan">Nombre del plan</param>
    /// <param name="precio">Precio mensual en DOP</param>
    /// <returns>Nueva instancia de PlanContratista</returns>
    /// <exception cref="ArgumentException">Si los parámetros no son válidos</exception>
    public static PlanContratista Create(string nombrePlan, decimal precio)
    {
        if (string.IsNullOrWhiteSpace(nombrePlan))
            throw new ArgumentException("El nombre del plan es requerido", nameof(nombrePlan));

        if (nombrePlan.Length > 50)
            throw new ArgumentException("El nombre del plan no puede exceder 50 caracteres", nameof(nombrePlan));

        if (precio < 0)
            throw new ArgumentException("El precio no puede ser negativo", nameof(precio));

        var plan = new PlanContratista
        {
            NombrePlan = nombrePlan.Trim(),
            Precio = precio,
            Activo = true
        };

        plan.RaiseDomainEvent(new PlanContratistaCreadoEvent(plan.PlanId, plan.NombrePlan, plan.Precio));

        return plan;
    }

    /// <summary>
    /// Actualiza el nombre del plan.
    /// </summary>
    public void ActualizarNombre(string nombrePlan)
    {
        if (string.IsNullOrWhiteSpace(nombrePlan))
            throw new ArgumentException("El nombre del plan es requerido", nameof(nombrePlan));

        if (nombrePlan.Length > 50)
            throw new ArgumentException("El nombre del plan no puede exceder 50 caracteres", nameof(nombrePlan));

        NombrePlan = nombrePlan.Trim();
    }

    /// <summary>
    /// Actualiza el precio del plan.
    /// </summary>
    public void ActualizarPrecio(decimal nuevoPrecio)
    {
        if (nuevoPrecio < 0)
            throw new ArgumentException("El precio no puede ser negativo", nameof(nuevoPrecio));

        var precioAnterior = Precio;
        Precio = nuevoPrecio;

        if (precioAnterior != nuevoPrecio)
        {
            RaiseDomainEvent(new PrecioContratistaPlanActualizadoEvent(
                PlanId,
                NombrePlan,
                precioAnterior,
                nuevoPrecio));
        }
    }

    /// <summary>
    /// Activa el plan haciéndolo disponible para compra.
    /// </summary>
    public void Activar()
    {
        if (Activo)
            throw new InvalidOperationException("El plan ya está activo");

        Activo = true;
    }

    /// <summary>
    /// Desactiva el plan impidiendo nuevas compras (suscripciones existentes no se afectan).
    /// </summary>
    public void Desactivar()
    {
        if (!Activo)
            throw new InvalidOperationException("El plan ya está desactivado");

        Activo = false;
        RaiseDomainEvent(new PlanContratistaDesactivadoEvent(PlanId, NombrePlan));
    }

    /// <summary>
    /// Calcula el precio anual del plan (12 meses).
    /// </summary>
    public decimal CalcularPrecioAnual() => Precio * 12;

    /// <summary>
    /// Calcula el precio con descuento por cantidad de meses.
    /// </summary>
    public decimal CalcularPrecioConDescuento(int meses, decimal porcentajeDescuento)
    {
        if (meses <= 0)
            throw new ArgumentException("Los meses deben ser mayor a 0", nameof(meses));

        if (porcentajeDescuento < 0 || porcentajeDescuento > 100)
            throw new ArgumentException("El descuento debe estar entre 0 y 100", nameof(porcentajeDescuento));

        var precioTotal = Precio * meses;
        var descuento = precioTotal * (porcentajeDescuento / 100);
        return precioTotal - descuento;
    }

    /// <summary>
    /// Calcula el costo total por cantidad de meses sin descuento.
    /// </summary>
    public decimal CalcularCostoTotal(int meses)
    {
        if (meses <= 0)
            throw new ArgumentException("Los meses deben ser mayor a 0", nameof(meses));

        return Precio * meses;
    }

    /// <summary>
    /// Verifica si el plan es gratuito.
    /// </summary>
    public bool EsGratuito() => Precio == 0;

    /// <summary>
    /// Obtiene la descripción del plan formateada.
    /// </summary>
    public string ObtenerDescripcion()
    {
        var precio = EsGratuito() ? "Gratis" : $"RD${Precio:N2}/mes";
        return $"{NombrePlan} - {precio}";
    }
}
